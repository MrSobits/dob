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
    [NUnit.Framework.DescriptionAttribute("справочник \"ООИ\"")]
    public partial class СправочникООИFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "directory_ooi.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("ru-RU"), "справочник \"ООИ\"", "", ProgrammingLanguage.CSharp, ((string[])(null)));
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
        [NUnit.Framework.DescriptionAttribute("успешное сохранение значения в поле \"Предельный срок эксплуатации\"")]
        [NUnit.Framework.TestCaseAttribute("30", null)]
        [NUnit.Framework.TestCaseAttribute("", null)]
        public virtual void УспешноеСохранениеЗначенияВПолеПредельныйСрокЭксплуатации(string значение, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("успешное сохранение значения в поле \"Предельный срок эксплуатации\"", exampleTags);
#line 5
this.ScenarioSetup(scenarioInfo);
#line 6
testRunner.When(string.Format("пользователь в справочнике ООИ по КЭ заполняет поле \"Предельный срок эксплуатации" +
                        "\" значением \"{0}\" и сохраняет КЭ", значение), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 7
testRunner.Then("по запись успешно сохраняется", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("запрет сохранения не целого значения в поле \"Предельный срок эксплуатации\"")]
        public virtual void ЗапретСохраненияНеЦелогоЗначенияВПолеПредельныйСрокЭксплуатации()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("запрет сохранения не целого значения в поле \"Предельный срок эксплуатации\"", ((string[])(null)));
#line 15
this.ScenarioSetup(scenarioInfo);
#line 16
testRunner.When("пользователь в справочнике ООИ по КЭ заполняет поле \"Предельный срок эксплуатации" +
                    "\" значением \"30,1\" и сохраняет КЭ", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 17
testRunner.Then("запись не сохраняется и падает ошибка", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "То ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
