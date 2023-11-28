using Bars.GkhGji.Enums;

namespace Bars.GkhGji.Integration.AppealCits
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Linq.Dynamic;
    using System.Text;
    using System.Xml.Serialization;
    using AutoMapper;
    using B4;
    using B4.DataAccess;
    using B4.IoC;
    using B4.Modules.FileStorage;
    using B4.Utils;
    using Castle.Windsor;
    using Entities;
    using Gkh.Enums;
    using Gkh.Import;
    using Ionic.Zip;

    class SedIntegrator
    {
        private readonly IWindsorContainer _container;
        private ILogImportManager _manager;
        private ILogImport _logImport;

        public SedIntegrator(IWindsorContainer container)
        {
            _container = container;

            Mapper.CreateMap<SedAppealCitDto, AppealCits>()
                .ForMember(x => x.NumberGji, opt => opt.MapFrom(src => src.Number))
                .ForMember(x => x.DateFrom, opt => opt.MapFrom(src => src.Date))
                .ForMember(x => x.Correspondent, opt => opt.MapFrom(src => src.Author))
                .ForMember(x => x.CorrespondentAddress, opt => opt.MapFrom(src => src.Address))
                .ForMember(x => x.Phone, opt => opt.MapFrom(src => src.Contacts.Phone))
                .ForMember(x => x.Email, opt => opt.MapFrom(src => src.Contacts.Email))
                .ForMember(x => x.Description, opt => opt.MapFrom(src => src.Text))
                .ForMember(x => x.ExternalId, opt => opt.MapFrom(src => src.SedId))
                .ForMember(x => x.TypeCorrespondent, opt => opt.MapFrom(src => TypeCorrespondent.CitizenHe));

            _manager = _container.Resolve<ILogImportManager>();
            _logImport = _container.Resolve<ILogImport>();

            _logImport.SetFileName("sed");
            _logImport.ImportKey = "SedImport";
        }

        /// <summary>
        /// Обработка полученного файла по URL
        /// </summary>
        /// <param name="files">Полученный файл </param>
        /// <returns></returns>
        public IDataResult<AppealCitResult> Import(FileData[] files)
        {
            FileData appeal = null;
            try
            {
                appeal = files.FirstOrDefault(x => x.Extention.ToLowerInvariant().EndsWith("xml"));

                if (appeal == null)
                {
                    _logImport.Error("Общая ошибка", "нет файла с обращением");
                    _manager.FileNameWithoutExtention = "Fake";

                    throw new ArgumentException("Нет файла обращения");
                }
                
                _manager.FileNameWithoutExtention = appeal.FileName;

                SedAppealCitDto data = ParseContent(appeal.Data);

                return ProcessData(data, files.Where(x => !x.Extention.ToLowerInvariant().EndsWith("xml")).ToArray());
            }
            catch (Exception ex)
            {
                if (!(ex is ArgumentException))
                {
                    _logImport.Error("Общая ошибка", ex.Message);
                }

                throw;
            }
            finally
            {
                if (appeal != null)
                {
                    _manager.Add(appeal, _logImport);
                }
                else
                {
                    _manager.AddLog(_logImport);
                }

                _manager.Save();

                _container.Release(_logImport);
                _container.Release(_manager);
            }
        }

        /// <summary>
        /// Заполнения обращение
        /// </summary>
        /// <param name="data">Полученные данные из файла </param>
        /// <param name="attachements">Полученный файл</param>
        /// <returns></returns>
        private IDataResult<AppealCitResult> ProcessData(SedAppealCitDto data, FileData[] attachements)
        {
            var appealDomain = _container.ResolveDomain<AppealCits>();
            using (_container.Using(appealDomain))
            {
                using (var tr = _container.Resolve<IDataTransaction>())
                {
                    var appeal = appealDomain.GetAll().FirstOrDefault(x => x.ExternalId == data.SedId);

                    if (appeal != null)
                    {
                        appealDomain.Delete(appeal.Id);
                    }

                    appeal = new AppealCits();

                    Mapper.Map(data, appeal);

                    SaveFiles(appeal, attachements);

                    appealDomain.Save(appeal);

                    CreateSource(data, appeal);

                    try
                    {
                        tr.Commit();

                        _logImport.Info("Обращение", "Успешное создание обращения", LogTypeChanged.Added);

                        return new AppealResult(new AppealCitResult(appeal.Id));
                    }
                    catch
                    {
                        tr.Rollback();
                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// Архивация файла 
        /// </summary>
        /// <param name="appeal">Обращение граждан</param>
        /// <param name="attachements">Файл</param>
        private void SaveFiles(AppealCits appeal, FileData[] attachements)
        {
            using (var zip = new ZipFile("Архив", Encoding.UTF8))
            {
                attachements.ForEach(x => zip.AddEntry("{0}.{1}".FormatUsing(x.FileName, x.Extention), x.Data));

                using (var ms = new MemoryStream())
                {
                    zip.Save(ms);

                    var fm = _container.Resolve<IFileManager>();
                    using (_container.Using(fm))
                    {
                        appeal.File = fm.SaveFile(ms, "Архив.zip");
                    }
                }
            }
        }
        
        private void CreateSource(SedAppealCitDto data, AppealCits appeal)
        {
            var sourceDomain = _container.ResolveDomain<AppealCitsSource>();
            using (_container.Using(sourceDomain))
            {
                var source = new AppealCitsSource
                {
                    AppealCits = appeal,
                    RevenueSource = FindFrom<RevenueSourceGji>(data.Source),
                    RevenueForm = FindFrom<RevenueFormGji>(data.ReceiptForm),
                    RevenueDate = data.DateSource,
                    RevenueSourceNumber = data.RnumberSource
                };

                if (source.RevenueForm != null && source.RevenueSource != null)
                {
                    sourceDomain.Save(source);
                }
                else
                {
                    if (source.RevenueForm == null)
                    {
                        _logImport.Info("Форма поступлений", "Нет формы для кода {0}".FormatUsing(data.ReceiptForm));
                    }

                    if (source.RevenueSource == null)
                    {
                        _logImport.Info("Источник поступлений", "Нет источника для кода {0}".FormatUsing(data.Source));
                    }
                }
            }
        }

        private T FindFrom<T>(string name) where T : class, IEntity
        {
            if (name.IsEmpty())
            {
                return default (T);
            }

            var domain = _container.ResolveDomain<T>();
            using (_container.Using(domain))
            {
                    return domain.GetAll().FirstOrDefault(DynamicExpression.ParseLambda<T, bool>("Name = @0", name));
            }
        }

        private SedAppealCitDto ParseContent(byte[] xml)
        {
            var serializer = new XmlSerializer(typeof(SedAppealCitDto));
            using (var ms = new MemoryStream(xml))
            {
                return serializer.Deserialize(ms) as SedAppealCitDto;
            }
        }
    }
    /// <summary>
    /// Конструктор 
    /// </summary>
    public class AppealCitResult
    {
        /// <summary>
        /// Конструктор 
        /// </summary>
        /// <param name="id">Идентификатор</param>
        public AppealCitResult(long id)
        {
            Id = id;
        }
        /// <summary>
        /// Идентификатор для получения 
        /// </summary>
        public long Id { get; private set; }
    }
    /// <summary>
    /// Конструктор 
    /// </summary>
    public class AppealResult : IDataResult<AppealCitResult>
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="data">Данные</param>
        public AppealResult(AppealCitResult data)
        {
            Data = data;
            Success = true;
        }

        /// <summary>
        /// Успешность.
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Сообщение.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Данные.
        /// </summary>
        public AppealCitResult Data { get; set; }

        /// <summary>
        /// Данные.
        /// </summary>
        object IDataResult.Data
        {
            get { return Data; }
            set { Data = (AppealCitResult)value; }
        }
    }
}