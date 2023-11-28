﻿@ScenarioInTransaction
Функция: заполнение протокола расчета информацией по перерасчету

@ignore
Сценарий: заполнение протокола расчета
Дано пользователь выбирает Период "2015 Февраль"
И пользователь в реестре ЛС выбирает лицевой счет "100000377"
Когда пользователь вызывает операцию расчета лс
И в Реестре неподтвержденных начислений появляется запись начислений 
И у этой записи Состояние Ожидает
И пользователь подтверждает эту запись начислений
Тогда у этой записи Состояние Подтверждено
И у ЛС в карточке лицевого счета по текущему периода есть сумма по столбцу Перерасчет
И у этого текущего периода есть детальная информация
#И у этой детальной информации есть запись по перерасчету
#И у этой записи по перерасчету заполнено поле Дата операции "текущая дата"
#И у этой записи по перерасчету заполнено поле Название операции "Перерасчет по тарифу решения"
#И у этой записи по перерасчету заполнено поле Изменение сальдо
#И у этой записи по перерасчету заполнено поле Документ ""
И по этому текущему периоду есть протокол
И у этого протокола есть сумма больше нуля в Перерасчет: Начисление по новым параметрам - Фактическое начисление
