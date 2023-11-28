namespace Bars.Gkh.Gis.DomainService.ImportExport.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Authentification;
    using B4;
    using B4.Config;
    using B4.DataAccess;
    using B4.Modules.FileStorage;
    using B4.Modules.Security;
    using B4.Utils;

    using Bars.Gkh.Gis.DomainService.BilConnection;
    using Bars.Gkh.Gis.KP_legacy;
    using Bars.Gkh.RegOperator.Entities;

    using Castle.Windsor;
    using Dapper;
    using Entities.Register.LoadedFileRegister;
    using Enum;
    using Gkh.Entities;
    using Gkh.Utils;
    using Utils;

    public class UnloadCounterValuesService : IUnloadCounterValuesService
    {
        /// <summary>
        /// IWindsorContainer
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Cервис получения строк соединения к серверам БД биллинга
        /// </summary>
        public IBilConnectionService BilConnectionService { get; set; }

        /// <summary>
        /// Выгрузить показания ПУ
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        public IDataResult Unload(BaseParams baseParams)
        {
            var userManager = Container.Resolve<IGkhUserManager>();
            var operatorContragentDomain = Container.ResolveDomain<OperatorContragent>();
            var cashPaymentCenter = Container.ResolveDomain<CashPaymentCenter>();
            var loadFileRegisterDomain = Container.ResolveDomain<LoadedFileRegister>();

            try
            {
                var user = userManager.GetActiveUser();

                if (user == null)
                {
                    return new BaseDataResult(false, "Не удалось получить текущего пользователя");
                }

                var userContragentDict = operatorContragentDomain.GetAll()
                    .Join(
                        cashPaymentCenter.GetAll(),
                        x => x.Contragent.Id,
                        y => y.Contragent.Id,
                        (x, y) => new {operContr = x, cashPaymCent = y})
                    .Where(x => x.operContr.Operator != null)
                    .Where(x => x.operContr.Operator.User.Id == user.Id)
                    .Select(
                        x => new
                        {
                            UserId = x.operContr.Operator.User.Id,
                            OrganizationName = x.cashPaymCent.Contragent.Name,
                            OrganizationCode = x.cashPaymCent.Identifier
                        });

                if (!userContragentDict.Any())
                {
                    return new BaseDataResult(
                          false,
                          "У вашей организации отсутствует код ЕРЦ, для его получения просьба написать на электронную почту " +
                              "МСАЖКХ РТ-Хасанова Фарида Ирековна Farida.Hasanova@tatar.ru");
                }

                //проверяем коды ЕРЦ
                foreach (var contragent in userContragentDict)
                {
                    var ercCode = contragent.OrganizationCode.ToInt();
                    if (ercCode == 0)
                        return new BaseDataResult(
                            false,
                            "У вашей организации отсутствует код ЕРЦ, для его получения просьба написать на электронную почту " +
                                "МСАЖКХ РТ-Хасанова Фарида Ирековна Farida.Hasanova@tatar.ru");
                }

                //выгрузка уже запущена
                if(loadFileRegisterDomain.GetAll()
                    .Where(x => x.Format == TypeImportFormat.UnloadCounterValuesFromPgmuRt)
                    .Where(x => x.CalculationDate.HasValue)
                    .Where(x => x.B4User != null)
                    .Where(x => x.B4User.Id == user.Id)
                    .Where(x =>
                        x.TypeStatus == TypeStatus.PreQueuing ||
                        x.TypeStatus == TypeStatus.Queuing ||
                        x.TypeStatus == TypeStatus.InProgress ||
                        x.TypeStatus == TypeStatus.Checking)
                    .Any())
                {
                    return new BaseDataResult(false, "Происходит выгрузка показаний приборов учета");
                }

                //проверки прошли успешно, начинаем выгрузку
                userContragentDict.ForEach(x => UnloadCountersValuesFromBilling(x.OrganizationCode.ToInt(), user));

                return new BaseDataResult();
            }
            finally
            {
                Container.Release(userManager);
                Container.Release(operatorContragentDomain);
                Container.Release(cashPaymentCenter);
                Container.Release(loadFileRegisterDomain);
            }
        }

        /// <summary>
        /// Получить список выгрузок показаний ПУ
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        public IDataResult GetList(BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();
            var loadFileRegisterDomain = Container.ResolveDomain<LoadedFileRegister>();
            var userManager = Container.Resolve<IGkhUserManager>();
            var operatorContragentDomain = Container.ResolveDomain<OperatorContragent>();
            var cashPaymentCenter = Container.ResolveDomain<CashPaymentCenter>();

            try
            {
                var user = userManager.GetActiveUser();

                if (user == null)
                {
                    return null;
                }

                var monthFilter = loadParam.FindInComplexFilter("Month");

                DateTime? filterForMonth = null;

                if (monthFilter != null)
                {
                    filterForMonth = new DateTime(monthFilter.Value.ToDateTime().Year, monthFilter.Value.ToDateTime().Month, 1);
                    loadParam.SetComplexFilterNull("Month");
                }

                var userContragentDict = operatorContragentDomain.GetAll()
                    .Join(cashPaymentCenter.GetAll(),
                        x => x.Contragent.Id,
                        y => y.Contragent.Id,
                        (x, y) => new { operContr = x, cashPaymCent = y })
                    .Where(x => x.operContr.Operator != null)
                    .Where(x => x.operContr.Operator.User.Id == user.Id)
                    .Select(x => new
                    {
                        UserId = x.operContr.Operator.User.Id,
                        OrganizationName = x.cashPaymCent.Contragent.Name,
                        OrganizationCode = x.cashPaymCent.Identifier
                    })
                    .ToList()
                    .GroupBy(x => x.UserId)
                    .ToDictionary(x => x.Key, x => x.Select(y => new
                                                    {
                                                        y.OrganizationName,
                                                        y.OrganizationCode
                                                    }).First());

                var data = loadFileRegisterDomain.GetAll()
                    .Where(x => x.Format == TypeImportFormat.UnloadCounterValuesFromPgmuRt)
                    .Where(x => x.CalculationDate.HasValue)
                    .Where(x => x.B4User != null)
                    //если текущий пользователь не админ, то показываем только его файлы
                    .WhereIf(user.Roles.All(y => y.Role.Name != "Администратор"), x => x.B4User.Id == user.Id)
                    .WhereIf(filterForMonth.HasValue, x => x.CalculationDate.Value == filterForMonth.Value)
                    .ToList()
                    .Select(x => new
                    {
                        FormationDate = x.ObjectCreateDate.ToUniversalTime(),
                        Month = new DateTime(x.CalculationDate.Value.Year, x.CalculationDate.Value.Month, 1),
                        User = x.B4User.Name,
                        OrganizationName = userContragentDict.ContainsKey(x.B4User.Id)
                            ? userContragentDict[x.B4User.Id].OrganizationName
                            : string.Empty,
                        OrganizationCode = userContragentDict.ContainsKey(x.B4User.Id)
                            ? userContragentDict[x.B4User.Id].OrganizationCode
                            : string.Empty,
                        x.TypeStatus,
                        x.File,
                        x.Log
                    })
                    .AsQueryable()
                    .OrderIf(loadParam.Order.Length == 0, false, x => x.FormationDate)
                    .OrderThenIf(loadParam.Order.Length == 0, false, x => x.Month)
                    .Filter(loadParam, Container);

                return new ListDataResult(data.Order(loadParam).Paging(loadParam).ToList(), data.Count());
            }
            finally
            {
                Container.Release(loadFileRegisterDomain);
                Container.Release(userManager);
                Container.Release(operatorContragentDomain);
                Container.Release(cashPaymentCenter);
            }
        }

        /// <summary>
        /// Выгрузка показаний ПУ из ПГУ по одному ЕРЦ
        /// </summary>
        /// <param name="ercCode">Код ЕРЦ, для которого надо выгрузить показания ПУ</param>
        /// <param name="user">Пользователь в МЖФ</param>
        /// <returns></returns>
        public IDataResult UnloadCountersValuesFromBilling(int ercCode, User user)
        {
            // Здесь будет результат работы, который потом запишется в лог-файл
            var listCounters = new List<string>();
            // Здесь будет протокол выгрузки
            var unloadLog = new List<string>();
            //статус зарузки
            var unloadStatus = TypeStatus.Queuing;
            //расчетный месяц
            var calculationDate = new DateTime();
            //уникальный код пачки
            var newNzpPack = 0;

            unloadLog.Add("Время начала выгрузки: " + DateTime.Now.ToString("dd.MM.yyyy hh:mm:ss"));

            try
            {
                //подключаемся к БД ПГУ
                var provider = new ConnectionProvider(Container.Resolve<IConfigProvider>());
                provider.Init(this.BilConnectionService.GetConnection(ConnectionType.GisConnStringPgu));

                using (var dbConnection = provider.CreateConnection())
                {
                    dbConnection.Open();
                    // Проверяем наличие новых показаний ПУ в counters_ord
                    var sqlQuery =
                        " SELECT COUNT(*) as count " +
                        " FROM webfon.counters_ord " +
                        " WHERE cur_val IS NOT NULL " +
                        " AND nzp_pack_ls IS NULL" +
                        " AND webfon.extract_erc(pkod) = " + ercCode;

                    if (dbConnection.ExecuteScalar<int>(sqlQuery) == 0)
                    {
                        unloadStatus = TypeStatus.NoData;
                        unloadLog.Add("Нет новых показаний ПУ!");
                        return new BaseDataResult();
                    }


                    listCounters.Clear();

                    using (var transaction = dbConnection.BeginTransaction())
                    {
                        try
                        {
                            // Создаем описание пачки в counters_pack
                            sqlQuery =
                                " insert into webfon.counters_pack (source_name, dat_pack, dat_when) " +
                                " values ('portal',current_date,current_timestamp);";
                            dbConnection.Execute(sqlQuery, transaction: transaction);

                            // Определяем новое serial-значение nzp_pack
                            sqlQuery =
                                "select lastval() as key";

                            newNzpPack = dbConnection.ExecuteScalar<int>(sqlQuery, transaction: transaction);

                            unloadLog.Add("Код пачки: " + newNzpPack);

                            // Выбираем те записи, которые собираемся выгрузить, и переносим информацию о них в counters_pack_ls
                            sqlQuery =
                                string.Format(
                                    " INSERT INTO webfon.counters_pack_ls" +
                                    " (nzp_pack, nzp_ck, pref, num_ls, pkod, dat_month, dat_vvod, order_num, cur_val, nzp_serv, service, num_cnt) " +
                                    " SELECT {0} , nzp_ck, pref, num_ls, pkod, dat_month, dat_vvod, order_num, cur_val, nzp_serv, service, num_cnt " +
                                    " from webfon.counters_ord " +
                                    " where 1=1 " +
                                    " and cur_val is not null " +
                                    " and nzp_pack_ls is null " +
                                    " and webfon.extract_erc(pkod) = {1}",
                                    newNzpPack, ercCode);
                            dbConnection.Execute(sqlQuery, transaction: transaction);

                            // Проставляем показаниям из counters_ord коды nzp_pack_ls
                            sqlQuery = string.Format(
                                " UPDATE webfon.counters_ord c " +
                                " SET nzp_pack_ls = p.nzp_pack_ls " +
                                " FROM webfon.counters_pack_ls p " +
                                " WHERE p.nzp_ck = c.nzp_ck " +
                                " AND p.nzp_pack = {0} " +
                                " AND c.cur_val is NOT NULL " +
                                " AND c.nzp_pack_ls IS NULL " +
                                " AND webfon.extract_erc(c.pkod) = {1} ",
                                newNzpPack,
                                ercCode);

                            dbConnection.Execute(sqlQuery, transaction: transaction, commandTimeout: 3600);

                            // Подсчет кол-ва ЛС
                            sqlQuery =
                                " update webfon.counters_pack set cnt_ls = " +
                                " coalesce(( select count(distinct pkod) from webfon.counters_pack_ls  ls " +
                                " where ls.nzp_pack = webfon.counters_pack.nzp_pack), 0) " +
                                " where nzp_pack= " + newNzpPack;
                            dbConnection.Execute(sqlQuery, transaction: transaction);


                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            throw new Exception("Ошибка при считывании из БД показаний ПУ: " + ex);
                        }
                    }

                    // Промежуточная таблица с записями на выгрузку
                    var tempPackLsTable = string.Format("t_cnt_pck_ls_{0}", DateTime.Now.Ticks);
                    sqlQuery = string.Format("drop table if exists {0}", tempPackLsTable);
                    dbConnection.Execute(sqlQuery);

                    sqlQuery = string.Format(
                        " create temp table {0} as " +
                        " select * from webfon.counters_pack_ls limit 0 ",
                        tempPackLsTable);
                    dbConnection.Execute(sqlQuery);

                    sqlQuery = string.Format(
                        " insert into {0} " +
                        " select * " +
                        " from webfon.counters_pack_ls " +
                        " where nzp_pack = {1} "
                        , tempPackLsTable
                        , newNzpPack);
                    dbConnection.Execute(sqlQuery);
                    sqlQuery = string.Format("create index ix_kljwh5344_01 on {0} (pref)", tempPackLsTable);
                    dbConnection.Execute(sqlQuery);
                    sqlQuery = string.Format("create index ix_kljwh5344_02 on {0} (dat_month, pkod)", tempPackLsTable);
                    dbConnection.Execute(sqlQuery);
                    sqlQuery = string.Format("analyze {0}", tempPackLsTable);
                    dbConnection.Execute(sqlQuery);

                    // Реализация выгрузки в файл
                    sqlQuery = string.Format(
                        " select " +
                        " nzp_pack AS NzpPack, " +
                        " TRIM(source_name) AS SourceName, " +
                        " dat_pack AS PackDate " +
                        " from webfon.counters_pack " +
                        " where nzp_pack = {0}",
                        newNzpPack);

                    var packInfo = dbConnection.Query<CounterValuePackInfo>(sqlQuery).FirstOrDefault();

                    sqlQuery = string.Format(
                        " select pkod AS Pkod, " +
                        " dat_month AS DatMonth, " +
                        " TRIM(pref) AS Pref, " +
                        " count(*) AS Cnt " +
                        " from {0} " +
                        " group by 1,2,3 " +
                        " order by pkod ", tempPackLsTable);

                    var pack = dbConnection.Query<CounterValuePack>(sqlQuery);

                    calculationDate = pack.FirstOrDefault().DatMonth;
                    unloadLog.Add("Расчетный месяц: " + calculationDate.ToShortDateString());

                    sqlQuery = string.Format(
                        " select distinct pkod AS Pkod, " +
                        " dat_month DatMonth, " +
                        " dat_vvod DatVvod, " +
                        " TRIM(pref) Pref, " +
                        " order_num OrderNum, " +
                        " cur_val CurVal, " +
                        " service Service, " +
                        " TRIM(num_cnt)  NumCnt" +
                        " from {0} " +
                        " order by pkod, dat_month, order_num "
                        , tempPackLsTable);

                    var counterValues = dbConnection.Query<CounterValue>(sqlQuery);

                    //формируем информационное описание
                    listCounters.Add(string.Format(
                        "***|{0}|{1}|{2}|{3}|{4}|",
                        packInfo.SourceName.Trim(),
                        packInfo.NzpPack,
                        packInfo.PackDate.ToShortDateString(),
                        packInfo.PackDate.ToShortDateString(),
                        pack.Count()
                        ));


                    foreach (var onePack in pack)
                    {

                        listCounters.Add(string.Format(
                            "###|{0}|{1}|{2}|{3}|",
                            onePack.Pkod,
                            onePack.DatMonth.ToShortDateString(),
                            onePack.Cnt,
                            onePack.Pref.Trim()));


                        counterValues
                            .Where(x => x.Pkod == onePack.Pkod
                                        && x.DatMonth == onePack.DatMonth
                                        && x.Pref == onePack.Pref)
                            .ForEach(x =>
                                listCounters.Add(string.Format(
                                    "@@@|{0}|{1}|{2}|{3}|{4}|",
                                    x.OrderNum,
                                    x.CurVal,
                                    x.Service.Trim(),
                                    x.DatVvod.ToShortDateString(),
                                    x.NumCnt.Trim()
                                    ))
                            );
                    }

                    unloadLog.Add("Показания ПУ успешно выгружены!");
                    unloadStatus = TypeStatus.Done;
                }
            }
            catch (Exception ex)
            {
                unloadLog.Add("Ошибка: " + ex);
                unloadStatus = TypeStatus.ProcessingError;
            }
            finally
            {
                unloadLog.Add("Время окончания выгрузки: " + DateTime.Now.ToString("dd.MM.yyyy hh:mm:ss"));

                //сохраняем в файл показания ПУ 
                var fileName = string.Format("ПОКАЗАНИЯ_ПУ_{0}_{1}_ПГМУ_РТ_{2}.txt", newNzpPack,
                    DateTime.Now.ToString("yyyyMMdd"), ercCode);
                var resultString = string.Join(Environment.NewLine, listCounters);
                var bytes = System.Text.Encoding.GetEncoding(1251).GetBytes(resultString);

                //сохраняем файл
                var file = Container.Resolve<IFileManager>().SaveFile(fileName, bytes);


                //сохраняем в файл протокол-загрузки
                var logFileName = string.Format("ПРОТОКОЛ_ВЫГРУЗКИ_{0}", fileName);
                resultString = string.Join(Environment.NewLine, unloadLog);
                bytes = System.Text.Encoding.GetEncoding(1251).GetBytes(resultString);
                //сохраняем лог-файл
                var logFile = Container.Resolve<IFileManager>().SaveFile(logFileName, bytes);

                if (file == null)
                {
                    throw new Exception("Не удалось сохранить файл показаний");
                }

                //пишем в gis_loaded_file_register
                var newLoadedFile = new LoadedFileRegister()
                {
                    //получаем текущего пользователя
                    B4User = user,
                    File = file,
                    FileName = file.FullName,
                    Size = file.Size,
                    TypeStatus = unloadStatus,
                    Log = logFile,
                    CalculationDate = calculationDate,
                    ImportName = TypeImportFormat.UnloadCounterValuesFromPgmuRt.GetDisplayName(),
                    Format = TypeImportFormat.UnloadCounterValuesFromPgmuRt,
                };

                Container.Resolve<IDomainService<LoadedFileRegister>>().Save(newLoadedFile);

            }
            return new BaseDataResult();
        }

        /// <summary>
        /// Вспомогательная сущность "Описатель пачки показаний ПУ"
        /// </summary>
        private class CounterValuePackInfo
        {
            /// <summary>
            /// Уникальный код пачки
            /// </summary>
            public int NzpPack { get; set; }

            /// <summary>
            /// Наименование источника показаний ПУ
            /// </summary>
            public string SourceName { get; set; }

            /// <summary>
            /// Дата формирования пачки
            /// </summary>
            public DateTime PackDate { get; set; }
        }

        /// <summary>
        /// Вспомогательная сущность "Пачка показаний ПУ"
        /// </summary>
        private class CounterValuePack
        {
            /// <summary>
            /// Платежный код
            /// </summary>
            public decimal Pkod { get; set; }

            /// <summary>
            /// Расчетный месяц
            /// </summary>
            public DateTime DatMonth { get; set; }

            /// <summary>
            /// Префикс банка данных
            /// </summary>
            public string Pref { get; set; }

            /// <summary>
            /// Кол-во показаний ПУ в пачке
            /// </summary>
            public int Cnt { get; set; }

        }

        /// <summary>
        /// Вспомогательная сущность "Показание ПУ"
        /// </summary>
        private class CounterValue
        {
            /// <summary>
            /// Платежный код
            /// </summary>
            public decimal Pkod { get; set; }

            /// <summary>
            /// Расчетный месяц
            /// </summary>
            public DateTime DatMonth { get; set; }

            /// <summary>
            /// Дата вводка показаний ПУ
            /// </summary>
            public DateTime DatVvod { get; set; }

            /// <summary>
            /// Префикс банка данных
            /// </summary>
            public string Pref { get; set; }

            /// <summary>
            /// Порядковый номер ПУ в ЕПД
            /// </summary>
            public int OrderNum { get; set; }

            /// <summary>
            /// Значение показания ПУ
            /// </summary>
            public decimal CurVal { get; set; }

            /// <summary>
            /// Наименование услуги
            /// </summary>
            public string Service { get; set; }

            /// <summary>
            /// Заводской номер ПУ
            /// </summary>
            public string NumCnt { get; set; }

        }
    }
}