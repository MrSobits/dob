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
namespace SpecFlowTestScenarios
{
    using TechTalk.SpecFlow;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "1.9.3.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [NUnit.Framework.TestFixtureAttribute()]
    [NUnit.Framework.DescriptionAttribute("тесткейсы uhдля раздела \"Кредитные организации\"")]
    public partial class ТесткейсыUhдляРазделаКредитныеОрганизацииFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "credit_org_boundaries.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("ru-RU"), "тесткейсы uhдля раздела \"Кредитные организации\"", "Участники процесса - Контрагенты - Кредитные организации", ProgrammingLanguage.CSharp, ((string[])(null)));
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
#line 5
#line 6
testRunner.Given("пользователь добавляет новую кредитную организацию", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line hidden
            TechTalk.SpecFlow.Table table1 = new TechTalk.SpecFlow.Table(new string[] {
                        "region",
                        "city",
                        "street",
                        "houseNumber"});
            table1.AddRow(new string[] {
                        "testregion",
                        "Камчатский край, Алеутский р-н, с. Никольское",
                        "ул. 50 лет Октября",
                        "test999"});
#line 7
testRunner.Given("пользователь выбирает Адрес", ((string)(null)), table1, "Дано ");
#line hidden
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("неудачное добавление кредитной организации с незаполненными обязательными полями," +
            " Наименование")]
        public virtual void НеудачноеДобавлениеКредитнойОрганизацииСНезаполненнымиОбязательнымиПолямиНаименование()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("неудачное добавление кредитной организации с незаполненными обязательными полями," +
                    " Наименование", ((string[])(null)));
