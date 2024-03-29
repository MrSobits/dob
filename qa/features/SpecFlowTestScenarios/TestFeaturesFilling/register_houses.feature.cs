﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (http://www.specflow.org/).
//      SpecFlow Version:1.9.3.0
//      SpecFlow Generator Version:1.9.0.0
//      Runtime Version:4.0.30319.18444
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
#pragma warning disable
namespace TestFeaturesFilling
{
    using TechTalk.SpecFlow;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "1.9.3.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [NUnit.Framework.TestFixtureAttribute()]
    [NUnit.Framework.DescriptionAttribute("реестр жилых домов")]
    public partial class РеестрЖилыхДомовFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "register_houses.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("ru-RU"), "реестр жилых домов", "", ProgrammingLanguage.CSharp, ((string[])(null)));
            testRunner.OnFeatureStart(featureInfo);
        }
        
        [NUnit.Framework.TestFixtureTearDownAttribute()]
        public virtual void FeatureTearDown()
        {
            testRunner.OnFeatureEnd();
            testRunner = null;
        }
        
        [NUnit.Framework.SetUpAttribute()]
        public virtual void TestInitialize()
        {
        }
        
        [NUnit.Framework.TearDownAttribute()]
        public virtual void ScenarioTearDown()
        {
            testRunner.OnScenarioEnd();
        }
        
        public virtual void ScenarioSetup(TechTalk.SpecFlow.ScenarioInfo scenarioInfo)
        {
            testRunner.OnScenarioStart(scenarioInfo);
        }
        
        public virtual void ScenarioCleanup()
        {
            testRunner.CollectScenarioErrors();
        }
        
        public virtual void FeatureBackground()
        {
#line 4
#line 5
testRunner.Given("пользователь добавляет новый дом в Реестр жилых домов", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line hidden
            TechTalk.SpecFlow.Table table1 = new TechTalk.SpecFlow.Table(new string[] {
                        "region",
                        "place"});
            table1.AddRow(new string[] {
                        "kamchatka",
                        "Камчатский край, Алеутский р-н, с. Никольское"});
#line 6
testRunner.And("у этого дома устанавливает поле Населённый пункт", ((string)(null)), table1, "И ");
#line hidden
            TechTalk.SpecFlow.Table table2 = new TechTalk.SpecFlow.Table(new string[] {
                        "region",
                        "street"});
            table2.AddRow(new string[] {
                        "kamchatka",
                        "ул. 50 лет Октября"});
#line 10
testRunner.And("у этого дома устанавливает поле Улица", ((string)(null)), table2, "И ");
#line hidden
            TechTalk.SpecFlow.Table table3 = new TechTalk.SpecFlow.Table(new string[] {
                        "region",
                        "houseNumber"});
            table3.AddRow(new string[] {
                        "kamchatka",
                        "75"});
#line 14
testRunner.And("у этого дома устанавливает поле Номер Дома", ((string)(null)), table3, "И ");
#line hidden
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("создание жилого дома")]
        public virtual void СозданиеЖилогоДома()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("создание жилого дома", ((string[])(null)));
#line 18
this.ScenarioSetup(scenarioInfo);
#line 4
this.FeatureBackground();
#line 19
testRunner.When("пользователь сохраняет этот жилой дом", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 20
testRunner.Then("запись по этому дому присутствует в реестр жилых домов", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("удаление жилого дома")]
        public virtual void УдалениеЖилогоДома()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("удаление жилого дома", ((string[])(null)));
#line 22
this.ScenarioSetup(scenarioInfo);
#line 4
this.FeatureBackground();
#line 23
testRunner.When("пользователь сохраняет этот жилой дом", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 24
testRunner.And("пользователь удаляет этот жилой дом", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 25
testRunner.Then("запись по этому дому отсутствует в реестре жилых домов", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("добавление помещения к жилому дому")]
        public virtual void ДобавлениеПомещенияКЖиломуДому()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("добавление помещения к жилому дому", ((string[])(null)));
#line 27
this.ScenarioSetup(scenarioInfo);
#line 4
this.FeatureBackground();
#line 28
testRunner.When("пользователь сохраняет этот жилой дом", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 29
testRunner.Given("пользователь добавляет к этому дому помещение", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 30
testRunner.And("пользователь у этого помещения заполняет поле № квартиры/помещения \"1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 31
testRunner.And("пользователь у этого помещения заполняет поле Общая площадь \"51\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 32
testRunner.And("пользователь у этого помещения заполняет поле Жилая площадь \"35\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 33
testRunner.And("пользователь у этого помещения заполняет поле Тип помещения \"Жилое\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 34
testRunner.And("пользователь у этого помещения заполняет поле Тип собственности \"Частная\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 35
testRunner.When("пользователь сохраняет это помещение у этого дома", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 36
testRunner.Then("запись по этому помещению присутствует в списке помещений дома", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("добавление к дому протокола решений собственников жилых помещений и привязка к сч" +
            "ету регопа")]
        [NUnit.Framework.TestCaseAttribute("1", "01.01.2015", "01.04.2015", "Счет регионального оператора", null)]
        public virtual void ДобавлениеКДомуПротоколаРешенийСобственниковЖилыхПомещенийИПривязкаКСчетуРегопа(string номер, string датаПротокола, string датаВступленияВСилу, string способФормированияФонда, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("добавление к дому протокола решений собственников жилых помещений и привязка к сч" +
                    "ету регопа", exampleTags);
#line 38
this.ScenarioSetup(scenarioInfo);
#line 4
this.FeatureBackground();
#line 39
testRunner.Given("есть дом с адресом \"с. Никольское, ул. 50 лет Октября, д. 10\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 40
testRunner.When("к этому дому пользователь прикрепляет протокол решений типа Протокол решения собс" +
                    "твенников жилых помещений", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 41
testRunner.And(string.Format("заполняет у протокола решений(Протокол решения собственников жилых помещений) пол" +
                        "е Номер \"{0}\"", номер), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 42
testRunner.And(string.Format("заполняет у протокола решений(Протокол решения собственников жилых помещений) пол" +
                        "е Дата протокола \"{0}\"", датаПротокола), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 43
testRunner.And(string.Format("заполняет у протокола решений(Протокол решения собственников жилых помещений) пол" +
                        "е Дата вступления в силу \"{0}\"", датаВступленияВСилу), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 44
testRunner.And(string.Format("заполняет у протокола решений(Протокол решения собственников жилых помещений) пол" +
                        "е способ формирования фонда \"{0}\"", способФормированияФонда), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 45
testRunner.And("сохраняет протокол решений(Протокол решения собственников жилых помещений)", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 46
testRunner.And("формирует у протокола решений(Протокол решения собственников жилых помещений) уве" +
                    "домление с номером = \"1\" и номером счета = \"112001\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 47
testRunner.And("переводит протокол решений(Протокол решения собственников жилых помещений) в стат" +
                    "ус = \"Утверждено\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 48
testRunner.And("прикрепляет этот дом к счету регопа", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 49
testRunner.Then("этот дом прикрепился к счёту регопа", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("добавление к дому протокола решений собственников жилых помещений и привязка к сп" +
            "ец счету")]
        [NUnit.Framework.TestCaseAttribute("1", "01.01.2015", "01.04.2015", "ОАО \"Россельхозбанк\"", "Специальный счет", "Региональный оператор", null)]
        public virtual void ДобавлениеКДомуПротоколаРешенийСобственниковЖилыхПомещенийИПривязкаКСпецСчету(string номер, string датаПротокола, string датаВступленияВСилу, string кредитнаяОрганизация, string способФормированияФонда, string владелецСпециальногоСчета, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("добавление к дому протокола решений собственников жилых помещений и привязка к сп" +
                    "ец счету", exampleTags);
#line 55
this.ScenarioSetup(scenarioInfo);
#line 4
this.FeatureBackground();
#line 56
testRunner.Given("есть дом с адресом \"с. Никольское, ул. 50 лет Октября, д. 10\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 57
testRunner.When("к этому дому пользователь прикрепляет протокол решений типа Протокол решения собс" +
                    "твенников жилых помещений", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 58
testRunner.And(string.Format("заполняет у протокола решений(Протокол решения собственников жилых помещений) пол" +
                        "е Номер \"{0}\"", номер), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 59
testRunner.And(string.Format("заполняет у протокола решений(Протокол решения собственников жилых помещений) пол" +
                        "е Дата протокола \"{0}\"", датаПротокола), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 60
testRunner.And(string.Format("заполняет у протокола решений(Протокол решения собственников жилых помещений) пол" +
                        "е Дата вступления в силу \"{0}\"", датаВступленияВСилу), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 61
testRunner.And(string.Format("заполняет у протокола решений(Протокол решения собственников жилых помещений) пол" +
                        "е способ формирования фонда \"{0}\"", способФормированияФонда), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 62
testRunner.And(string.Format("заполняет у протокола решений(Протокол решения собственников жилых помещений) пол" +
                        "е Владелец специального счета \"{0}\"", владелецСпециальногоСчета), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 63
testRunner.And(string.Format("заполняет у протокола решений(Протокол решения собственников жилых помещений) пол" +
                        "е Кредитная организация \"{0}\"", кредитнаяОрганизация), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 64
testRunner.And("сохраняет протокол решений(Протокол решения собственников жилых помещений)", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 65
testRunner.And("формирует у протокола решений(Протокол решения собственников жилых помещений) уве" +
                    "домление с номером = \"2\" и номером счета = \"113\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 66
testRunner.And("переводит протокол решений(Протокол решения собственников жилых помещений) в стат" +
                    "ус = \"Утверждено\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 67
testRunner.Then("этот дом появится в специальных счетах регопа", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("добавление к дому протокола решения органа государственной власти и привязка к сч" +
            "ету регопа")]
        [NUnit.Framework.TestCaseAttribute("1", "01.01.2015", "01.04.2015", null)]
        public virtual void ДобавлениеКДомуПротоколаРешенияОрганаГосударственнойВластиИПривязкаКСчетуРегопа(string номер, string датаПротокола, string датаВступленияВСилу, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("добавление к дому протокола решения органа государственной власти и привязка к сч" +
                    "ету регопа", exampleTags);
#line 73
this.ScenarioSetup(scenarioInfo);
#line 4
this.FeatureBackground();
#line 74
testRunner.Given("есть дом с адресом \"с. Никольское, ул. 50 лет Октября, д. 10\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 75
testRunner.When("к этому дому пользователь прикрепляет протокол решений типа Протокол решения орга" +
                    "на государственной власти", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 76
testRunner.And(string.Format("заполняет у протокола решений(Протокол решения органа государственной власти) пол" +
                        "е Номер \"{0}\"", номер), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 77
testRunner.And(string.Format("заполняет у протокола решений(Протокол решения органа государственной власти) пол" +
                        "е Дата протокола \"{0}\"", датаПротокола), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 78
testRunner.And(string.Format("заполняет у протокола решений(Протокол решения органа государственной власти) пол" +
                        "е Дата вступления в силу \"{0}\"", датаВступленияВСилу), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 79
testRunner.And("ставит отметку Способ формирования фонда на счету регионального оператора у прото" +
                    "кола решений(Протокол решения органа государственной власти)", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 80
testRunner.And("сохраняет протокол решений(Протокол решения органа государственной власти)", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 81
testRunner.And("переводит протокол решений(Протокол решения органа государственной власти) в стат" +
                    "ус = \"Утверждено\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 82
testRunner.And("прикрепляет этот дом к счету регопа", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 83
testRunner.Then("этот дом прикрепился к счёту регопа", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("добавление к дому протокола решения органа государственной власти и ошибка привяз" +
            "ки к счету регопа")]
        [NUnit.Framework.TestCaseAttribute("1", "01.01.2015", "01.01.2017", null)]
        public virtual void ДобавлениеКДомуПротоколаРешенияОрганаГосударственнойВластиИОшибкаПривязкиКСчетуРегопа(string номер, string датаПротокола, string датаВступленияВСилу, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("добавление к дому протокола решения органа государственной власти и ошибка привяз" +
                    "ки к счету регопа", exampleTags);
#line 89
this.ScenarioSetup(scenarioInfo);
#line 4
this.FeatureBackground();
#line 90
testRunner.Given("есть дом с адресом \"с. Никольское, ул. 50 лет Октября, д. 10\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 91
testRunner.When("к этому дому пользователь прикрепляет протокол решений типа Протокол решения орга" +
                    "на государственной власти", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 92
testRunner.And(string.Format("заполняет у протокола решений(Протокол решения органа государственной власти) пол" +
                        "е Номер \"{0}\"", номер), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 93
testRunner.And(string.Format("заполняет у протокола решений(Протокол решения органа государственной власти) пол" +
                        "е Дата протокола \"{0}\"", датаПротокола), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 94
testRunner.And(string.Format("заполняет у протокола решений(Протокол решения органа государственной власти) пол" +
                        "е Дата вступления в силу \"{0}\"", датаВступленияВСилу), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 95
testRunner.And("сохраняет протокол решений(Протокол решения органа государственной власти)", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 96
testRunner.And("переводит протокол решений(Протокол решения органа государственной власти) в стат" +
                    "ус = \"Утверждено\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 97
testRunner.And("прикрепляет этот дом к счету регопа", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 98
testRunner.Then("этот дом не прикрепился к счёту регопа", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
