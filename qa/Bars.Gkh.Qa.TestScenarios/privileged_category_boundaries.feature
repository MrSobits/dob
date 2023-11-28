﻿@ScenarioInTransaction

Функционал: тесткейсы граничных значений для раздела "Группы льготных категорий граждан"
Справочники - Региональный фонд - Группы льготных категорий граждан

@ignore
#GKH-2391
Сценарий: неудачное добавление группы льготных категорий граждан при незаполненных обязательных полях
Дано пользователь добавляет новую группу льготных категорий граждан
Когда пользователь сохраняет эту группу льготных категорий граждан
Тогда запись по этой группе льготных категорий граждан отсутствует в справочнике групп льготных категорий граждан
И падает ошибка с текстом "Не заполнены обязательные поля: Код Наименование Процент льготы Предельное значение площади Действует с"

@ignore
#GKH-2391
Сценарий: удачное добавление группы льготных категорий граждан при вводе граничных условий в 300 знаков, Код
Дано пользователь добавляет новую группу льготных категорий граждан
И пользователь у этой группы льготных категорий граждан заполняет поле Код 300 символов "1"
И пользователь у этой группы льготных категорий граждан заполняет поле Наименование "группа льготных категорий граждан тест"
И пользователь у этой группы льготных категорий граждан заполняет поле Процент льготы "10"
И пользователь у этой группы льготных категорий граждан заполняет поле Предельное значение площади "10"
И пользователь у этой группы льготных категорий граждан заполняет поле Действует с "01/01/2015"
И пользователь у этой группы льготных категорий граждан заполняет поле Действует по "01/02/2015"
Когда пользователь сохраняет эту группу льготных категорий граждан
Тогда запись по этой группе льготных категорий граждан присутствует в справочнике групп льготных категорий граждан

@ignore
#GKH-2391
Сценарий: неудачное добавление группы льготных категорий граждан при вводе граничных условий в 301 знаков, Код
Дано пользователь добавляет новую группу льготных категорий граждан
И пользователь у этой группы льготных категорий граждан заполняет поле Код 301 символов "1"
И пользователь у этой группы льготных категорий граждан заполняет поле Наименование "группа льготных категорий граждан тест"
И пользователь у этой группы льготных категорий граждан заполняет поле Процент льготы "10"
И пользователь у этой группы льготных категорий граждан заполняет поле Предельное значение площади "10"
И пользователь у этой группы льготных категорий граждан заполняет поле Действует с "01/01/2015"
И пользователь у этой группы льготных категорий граждан заполняет поле Действует по "01/02/2015"
Когда пользователь сохраняет эту группу льготных категорий граждан
Тогда запись по этой группе льготных категорий граждан отсутствует в справочнике групп льготных категорий граждан
И падает ошибка с текстом "Не заполнены обязательные поля: Код"

@ignore
#GKH-2391
Сценарий: удачное добавление группы льготных категорий граждан при вводе граничных условий в 300 знаков, Наименование
Дано пользователь добавляет новую группу льготных категорий граждан
И пользователь у этой группы льготных категорий граждан заполняет поле Код "тест"
И пользователь у этой группы льготных категорий граждан заполняет поле Наименование 300 символов "1"
И пользователь у этой группы льготных категорий граждан заполняет поле Процент льготы "10"
И пользователь у этой группы льготных категорий граждан заполняет поле Предельное значение площади "10"
И пользователь у этой группы льготных категорий граждан заполняет поле Действует с "01/01/2015"
И пользователь у этой группы льготных категорий граждан заполняет поле Действует по "01/02/2015"
Когда пользователь сохраняет эту группу льготных категорий граждан
Тогда запись по этой группе льготных категорий граждан присутствует в справочнике групп льготных категорий граждан

@ignore
#GKH-2391
Сценарий: неудачное добавление группы льготных категорий граждан при вводе граничных условий в 301 знаков, Наименование
Дано пользователь добавляет новую группу льготных категорий граждан
И пользователь у этой группы льготных категорий граждан заполняет поле Код "тест"
И пользователь у этой группы льготных категорий граждан заполняет поле Наименование 301 символов "1"
И пользователь у этой группы льготных категорий граждан заполняет поле Процент льготы "10"
И пользователь у этой группы льготных категорий граждан заполняет поле Предельное значение площади "10"
И пользователь у этой группы льготных категорий граждан заполняет поле Действует с "01/01/2015"
И пользователь у этой группы льготных категорий граждан заполняет поле Действует по "01/02/2015"
Когда пользователь сохраняет эту группу льготных категорий граждан
Тогда запись по этой группе льготных категорий граждан отсутствует в справочнике групп льготных категорий граждан
И падает ошибка с текстом "Не заполнены обязательные поля: Наименование"

Сценарий: удачное добавление группы льготных категорий граждан при вводе допустимого формата даты, Действует с
Дано пользователь добавляет новую группу льготных категорий граждан
И пользователь у этой группы льготных категорий граждан заполняет поле Код "тест"
И пользователь у этой группы льготных категорий граждан заполняет поле Наименование "группа льготных категорий граждан тест"
И пользователь у этой группы льготных категорий граждан заполняет поле Процент льготы "10"
И пользователь у этой группы льготных категорий граждан заполняет поле Предельное значение площади "10"
И пользователь у этой группы льготных категорий граждан заполняет поле Действует с "01/01/2015"
И пользователь у этой группы льготных категорий граждан заполняет поле Действует по "01/02/2015"
Когда пользователь сохраняет эту группу льготных категорий граждан
Тогда запись по этой группе льготных категорий граждан присутствует в справочнике групп льготных категорий граждан

Сценарий: удачное добавление группы льготных категорий граждан при вводе допустимого формата даты, Действует по
Дано пользователь добавляет новую группу льготных категорий граждан
И пользователь у этой группы льготных категорий граждан заполняет поле Код "тест"
И пользователь у этой группы льготных категорий граждан заполняет поле Наименование "группа льготных категорий граждан тест"
И пользователь у этой группы льготных категорий граждан заполняет поле Процент льготы "10"
И пользователь у этой группы льготных категорий граждан заполняет поле Предельное значение площади "10"
И пользователь у этой группы льготных категорий граждан заполняет поле Действует с "01/01/2015"
И пользователь у этой группы льготных категорий граждан заполняет поле Действует по "01/02/2015"
Когда пользователь сохраняет эту группу льготных категорий граждан
Тогда запись по этой группе льготных категорий граждан присутствует в справочнике групп льготных категорий граждан