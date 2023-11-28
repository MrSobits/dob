﻿
Функционал: тесткейсы для справочника "Должности"
Справочники - Общие - Должности


Предыстория: 
Дано пользователь добавляет новую должность
И пользователь у этой должности заполняет поле Наименование "тест"
И пользователь у этой должности заполняет поле Родительный "тест"
И пользователь у этой должности заполняет поле Дательный "тест"
И пользователь у этой должности заполняет поле Винительный "тест"
И пользователь у этой должности заполняет поле Творительный "тест"
И пользователь у этой должности заполняет поле Предложный "тест"

Сценарий: успешное добавление должности
Когда пользователь сохраняет эту должность
Тогда запись по этой должности присутствует в справочнике должностей

Сценарий: успешное удаление должности
Когда пользователь сохраняет эту должность
И пользователь удаляет эту должность
Тогда запись по этой должности отсутствует в справочнике должностей

