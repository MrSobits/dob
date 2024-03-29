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
    [NUnit.Framework.DescriptionAttribute("Доработки к Реестру документов ГЖИ")]
    public partial class ДоработкиКРееструДокументовГЖИFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "register_gji_docs.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("ru-RU"), "Доработки к Реестру документов ГЖИ", "", ProgrammingLanguage.CSharp, ((string[])(null)));
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
        [NUnit.Framework.DescriptionAttribute("установка срока устранения больше 6ти месяцев")]
        [NUnit.Framework.TestCaseAttribute("Акты проверок", "19.02.2015", "19.09.2015", null)]
        [NUnit.Framework.TestCaseAttribute("Предписания", "19.02.2015", "19.09.2015", null)]
        public virtual void УстановкаСрокаУстраненияБольше6ТиМесяцев(string типДокумента, string текущаяДата, string срокУстранения, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("установка срока устранения больше 6ти месяцев", exampleTags);
#line 5
this.ScenarioSetup(scenarioInfo);
#line 6
testRunner.When(string.Format("пользователь выбирает Тип документа \"{0}\"", типДокумента), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 7
testRunner.And(string.Format("в поле \"{0}\" с текущей даты \"{1}\" меняет дату на новую", срокУстранения, текущаяДата), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 8
testRunner.And("сохраняет данные", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 9
testRunner.Then(string.Format("в результатах проверки успешно меняется значение в поле \"{0}\" на новое", срокУстранения), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("установка максимального срока устранения больше 12ти месяцев")]
        [NUnit.Framework.TestCaseAttribute("Акты проверок", "19.02.2015", "20.02.2016", null)]
        [NUnit.Framework.TestCaseAttribute("Предписания", "19.02.2015", "20.02.2016", null)]
        public virtual void УстановкаМаксимальногоСрокаУстраненияБольше12ТиМесяцев(string типДокумента, string текущаяДата, string срокУстранения, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("установка максимального срока устранения больше 12ти месяцев", exampleTags);
#line 18
this.ScenarioSetup(scenarioInfo);
#line 19
testRunner.When("пользователь выбирает Тип документа \"Акт проверок\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 20
testRunner.And(string.Format("в поле \"{0}\" с текущей даты \"{1}\" меняет дату на новую", срокУстранения, текущаяДата), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 21
testRunner.And("сохраняет данные", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 22
testRunner.Then(string.Format("выходит ошибка и не меняется значение в поле \"{0}\"", срокУстранения), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
