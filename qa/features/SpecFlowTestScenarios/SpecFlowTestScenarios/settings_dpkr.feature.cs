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
    [NUnit.Framework.DescriptionAttribute("доработки раздела \"Настройка параметров ДПКР\"")]
    public partial class ДоработкиРазделаНастройкаПараметровДПКРFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "settings_dpkr.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("ru-RU"), "доработки раздела \"Настройка параметров ДПКР\"", "", ProgrammingLanguage.CSharp, ((string[])(null)));
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
        [NUnit.Framework.DescriptionAttribute("актуализация ДПКР в случает отсутствия Учета краткосрочной программы")]
        public virtual void АктуализацияДПКРВСлучаетОтсутствияУчетаКраткосрочнойПрограммы()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("актуализация ДПКР в случает отсутствия Учета краткосрочной программы", ((string[])(null)));
#line 5
this.ScenarioSetup(scenarioInfo);
#line 6
testRunner.When("пользователь актуализирует программу", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 7
testRunner.And("в настройках ДПКР в поле \"Учет краткосрочной программы\" указано значение \"Не испо" +
                    "льзуется\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 8
testRunner.Then("не проверяется наличие краткосрочной программы", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("актуализация ДПКР в случает начилия Учета краткосрочной программы")]
        [NUnit.Framework.TestCaseAttribute("Добавить новые записи", null)]
        [NUnit.Framework.TestCaseAttribute("Актуализировать стоимость", null)]
        [NUnit.Framework.TestCaseAttribute("Актуализировать год", null)]
        [NUnit.Framework.TestCaseAttribute("Удалить лишние записи", null)]
        [NUnit.Framework.TestCaseAttribute("Группировка ООИ", null)]
        [NUnit.Framework.TestCaseAttribute("Рассчитать очередность", null)]
        [NUnit.Framework.TestCaseAttribute("Актуализировать из КПКР", null)]
        public virtual void АктуализацияДПКРВСлучаетНачилияУчетаКраткосрочнойПрограммы(string действие, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("актуализация ДПКР в случает начилия Учета краткосрочной программы", exampleTags);
#line 11
this.ScenarioSetup(scenarioInfo);
#line 12
testRunner.When("в настройках ДПКР в поле \"Учет краткосрочной программы\" указано значение \"Использ" +
                    "уется\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 13
testRunner.And(string.Format("пользователь в версии ДПКР актуализирует ДПКР по действию \"{0}\"", действие), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 14
testRunner.Then("в актуализации не учитываются версии с видимостью \"Видимость\" = \"Скрытая\" и состо" +
                    "янием \"Состояние\" = \"Закрыта\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("проверка отказа в проведении актуализации")]
        [NUnit.Framework.TestCaseAttribute("Добавить новые записи", null)]
        [NUnit.Framework.TestCaseAttribute("Актуализировать стоимость", null)]
        [NUnit.Framework.TestCaseAttribute("Актуализировать год", null)]
        [NUnit.Framework.TestCaseAttribute("Удалить лишние записи", null)]
        [NUnit.Framework.TestCaseAttribute("Группировка ООИ", null)]
        [NUnit.Framework.TestCaseAttribute("Рассчитать очередность", null)]
        [NUnit.Framework.TestCaseAttribute("Актуализировать из КПКР", null)]
        public virtual void ПроверкаОтказаВПроведенииАктуализации(string действие, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("проверка отказа в проведении актуализации", exampleTags);
#line 27
this.ScenarioSetup(scenarioInfo);
#line 28
testRunner.When("в настройках ДПКР в поле \"Учет краткосрочной программы\" указано значение \"Использ" +
                    "уется\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 29
testRunner.And(string.Format("пользователь в версии ДПКР актуализирует ДПКР по действию \"{0}\"", действие), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 30
testRunner.And("в период актуализации \"период\" входит период \"период\" программы капитального ремо" +
                    "нта", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 31
testRunner.Then("действие по актуализации не выполняется и падает ошибка", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
