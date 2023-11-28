namespace Bars.GkhGji
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.Gkh;

    public class GkhEdoIntegViewCollection : BaseGkhViewCollection
    {
        public override int Number 
        {
            get
            {
                return 1;
            }
        }

        public override List<string> GetDropAll(DbmsKind dbmsKind)
        {
            var deleteView = this.GetType().GetMethods(BindingFlags.Instance | BindingFlags.NonPublic).Where(x => x.Name.Contains("DeleteView")).ToList();

            var queries = new List<string>();

            foreach (var method in deleteView)
            {
                var str = (string)method.Invoke(this, new object[] { dbmsKind });
                if (!string.IsNullOrEmpty(str))
                {
                    queries.Add(str);
                }
            }

            var deleteFunc = this.GetType().GetMethods(BindingFlags.Instance | BindingFlags.NonPublic).Where(x => x.Name.Contains("DeleteFunction")).ToList();
            foreach (var method in deleteFunc)
            {
                var str = (string)method.Invoke(this, new object[] { dbmsKind });
                if (!string.IsNullOrEmpty(str))
                {
                    queries.Add(str);
                }
            }

            return queries;
        }

        public override List<string> GetCreateAll(DbmsKind dbmsKind)
        {
            var queries = new List<string>();
            queries.AddRange(base.GetCreateAll(dbmsKind));
            return queries;
        }

        #region Вьюхи
        #region Create

        /// <summary>
        /// Вьюха обращений граждан
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string CreateViewAppealCits(DbmsKind dbmsKind)
        {
            if (dbmsKind == DbmsKind.Oracle)
            {
                return @"CREATE OR REPLACE VIEW view_gji_appeal_cits_edo AS 
SELECT 
    gac.id, 
    gac.DOCUMENT_NUMBER, 
    gac.gji_number, 
    gac.date_from, 
    gac.check_time, 
    gac.questions_count, 
    countro.count_ro, 
    mun.municipality, 
    c.name AS contragent_name, 
    gac.state_id,
    CASE WHEN edo.IS_EDO IS NULL THEN 0 else edo.IS_EDO end AS IS_EDO,
    gac.executant_id, 
    gac.tester_id, 
    gac.SURETY_RESOLVE_ID,
    gac.EXECUTE_DATE,
    gac.ZONAINSP_ID,
    gjiGetRobjectAdrAppeal(gac.id) AS ro_adr,
    gac.correspondent, 
    mun.municipality_id,
    edo.ADDRESS_EDO,
    acs.cnt as count_subject
FROM gji_appeal_citizens gac
    LEFT JOIN ( 
        SELECT 
            count(garo.reality_object_id) AS count_ro, 
            gac1.id AS gac_id
        FROM gji_appeal_citizens gac1
            JOIN gji_appcit_ro garo ON garo.appcit_id = gac1.id
            JOIN gkh_reality_object gro ON gro.id = garo.reality_object_id
            GROUP BY gac1.id
        ) countro ON countro.gac_id = gac.id
    LEFT JOIN ( 
        SELECT 
            gaac.id AS gac_id, 
            ( 
                SELECT 
                    gdm.name AS municipality
                FROM gji_appeal_citizens gac1
                    JOIN gji_appcit_ro garo ON garo.appcit_id = gac1.id
                    JOIN gkh_reality_object gro ON gro.id = garo.reality_object_id
                    JOIN gkh_dict_municipality gdm ON gdm.id = gro.municipality_id
                    WHERE gac1.id = gaac.id AND rownum =1
            ) AS municipality, 
            ( 
                SELECT 
                    gdm.id AS municipality_id
                FROM gji_appeal_citizens gac1
                    JOIN gji_appcit_ro garo ON garo.appcit_id = gac1.id
                    JOIN gkh_reality_object gro ON gro.id = garo.reality_object_id
                    JOIN gkh_dict_municipality gdm ON gdm.id = gro.municipality_id
                    WHERE gac1.id = gaac.id AND rownum =1
            ) AS municipality_id
        FROM gji_appeal_citizens gaac
        ) mun ON mun.gac_id = gac.id
    left join (
        select 
            acs.appcit_id,
            count(id) as cnt 
        from GJI_APPCIT_STATSUBJ acs 
        group by acs.appcit_id
    ) acs on acs.appcit_id = gac.id
    LEFT JOIN gkh_managing_organization mo ON mo.id = gac.managing_org_id
    LEFT JOIN gkh_contragent c ON c.id = mo.contragent_id
    LEFT JOIN INTGEDO_APPCITS edo ON edo.APPEAL_CITS_ID = gac.Id";
            }

            return @"
CREATE OR REPLACE VIEW view_gji_appeal_cits_edo AS 
SELECT 
    gac.id, 
    gac.DOCUMENT_NUMBER, 
    gac.gji_number, 
    gac.date_from, 
    gac.check_time, 
    gac.questions_count, 
    countro.count_ro, 
    mun.municipality, 
    c.name AS contragent_name,
    gac.state_id,
    CASE WHEN edo.IS_EDO IS NULL THEN false else edo.IS_EDO end AS IS_EDO,
    gac.executant_id, 
    gac.tester_id, 
    gac.SURETY_RESOLVE_ID,
    gac.EXECUTE_DATE,
    gac.ZONAINSP_ID,
    gjiGetRobjectAdrAppeal(gac.id) AS ro_adr,
    gac.correspondent, 
    mun.municipality_id,
    edo.address_edo,
    acs.cnt as count_subject
FROM gji_appeal_citizens gac
    LEFT JOIN ( 
        SELECT 
            count(garo.reality_object_id) AS count_ro, 
            gac1.id AS gac_id
        FROM gji_appeal_citizens gac1
            JOIN gji_appcit_ro garo ON garo.appcit_id = gac1.id
            JOIN gkh_reality_object gro ON gro.id = garo.reality_object_id
            GROUP BY gac1.id
        ) countro ON countro.gac_id = gac.id
    LEFT JOIN ( 
        SELECT 
            gaac.id AS gac_id, 
            ( 
                SELECT 
                    gdm.name AS municipality
                FROM gji_appeal_citizens gac1
                    JOIN gji_appcit_ro garo ON garo.appcit_id = gac1.id
                    JOIN gkh_reality_object gro ON gro.id = garo.reality_object_id
                    JOIN gkh_dict_municipality gdm ON gdm.id = gro.municipality_id
                    WHERE gac1.id = gaac.id
                LIMIT 1
            ) AS municipality,
            ( 
                SELECT 
                    gdm.id AS municipality_id
                FROM gji_appeal_citizens gac1
                    JOIN gji_appcit_ro garo ON garo.appcit_id = gac1.id
                    JOIN gkh_reality_object gro ON gro.id = garo.reality_object_id
                    JOIN gkh_dict_municipality gdm ON gdm.id = gro.municipality_id
                    WHERE gac1.id = gaac.id
                LIMIT 1
            ) AS municipality_id
        FROM gji_appeal_citizens gaac
        ) mun ON mun.gac_id = gac.id
    left join (
        select 
            acs.appcit_id,
            count(id) as cnt 
        from GJI_APPCIT_STATSUBJ acs 
        group by acs.appcit_id
    ) acs on acs.appcit_id = gac.id
    LEFT JOIN gkh_managing_organization mo ON mo.id = gac.managing_org_id
    LEFT JOIN gkh_contragent c ON c.id = mo.contragent_id
    LEFT JOIN INTGEDO_APPCITS edo ON edo.APPEAL_CITS_ID = gac.Id";
        }

        #endregion Create
        #region Delete

        private string DeleteViewAppealCits(DbmsKind dbmsKind)
        {
            var viewName = "view_gji_appeal_cits_edo";
            if (dbmsKind == DbmsKind.Oracle)
            {
                return DropViewOracleQuery(viewName);
            }

            return DropViewPostgreQuery(viewName);
        }


        #endregion Delete
        #endregion Вьюхи
    }
}