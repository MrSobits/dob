﻿
Функционал: тесткейсы для справочника "Муниципальные образования"
Справочники - Общие - Муниципальные образования


Предыстория: 
Дано пользователь добавляет новое муниципальное образование
И пользователь у этого муниципального образования заполняет поле Наименование "тест"
И пользователь у этого муниципального образования заполняет поле Группа "тест"
И пользователь у этого муниципального образования заполняет поле Сокращение "тест"
И пользователь у этого муниципального образования заполняет поле Код "тест"
И пользователь у этого муниципального образования заполняет поле Федеральный номер "тест"
И пользователь у этого муниципального образования заполняет поле ОКАТО "тест"
И пользователь у этого муниципального образования заполняет поле OKTMO "тест"
И пользователь у этого муниципального образования заполняет поле Описание/ комментарий "тест"
И пользователь у этого муниципального образования заполняет поле Проверять корректность сертификата ЭЦП "false"


Сценарий: успешное добавление муниципального образования
Когда пользователь сохраняет это муниципальное образование
Тогда запись по этому муниципальному образованию присутствует в справочнике муниципальных образований

Сценарий: успешное удаление муниципального образования
Когда пользователь сохраняет это муниципальное образование
И пользователь удаляет это муниципальное образование
Тогда запись по этому муниципальному образованию отсутствует в справочнике муниципальных образований

Сценарий: выбор региона в карточке муниципального образования
Когда пользователь сохраняет это муниципальное образование
И у этого муниципального образования устанавливает поле Регион 
| region  | RegionName  |
| sahalin | Сахалинская |
И пользователь сохраняет это муниципальное образование
Тогда запись по этому муниципальному образованию присутствует в справочнике муниципальных образований

Сценарий: выбор муниципального образования в карточке муниципального образования
Когда пользователь сохраняет это муниципальное образование
И у этого муниципального образования устанавливает поле Муниципальное образование 
| region  | RegionName  | AddressName      |
| sahalin | Сахалинская | обл. Сахалинская |
И пользователь сохраняет это муниципальное образование
Тогда запись по этому муниципальному образованию присутствует в справочнике муниципальных образований

