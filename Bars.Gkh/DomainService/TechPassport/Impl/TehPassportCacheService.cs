namespace Bars.Gkh.DomainService.TechPassport.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;

    using Bars.B4.Config;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Utils;

    using Castle.Core.Internal;
    using Castle.Windsor;

    using Dapper;

    /// <summary>
    /// Сервис для работы с кэшем технического пасспорта
    /// </summary>
    public class TehPassportCacheService : ITehPassportCacheService
    {
        public ISessionProvider SessionProvider { get; set; }
        public IWindsorContainer Container { get; set; }

        private const string NamespaceName = "mat";
        private const string ViewName = "view_tp_teh_passport";
        private const string ViewFullName = TehPassportCacheService.NamespaceName + "." + TehPassportCacheService.ViewName;
        private const int BulkSize = 5000;

        /// <summary>
        /// Время жизни кэша в минутах
        /// </summary>
        private readonly int cacheDuration;

        /// <summary>
        /// Следующее время сброса кэша
        /// </summary>
        private DateTime dropCacheDateTime;

        private bool isCacheTableInit;

        public TehPassportCacheService(IConfigProvider configProvider)
        {
            var defaultDuration = 60;
            this.cacheDuration = configProvider?.GetConfig().AppSettings.GetAs("TehPassportCacheDuration", defaultDuration, true) ?? defaultDuration;
        }

        private IDbConnection Connection
        {
            get
            {
                if (!this.isCacheTableInit || this.dropCacheDateTime < DateTime.Now)
                {
                    this.CreateOrUpdateCacheTable();
                }

                return this.SessionProvider.GetCurrentSession().Connection;
            }
        }

        /// <inheritdoc />
        public string GetValue(long realityObjectId, string formCode, int row, int column)
        {
            return this.Connection
                .Query<string>($@"
                    SELECT value
                    FROM {TehPassportCacheService.ViewFullName}
                    WHERE reality_obj_id = @RealityObjectId
                        AND form_code = @FormCode
                        AND row_id = @RowId
                        AND column_id = @ColumnId;",
                    new
                    {
                        RealityObjId = realityObjectId,
                        FormCode = formCode,
                        RowId = row,
                        ColumnId = column
                    })
                .FirstOrDefault();
        }

        /// <inheritdoc />
        public Dictionary<long, string> GetCacheByRealityObjects(string formCode, int row, int column, ICollection<long> filterIds)
        {
            if (filterIds.IsNullOrEmpty())
            {
                return this.Connection
                    .Query<TehPassportCacheCell>($@"
                    SELECT
                        reality_obj_id as RealityObjectId,
                        form_code as FormCode,
                        row_id as RowId,
                        column_id as ColumnId,
                        value as Value
                    FROM {TehPassportCacheService.ViewFullName}
                    WHERE form_code = @FormCode
                        AND row_id = @RowId
                        AND column_id = @ColumnId;",
                        new
                        {
                            FormCode = formCode,
                            RowId = row,
                            ColumnId = column
                        })
                    .GroupBy(x => x.RealityObjectId, x => x.Value)
                    .ToDictionary(
                        x => x.Key,
                        x => x.FirstOrDefault(v => v.IsNotEmpty()));
            }
            else
            {
                var count = filterIds.Count;
                var take = TehPassportCacheService.BulkSize;
                var result = new List<TehPassportCacheCell>();

                for (var skip = 0; skip < count; skip += take)
                {
                    var partIds = filterIds
                        .Skip(skip)
                        .Take(take)
                        .AggregateWithSeparator(x => x.ToString(), ",");

                    var partResult = this.Connection
                        .Query<TehPassportCacheCell>($@"
                    SELECT
                        reality_obj_id as RealityObjectId,
                        form_code as FormCode,
                        row_id as RowId,
                        column_id as ColumnId,
                        value as Value
                    FROM {TehPassportCacheService.ViewFullName}
                    WHERE form_code = @FormCode
                        AND row_id = @RowId
                        AND column_id = @ColumnId
                        AND reality_obj_id in ({partIds});",
                            new
                            {
                                FormCode = formCode,
                                RowId = row,
                                ColumnId = column
                            })
                        .ToList();

                    result.AddRange(partResult);
                }

                return result.GroupBy(x => x.RealityObjectId, x => x.Value)
                    .ToDictionary(
                        x => x.Key,
                        x => x.FirstOrDefault(v => v.IsNotEmpty()));
            }
        }

        /// <inheritdoc />
        public List<TehPassportCacheCell> GetCacheByRealityObjectsAndRows(string formCode, int column, ICollection<long> filterIds)
        {
            if (filterIds.IsNullOrEmpty())
            {
                return this.Connection
                    .Query<TehPassportCacheCell>($@"
                    SELECT
                        reality_obj_id as RealityObjectId,
                        form_code as FormCode,
                        row_id as RowId,
                        column_id as ColumnId,
                        value as Value
                    FROM {TehPassportCacheService.ViewFullName}
                    WHERE form_code = @FormCode
                        AND column_id = @ColumnId;",
                        new
                        {
                            FormCode = formCode,
                            ColumnId = column
                        })
                    .ToList();
            }
            else
            {
                var count = filterIds.Count;
                var take = TehPassportCacheService.BulkSize;
                var result = new List<TehPassportCacheCell>();

                for (var skip = 0; skip < count; skip += take)
                {
                    var partIds = filterIds
                        .Skip(skip)
                        .Take(take)
                        .AggregateWithSeparator(x => x.ToString(), ",");

                    var partResult = this.Connection
                        .Query<TehPassportCacheCell>($@"
                    SELECT
                        reality_obj_id as RealityObjectId,
                        form_code as FormCode,
                        row_id as RowId,
                        column_id as ColumnId,
                        value as Value
                    FROM {TehPassportCacheService.ViewFullName}
                    WHERE form_code = @FormCode
                        AND column_id = @ColumnId
                        AND reality_obj_id in ({partIds});",
                            new
                            {
                                FormCode = formCode,
                                ColumnId = column
                            })
                        .ToList();

                    result.AddRange(partResult);
                }

                return result;
            }
        }

        /// <inheritdoc />
        public List<TehPassportCacheCell> GetCacheByRealityObjectsAndColumns(string formCode, int row, ICollection<long> filterIds)
        {
            if (filterIds.IsNullOrEmpty())
            {
                return this.Connection
                    .Query<TehPassportCacheCell>($@"
                    SELECT
                        reality_obj_id as RealityObjectId,
                        form_code as FormCode,
                        row_id as RowId,
                        column_id as ColumnId,
                        value as Value
                    FROM {TehPassportCacheService.ViewFullName}
                    WHERE form_code = @FormCode
                        AND row_id = @RowId;",
                        new
                        {
                            FormCode = formCode,
                            RowId = row
                        })
                    .ToList();
            }
            else
            {
                var count = filterIds.Count;
                var take = TehPassportCacheService.BulkSize;
                var result = new List<TehPassportCacheCell>();

                for (var skip = 0; skip < count; skip += take)
                {
                    var partIds = filterIds
                        .Skip(skip)
                        .Take(take)
                        .AggregateWithSeparator(x => x.ToString(), ",");

                    var partResult = this.Connection
                        .Query<TehPassportCacheCell>($@"
                    SELECT
                        reality_obj_id as RealityObjectId,
                        form_code as FormCode,
                        row_id as RowId,
                        column_id as ColumnId,
                        value as Value
                    FROM {TehPassportCacheService.ViewFullName}
                    WHERE form_code = @FormCode
                        AND row_id = @RowId
                        AND reality_obj_id in ({partIds});",
                            new
                            {
                                FormCode = formCode,
                                RowId = row
                            })
                        .ToList();

                    result.AddRange(partResult);
                }

                return result;
            }
        }

        /// <inheritdoc />
        public Dictionary<int, List<TehPassportCacheCell>> GetRows(long realityObjectId, string formCode)
        {
            return this.Connection
                .Query<TehPassportCacheCell>($@"
                    SELECT
                        reality_obj_id as RealityObjectId,
                        form_code as FormCode,
                        row_id as RowId,
                        column_id as ColumnId,
                        value as Value
                    FROM {TehPassportCacheService.ViewFullName}
                    WHERE reality_obj_id = @RealityObjId
                        AND form_code = @FormCode;",
                    new
                    {
                        RealityObjId = realityObjectId,
                        FormCode = formCode
                    })
                .GroupBy(x => x.RowId)
                .ToDictionary(x => x.Key, x => x.ToList());
        }

        /// <inheritdoc />
        public Dictionary<int, List<TehPassportCacheCell>> FindRowsByColumnValue(long realityObjectId, string formCode, int column, string value)
        {
            return this.GetRows(realityObjectId, formCode)
                    ?.Where(x => x.Value.Any(v => v.ColumnId == column))
                    .ToDictionary(x => x.Key, x => x.Value.ToList())
                ?? new Dictionary<int, List<TehPassportCacheCell>>();
        }

        /// <inheritdoc />
        public bool HasValue(long realityObjectId, string formCode, int row, int column)
        {
            return this.GetValue(realityObjectId, formCode, row, column).IsNotEmpty();
        }

        /// <inheritdoc />
        public void CreateOrUpdateCacheTable()
        {
            this.SessionProvider.GetCurrentSession()
                .Connection
                .Query($@"
CREATE OR REPLACE FUNCTION create_view_tp_teh_passport()
RETURNS void AS
$BODY$
BEGIN
    CREATE SCHEMA IF NOT EXISTS {TehPassportCacheService.NamespaceName};
    IF (SELECT NOT EXISTS (
            SELECT 1
            FROM pg_catalog.pg_class c
                JOIN pg_namespace n ON n.oid = c.relnamespace
            WHERE c.relkind = 'm'
                AND n.nspname = '{TehPassportCacheService.NamespaceName}'
                AND c.relname = '{TehPassportCacheService.ViewName}')) THEN
        BEGIN
            CREATE MATERIALIZED VIEW {TehPassportCacheService.ViewFullName} AS
                SELECT
                tp.reality_obj_id,
                tpv.form_code,
                split_part(tpv.cell_code::text, ':'::text, 1)::numeric AS row_id,
                split_part(tpv.cell_code::text, ':'::text, 2)::numeric AS column_id,
                tpv.value
                FROM tp_teh_passport_value tpv
                    JOIN tp_teh_passport tp ON tp.id = tpv.teh_passport_id
            WITH DATA;

            CREATE INDEX ON {TehPassportCacheService.ViewFullName} (reality_obj_id);
            CREATE INDEX ON {TehPassportCacheService.ViewFullName} (form_code);
            CREATE INDEX ON {TehPassportCacheService.ViewFullName} (row_id);
            CREATE INDEX ON {TehPassportCacheService.ViewFullName} (column_id);
        END;
    ELSE
        REFRESH MATERIALIZED VIEW {TehPassportCacheService.ViewFullName};
    END IF;

    ANALYZE {TehPassportCacheService.ViewFullName};

END;
$BODY$
  LANGUAGE 'plpgsql';

SELECT create_view_tp_teh_passport();
");
            this.isCacheTableInit = true;
            this.dropCacheDateTime = DateTime.Now.AddMinutes(this.cacheDuration);
        }

        /// <inheritdoc />
        public void DropCacheTable()
        {
            this.SessionProvider.GetCurrentSession()
                .Connection
                .Query($"DROP MATERIALIZED VIEW {TehPassportCacheService.ViewFullName};");
        }
    }
}