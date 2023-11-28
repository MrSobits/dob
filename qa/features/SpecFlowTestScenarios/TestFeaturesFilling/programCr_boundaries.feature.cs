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
    [NUnit.Framework.DescriptionAttribute("тесткейсы граничных значений для раздела \"Программы капитального ремонта\"")]
    public partial class ТесткейсыГраничныхЗначенийДляРазделаПрограммыКапитальногоРемонтаFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "programCr_boundaries.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("ru-RU"), "тесткейсы граничных значений для раздела \"Программы капитального ремонта\"", "Капитальный ремонт - Программы капитального ремонта - Программы капитального ремо" +
                    "нта", ProgrammingLanguage.CSharp, ((string[])(null)));
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
#line 6
#line hidden
            TechTalk.SpecFlow.Table table1 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name",
                        "DateStart"});
            table1.AddRow(new string[] {
                        "период программы тестовый",
                        "01.01.2015"});
#line 7
testRunner.Given("добавлен период программ", ((string)(null)), table1, "Дано ");
#line hidden
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("неудачное добавление программы капитального ремонта при незаполненных обязательны" +
            "х полях")]
        public virtual void НеудачноеДобавлениеПрограммыКапитальногоРемонтаПриНезаполненныхОбязательныхПолях()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("неудачное добавление программы капитального ремонта при незаполненных обязательны" +
                    "х полях", ((string[])(null)));
