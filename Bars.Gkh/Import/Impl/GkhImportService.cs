namespace Bars.Gkh.Import.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using B4;
    using B4.IoC;
    using B4.Modules.Tasks.Common.Contracts;
    using B4.Modules.Tasks.Common.Contracts.Result;
    using B4.Utils;
    using Bars.B4.Modules.Tasks.Common.Service;
    using Castle.Windsor;

    public class GkhImportService : IGkhImportService
    {
        private readonly IWindsorContainer _container;
        private readonly ITaskManager _taskMan;

        public GkhImportService(
            IWindsorContainer container,
            ITaskManager taskMan)
        {
            _container = container;
            _taskMan = taskMan;
        }

        public List<GkhImportInfo> GetImportInfoList(BaseParams baseParams)
        {
            var codeImport = baseParams.Params.GetAs<string>("codeImport", ignoreCase: true);

            var imports = _container.ResolveAll<IGkhImport>();

            using (_container.Using(imports))
            {
                var importsData =  imports
                    .WhereIf(codeImport.IsNotEmpty(), x => x.CodeImport == codeImport)
                    .Select(x => new GkhImportInfo
                    {
                        Key = x.Key,
                        Name = x.Name,
                        PossibleFileExtensions = x.PossibleFileExtensions,
                        Dependencies = x.Dependencies,
                        PermissionName = x.PermissionName
                    })
                    .ToList();

                importsData.Add(new GkhImportInfo
                {
                    Key = "AmirsImport",
                    Name = "Импорт постановлений АМИРС",
                    PossibleFileExtensions = "xlsx",
                    Dependencies = null,
                    PermissionName = "Import.AmirsImport"
                });

                return importsData;
            }
        }

        public IDataResult Import(BaseParams baseParams)
        {
            return _taskMan.CreateTasks(this, baseParams);
        }

        #region Implementation of ITaskProvider

        public CreateTasksResult CreateTasks(BaseParams @params)
        {
            string importId = @params.Params["importId"].ToStr();
            var userIdentity = _container.Resolve<IUserIdentity>();
            @params.Params["userId"] = userIdentity.UserId;

            if (importId.IsEmpty())
            {
                throw new Exception("Не найден параметр импорта");
            }

            var import = _container.Resolve<IGkhImport>(importId);

            using (_container.Using(import))
            {
                if (import == null)
                {
                    throw new NotImplementedException("Импорт не реализован. Код: {0}".FormatUsing(importId));
                }

                string[] importKeys = import.Dependencies;

                var dependency = new List<Dependency>
                {
                    new Dependency
                    {
                        Scope = DependencyScope.InsideExecutors,
                        Key = importId
                    }
                };

                if (importKeys.IsNotEmpty())
                {
                    dependency.AddRange(
                        importKeys.Select(
                            i => new Dependency
                            {
                                Scope = DependencyScope.InsideExecutors,
                                Key = i
                            }));
                }

                string message;
                if (!import.Validate(@params, out message))
                {
                    throw new ValidationException(message);
                }

                return new CreateTasksResult(
                    new[]
                    {
                        new TaskDescriptor(import.Name, importId, @params)
                        {
                            Dependencies = dependency.ToArray()
                        }
                    });
            }
        }

        public string TaskCode { get { return "GkhImports"; } }

        #endregion
    }
}