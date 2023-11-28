﻿
Функция: заполнение протокола расчета информацией по перерасчету

Сценарий: заполнение протокола расчета
Когда пользователь по текущему периоду производит расчет начислений в Реестре лицевых счетов
И в Реестре неподтвержденных начислений появляется запись с количеством ЛС, которые попадают в условия расчета лс
И у этой записи Состояние "Ожидает"
И пользователь у этой записи подтверждает начисления
Тогда у этой записи Состояние "Подтверждено"
И у ЛС "100000001" в карточке лицевого счета по текущему периода есть сумма по столбцу Перерасчет
И у этого текущего периода есть детальная информация
И у этой детальной информации есть запись по перерасчету
И у этой записи по перерасчету заполнено поле Дата операции "текущая дата"
И у этой записи по перерасчету заполнено поле Название операции "Перерасчет по тарифу решения"
И у этой записи по перерасчету заполнено поле Изменение сальдо
И у этой записи по перерасчету заполнено поле Документ ""
И по этому текущему периоду есть протокол
И у этого протокола есть сумма больше нуля в Перерасчет: Начисление по новым параметрам - Фактическое начисление
