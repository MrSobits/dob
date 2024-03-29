﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (http://www.specflow.org/).
//      SpecFlow Version:1.9.0.77
//      SpecFlow Generator Version:1.9.0.0
//      Runtime Version:4.0.30319.42000
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
#pragma warning disable
namespace Bars.Gkh.Qa.TestScenarios
{
    using TechTalk.SpecFlow;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "1.9.0.77")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [NUnit.Framework.TestFixtureAttribute()]
    [NUnit.Framework.DescriptionAttribute("Закрытие лицевого счета")]
    [NUnit.Framework.CategoryAttribute("ScenarioInTransaction")]
    public partial class ЗакрытиеЛицевогоСчетаFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "close_account.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("ru-RU"), "Закрытие лицевого счета", "", ProgrammingLanguage.CSharp, new string[] {
                        "ScenarioInTransaction"});
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
        [NUnit.Framework.DescriptionAttribute("успешное закрытие лицевого счета с переходом в статус \"Закрыт\"")]
        public virtual void УспешноеЗакрытиеЛицевогоСчетаСПереходомВСтатусЗакрыт()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("успешное закрытие лицевого счета с переходом в статус \"Закрыт\"", ((string[])(null)));
#line 5
this.ScenarioSetup(scenarioInfo);
#line 6
testRunner.Given("пользователь в реестре ЛС выбирает лицевой счет \"010032049\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Допустим ");
#line 7
testRunner.When("пользователь для текщего ЛС вызывает операцию Закрытие", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 8
testRunner.Given("пользователь в закрытии ЛС заполняет поле Причина \"тест\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Допустим ");
#line 9
testRunner.And("пользователь в закрытии ЛС заполняет поле Документ-основание \"1.pdf\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 10
testRunner.And("пользователь в закрытии ЛС заполняет поле Дата закрытия \"текущая дата\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 11
testRunner.When("пользователь в закрытии ЛС сохраняет изменения", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 12
testRunner.Then("у этого лицевого счета в истории изменений присутствует запись с наименованием па" +
                    "раметра \"Доля собственности\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line 13
testRunner.And("у этой записи, в истории изменений ЛС, Описание измененного атрибута \"Изменение д" +
                    "оли собственности в связи с закрытием ЛС\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 14
testRunner.And("у этой записи, в истории изменений ЛС, Значение \"0\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 15
testRunner.And("у этой записи, в истории изменений ЛС, Дата начала действия значения \"текущая дат" +
                    "а\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 16
testRunner.And("у этой записи, в истории изменений ЛС, Дата установки значения \"текущая дата\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 17
testRunner.And("у этой записи, в истории изменений ЛС, Причина \"тест\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 18
testRunner.Then("у этого лицевого счета в истории изменений присутствует запись с наименованием па" +
                    "раметра \"Закрытие\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line 19
testRunner.And("у этой записи, в истории изменений ЛС, Описание измененного атрибута \"Для ЛС уста" +
                    "новлен статус \"Закрыт с долгом\"\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 20
testRunner.And("у этой записи, в истории изменений ЛС, Значение \"\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 21
testRunner.And("у этой записи, в истории изменений ЛС, Дата начала действия значения \"текущая дат" +
                    "а\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 22
testRunner.And("у этой записи, в истории изменений ЛС, Дата установки значения \"текущая дата\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 23
testRunner.And("у этой записи, в истории изменений ЛС, Причина \"тест\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 24
testRunner.And("у этого ЛС в карточке заполнено поле Доля собственности \"0\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 25
testRunner.And("у этого ЛС в карточке заполнено поле Статус \"Закрыт с долгом\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("неудачное закрытие ЛС для закрытого ЛС с долгом")]
        public virtual void НеудачноеЗакрытиеЛСДляЗакрытогоЛССДолгом()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("неудачное закрытие ЛС для закрытого ЛС с долгом", ((string[])(null)));
#line 30
this.ScenarioSetup(scenarioInfo);
#line 31
testRunner.Given("пользователь в реестре ЛС выбирает лицевой счет \"010125423\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Допустим ");
#line 32
testRunner.When("пользователь для текщего ЛС вызывает операцию Закрытие", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 33
testRunner.Given("пользователь в закрытии ЛС заполняет поле Причина \"тест\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Допустим ");
#line 34
testRunner.And("пользователь в закрытии ЛС заполняет поле Документ-основание \"1.pdf\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 35
testRunner.And("пользователь в закрытии ЛС заполняет поле Дата закрытия \"текущая дата\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 36
testRunner.When("пользователь в закрытии ЛС сохраняет изменения", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 37
testRunner.Then("падает ошибка с текстом \"Имеется долг. Счет уже закрыт с долгом!\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("неудачное закрытие ЛС для закрытого ЛС")]
        public virtual void НеудачноеЗакрытиеЛСДляЗакрытогоЛС()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("неудачное закрытие ЛС для закрытого ЛС", ((string[])(null)));
#line 39
this.ScenarioSetup(scenarioInfo);
#line 40
testRunner.Given("пользователь в реестре ЛС выбирает лицевой счет \"140132917\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Допустим ");
#line 41
testRunner.When("пользователь для текщего ЛС вызывает операцию Закрытие", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 42
testRunner.Given("пользователь в закрытии ЛС заполняет поле Причина \"тест\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Допустим ");
#line 43
testRunner.And("пользователь в закрытии ЛС заполняет поле Документ-основание \"1.pdf\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 44
testRunner.And("пользователь в закрытии ЛС заполняет поле Дата закрытия \"текущая дата\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 45
testRunner.When("пользователь в закрытии ЛС сохраняет изменения", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 46
testRunner.Then("падает ошибка с текстом \"Счет уже закрыт!\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
