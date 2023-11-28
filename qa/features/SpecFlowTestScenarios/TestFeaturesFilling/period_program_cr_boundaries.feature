﻿
Функционал: тесткейсы граничных значений для раздела "Периоды программ"
Справочники - Капитальный ремонт - Периоды программ


Сценарий: неудачное добавление периода программы при незаполненных обязательных полях
Дано пользователь добавляет новый период программы
Когда пользователь сохраняет этот период программы
Тогда запись по этому периоду программы отсутствует в справочнике периодов программ
И падает ошибка с текстом "Не заполнены обязательные поля: Наименование Дата начала"

Сценарий: удачное добавление периода программы при вводе граничных условий в 300 знаков, Наименование
Дано пользователь добавляет новый период программы
И пользователь у этого периода программы заполняет поле Наименование 300 символов "1"
И пользователь у этого периода программы заполняет поле Дата начала "01.01.2015"
Когда пользователь сохраняет этот период программы
Тогда запись по этому периоду программы присутствует в справочнике периодов программ

Сценарий: неудачное добавление периода программы при вводе граничных условий в 301 знаков, Наименование
Дано пользователь добавляет новый период программы
И пользователь у этого периода программы заполняет поле Наименование 301 символов "1"
И пользователь у этого периода программы заполняет поле Дата начала "01.01.2015"
Когда пользователь сохраняет этот период программы
Тогда запись по этому периоду программы отсутствует в справочнике периодов программ
И падает ошибка с текстом "Не заполнены обязательные поля: Наименование"