#line 12
this.ScenarioSetup(scenarioInfo);
#line 5
this.FeatureBackground();
#line 13
testRunner.Given("пользователь у этой кредитной организации заполняет поле Адрес в пределах субъект" +
                    "а", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 14
testRunner.And("пользователь у этой кредитной организации заполняет поле ИНН \"6501236431\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 15
testRunner.When("пользователь сохраняет эту кредитную организацию", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 16
testRunner.Then("запись по этой кредитной организации отсутствует в списке", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line 17
testRunner.And("падает ошибка с текстом \"Не заполнены обязательные поля, Наименование\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("неудачное добавление кредитной организации с незаполненными обязательными полями," +
            " Адрес")]
        public virtual void НеудачноеДобавлениеКредитнойОрганизацииСНезаполненнымиОбязательнымиПолямиАдрес()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("неудачное добавление кредитной организации с незаполненными обязательными полями," +
                    " Адрес", ((string[])(null)));
#line 20
this.ScenarioSetup(scenarioInfo);
#line 5
this.FeatureBackground();
#line 21
testRunner.And("пользователь у этой кредитной организации заполняет поле Наименование \"кредитная " +
                    "организация тест\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 22
testRunner.And("пользователь у этой кредитной организации заполняет поле ИНН \"6501236431\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 23
testRunner.When("пользователь сохраняет эту кредитную организацию", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 24
testRunner.Then("запись по этой кредитной организации отсутствует в списке", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line 25
testRunner.And("падает ошибка с текстом \"Не заполнены обязательные поля, Адрес\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("неудачное добавление кредитной организации с незаполненными обязательными полями," +
            " Инн")]
        public virtual void НеудачноеДобавлениеКредитнойОрганизацииСНезаполненнымиОбязательнымиПолямиИнн()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("неудачное добавление кредитной организации с незаполненными обязательными полями," +
                    " Инн", ((string[])(null)));
#line 28
this.ScenarioSetup(scenarioInfo);
#line 5
this.FeatureBackground();
#line 29
testRunner.Given("пользователь у этой кредитной организации заполняет поле Адрес в пределах субъект" +
                    "а", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 30
testRunner.And("пользователь у этой кредитной организации заполняет поле Наименование \"кредитная " +
                    "организация тест\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 31
testRunner.When("пользователь сохраняет эту кредитную организацию", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 32
testRunner.Then("запись по этой кредитной организации отсутствует в списке", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line 33
testRunner.And("падает ошибка с текстом \"Не заполнены обязательные поля, Инн\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("удачное добавление кредитной организации при вводе граничных условий в 300 знаков" +
            ", Наименование")]
        public virtual void УдачноеДобавлениеКредитнойОрганизацииПриВводеГраничныхУсловийВ300ЗнаковНаименование()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("удачное добавление кредитной организации при вводе граничных условий в 300 знаков" +
                    ", Наименование", ((string[])(null)));
#line 35
this.ScenarioSetup(scenarioInfo);
#line 5
this.FeatureBackground();
#line 36
testRunner.Given("пользователь у этой кредитной организации заполняет поле Адрес в пределах субъект" +
                    "а", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 37
testRunner.And("пользователь у этой кредитной организации заполняет поле Наименование 300 знаков " +
                    "\'1\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 38
testRunner.And("пользователь у этой кредитной организации заполняет поле ИНН \"6501236431\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 39
testRunner.When("пользователь сохраняет эту кредитную организацию", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 40
testRunner.Then("запись по этой кредитной организации присутствует в списке", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("неудачное добавление кредитной организации при вводе граничных условий в 301 знак" +
            "ов, Наименование")]
        public virtual void НеудачноеДобавлениеКредитнойОрганизацииПриВводеГраничныхУсловийВ301ЗнаковНаименование()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("неудачное добавление кредитной организации при вводе граничных условий в 301 знак" +
                    "ов, Наименование", ((string[])(null)));
#line 43
this.ScenarioSetup(scenarioInfo);
#line 5
this.FeatureBackground();
#line 44
testRunner.Given("пользователь у этой кредитной организации заполняет поле Адрес в пределах субъект" +
                    "а", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 45
testRunner.And("пользователь у этой кредитной организации заполняет поле Наименование 301 знаков " +
                    "\'1\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 46
testRunner.And("пользователь у этой кредитной организации заполняет поле ИНН \"6501236431\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 47
testRunner.When("пользователь сохраняет эту кредитную организацию", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 48
testRunner.Then("запись по этой кредитной организации отсутствует в списке", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line 49
testRunner.And("падает ошибка с текстом \"Количество знаков в поле Наименование не должно превышат" +
                    "ь 300 символов\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("неудачное добавление кредитной организации при вводе некорректных значений в поле" +
            ", Инн")]
        public virtual void НеудачноеДобавлениеКредитнойОрганизацииПриВводеНекорректныхЗначенийВПолеИнн()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("неудачное добавление кредитной организации при вводе некорректных значений в поле" +
                    ", Инн", ((string[])(null)));
#line 52
this.ScenarioSetup(scenarioInfo);
#line 5
this.FeatureBackground();
#line 53
testRunner.Given("пользователь у этой кредитной организации заполняет поле Адрес в пределах субъект" +
                    "а", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 54
testRunner.And("пользователь у этой кредитной организации заполняет поле Наименование \"ntncncnc\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 55
testRunner.And("пользователь у этой кредитной организации заполняет поле ИНН \"1234121224\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 56
testRunner.When("пользователь сохраняет эту кредитную организацию", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 57
testRunner.Then("запись по этой кредитной организации отсутствует в списке", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line 58
testRunner.And("падает ошибка с текстом \"Введен некорректный ИНН\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("удачное добавление кредитной организации при вводе граничных условий в 9 знаков, " +
            "БИК")]
        public virtual void УдачноеДобавлениеКредитнойОрганизацииПриВводеГраничныхУсловийВ9ЗнаковБИК()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("удачное добавление кредитной организации при вводе граничных условий в 9 знаков, " +
                    "БИК", ((string[])(null)));
#line 60
this.ScenarioSetup(scenarioInfo);
#line 5
this.FeatureBackground();
#line 61
testRunner.Given("пользователь у этой кредитной организации заполняет поле Адрес в пределах субъект" +
                    "а", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 62
testRunner.And("пользователь у этой кредитной организации заполняет поле Наименование \"ntncncnc\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 63
testRunner.And("пользователь у этой кредитной организации заполняет поле ИНН \"6501236431\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 64
testRunner.And("пользователь у этой кредитной организации заполняет поле БИК 9 знаков \'1\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 65
testRunner.When("пользователь сохраняет эту кредитную организацию", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 66
testRunner.Then("запись по этой кредитной организации присутствует в списке", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("неудачное добавление кредитной организации при вводе граничных условий в 8 знаков" +
            ", БИК")]
        public virtual void НеудачноеДобавлениеКредитнойОрганизацииПриВводеГраничныхУсловийВ8ЗнаковБИК()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("неудачное добавление кредитной организации при вводе граничных условий в 8 знаков" +
                    ", БИК", ((string[])(null)));
#line 69
this.ScenarioSetup(scenarioInfo);
#line 5
this.FeatureBackground();
#line 70
testRunner.Given("пользователь у этой кредитной организации заполняет поле Адрес в пределах субъект" +
                    "а", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 71
testRunner.And("пользователь у этой кредитной организации заполняет поле Наименование \"ntncncnc\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 72
testRunner.And("пользователь у этой кредитной организации заполняет поле ИНН \"6501236431\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 73
testRunner.And("пользователь у этой кредитной организации заполняет поле БИК 8 знаков \'1\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 74
testRunner.When("пользователь сохраняет эту кредитную организацию", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 75
testRunner.Then("запись по этой кредитной организации отсутствует в списке", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line 76
testRunner.And("падает ошибка с текстом \"Введен некорректный БИК\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("неудачное добавление кредитной организации при вводе граничных условий в 10 знако" +
            "в, БИК")]
        public virtual void НеудачноеДобавлениеКредитнойОрганизацииПриВводеГраничныхУсловийВ10ЗнаковБИК()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("неудачное добавление кредитной организации при вводе граничных условий в 10 знако" +
                    "в, БИК", ((string[])(null)));
#line 79
this.ScenarioSetup(scenarioInfo);
#line 5
this.FeatureBackground();
#line 80
testRunner.Given("пользователь у этой кредитной организации заполняет поле Адрес в пределах субъект" +
                    "а", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 81
testRunner.And("пользователь у этой кредитной организации заполняет поле Наименование \"ntncncnc\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 82
testRunner.And("пользователь у этой кредитной организации заполняет поле ИНН \"6501236431\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 83
testRunner.And("пользователь у этой кредитной организации заполняет поле БИК 10 знаков \'1\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 84
testRunner.When("пользователь сохраняет эту кредитную организацию", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 85
testRunner.Then("запись по этой кредитной организации отсутствует в списке", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line 86
testRunner.And("падает ошибка с текстом \"Введен некорректный БИК\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
