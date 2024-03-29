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
    [NUnit.Framework.DescriptionAttribute("тесткейсы граничных значений для справочника \"Нормативные документы\"")]
    public partial class ТесткейсыГраничныхЗначенийДляСправочникаНормативныеДокументыFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "normative_docs_boundaries.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("ru-RU"), "тесткейсы граничных значений для справочника \"Нормативные документы\"", "Справочники - Общие - Нормативные документы", ProgrammingLanguage.CSharp, ((string[])(null)));
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
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("неудачное добавление нормативно-правового документа при незаполненных обязательны" +
            "х полях")]
        public virtual void НеудачноеДобавлениеНормативно_ПравовогоДокументаПриНезаполненныхОбязательныхПолях()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("неудачное добавление нормативно-правового документа при незаполненных обязательны" +
                    "х полях", ((string[])(null)));
#line 6
this.ScenarioSetup(scenarioInfo);
#line 7
testRunner.Given("пользователь добавляет новый нормативно-правовой документ", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 8
testRunner.When("пользователь сохраняет этот нормативно-правовой документ", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 9
testRunner.Then("запись по этому нормативно-правовому документу не сохраняется и падает ошибка с т" +
                    "екстом \"Не заполнены обязательные поля: Полное наименование Наименование Код\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("удачное добавление нормативно-правового документа при вводе граничных условий в 1" +
            "000 знаков, Полное наименование")]
        public virtual void УдачноеДобавлениеНормативно_ПравовогоДокументаПриВводеГраничныхУсловийВ1000ЗнаковПолноеНаименование()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("удачное добавление нормативно-правового документа при вводе граничных условий в 1" +
                    "000 знаков, Полное наименование", ((string[])(null)));
#line 11
this.ScenarioSetup(scenarioInfo);
#line 12
testRunner.Given("пользователь добавляет новый нормативно-правовой документ", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 13
testRunner.And("пользователь у этого нормативно-правового документа заполняет поле Полное наимено" +
                    "вание 1000 символов \"1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 14
testRunner.And("пользователь у этого нормативно-правового документа заполняет поле Наименование \"" +
                    "тест\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 15
testRunner.And("пользователь у этого нормативно-правового документа заполняет поле Код \"тест\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 16
testRunner.When("пользователь сохраняет этот нормативно-правовой документ", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 17
testRunner.Then("запись по этому нормативно-правовому документу присутствует в справочнике нормати" +
                    "вно-правовых документов", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("неудачное добавление нормативно-правового документа при вводе граничных условий в" +
            " 1001 знаков, Полное наименование")]
        public virtual void НеудачноеДобавлениеНормативно_ПравовогоДокументаПриВводеГраничныхУсловийВ1001ЗнаковПолноеНаименование()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("неудачное добавление нормативно-правового документа при вводе граничных условий в" +
                    " 1001 знаков, Полное наименование", ((string[])(null)));
#line 19
this.ScenarioSetup(scenarioInfo);
#line 20
testRunner.Given("пользователь добавляет новый нормативно-правовой документ", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 21
testRunner.And("пользователь у этого нормативно-правового документа заполняет поле Полное наимено" +
                    "вание 1001 символов \"1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 22
testRunner.And("пользователь у этого нормативно-правового документа заполняет поле Наименование \"" +
                    "тест\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 23
testRunner.And("пользователь у этого нормативно-правового документа заполняет поле Код \"тест\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 24
testRunner.When("пользователь сохраняет этот нормативно-правовой документ", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 25
testRunner.Then("запись по этому нормативно-правовому документу не сохраняется и падает ошибка с т" +
                    "екстом \"Не заполнены обязательные поля: Полное наименование\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("удачное добавление нормативно-правового документа при вводе граничных условий в 3" +
            "00 знаков, Наименование")]
        public virtual void УдачноеДобавлениеНормативно_ПравовогоДокументаПриВводеГраничныхУсловийВ300ЗнаковНаименование()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("удачное добавление нормативно-правового документа при вводе граничных условий в 3" +
                    "00 знаков, Наименование", ((string[])(null)));
#line 27
this.ScenarioSetup(scenarioInfo);
#line 28
testRunner.Given("пользователь добавляет новый нормативно-правовой документ", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 29
testRunner.And("пользователь у этого нормативно-правового документа заполняет поле Полное наимено" +
                    "вание \"тест\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 30
testRunner.And("пользователь у этого нормативно-правового документа заполняет поле Наименование 3" +
                    "00 символов \"1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 31
testRunner.And("пользователь у этого нормативно-правового документа заполняет поле Код \"тест\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 32
testRunner.When("пользователь сохраняет этот нормативно-правовой документ", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 33
testRunner.Then("запись по этому нормативно-правовому документу присутствует в справочнике нормати" +
                    "вно-правовых документов", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("удачное добавление нормативно-правового документа при вводе граничных условий в 3" +
            "01 знаков, Наименование")]
        public virtual void УдачноеДобавлениеНормативно_ПравовогоДокументаПриВводеГраничныхУсловийВ301ЗнаковНаименование()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("удачное добавление нормативно-правового документа при вводе граничных условий в 3" +
                    "01 знаков, Наименование", ((string[])(null)));
#line 35
this.ScenarioSetup(scenarioInfo);
#line 36
testRunner.Given("пользователь добавляет новый нормативно-правовой документ", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 37
testRunner.And("пользователь у этого нормативно-правового документа заполняет поле Полное наимено" +
                    "вание \"тест\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 38
testRunner.And("пользователь у этого нормативно-правового документа заполняет поле Наименование 3" +
                    "01 символов \"1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 39
testRunner.And("пользователь у этого нормативно-правового документа заполняет поле Код \"тест\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 40
testRunner.When("пользователь сохраняет этот нормативно-правовой документ", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 41
testRunner.Then("запись по этому нормативно-правовому документу не сохраняется и падает ошибка с т" +
                    "екстом \"Не заполнены обязательные поля: Наименование\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("неуспешное удаление нормативно-правового документа из справочника нормативно-прав" +
            "овых документов")]
        public virtual void НеуспешноеУдалениеНормативно_ПравовогоДокументаИзСправочникаНормативно_ПравовыхДокументов()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("неуспешное удаление нормативно-правового документа из справочника нормативно-прав" +
                    "овых документов", ((string[])(null)));
#line 43
this.ScenarioSetup(scenarioInfo);
#line 44
testRunner.When("пользователь сохраняет этот нормативно-правовой документ", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 45
testRunner.And("пользователь у этого нормативно-правового документа заполняет поле Номер \"тест\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 46
testRunner.And("пользователь у этого нормативно-правового документа заполняет поле Текст \"тест\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 47
testRunner.And("пользователь сохраняет этот пункт нормативно-правового документа", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 48
testRunner.And("пользователь удаляет этот нормативно-правовой документ", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 49
testRunner.Then("выводится сообщение об ошибке с текстом \"Данный нормативно - правовой дкоумент со" +
                    "держит пункты \"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line 50
testRunner.And("запись по этому пункту нормативно-правового документа присутствует в этом нормати" +
                    "вно-правововом документе", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
