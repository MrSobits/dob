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
    [NUnit.Framework.DescriptionAttribute("тесткейсы для раздела \"Приборы учета\"")]
    [NUnit.Framework.CategoryAttribute("ScenarioInTransaction")]
    public partial class ТесткейсыДляРазделаПриборыУчетаFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "metering_devices.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("ru-RU"), "тесткейсы для раздела \"Приборы учета\"", "Справочники - Жилищно-коммунальное хозяйство - Приборы учета", ProgrammingLanguage.CSharp, new string[] {
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
        
        public virtual void FeatureBackground()
        {
#line 5
#line 6
testRunner.Given("пользователь добавляет новый прибор учета", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 7
testRunner.And("пользователь у этого прибора учета заполняет поле Наименование \"прибор учета тест" +
                    "\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 8
testRunner.And("пользователь у этого прибора учета заполняет поле Класс точности \"тест\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 9
testRunner.And("пользователь у этого прибора учета заполняет поле Описание \"тест\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line hidden
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("успешное добавление прибора учета")]
        [NUnit.Framework.TestCaseAttribute("Индивидуальный", null)]
        [NUnit.Framework.TestCaseAttribute("Общедомовой", null)]
        public virtual void УспешноеДобавлениеПрибораУчета(string типУчета, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("успешное добавление прибора учета", exampleTags);
#line 12
this.ScenarioSetup(scenarioInfo);
#line 5
this.FeatureBackground();
#line 13
testRunner.Given(string.Format("пользователь у этого прибора учета заполняет поле Тип учета \"{0}\"", типУчета), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 14
testRunner.When("пользователь сохраняет этот прибор учета", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 15
testRunner.Then("запись по этому прибору учета присутствует в справочнике приборов учета", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("успешное удаление прибора учета из справочника приборов учета")]
        public virtual void УспешноеУдалениеПрибораУчетаИзСправочникаПриборовУчета()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("успешное удаление прибора учета из справочника приборов учета", ((string[])(null)));
#line 23
this.ScenarioSetup(scenarioInfo);
#line 5
this.FeatureBackground();
#line 24
testRunner.When("пользователь сохраняет этот прибор учета", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 25
testRunner.And("пользователь удаляет этот прибор учета", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 26
testRunner.Then("запись по этому прибору учета отсутствует в справочнике приборов учета", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
