namespace Bars.Gkh.FormatDataExport.Domain.Impl
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Management.Instrumentation;

    using Bars.B4.Logging;
    using Bars.B4.Utils;
    using Bars.Gkh.FormatDataExport.Enums;
    using Bars.Gkh.FormatDataExport.ExportableEntities;
    using Bars.Gkh.Utils;

    using Castle.Windsor;

    public class ExportableEntityResolver : IExportableEntityResolver
    {
        public IWindsorContainer Container { get; set; }

        public IEnumerable<IExportableEntity> ExportableEntities { get; set; }
        public IEnumerable<IExportableEntityGroup> ExportableEntityGroups { get; set; }
        public IFormatDataExportRoleService FormatDataExportRoleService { get; set; }
        public ILogManager LogManager { get; set; }

        /// <inheritdoc />
        public IExportableEntity GetEntity(string entityCode, FormatDataExportProviderType providerType)
        {
            var allowFlag = this.FormatDataExportRoleService.GetProviderFlag(providerType);

            var entityList = this.ExportableEntities
                .Where(x => x.Code == entityCode)
                .ToList();

            var entity = entityList
                .SingleOrDefault(x => allowFlag.CheckFlags(x.AllowProviderFlags));

            if (entity == null)
            {
                throw new InstanceNotFoundException($"Не найдена сущность с кодом '{entityCode}' доступная {providerType.GetDisplayName()}");
            }

            return entity;
        }

        /// <inheritdoc />
        public ICollection<IExportableEntity> GetEntityList(FormatDataExportProviderType providerType)
        {
            var allowFlag = this.FormatDataExportRoleService.GetProviderFlag(providerType);

            return this.ExportableEntities.Where(x => allowFlag.CheckFlags(x.AllowProviderFlags)).ToList();
        }

        /// <inheritdoc />
        public ICollection<IExportableEntity> GetInheritedEntityList(IList<string> entityGroupCodes, FormatDataExportProviderType providerType)
        {
            var result = new HashSet<string>();

            var codes = this.ExportableEntityGroups.Where(x => entityGroupCodes.Contains(x.Code))
                .SelectMany(x => x.InheritedEntityCodeList)
                .Distinct();

            foreach (var code in codes)
            {
                this.FindInheritedEntities(code, ref result, providerType);
            }

            return this.ExportableEntities.Where(x => result.Contains(x.Code)).ToList();
        }

        private void FindInheritedEntities(string entityCode, ref HashSet<string> codeSet, FormatDataExportProviderType providerType)
        {
            var entity = this.ExportableEntities.SingleOrDefault(x => x.Code == entityCode);

            if (entity == null)
            {
                this.LogManager.Warning($"Не найдена экспортируемая сущность: '{entityCode}'");
                return;
            }

            var allowFlag = this.FormatDataExportRoleService.GetProviderFlag(providerType);

            if (!allowFlag.CheckFlags(entity.AllowProviderFlags))
            {
                this.LogManager.Warning($"Отсутствуют права для доступа к экспортируемой сущности: '{entityCode}'");
                return;
            }

            if (!codeSet.Add(entityCode))
            {
                return;
            }

            foreach (var code in entity.GetInheritedEntityCodeList())
            {
                this.FindInheritedEntities(code, ref codeSet, providerType);
            }
        }
    }
}