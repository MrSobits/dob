﻿
Функция: Закрытие лицевого счета

Сценарий: успешное закрытие лицевого счета с переходом в статус "Закрыт"
Когда пользователь для ЛС "" вызывает операцию Закрытие счета
И пользователь заполняет поле Причина "тест"
И пользователь заполняет поле Документ-основание "1.pdf"
И пользователь заполняет поле Дата закрытия "текущая дата"
И пользователь сохраняет изменения
Тогда в карточке ЛС "" в истории изменений есть запись по изменению доли собственности
И у этой записи по изменению доли собственности заполнено поле Наименование параметра "Доля собственности"
И у этой записи по изменению доли собственности заполнено поле Описание измененного атрибута "Изменение доли собственности в связи с закрытием ЛС"
И у этой записи по изменению доли собственности заполнено поле Значение "0"
И у этой записи по изменению доли собственности заполнено поле Дата начала действия значения "текущая дата"
И у этой записи по изменению доли собственности заполнено поле Дата установки значения "текущая дата"
И у этой записи по изменению доли собственности заполнено поле Причина "тест"
И в карточке ЛС "" в истории изменений есть запись по закрытию лс
И у этой записи по закрытию лс заполнено поле Наименование параметра "Закрытие"
И у этой записи по закрытию лс заполнено поле Описание измененного атрибута "Для ЛС установлен статус "Закрыт с долгом""
И у этой записи по закрытию лс заполнено поле Значение "пусто"
И у этой записи по закрытию лс заполнено поле Дата начала действия значения "текущая дата"
И у этой записи по закрытию лс заполнено поле Дата установки значения "текущая дата"
И у этой записи по закрытию лс заполнено поле Причина "тест"
И в карточке лс "" появляется запись по операции по текущему периоду
И записи по операции по предыдущему периоду есть детальная информация
И у этой детальной информации есть запись по закрытию лс
И у этой записи по сальдо заполнено поле Дата операции "текущая дата"
И у этой записи по сальдо заполнено поле Название операции "Закрытие"
И у этой записи по сальдо заполнено поле Изменение сальдо
И в карточке лс "" заполнено поле Доля собственности "0"
И у лицевого счета заполнено поле Статус "Закрыт с долгом"

Сценарий:  неудачное закрытие лс без выбора ЛС
Когда пользователь вызывает операцию Закрытие счета без ваыбора лс
Тогда выходит сообщение об ошибке с текстом "Необходимо выбрать один лицевой счет!"

#Закрытие 2х лицевых счетов одновременно неразрешено по логике запрещено. 
#Поэтому при выборе 2х Лс и проведении операции Закрытия, выходит ошибка "Необходимо выбрать один лицевой счет!"

Сценарий:  неудачное закрытие ЛС для закрытого ЛС с долгом
Когда пользователь для ЛС "" вызывает операцию Закрытие счета без выбора лс
И пользователь заполняет поле Причина "тест"
И пользователь заполняет поле Дата закрытия "текущая дата"
И пользователь сохраняет изменения
Тогда выходит сообщение об ошибке с текстом "Имеется долг. Счет уже закрыт с долгом!"

Сценарий:  неудачное закрытие ЛС для закрытого ЛС
Когда пользователь для ЛС "" вызывает операцию Закрытие счета без выбора лс
И пользователь заполняет поле Причина "тест"
И пользователь заполняет поле Дата закрытия "текущая дата"
И пользователь сохраняет изменения
Тогда выходит сообщение об ошибке с текстом "Счет уже закрыт!"
