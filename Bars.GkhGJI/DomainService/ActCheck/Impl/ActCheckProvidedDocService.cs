namespace Bars.GkhGji.DomainService.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;

    using Castle.Windsor;

    public class ActCheckProvidedDocService : IActCheckProvidedDocService
    {
        public IWindsorContainer Container { get; set; }

        public IDataResult AddProvidedDocs(BaseParams baseParams)
        {
            var serviceDocs = this.Container.Resolve<IDomainService<ActCheckProvidedDoc>>();

            try
            {
                var documentId = baseParams.Params.ContainsKey("documentId")
                                     ? baseParams.Params["documentId"].ToLong()
                                     : 0;
                var providedDocIds = baseParams.Params.ContainsKey("providedDocIds")
                                         ? baseParams.Params["providedDocIds"].ToString()
                                         : "";

                // в этом списке будут id gпредоставляемых документов, которые уже связаны с этим распоряжением
                // (чтобы недобавлять несколько одинаковых документов в одно и тоже распоряжение)
                var listIds = new List<long>();

                var provIds = providedDocIds.Split(',').Select(id => ObjectParseExtention.ToLong(id)).ToList();

                listIds.AddRange(
                    serviceDocs.GetAll()
                               .Where(x => x.ActCheck.Id == documentId)
                               .Select(x => x.ProvidedDoc.Id)
                               .Distinct()
                               .ToList());

                var listToSave = new List<ActCheckProvidedDoc>();
                
                foreach (var newId in provIds)
                {

                    // Если среди существующих документов уже есть такой документ то пролетаем мимо
                    if (listIds.Contains(newId))
                    {
                        continue;
                    }

                    // Если такого эксперта еще нет то добалвяем
                    listToSave.Add(new ActCheckProvidedDoc
                        {
                            ActCheck = new ActCheck { Id = documentId },
                            ProvidedDoc = new ProvidedDocGji { Id = newId }
                        });
                }

                if (listToSave.Count > 0)
                {
                    using (var tr = this.Container.Resolve<IDataTransaction>())
                    {

                        try
                        {
                            listToSave.ForEach(serviceDocs.Save);

                            tr.Commit();
                        }
                        catch (Exception exc)
                        {
                            tr.Rollback();
                            throw exc;
                        }
                    }
                }
                
                return new BaseDataResult();
            }
            catch (ValidationException e)
            {
                return new BaseDataResult { Success = false, Message = e.Message };
            }
            finally
            {
                this.Container.Release(serviceDocs);
            }
        }
    }
}