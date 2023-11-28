﻿@ScenarioInTransaction
Функционал: тесткейсы для справочника "Муниципальные образования"
Справочники - Общие - Муниципальные образования

Предыстория: 
Дано пользователь добавляет новое муниципальное образование
И пользователь у этого муниципального образования заполняет поле Наименование "тест"
И пользователь у этого муниципального образования заполняет поле Регион "тест"
И пользователь у этого муниципального образования заполняет поле Группа "тест"

Сценарий: успешное добавление муниципального образования
Когда пользователь сохраняет это муниципальное образование
Тогда запись по этому муниципальному образованию присутствует в справочнике муниципальных образований

Сценарий: успешное удаление муниципального образования
Когда пользователь сохраняет это муниципальное образование
И пользователь удаляет это муниципальное образование
Тогда запись по этому муниципальному образованию отсутствует в справочнике муниципальных образований