#line 12
this.ScenarioSetup(scenarioInfo);
#line 6
this.FeatureBackground();
#line 13
testRunner.Given("пользователь добавляет новую программу капитального ремонта", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 14
testRunner.When("пользователь сохраняет эту программу капитального ремонта", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 15
testRunner.Then("запись по этой программе капитального ремонта отсутствует в разделе программ капи" +
                    "тального ремонта", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line 16
testRunner.And("падает ошибка с текстом \"Не заполнены обязательные поля: Наименование Период\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("успешное добавление программы капитального ремонта при вводе граничных условий в " +
            "300 знаков, Наименование")]
        public virtual void УспешноеДобавлениеПрограммыКапитальногоРемонтаПриВводеГраничныхУсловийВ300ЗнаковНаименование()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("успешное добавление программы капитального ремонта при вводе граничных условий в " +
                    "300 знаков, Наименование", ((string[])(null)));
#line 18
this.ScenarioSetup(scenarioInfo);
#line 6
this.FeatureBackground();
#line 19
testRunner.Given("пользователь добавляет новую программу капитального ремонта", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 20
testRunner.And("пользователь у этой программы капитального ремонта заполняет поле Наименование 30" +
                    "0 символов \"1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 21
testRunner.And("пользователь у этой программы капитального ремонта заполняет поле Период", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 22
testRunner.When("пользователь сохраняет эту программу капитального ремонта", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 23
testRunner.Then("запись по этой программе капитального ремонта присутствует в разделе программ кап" +
                    "итального ремонта", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("неудачное добавление программы капитального ремонта при вводе граничных условий в" +
            " 301 знаков, Наименование")]
        public virtual void НеудачноеДобавлениеПрограммыКапитальногоРемонтаПриВводеГраничныхУсловийВ301ЗнаковНаименование()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("неудачное добавление программы капитального ремонта при вводе граничных условий в" +
                    " 301 знаков, Наименование", ((string[])(null)));
#line 25
this.ScenarioSetup(scenarioInfo);
#line 6
this.FeatureBackground();
#line 26
testRunner.Given("пользователь добавляет новую программу капитального ремонта", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 27
testRunner.And("пользователь у этой программы капитального ремонта заполняет поле Наименование 30" +
                    "1 символов \"1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 28
testRunner.And("пользователь у этой программы капитального ремонта заполняет поле Период", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 29
testRunner.When("пользователь сохраняет эту программу капитального ремонта", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 30
testRunner.Then("запись по этой программе капитального ремонта отсутствует в разделе программ капи" +
                    "тального ремонта", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line 31
testRunner.And("падает ошибка с текстом \"Не заполнены обязательные поля: Наименование\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("успешное добавление программы капитального ремонта при вводе граничных условий в " +
            "200 знаков, Код")]
        public virtual void УспешноеДобавлениеПрограммыКапитальногоРемонтаПриВводеГраничныхУсловийВ200ЗнаковКод()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("успешное добавление программы капитального ремонта при вводе граничных условий в " +
                    "200 знаков, Код", ((string[])(null)));
#line 33
this.ScenarioSetup(scenarioInfo);
#line 6
this.FeatureBackground();
#line 34
testRunner.Given("пользователь добавляет новую программу капитального ремонта", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 35
testRunner.And("пользователь у этой программы капитального ремонта заполняет поле Наименование \"п" +
                    "рограмма капитального ремонта тестовая\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 36
testRunner.And("пользователь у этой программы капитального ремонта заполняет поле Код 200 символо" +
                    "в \"1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 37
testRunner.And("пользователь у этой программы капитального ремонта заполняет поле Период", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 38
testRunner.When("пользователь сохраняет эту программу капитального ремонта", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 39
testRunner.Then("запись по этой программе капитального ремонта присутствует в разделе программ кап" +
                    "итального ремонта", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("неудачное добавление программы капитального ремонта при вводе граничных условий в" +
            " 201 знаков, Код")]
        public virtual void НеудачноеДобавлениеПрограммыКапитальногоРемонтаПриВводеГраничныхУсловийВ201ЗнаковКод()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("неудачное добавление программы капитального ремонта при вводе граничных условий в" +
                    " 201 знаков, Код", ((string[])(null)));
#line 41
this.ScenarioSetup(scenarioInfo);
#line 6
this.FeatureBackground();
#line 42
testRunner.Given("пользователь добавляет новую программу капитального ремонта", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 43
testRunner.And("пользователь у этой программы капитального ремонта заполняет поле Наименование \"п" +
                    "рограмма капитального ремонта тестовая\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 44
testRunner.And("пользователь у этой программы капитального ремонта заполняет поле Код 201 символо" +
                    "в \"1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 45
testRunner.And("пользователь у этой программы капитального ремонта заполняет поле Период", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 46
testRunner.When("пользователь сохраняет эту программу капитального ремонта", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 47
testRunner.Then("запись по этой программе капитального ремонта отсутствует в разделе программ капи" +
                    "тального ремонта", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line 48
testRunner.And("падает ошибка с текстом \"Не заполнены обязательные поля: Код\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("успешное добавление программы капитального ремонта при вводе граничных условий в " +
            "2000 знаков, Примечание")]
        public virtual void УспешноеДобавлениеПрограммыКапитальногоРемонтаПриВводеГраничныхУсловийВ2000ЗнаковПримечание()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("успешное добавление программы капитального ремонта при вводе граничных условий в " +
                    "2000 знаков, Примечание", ((string[])(null)));
#line 50
this.ScenarioSetup(scenarioInfo);
#line 6
this.FeatureBackground();
#line 51
testRunner.Given("пользователь добавляет новую программу капитального ремонта", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 52
testRunner.And("пользователь у этой программы капитального ремонта заполняет поле Наименование \"п" +
                    "рограмма капитального ремонта тестовая\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 53
testRunner.And("пользователь у этой программы капитального ремонта заполняет поле Примечание 2000" +
                    " символов \"1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 54
testRunner.And("пользователь у этой программы капитального ремонта заполняет поле Период", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 55
testRunner.When("пользователь сохраняет эту программу капитального ремонта", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 56
testRunner.Then("запись по этой программе капитального ремонта присутствует в разделе программ кап" +
                    "итального ремонта", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("неудачное добавление программы капитального ремонта при вводе граничных условий в" +
            " 2001 знаков, Примечание")]
        public virtual void НеудачноеДобавлениеПрограммыКапитальногоРемонтаПриВводеГраничныхУсловийВ2001ЗнаковПримечание()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("неудачное добавление программы капитального ремонта при вводе граничных условий в" +
                    " 2001 знаков, Примечание", ((string[])(null)));
#line 58
this.ScenarioSetup(scenarioInfo);
#line 6
this.FeatureBackground();
#line 59
testRunner.Given("пользователь добавляет новую программу капитального ремонта", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 60
testRunner.And("пользователь у этой программы капитального ремонта заполняет поле Наименование \"п" +
                    "рограмма капитального ремонта тестовая\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 61
testRunner.And("пользователь у этой программы капитального ремонта заполняет поле Примечание 2001" +
                    " символов \"1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 62
testRunner.And("пользователь у этой программы капитального ремонта заполняет поле Период", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 63
testRunner.When("пользователь сохраняет эту программу капитального ремонта", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 64
testRunner.Then("запись по этой программе капитального ремонта отсутствует в разделе программ капи" +
                    "тального ремонта", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line 65
testRunner.And("падает ошибка с текстом \"Не заполнены обязательные поля: Примечание\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
