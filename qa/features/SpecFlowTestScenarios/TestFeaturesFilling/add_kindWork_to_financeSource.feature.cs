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
    [NUnit.Framework.DescriptionAttribute("тесткейсы для добавления видов работ к разрезу финансирования в разделе \"Разрезы " +
        "финансирования\"")]
    public partial class ТесткейсыДляДобавленияВидовРаботКРазрезуФинансированияВРазделеРазрезыФинансированияFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "add_kindWork_to_financeSource.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("ru-RU"), "тесткейсы для добавления видов работ к разрезу финансирования в разделе \"Разрезы " +
                    "финансирования\"", "Справочники - Капитальный ремонт - Разрезы финансирования", ProgrammingLanguage.CSharp, ((string[])(null)));
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
#line hidden
            TechTalk.SpecFlow.Table table1 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name",
                        "TypeFinanceGroup",
                        "TypeFinance"});
            table1.AddRow(new string[] {
                        "разрез финансирования тестовый",
                        "ул. 50 лет Октября",
                        "Другие"});
#line 6
testRunner.Given("добавлен разрез финансирования", ((string)(null)), table1, "Дано ");
#line hidden
            TechTalk.SpecFlow.Table table2 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name",
                        "ShortName",
                        "Description"});
            table2.AddRow(new string[] {
                        "тестовая единица измерения",
                        "тест",
                        "тест"});
#line 10
testRunner.And("добавлена единица измерения", ((string)(null)), table2, "И ");
#line hidden
            TechTalk.SpecFlow.Table table3 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name",
                        "UnitMeasure",
                        "Code"});
            table3.AddRow(new string[] {
                        "тестовый вид работы1",
                        "тестовая единица измерения1",
                        "тест1"});
            table3.AddRow(new string[] {
                        "тестовый вид работы2",
                        "тестовая единица измерения2",
                        "тест2"});
#line 14
testRunner.And("добавлен вид работы", ((string)(null)), table3, "И ");
#line hidden
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("успешное добавление вида работ к разрезу финансирования")]
        public virtual void УспешноеДобавлениеВидаРаботКРазрезуФинансирования()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("успешное добавление вида работ к разрезу финансирования", ((string[])(null)));
#line 20
this.ScenarioSetup(scenarioInfo);
#line 5
this.FeatureBackground();
#line 21
testRunner.Given("пользователь к этому разрезу финансирования добавляет запись по виду работы тесто" +
                    "вый вид работы1", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 22
testRunner.And("пользователь к этому разрезу финансирования добавляет запись по виду работы тесто" +
                    "вый вид работы2", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 23
testRunner.When("пользователь сохраняет этот разрез финансирования", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 24
testRunner.Then("записи по этим видам работ присутствуют в этом разрезе финансирования", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("успешное удаление вида работ из разреза финансирования")]
        public virtual void УспешноеУдалениеВидаРаботИзРазрезаФинансирования()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("успешное удаление вида работ из разреза финансирования", ((string[])(null)));
#line 26
this.ScenarioSetup(scenarioInfo);
#line 5
this.FeatureBackground();
#line 27
testRunner.Given("пользователь к этому разрезу финансирования добавляет запись по виду работы тесто" +
                    "вый вид работы1", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 28
testRunner.And("пользователь к этому разрезу финансирования добавляет запись по виду работы тесто" +
                    "вый вид работы2", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 29
testRunner.When("пользователь сохраняет этот разрез финансирования", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 30
testRunner.And("пользователь удаляет эту запись по виду работы тестовый вид работы1", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 31
testRunner.And("пользователь удаляет эту запись по виду работы тестовый вид работы2", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 32
testRunner.Then("записи по этим видам работ отсутствуют в этом разрезе финансирования", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("неудачное удаление разреза финансирования")]
        public virtual void НеудачноеУдалениеРазрезаФинансирования()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("неудачное удаление разреза финансирования", ((string[])(null)));
#line 34
this.ScenarioSetup(scenarioInfo);
#line 5
this.FeatureBackground();
#line 35
testRunner.Given("пользователь к этому разрезу финансирования добавляет запись по виду работы тесто" +
                    "вый вид работы1", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 36
testRunner.And("пользователь к этому разрезу финансирования добавляет запись по виду работы тесто" +
                    "вый вид работы2", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 37
testRunner.When("пользователь сохраняет этот разрез финансирования", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 38
testRunner.And("пользователь удаляет этот разрез финансирования", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 39
testRunner.Then("запись по этому разрезу финансирования присутствует в разделе разрезов финансиров" +
                    "ания", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line 40
testRunner.And("падает ошибка с текстом \"Виды работ источников финансирования\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("неудачное добавление дубля вида работ к разрезу финансирования")]
        public virtual void НеудачноеДобавлениеДубляВидаРаботКРазрезуФинансирования()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("неудачное добавление дубля вида работ к разрезу финансирования", ((string[])(null)));
#line 42
this.ScenarioSetup(scenarioInfo);
#line 5
this.FeatureBackground();
#line 43
testRunner.Given("пользователь к этому разрезу финансирования добавляет запись по виду работы тесто" +
                    "вый вид работы1", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 44
testRunner.And("пользователь к этому разрезу финансирования добавляет запись по виду работы тесто" +
                    "вый вид работы2", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 45
testRunner.When("пользователь сохраняет этот разрез финансирования", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 46
testRunner.And("пользователь к этому разрезу финансирования добавляет запись по виду работы тесто" +
                    "вый вид работы1", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 47
testRunner.And("пользователь к этому разрезу финансирования добавляет запись по виду работы тесто" +
                    "вый вид работы2", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 48
testRunner.Then("дубли записей по этим видам работ отсутствуют в этом разрезе финансирования", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
