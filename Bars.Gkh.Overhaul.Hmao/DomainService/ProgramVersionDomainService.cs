namespace Bars.Gkh.Overhaul.Hmao.DomainService
{
    using System;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.DomainService.BaseParams;
    using Bars.Gkh.Overhaul.Hmao.Entities;

	/// <summary>
	/// Домен сервис для Версия программы
	/// </summary>
    public class ProgramVersionDomainService : BaseDomainService<ProgramVersion>
    {
        /// <summary>
        /// Данный метод удаления перекрыл потому что перед кдалением надо удалить большие объекмы данных
        /// В связи с чем необходимо работать с Session и выполнят ьпрямые запросы что нельзя сделать
        /// через Интерцепторы
        /// </summary>
        public virtual void Delete(object id)
        {
            // Удаляем огромное количество записей относящиеся к версии
	        this.DeleteChildrenRecords((int)id);
            
            base.Delete(id);
        }


        /// <summary>
        /// Данный метод удаления перекрыл потому что перед кдалением надо удалить большие объекмы данных
        /// В связи с чем необходимо работать с Session и выполнят ьпрямые запросы что нельзя сделать
        /// через Интерцепторы
        /// </summary>
        public override IDataResult Delete(BaseParams baseParams)
        {
            var ids = Converter.ToIntArray(baseParams.Params, "records");

            foreach (var id in ids)
            {
                // Удаляем огромное количество записей относящиеся к версии
	            this.DeleteChildrenRecords(id);
            }

            return base.Delete(baseParams);
        }

        private void DeleteChildrenRecords(int programId)
        {
            //Поскольку много записей приходится удалять то делаю так
            var sessionProvider = this.Container.Resolve<ISessionProvider>();
            var session = sessionProvider.GetCurrentSession();

            try
            {
                using (var transaction = session.BeginTransaction())
                {
                    try
                    {
                        // удаляем Субсидирвоание по версии
                        session.CreateSQLQuery($@"delete from ovrhl_subsidy_rec_version where version_id = {programId}").ExecuteUpdate();

                        // удаляем записи краткосрочной программы
                        session.CreateSQLQuery(
                            $@"delete from ovrhl_short_prog_rec where stage2_id in ( 
	                                            select t.id from ovrhl_stage2_version t 
	                                            join ovrhl_version_rec st3 on st3.id = t.st3_version_id and st3.version_id = {programId} ) ").ExecuteUpdate();

                        // удаляем Корректировки по версии
                        session.CreateSQLQuery(
                            $@"delete from ovhl_dpkr_correct_st2 where st2_version_id in (
	                                            select st2.id from ovrhl_stage2_version st2
	                                            join ovrhl_version_rec st3 on st3.id = st2.st3_version_id and st3.version_id = {programId} )").ExecuteUpdate();

                        // удаляем связь с видами работ
                        session.CreateSQLQuery(
                            $@"delete from ovrhl_type_work_cr_st1 where st1_id in ( 
                                                select st1.id from ovrhl_stage1_version st1
                                                join ovrhl_stage2_version st2 on st2.id = st1.stage2_version_id
	                                            join ovrhl_version_rec st3 on st3.id = st2.st3_version_id and st3.version_id = {programId} )").ExecuteUpdate();

                        // удаляем связь с решениями собственников
                        session.CreateSQLQuery(string.Format(@"delete from ovrhl_change_year_owner_decision  where stage1_id  in ( 
                                                select st1.id from ovrhl_stage1_version st1
                                                join ovrhl_stage2_version st2 on st2.id = st1.stage2_version_id
	                                            join ovrhl_version_rec st3 on st3.id = st2.st3_version_id and st3.version_id = {0} )",
                                                            programId)).ExecuteUpdate();

                        // удаляем версию 1 этапа
                        session.CreateSQLQuery(
                            $@"delete from ovrhl_stage1_version where stage2_version_id in (
	                                            select st2.id from ovrhl_stage2_version st2
	                                            join ovrhl_version_rec st3 on st3.id = st2.st3_version_id and st3.version_id = {programId} ) ").ExecuteUpdate();

                        // удаляем запись опубликованной программы
                        session.CreateSQLQuery(
                            $@"delete from ovrhl_publish_prg_rec where publish_prg_id = (select id from ovrhl_publish_prg where version_id = {
                                programId
                            } )").ExecuteUpdate();

                        // удаляем версию 2 этапа
                        session.CreateSQLQuery(
                            $@"delete from ovrhl_stage2_version where st3_version_id in (select id from ovrhl_version_rec where version_id = {
                                programId
                            } ) ").ExecuteUpdate();

                        // удаляем версию 3 этапа
                        session.CreateSQLQuery($@"delete from ovrhl_version_rec where version_id = {programId}").ExecuteUpdate();

                        // удаляем Версию дифицитов МО
                        session.CreateSQLQuery($@"delete from ovrhl_short_prog_difitsit where version_id = {programId}").ExecuteUpdate();

                        // удаляем Версию дифицитов МО
                        session.CreateSQLQuery($@"delete from ovrhl_version_prm where version_id = {programId}").ExecuteUpdate();

                        // удаляем Опубликованные программы
                        session.CreateSQLQuery($@"delete from ovrhl_publish_prg where version_id = {programId}").ExecuteUpdate();

						// удаляем Логи актуализации
						session.CreateSQLQuery($@"delete from ovrhl_actualize_log where version_id = {programId}").ExecuteUpdate();
						
						transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
            finally 
            {
                sessionProvider.CloseCurrentSession();
            }
            
        }
    }
}
