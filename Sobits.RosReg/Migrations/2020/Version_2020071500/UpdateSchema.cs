﻿namespace Sobits.RosReg.Migrations._2020.Version_2020071500
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using Sobits.RosReg.Entities;
    using Sobits.RosReg.Map;

    [Migration("2020071500")]
    [MigrationDependsOn(typeof(_2020.Version_2020052700.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        private readonly SchemaQualifiedObjectName egrnRightLegalTable = new SchemaQualifiedObjectName
            { Name = ExtractEgrnRightLegalMap.TableName, Schema = ExtractEgrnRightLegalMap.SchemaName };

        private readonly SchemaQualifiedObjectName egrnRightLegalResidentTable = new SchemaQualifiedObjectName
            { Name = ExtractEgrnRightLegalResidentMap.TableName, Schema = ExtractEgrnMap.SchemaName };

        private readonly SchemaQualifiedObjectName egrnRightLegalNotResidentTable = new SchemaQualifiedObjectName
            { Name = ExtractEgrnRightLegalNotResidentMap.TableName, Schema = ExtractEgrnMap.SchemaName };

        private readonly SchemaQualifiedObjectName egrnRightTable = new SchemaQualifiedObjectName
            { Name = ExtractEgrnRightMap.TableName, Schema = ExtractEgrnMap.SchemaName };

        public override void Up()
        {
            this.Database.ExecuteNonQuery(@"
             alter table rosreg.extractegrnrightlegal drop constraint if exists fk_extractegrnrightlegal_extractegrnright_id;
             alter table rosreg.extractegrnrightlegalresident drop constraint if exists fk_extractegrnrightlegalresident_extractegrnrightlegal_id;
             alter table rosreg.extractegrnrightlegalnotresident drop constraint if exists fk_extractegrnrightlegalnotresident_extractegrnrightlegal_id;");
            
            this.Database.AddTable(this.egrnRightLegalTable,
                new Column("ID", DbType.Int64, 22, ColumnProperty.PrimaryKeyWithIdentity),
                new Column(nameof(ExtractEgrnRightLegal.OwnerType).ToLower(), DbType.Int16),
                new Column(nameof(ExtractEgrnRightLegal.IncorporationForm).ToLower(), DbType.String),
                new Column(nameof(ExtractEgrnRightLegal.Name).ToLower(), DbType.String),
                new Column(nameof(ExtractEgrnRightLegal.Inn).ToLower(), DbType.String),
                new Column(nameof(ExtractEgrnRightLegal.Email).ToLower(), DbType.String),
                new Column(nameof(ExtractEgrnRightLegal.MailingAddress).ToLower(), DbType.String),
                new Column(nameof(ExtractEgrnRightLegal.RightId).ToLower(), DbType.Int64)
            );
            this.Database.AddTable(this.egrnRightLegalResidentTable,
                new Column("ID", DbType.Int64, 22, ColumnProperty.Unique),
                new Column(nameof(ExtractEgrnRightLegalResident.Ogrn).ToLower(), DbType.String)
            );

            this.Database.AddTable(this.egrnRightLegalNotResidentTable,
                new Column("ID", DbType.Int64, 22, ColumnProperty.Unique),
                new Column(nameof(ExtractEgrnRightLegalNotResident.IncorporationCountry).ToLower(), DbType.String),
                new Column(nameof(ExtractEgrnRightLegalNotResident.RegistrationNumber).ToLower(), DbType.String),
                new Column(nameof(ExtractEgrnRightLegalNotResident.DateStateReg).ToLower(), DbType.Date),
                new Column(nameof(ExtractEgrnRightLegalNotResident.RegistrationOrgan).ToLower(), DbType.String),
                new Column(nameof(ExtractEgrnRightLegalNotResident.RegAddresSubject).ToLower(), DbType.String)
            );
            
            this.Database.ExecuteNonQuery(@"             
                alter table rosreg.extractegrnrightlegal
                add constraint fk_extractegrnrightlegal_extractegrnright_id foreign key (rightid) references rosreg.extractegrnright (id) on delete cascade;
                
                alter table rosreg.extractegrnrightlegalresident
                add constraint fk_extractegrnrightlegalresident_extractegrnrightlegal_id foreign key (id) references rosreg.extractegrnrightlegal (id) on delete cascade;
                    
                alter table rosreg.extractegrnrightlegalnotresident
                add constraint fk_extractegrnrightlegalnotresident_extractegrnrightlegal_id foreign key (id) references rosreg.extractegrnrightlegal (id) on delete cascade;"
            );
        }



        public override void Down()
        {
            this.Database.ExecuteNonQuery(@"
             alter table rosreg.extractegrnrightlegal drop constraint if exists fk_extractegrnrightlegal_extractegrnright_id;
             alter table rosreg.extractegrnrightlegalresident drop constraint if exists fk_extractegrnrightlegalresident_extractegrnrightlegal_id;
             alter table rosreg.extractegrnrightlegalnotresident drop constraint if exists fk_extractegrnrightlegalnotresident_extractegrnrightlegal_id;");

            this.Database.RemoveTable(this.egrnRightLegalNotResidentTable);
            this.Database.RemoveTable(this.egrnRightLegalResidentTable);
            this.Database.RemoveTable(this.egrnRightLegalTable);
            
        }
    }
}