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
    [NUnit.Framework.DescriptionAttribute("тесткейсы для справочника \"Единицы измерения\"")]
    public partial class ТесткейсыДляСправочникаЕдиницыИзмеренияFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "UnitMeasure.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("ru-RU"), "тесткейсы для справочника \"Единицы измерения\"", "Справочники - Общие - Единицы измерения", ProgrammingLanguage.CSharp, ((string[])(null)));
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
testRunner.Given("пользователь добавляет новую единицу измерения", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 8
testRunner.And("пользователь у этой единицы измерения заполняет поле Наименование \"тест\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 9
testRunner.And("пользователь у этой единицы измерения заполняет поле Краткое наименование \"тест\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 10
testRunner.And("пользователь у этой единицы измерения заполняет поле Описание \"тест\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line hidden
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("успешное добавление единицы измерения")]
        public virtual void УспешноеДобавлениеЕдиницыИзмерения()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("успешное добавление единицы измерения", ((string[])(null)));
#line 12
this.ScenarioSetup(scenarioInfo);
#line 6
this.FeatureBackground();
#line 13
testRunner.When("пользователь сохраняет эту единицы измерения", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 14
testRunner.Then("запись по этой единице измерения присутствует в справочнике единиц измерения", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("успешное удаление записи из справочника единиц измерения")]
        public virtual void УспешноеУдалениеЗаписиИзСправочникаЕдиницИзмерения()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("успешное удаление записи из справочника единиц измерения", ((string[])(null)));
#line 16
this.ScenarioSetup(scenarioInfo);
#line 6
this.FeatureBackground();
#line 17
testRunner.When("пользователь сохраняет эту единицы измерения", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 18
testRunner.And("пользователь удаляет эту единицу измерения", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 19
testRunner.Then("запись по этой единице измерения отсутствует в справочнике единиц измерения", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
