﻿
Функция: 

Предыстория:
Допустим пользователь в реестре ЛС выбирает лицевой счет "100000377"

Сценарий: Ошибка печати физиков

Допустим пользователь в реестре ЛС выбирает Период "2015 Февраль"
И пользователь в реестре ЛС выбирает текущий ЛС
Когда пользователь в реестре ЛС выбирает действие Выгрузка - Предпросмотр документов на оплату 
И пользователь в реестре ЛС выбирает предпросмотр текущего ЛС
И Пользователь в реестре ЛС выбирает действие Выгрузка - Документы на оплату
Тогда Не выпало не одной ошибки
И в реестре задач появилась задача с Наименованием "Формирование документов на оплату"