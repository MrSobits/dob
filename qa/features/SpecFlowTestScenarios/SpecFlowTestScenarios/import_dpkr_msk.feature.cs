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
    [NUnit.Framework.DescriptionAttribute("тестирование Импорта ДПКР (Москва)")]
    public partial class ТестированиеИмпортаДПКРМоскваFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "import_dpkr_msk.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("ru-RU"), "тестирование Импорта ДПКР (Москва)", "", ProgrammingLanguage.CSharp, ((string[])(null)));
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
testRunner.Given("Пользователь логин \"admin\", пароль \"admin\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 7
testRunner.And("тестируемая система \"http://gkh-test.bars-open.ru/dev-msk\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line hidden
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("проверка формата импортируемого файла на наличие обязательных аттрибутов")]
        [NUnit.Framework.TestCaseAttribute("ID", null)]
        [NUnit.Framework.TestCaseAttribute("UID", null)]
        [NUnit.Framework.TestCaseAttribute("СЕРИЯ", null)]
        [NUnit.Framework.TestCaseAttribute("MKDB02", null)]
        [NUnit.Framework.TestCaseAttribute("MKDC01", null)]
        [NUnit.Framework.TestCaseAttribute("MKDC02", null)]
        [NUnit.Framework.TestCaseAttribute("MKDC03", null)]
        [NUnit.Framework.TestCaseAttribute("MKDC06", null)]
        [NUnit.Framework.TestCaseAttribute("MKDC07", null)]
        [NUnit.Framework.TestCaseAttribute("MKDC08", null)]
        public virtual void ПроверкаФорматаИмпортируемогоФайлаНаНаличиеОбязательныхАттрибутов(string аттрибут, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("проверка формата импортируемого файла на наличие обязательных аттрибутов", exampleTags);
#line 10
this.ScenarioSetup(scenarioInfo);
#line 5
this.FeatureBackground();
#line 11
testRunner.Given("файл для импорта данных", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 12
testRunner.When("пользователь импортирует файл", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 13
testRunner.Then(string.Format("система проверяет файл на соответствие формату по наличию обязательных аттрибутов" +
                        " в файле {0}", аттрибут), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "То ");
#line 14
testRunner.When(string.Format("в файле не указано значение хотя бы по одному из аттрибутов {0}", аттрибут), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Если ");
#line 15
testRunner.Then("файл не грузится и в лог записывается ошибка и причина ошибки загрузки файла, кол" +
                    "ичество загруженных записей, количество незагруженных записей, прочая информация" +
                    "", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "То ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("проверка формата импортируемого файла на наличие других аттрибутов")]
        [NUnit.Framework.TestCaseAttribute("МС (ЭС)", null)]
        [NUnit.Framework.TestCaseAttribute("МС (ХВС-М)", null)]
        [NUnit.Framework.TestCaseAttribute("МС (ХВС)", null)]
        [NUnit.Framework.TestCaseAttribute("МС (Ф-Б)", null)]
        [NUnit.Framework.TestCaseAttribute("МС (Ф)", null)]
        [NUnit.Framework.TestCaseAttribute("МС (СТРОП)", null)]
        [NUnit.Framework.TestCaseAttribute("МС (ППАиДУ)", null)]
        [NUnit.Framework.TestCaseAttribute("МС (ПОДВАЛ)", null)]
        [NUnit.Framework.TestCaseAttribute("МС (ПВ)", null)]
        [NUnit.Framework.TestCaseAttribute("МС (ОС-М)", null)]
        [NUnit.Framework.TestCaseAttribute("МС (ОС)", null)]
        [NUnit.Framework.TestCaseAttribute("МС (МУС)", null)]
        [NUnit.Framework.TestCaseAttribute("МС (КРОВЛЯ)", null)]
        [NUnit.Framework.TestCaseAttribute("МС (КАН-М)", null)]
        [NUnit.Framework.TestCaseAttribute("МС (КАН)", null)]
        [NUnit.Framework.TestCaseAttribute("МС (ГС)", null)]
        [NUnit.Framework.TestCaseAttribute("МС (ГВС-М)", null)]
        [NUnit.Framework.TestCaseAttribute("МС (ГВС)", null)]
        [NUnit.Framework.TestCaseAttribute("МС (ВДСК)", null)]
        [NUnit.Framework.TestCaseAttribute("КРП (ЭС)", null)]
        [NUnit.Framework.TestCaseAttribute("КРП (ХВС-М)", null)]
        [NUnit.Framework.TestCaseAttribute("КРП (ХВС)", null)]
        [NUnit.Framework.TestCaseAttribute("КРП (Ф-Б)", null)]
        [NUnit.Framework.TestCaseAttribute("КРП (Ф)", null)]
        [NUnit.Framework.TestCaseAttribute("КРП (СТРОП)", null)]
        [NUnit.Framework.TestCaseAttribute("КРП (ППАиДУ)", null)]
        [NUnit.Framework.TestCaseAttribute("КРП (ПОДВАЛ)", null)]
        [NUnit.Framework.TestCaseAttribute("КРП (ПВ)", null)]
        [NUnit.Framework.TestCaseAttribute("КРП (ОС-М)", null)]
        [NUnit.Framework.TestCaseAttribute("КРП (ОС)", null)]
        [NUnit.Framework.TestCaseAttribute("КРП (МУС)", null)]
        [NUnit.Framework.TestCaseAttribute("КРП (КРОВЛЯ)", null)]
        [NUnit.Framework.TestCaseAttribute("КРП (КАН-М)", null)]
        [NUnit.Framework.TestCaseAttribute("КРП (КАН)", null)]
        [NUnit.Framework.TestCaseAttribute("КРП (ГС)", null)]
        [NUnit.Framework.TestCaseAttribute("КРП (ГВС-М)", null)]
        [NUnit.Framework.TestCaseAttribute("КРП (ГВС)", null)]
        [NUnit.Framework.TestCaseAttribute("КРП (ВДСК)", null)]
        [NUnit.Framework.TestCaseAttribute("БАЛЛЫ (ЭС)", null)]
        [NUnit.Framework.TestCaseAttribute("БАЛЛЫ (ХВС-М)", null)]
        [NUnit.Framework.TestCaseAttribute("БАЛЛЫ (ХВС)", null)]
        [NUnit.Framework.TestCaseAttribute("БАЛЛЫ (ФАС)", null)]
        [NUnit.Framework.TestCaseAttribute("БАЛЛЫ (Ф-Б)", null)]
        [NUnit.Framework.TestCaseAttribute("БАЛЛЫ (СТРОП)", null)]
        [NUnit.Framework.TestCaseAttribute("БАЛЛЫ (ППА)", null)]
        [NUnit.Framework.TestCaseAttribute("БАЛЛЫ (ПВ)", null)]
        [NUnit.Framework.TestCaseAttribute("БАЛЛЫ (ОС-М)", null)]
        [NUnit.Framework.TestCaseAttribute("БАЛЛЫ (ОС)", null)]
        [NUnit.Framework.TestCaseAttribute("БАЛЛЫ (МУС)", null)]
        [NUnit.Framework.TestCaseAttribute("БАЛЛЫ (КРОВ)", null)]
        [NUnit.Framework.TestCaseAttribute("БАЛЛЫ (КАН-М)", null)]
        [NUnit.Framework.TestCaseAttribute("БАЛЛЫ (КАН)", null)]
        [NUnit.Framework.TestCaseAttribute("БАЛЛЫ (ГС)", null)]
        [NUnit.Framework.TestCaseAttribute("БАЛЛЫ (ГВС-М)", null)]
        [NUnit.Framework.TestCaseAttribute("БАЛЛЫ (ГВС)", null)]
        [NUnit.Framework.TestCaseAttribute("БАЛЛЫ (ВДСК)", null)]
        [NUnit.Framework.TestCaseAttribute("ЭСП", null)]
        [NUnit.Framework.TestCaseAttribute("ГСП", null)]
        [NUnit.Framework.TestCaseAttribute("КАНП", null)]
        [NUnit.Framework.TestCaseAttribute("ЦОП", null)]
        [NUnit.Framework.TestCaseAttribute("МУСП", null)]
        [NUnit.Framework.TestCaseAttribute("ППАП", null)]
        [NUnit.Framework.TestCaseAttribute("ХВСП", null)]
        [NUnit.Framework.TestCaseAttribute("ПВП", null)]
        [NUnit.Framework.TestCaseAttribute("ГВСП", null)]
        [NUnit.Framework.TestCaseAttribute("ГВС-МП", null)]
        [NUnit.Framework.TestCaseAttribute("ХВС-МП", null)]
        [NUnit.Framework.TestCaseAttribute("ЦО-МП", null)]
        [NUnit.Framework.TestCaseAttribute("КАН-МП", null)]
        [NUnit.Framework.TestCaseAttribute("ФАСП", null)]
        [NUnit.Framework.TestCaseAttribute("ВДСКП", null)]
        [NUnit.Framework.TestCaseAttribute("КРОП", null)]
        [NUnit.Framework.TestCaseAttribute("СТРОПП", null)]
        [NUnit.Framework.TestCaseAttribute("ПОДП", null)]
        public virtual void ПроверкаФорматаИмпортируемогоФайлаНаНаличиеДругихАттрибутов(string аттрибут, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("проверка формата импортируемого файла на наличие других аттрибутов", exampleTags);
#line 31
this.ScenarioSetup(scenarioInfo);
#line 5
this.FeatureBackground();
#line 32
testRunner.Given("файл для импорта данных", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 33
testRunner.When("пользователь импортирует файл", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 34
testRunner.Then(string.Format("система проверяет файл на соответствие формату по наличию аттрибутов в файле {0}", аттрибут), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "То ");
#line 35
testRunner.When(string.Format("в файле не указано значение хотя бы по одному из аттрибутов {0}", аттрибут), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Если ");
#line 36
testRunner.Then("файл загружается", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "То ");
#line 37
testRunner.And("и в лог записывается количество загруженных записей, количество незагруженных зап" +
                    "исей, прочая информация", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 38
testRunner.When(string.Format("в файле отсутствует хотя бы один из аттрибутов {0}", аттрибут), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Если ");
#line 39
testRunner.Then("файл не грузится и в лог записывается ошибка и причина ошибки загрузки файла", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "То ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("проверка загрузки аттрибутов из файла к нам в систему")]
        [NUnit.Framework.TestCaseAttribute("ID", null)]
        [NUnit.Framework.TestCaseAttribute("UID", null)]
        [NUnit.Framework.TestCaseAttribute("MKDB02", null)]
        [NUnit.Framework.TestCaseAttribute("MKDC01", null)]
        [NUnit.Framework.TestCaseAttribute("MKDC02", null)]
        [NUnit.Framework.TestCaseAttribute("MKDC03", null)]
        [NUnit.Framework.TestCaseAttribute("MKDC06", null)]
        [NUnit.Framework.TestCaseAttribute("MKDC07", null)]
        [NUnit.Framework.TestCaseAttribute("MKDC08", null)]
        [NUnit.Framework.TestCaseAttribute("КРП (ЭС)", null)]
        [NUnit.Framework.TestCaseAttribute("КРП (ХВС-М)", null)]
        [NUnit.Framework.TestCaseAttribute("КРП (ХВС)", null)]
        [NUnit.Framework.TestCaseAttribute("КРП (Ф-Б)", null)]
        [NUnit.Framework.TestCaseAttribute("КРП (Ф)", null)]
        [NUnit.Framework.TestCaseAttribute("КРП (СТРОП)", null)]
        [NUnit.Framework.TestCaseAttribute("КРП (ППАиДУ)", null)]
        [NUnit.Framework.TestCaseAttribute("КРП (ПОДВАЛ)", null)]
        [NUnit.Framework.TestCaseAttribute("КРП (ПВ)", null)]
        [NUnit.Framework.TestCaseAttribute("КРП (ОС-М)", null)]
        [NUnit.Framework.TestCaseAttribute("КРП (ОС)", null)]
        [NUnit.Framework.TestCaseAttribute("КРП (МУС)", null)]
        [NUnit.Framework.TestCaseAttribute("КРП (КРОВЛЯ)", null)]
        [NUnit.Framework.TestCaseAttribute("КРП (КАН-М)", null)]
        [NUnit.Framework.TestCaseAttribute("КРП (КАН)", null)]
        [NUnit.Framework.TestCaseAttribute("КРП (ГС)", null)]
        [NUnit.Framework.TestCaseAttribute("КРП (ГВС-М)", null)]
        [NUnit.Framework.TestCaseAttribute("КРП (ГВС)", null)]
        [NUnit.Framework.TestCaseAttribute("КРП (ВДСК)", null)]
        [NUnit.Framework.TestCaseAttribute("БАЛЛЫ (ЭС)", null)]
        [NUnit.Framework.TestCaseAttribute("БАЛЛЫ (ХВС-М)", null)]
        [NUnit.Framework.TestCaseAttribute("БАЛЛЫ (ХВС)", null)]
        [NUnit.Framework.TestCaseAttribute("БАЛЛЫ (ФАС)", null)]
        [NUnit.Framework.TestCaseAttribute("БАЛЛЫ (Ф-Б)", null)]
        [NUnit.Framework.TestCaseAttribute("БАЛЛЫ (СТРОП)", null)]
        [NUnit.Framework.TestCaseAttribute("БАЛЛЫ (ППА)", null)]
        [NUnit.Framework.TestCaseAttribute("БАЛЛЫ (ПВ)", null)]
        [NUnit.Framework.TestCaseAttribute("БАЛЛЫ (ОС-М)", null)]
        [NUnit.Framework.TestCaseAttribute("БАЛЛЫ (ОС)", null)]
        [NUnit.Framework.TestCaseAttribute("БАЛЛЫ (МУС)", null)]
        [NUnit.Framework.TestCaseAttribute("БАЛЛЫ (КРОВ)", null)]
        [NUnit.Framework.TestCaseAttribute("БАЛЛЫ (КАН-М)", null)]
        [NUnit.Framework.TestCaseAttribute("БАЛЛЫ (КАН)", null)]
        [NUnit.Framework.TestCaseAttribute("БАЛЛЫ (ГС)", null)]
        [NUnit.Framework.TestCaseAttribute("БАЛЛЫ (ГВС-М)", null)]
        [NUnit.Framework.TestCaseAttribute("БАЛЛЫ (ГВС)", null)]
        [NUnit.Framework.TestCaseAttribute("БАЛЛЫ (ВДСК)", null)]
        [NUnit.Framework.TestCaseAttribute("ЭСП", null)]
        [NUnit.Framework.TestCaseAttribute("ГСП", null)]
        [NUnit.Framework.TestCaseAttribute("КАНП", null)]
        [NUnit.Framework.TestCaseAttribute("ЦОП", null)]
        [NUnit.Framework.TestCaseAttribute("МУСП", null)]
        [NUnit.Framework.TestCaseAttribute("ППАП", null)]
        [NUnit.Framework.TestCaseAttribute("ХВСП", null)]
        [NUnit.Framework.TestCaseAttribute("ПВП", null)]
        [NUnit.Framework.TestCaseAttribute("ГВСП", null)]
        [NUnit.Framework.TestCaseAttribute("ГВС-МП", null)]
        [NUnit.Framework.TestCaseAttribute("ХВС-МП", null)]
        [NUnit.Framework.TestCaseAttribute("ЦО-МП", null)]
        [NUnit.Framework.TestCaseAttribute("КАН-МП", null)]
        [NUnit.Framework.TestCaseAttribute("ФАСП", null)]
        [NUnit.Framework.TestCaseAttribute("ВДСКП", null)]
        [NUnit.Framework.TestCaseAttribute("КРОП", null)]
        [NUnit.Framework.TestCaseAttribute("СТРОПП", null)]
        [NUnit.Framework.TestCaseAttribute("ПОДП", null)]
        [NUnit.Framework.TestCaseAttribute("ДОМ БАЛЛЫ (ПОЛНЫЕ)", null)]
        [NUnit.Framework.TestCaseAttribute("ОЧЕРЕДЬ", null)]
        public virtual void ПроверкаЗагрузкиАттрибутовИзФайлаКНамВСистему(string аттрибут, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("проверка загрузки аттрибутов из файла к нам в систему", exampleTags);
#line 119
this.ScenarioSetup(scenarioInfo);
#line 5
this.FeatureBackground();
#line 120
testRunner.Given("файл для импорта данных дпкр", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 121
testRunner.When("пользователь импортирует файл", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 122
testRunner.Then(string.Format("система грузит данные из файла по аттрибутам {0}", аттрибут), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "То ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("проверка загрузки аттрибута \"Год последнего ремонта\"")]
        [NUnit.Framework.TestCaseAttribute("КРП (ЭС)", null)]
        [NUnit.Framework.TestCaseAttribute("КРП (ХВС-М)", null)]
        [NUnit.Framework.TestCaseAttribute("КРП (ХВС)", null)]
        [NUnit.Framework.TestCaseAttribute("КРП (Ф-Б)", null)]
        [NUnit.Framework.TestCaseAttribute("КРП (Ф)", null)]
        [NUnit.Framework.TestCaseAttribute("КРП (СТРОП)", null)]
        [NUnit.Framework.TestCaseAttribute("КРП (ППАиДУ)", null)]
        [NUnit.Framework.TestCaseAttribute("КРП (ПОДВАЛ)", null)]
        [NUnit.Framework.TestCaseAttribute("КРП (ПВ)", null)]
        [NUnit.Framework.TestCaseAttribute("КРП (ОС-М)", null)]
        [NUnit.Framework.TestCaseAttribute("КРП (ОС)", null)]
        [NUnit.Framework.TestCaseAttribute("КРП (МУС)", null)]
        [NUnit.Framework.TestCaseAttribute("КРП (КРОВЛЯ)", null)]
        [NUnit.Framework.TestCaseAttribute("КРП (КАН-М)", null)]
        [NUnit.Framework.TestCaseAttribute("КРП (КАН)", null)]
        [NUnit.Framework.TestCaseAttribute("КРП (ГС)", null)]
        [NUnit.Framework.TestCaseAttribute("КРП (ГВС-М)", null)]
        [NUnit.Framework.TestCaseAttribute("КРП (ГВС)", null)]
        [NUnit.Framework.TestCaseAttribute("КРП (ВДСК)", null)]
        public virtual void ПроверкаЗагрузкиАттрибутаГодПоследнегоРемонта(string аттрибут, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("проверка загрузки аттрибута \"Год последнего ремонта\"", exampleTags);
#line 194
this.ScenarioSetup(scenarioInfo);
#line 5
this.FeatureBackground();
#line 195
testRunner.Given("файл для импорта данных", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 196
testRunner.When("пользователь импортирует файл", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 197
testRunner.And(string.Format("данные из аттрибута {0} загружаются без ошибок", аттрибут), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line 198
testRunner.Then("система находит дома по соотнесению аттрибута \"uid\" с аттрибутом \"id\" из нашей си" +
                    "стемы", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "То ");
#line 199
testRunner.And("в каждом найденном доме загружаются данные из файла по конструктивным характерист" +
                    "икам дома", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("проверка загрузки аттрибута \"Срок эксплуатации\"")]
        [NUnit.Framework.TestCaseAttribute("МС (ЭС)", "Система электроснабжения", "20", "Система электроснабжения (20)", null)]
        [NUnit.Framework.TestCaseAttribute("МС (ХВС-М)", "Системы холодного водоснабжения (магистрали)", "30", "Системы холодного водоснабжения (магистрали) (30)", null)]
        [NUnit.Framework.TestCaseAttribute("МС (ХВС)", "Системы холодного водоснабжения (стояки)", "21", "Системы холодного водоснабжения (стояки) (21)", null)]
        [NUnit.Framework.TestCaseAttribute("МС (Ф-Б)", "Балконная плита", "30", "Балконная плита (30)", null)]
        [NUnit.Framework.TestCaseAttribute("МС (Ф)", "Фасад", "50", "Фасад (50)", null)]
        [NUnit.Framework.TestCaseAttribute("МС (СТРОП)", "Стропилы", "50", "Стропилы (50)", null)]
        [NUnit.Framework.TestCaseAttribute("МС (ППАиДУ)", "Внутридомовая система дымоудаления и противопожарной автоматики", "30", "Внутридомовая система дымоудаления и противопожарной автоматики (30)", null)]
        [NUnit.Framework.TestCaseAttribute("МС (ПОДВАЛ)", "Подвальные помещения, относящиеся к общему имуществу", "21", "Подвальные помещения (21)", null)]
        [NUnit.Framework.TestCaseAttribute("МС (ПВ)", "Пожарный водопровод", "30", "Пожарный водопровод (30)", null)]
        [NUnit.Framework.TestCaseAttribute("МС (ОС-М)", "Системы отопления (магистрали)", "20", "Системы отопления (магистрали) (20)", null)]
        [NUnit.Framework.TestCaseAttribute("МС (ОС)", "Системы отопления (стояки)", "30", "Системы отопления (стояки) (30)", null)]
        [NUnit.Framework.TestCaseAttribute("МС (МУС)", "Мусоропроводы", "45", "Мусоропроводы (45)", null)]
        [NUnit.Framework.TestCaseAttribute("МС (КРОВЛЯ)", "Крыша", "15", "Крыша (15)", null)]
        [NUnit.Framework.TestCaseAttribute("МС (КАН-М)", "Системы канализации и водоотведения (магистрали)", "40", "Системы канализации и водоотведения (магистрали) (40)", null)]
        [NUnit.Framework.TestCaseAttribute("МС (КАН)", "Системы канализации и водоотведения (стояки)", "40", "Системы канализации и водоотведения (стояки) (40)", null)]
        [NUnit.Framework.TestCaseAttribute("МС (ГС)", "Газовые сети", "20", "Газовые сети (20)", null)]
        [NUnit.Framework.TestCaseAttribute("МС (ГВС-М)", "Системы горячего водоснабжения (магистрали)", "20", "Системы горячего водоснабжения (магистрали) (20)", null)]
        [NUnit.Framework.TestCaseAttribute("МС (ГВС)", "Системы горячего водоснабжения (стояки)", "30", "Системы горячего водоснабжения (стояки) (30)", null)]
        [NUnit.Framework.TestCaseAttribute("МС (ВДСК)", "Водосток", "35", "Водосток (35)", null)]
        public virtual void ПроверкаЗагрузкиАттрибутаСрокЭксплуатации(string аттрибут, string наименование_Кэ, string значение, string новый_Кэ, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("проверка загрузки аттрибута \"Срок эксплуатации\"", exampleTags);
#line 224
this.ScenarioSetup(scenarioInfo);
#line 5
this.FeatureBackground();
#line 225
testRunner.Given("файл для импорта данных", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 226
testRunner.When("пользователь импортирует файл", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 227
testRunner.Then("система находит дома по соотнесению аттрибута \"uid\" с аттрибутом \"id\" из нашей си" +
                    "стемы", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "То ");
#line 228
testRunner.And(string.Format("по найденному дому в конструктивных характеристиках по аттрибутам {0} создает кон" +
                        "структивный элемент {1} в соответствии с указанным в аттрибуте значением {2} и н" +
                        "аименованием КЭ {3} (формула слудующая: {1} = {3} ({2}))", аттрибут, новый_Кэ, значение, наименование_Кэ), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("проверка повторной загрузки файла импорта")]
        public virtual void ПроверкаПовторнойЗагрузкиФайлаИмпорта()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("проверка повторной загрузки файла импорта", ((string[])(null)));
#line 253
this.ScenarioSetup(scenarioInfo);
#line 5
this.FeatureBackground();
#line 254
testRunner.Given("файл для импорта данных", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 255
testRunner.When("пользователь повторно импортирует файл", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 256
testRunner.Then("все перечисленные в файле данные по дому перезаписываются", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "То ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("добавление записей в ДПКР")]
        public virtual void ДобавлениеЗаписейВДПКР()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("добавление записей в ДПКР", ((string[])(null)));
#line 259
this.ScenarioSetup(scenarioInfo);
#line 5
this.FeatureBackground();
#line 260
testRunner.Given("файл для импорта данных", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line 261
testRunner.When("пользователь импортирует файл", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 262
testRunner.Then("создается запись в разделе \"Долгосрочная программа\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "То ");
#line 263
testRunner.And("новая запись в разделе дома \"Конструктивные характеристики\" не создается", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("наличие КЭ для ООИ")]
        public virtual void НаличиеКЭДляООИ()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("наличие КЭ для ООИ", ((string[])(null)));
#line 266
this.ScenarioSetup(scenarioInfo);
#line 5
this.FeatureBackground();
#line 267
testRunner.When("пользователь заходит в карточку дома", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line 268
testRunner.Then("для каждого ООИ количество КЭ = \"1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line 269
testRunner.But("для ООИ \"Фасад\" и \"Электроснабжение\" количество КЭ >= \"1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Но ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
