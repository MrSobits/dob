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
    [NUnit.Framework.DescriptionAttribute("тесткейсы для раздела \"Расчетно-кассовые центры\"")]
    public partial class ТесткейсыДляРазделаРасчетно_КассовыеЦентрыFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "cash_payment_center.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("ru-RU"), "тесткейсы для раздела \"Расчетно-кассовые центры\"", "", ProgrammingLanguage.CSharp, ((string[])(null)));
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
#line 4
#line 5
testRunner.Given("пользователь добавляет новый ркц", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line hidden
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("успешное добавление ркц")]
        public virtual void УспешноеДобавлениеРкц()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("успешное добавление ркц", ((string[])(null)));
#line 7
this.ScenarioSetup(scenarioInfo);
#line 4
this.FeatureBackground();
#line 8
testRunner.Given("пользователь у этого ркц заполняет поле Контрагент \"Тестовый конрагент\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 9
testRunner.And("пользователь у этого ркц заполняет поле Идентификатор РКЦ \"1001\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 10
testRunner.When("пользователь сохраняет этот ркц", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 11
testRunner.Then("запись по этому ркц присутствует в разделе расчетно-кассовых центров", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("успешное удаление ркц")]
        public virtual void УспешноеУдалениеРкц()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("успешное удаление ркц", ((string[])(null)));
#line 13
this.ScenarioSetup(scenarioInfo);
#line 4
this.FeatureBackground();
#line 14
testRunner.Given("пользователь у этого ркц заполняет поле Контрагент \"Тестовый конрагент\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 15
testRunner.And("пользователь у этого ркц заполняет поле Идентификатор РКЦ \"1001\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 16
testRunner.When("пользователь сохраняет этот ркц", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 17
testRunner.And("пользователь удаляет этот ркц", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 18
testRunner.Then("запись по этому ркц отсутствует в разделе расчетно-кассовых центров", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("успешное добавление муниципального образования к ркц")]
        public virtual void УспешноеДобавлениеМуниципальногоОбразованияКРкц()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("успешное добавление муниципального образования к ркц", ((string[])(null)));
#line 20
this.ScenarioSetup(scenarioInfo);
#line 4
this.FeatureBackground();
#line 21
testRunner.Given("пользователь у этого ркц заполняет поле Контрагент \"Тестовый конрагент\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 22
testRunner.And("пользователь у этого ркц заполняет поле Идентификатор РКЦ \"1001\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 23
testRunner.When("пользователь сохраняет этот ркц", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 24
testRunner.And("пользователь добавляет муниципальное образование \"Алеутский муниципальный район\" " +
                    "к этому ркц", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 25
testRunner.Then("записи по этому муниципальному образованию присутствуют в списке муниципальных об" +
                    "разований этого ркц", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("успешное удаление муниципального образования с ркц")]
        public virtual void УспешноеУдалениеМуниципальногоОбразованияСРкц()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("успешное удаление муниципального образования с ркц", ((string[])(null)));
#line 27
this.ScenarioSetup(scenarioInfo);
#line 4
this.FeatureBackground();
#line 28
testRunner.Given("пользователь у этого ркц заполняет поле Контрагент \"Тестовый конрагент\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 29
testRunner.And("пользователь у этого ркц заполняет поле Идентификатор РКЦ \"1001\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 30
testRunner.When("пользователь сохраняет этот ркц", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 31
testRunner.And("пользователь добавляет муниципальное образование \"Алеутский муниципальный район\" " +
                    "к этому ркц", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 32
testRunner.And("пользователь удаляет это муниципальное образование с этого ркц", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 33
testRunner.Then("записи по этому муниципальному образованию отсутствуют в списке муниципальных обр" +
                    "азований этого ркц", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("проверка на уникальность контрагента")]
        public virtual void ПроверкаНаУникальностьКонтрагента()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("проверка на уникальность контрагента", ((string[])(null)));
#line 35
this.ScenarioSetup(scenarioInfo);
#line 4
this.FeatureBackground();
#line 36
testRunner.Given("пользователь у этого ркц заполняет поле Контрагент \"Тестовый конрагент\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 37
testRunner.And("пользователь у этого ркц заполняет поле Идентификатор РКЦ \"1001\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 38
testRunner.And("пользователь сохраняет этот ркц", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 39
testRunner.And("пользователь у этого ркц заполняет поле Контрагент \"Тестовый конрагент\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 40
testRunner.And("пользователь у этого ркц заполняет поле Идентификатор РКЦ \"10011\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 41
testRunner.When("пользователь сохраняет этот ркц", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 42
testRunner.Then("запись по этому ркц отсутствует в разделе расчетно-кассовых центров", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line 43
testRunner.And("выходит сообщение с текстом \"Для указанного контрагента уже существует расчетно-к" +
                    "ассовый центр.\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("проверка на уникальность идентификатора РКЦ")]
        public virtual void ПроверкаНаУникальностьИдентификатораРКЦ()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("проверка на уникальность идентификатора РКЦ", ((string[])(null)));
#line 45
this.ScenarioSetup(scenarioInfo);
#line 4
this.FeatureBackground();
#line 46
testRunner.Given("пользователь у этого ркц заполняет поле Контрагент \"Тестовый конрагент\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 47
testRunner.And("пользователь у этого ркц заполняет поле Идентификатор РКЦ \"1001\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 48
testRunner.And("пользователь сохраняет этот ркц", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 49
testRunner.And("пользователь у этого ркц заполняет поле Контрагент \"тестовый контрагент_0\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 50
testRunner.And("пользователь у этого ркц заполняет поле Идентификатор РКЦ \"1001\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 51
testRunner.When("пользователь сохраняет этот ркц", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 52
testRunner.Then("запись по этому ркц отсутствует в разделе расчетно-кассовых центров", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line 53
testRunner.And("выходит сообщение с текстом \"Указанный идентификатор уже существует. Необходимо у" +
                    "казать уникальный идентификатор.\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("неудачная привязка лс к ркц при пересечении дат договоров")]
        public virtual void НеудачнаяПривязкаЛсКРкцПриПересеченииДатДоговоров()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("неудачная привязка лс к ркц при пересечении дат договоров", ((string[])(null)));
#line 55
this.ScenarioSetup(scenarioInfo);
#line 4
this.FeatureBackground();
#line 56
testRunner.Given("пользователь у этого ркц заполняет поле Контрагент \"Тестовый конрагент\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 57
testRunner.And("пользователь у этого ркц заполняет поле Идентификатор РКЦ \"1001\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 58
testRunner.And("пользователь сохраняет этот ркц", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 59
testRunner.And("пользователь у этого ркц заполняет поле Контрагент \"тестовый контрагент_0\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 60
testRunner.And("пользователь у этого ркц заполняет поле Идентификатор РКЦ \"10011\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 61
testRunner.And("пользователь сохраняет этот ркц", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 62
testRunner.And("пользователь у ркц с конрагентом \"Тестовый конрагент\" в объектах ркц добавляет но" +
                    "вый объект", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 63
testRunner.And("пользователь у ркц с конрагентом \"Тестовый конрагент\" у этого объекта заполняет п" +
                    "оле Дата начала действия договора \"текущая дата\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 64
testRunner.And("пользователь у ркц с конрагентом \"Тестовый конрагент\" у этого объекта добавляет л" +
                    "ицевой счет \"050132813\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 65
testRunner.And("пользователь у ркц с конрагентом \"Тестовый конрагент\" сохраняет этот объект", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 66
testRunner.And("пользователь у ркц с конрагентом \"тестовый контрагент_0\" в объектах ркц добавляет" +
                    " новый объект", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 67
testRunner.And("пользователь у ркц с конрагентом \"тестовый контрагент_0\" у этого объекта заполняе" +
                    "т поле Дата начала действия договора \"текущая дата\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 68
testRunner.And("пользователь у ркц с конрагентом \"тестовый контрагент_0\" у этого объекта добавляе" +
                    "т лицевой счет \"050132813\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 69
testRunner.And("пользователь у ркц с конрагентом \"тестовый контрагент_0\" сохраняет этот объект", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 70
testRunner.Then("запись по этому ркц отсутствует в разделе расчетно-кассовых центров", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line 71
testRunner.And("выходит сообщение с текстом \"Некоторые счета имеют действующий договор. Для добав" +
                    "ления нового договора необходимо закрыть прошлый.\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("успешная привязка лс к ркц при непересечении дат договоров")]
        public virtual void УспешнаяПривязкаЛсКРкцПриНепересеченииДатДоговоров()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("успешная привязка лс к ркц при непересечении дат договоров", ((string[])(null)));
#line 73
this.ScenarioSetup(scenarioInfo);
#line 4
this.FeatureBackground();
#line 74
testRunner.Given("пользователь у этого ркц заполняет поле Контрагент \"Тестовый конрагент\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 75
testRunner.And("пользователь у этого ркц заполняет поле Идентификатор РКЦ \"1001\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 76
testRunner.And("пользователь сохраняет этот ркц", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 77
testRunner.And("пользователь у этого ркц заполняет поле Контрагент \"тестовый контрагент_0\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 78
testRunner.And("пользователь у этого ркц заполняет поле Идентификатор РКЦ \"10011\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 79
testRunner.And("пользователь сохраняет этот ркц", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 80
testRunner.And("пользователь у ркц с конрагентом \"Тестовый конрагент\" в объектах ркц добавляет но" +
                    "вый объект", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 81
testRunner.And("пользователь у ркц с конрагентом \"Тестовый конрагент\" у этого объекта заполняет п" +
                    "оле Дата начала действия договора \"11.01.9999\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 82
testRunner.And("пользователь у ркц с конрагентом \"Тестовый конрагент\" у этого объекта заполняет п" +
                    "оле Дата окончания действия договора \"13.01.9999\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 83
testRunner.And("пользователь у ркц с конрагентом \"Тестовый конрагент\" у этого объекта добавляет л" +
                    "ицевой счет \"050132813\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 84
testRunner.And("пользователь у ркц с конрагентом \"Тестовый конрагент\" сохраняет этот объект", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 85
testRunner.And("пользователь у ркц с конрагентом \"тестовый контрагент_0\" в объектах ркц добавляет" +
                    " новый объект", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 86
testRunner.And("пользователь у ркц с конрагентом \"тестовый контрагент_0\" у этого объекта заполняе" +
                    "т поле Дата начала действия договора \"14.01.9999\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 87
testRunner.And("пользователь у ркц с конрагентом \"тестовый контрагент_0\" у этого объекта добавляе" +
                    "т лицевой счет \"050132813\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 88
testRunner.And("пользователь у ркц с конрагентом \"тестовый контрагент_0\" сохраняет этот объект", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 89
testRunner.Then("запись по этому ркц присутствует в разделе расчетно-кассовых центров", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line 90
testRunner.And("выходит сообщение с текстом \"Лицевые счета сохранены успешно\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
