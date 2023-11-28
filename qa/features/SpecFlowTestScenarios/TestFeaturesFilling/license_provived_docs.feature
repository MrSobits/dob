﻿
Функционал: тесткейсы для раздела "Документы для выдачи лицензии"
Справочники - ГЖИ - Документы для выдачи лицензии


Предыстория: 
Дано пользователь добавляет новый документ для выдачи лицензии
И пользователь у этого документа для выдачи лицензии заполняет поле Наименование "документ для выдачи лицензии тест"
И пользователь у этого документа для выдачи лицензии заполняет поле Код "тест"

Сценарий: успешное добавление документа для выдачи лицензии
Когда пользователь сохраняет этот документ для выдачи лицензии
Тогда запись по этому документу для выдачи лицензии присутствует в справочнике документов для выдачи лицензии

Сценарий: успешное удаление записи из справочника документов для выдачи лицензии
Когда пользователь сохраняет этот документ для выдачи лицензии
И пользователь удаляет этот документ для выдачи лицензии
Тогда запись по этому документу для выдачи лицензии отсутствует в справочнике документов для выдачи лицензии

Сценарий: успешное добавление дубля документа для выдачи лицензии
Дано пользователь сохраняет этот документ для выдачи лицензии
Дано пользователь добавляет новый документ для выдачи лицензии
И пользователь у этого документа для выдачи лицензии заполняет поле Наименование "документ для выдачи лицензии тест"
И пользователь у этого документа для выдачи лицензии заполняет поле Код "тест"
Когда пользователь сохраняет этот документ для выдачи лицензии
Тогда запись по этому документу для выдачи лицензии присутствует в справочнике документов для выдачи лицензии
