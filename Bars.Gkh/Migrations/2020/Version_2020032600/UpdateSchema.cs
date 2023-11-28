namespace Bars.Gkh.Migrations._2020.Version_2020032600
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    
    [Migration("2020032600")]
    
    [MigrationDependsOn(typeof(Version_2020032100.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <inheritdoc/>
        public override void Up()
        {
            this.Database.AddColumn("CLW_LAWSUIT", new Column("IS_DENIED", DbType.Boolean, false));
            this.Database.AddColumn("CLW_LAWSUIT",new Column("DENIED_DATE", DbType.DateTime));
            this.Database.AddColumn("CLW_LAWSUIT",new Column("IS_DENIED_ADMISSION", DbType.Boolean,false));
            this.Database.AddColumn("CLW_LAWSUIT",new Column("DENIED_ADMISSION_DATE", DbType.DateTime));
            this.Database.AddColumn("CLW_LAWSUIT",new Column("IS_DIRECTED_BY_JURIDICTION", DbType.Boolean,false));
            this.Database.AddColumn("CLW_LAWSUIT",new Column("DIRECTED_BY_JURIDICTION_DATE", DbType.DateTime));
        }

        public override void Down()
        {
            this.Database.RemoveColumn("CLW_LAWSUIT", "IS_DENIED");
            this.Database.RemoveColumn("CLW_LAWSUIT", "DENIED_DATE");
            this.Database.RemoveColumn("CLW_LAWSUIT", "IS_DENIED_ADMISSION");
            this.Database.RemoveColumn("CLW_LAWSUIT", "DENIED_ADMISSION_DATE");
            this.Database.RemoveColumn("CLW_LAWSUIT", "IS_DIRECTED_BY_JURIDICTION");
            this.Database.RemoveColumn("CLW_LAWSUIT", "DIRECTED_BY_JURIDICTION_DATE");
        }
    }
}