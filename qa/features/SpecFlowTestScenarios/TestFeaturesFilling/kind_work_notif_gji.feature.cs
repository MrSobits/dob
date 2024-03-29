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
    [NUnit.Framework.DescriptionAttribute("тесткейсы для справочника \"Виды работ (уведомления)\"")]
    public partial class ТесткейсыДляСправочникаВидыРаботУведомленияFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "kind_work_notif_gji.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("ru-RU"), "тесткейсы для справочника \"Виды работ (уведомления)\"", "Справочники - Жилищно-коммунальное хозяйство - Виды работ (уведомления)", ProgrammingLanguage.CSharp, ((string[])(null)));
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
#line 7
testRunner.Given("пользователь добавляет новый вид работы (уведомление)", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 8
testRunner.And("пользователь у этого вида работы (уведомление) заполняет поле Наименование \"вид р" +
                    "аботы (уведомление) тест\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 9
testRunner.And("пользователь у этого вида работы (уведомление) заполняет поле Код \"тест\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line hidden
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("успешное добавление вида работы (уведомление)")]
        public virtual void УспешноеДобавлениеВидаРаботыУведомление()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("успешное добавление вида работы (уведомление)", ((string[])(null)));
#line 12
this.ScenarioSetup(scenarioInfo);
#line 6
this.FeatureBackground();
#line 13
testRunner.When("пользователь сохраняет этот вид работы (уведомление)", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 14
testRunner.Then("запись по этому виду работы (уведомление) присутствует в справочнике видов работы" +
                    " (уведомление)", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("успешное удаление записи из справочника видов работ (уведомление)")]
        public virtual void УспешноеУдалениеЗаписиИзСправочникаВидовРаботУведомление()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("успешное удаление записи из справочника видов работ (уведомление)", ((string[])(null)));
#line 16
this.ScenarioSetup(scenarioInfo);
#line 6
this.FeatureBackground();
#line 17
testRunner.When("пользователь сохраняет этот вид работы (уведомление)", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 18
testRunner.And("пользователь удаляет этот вид работы (уведомление)", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 19
testRunner.Then("запись по этому виду работы (уведомление) отсутствует в справочнике видов работы " +
                    "(уведомление)", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("успешное добавление дубля вида работы (уведомление)")]
        public virtual void УспешноеДобавлениеДубляВидаРаботыУведомление()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("успешное добавление дубля вида работы (уведомление)", ((string[])(null)));
#line 21
this.ScenarioSetup(scenarioInfo);
#line 6
this.FeatureBackground();
#line 22
testRunner.Given("пользователь сохраняет этот вид работы (уведомление)", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 23
testRunner.Given("пользователь добавляет новый вид работы (уведомление)", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 24
testRunner.And("пользователь у этого вида работы (уведомление) заполняет поле Наименование \"вид р" +
                    "аботы (уведомление) тест\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 25
testRunner.And("пользователь у этого вида работы (уведомление) заполняет поле Код \"тест\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 26
testRunner.When("пользователь сохраняет этот вид работы (уведомление)", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 27
testRunner.Then("запись по этому виду работы (уведомление) присутствует в справочнике видов работы" +
                    " (уведомление)", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
