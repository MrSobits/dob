﻿
Функция: Подтверждение начислений


Сценарий: успешное подтверждение начислений
Дано пользователь вызывает операцию расчета лс
И в задачах появляется запись по расчету лс
И у этой записи по расчету лс заполнено поле Дата запуска "текущая дата"
И у этой записи по расчету лс заполнено поле Наименование "Расчет задолжности ЛС"
И у этой записи по расчету лс заполнено поле Статус "Успешно выполнена"
И у этой записи по расчету лс заполнено поле Процент выполнения "100"
И у этой записи по расчету лс заполнено поле Ход выполнения "Завершено"
И в Реестре неподтвержденных начислений появляется запись начислений 
И у этой записи начислений Состояние "Ожидание"
И у этой записи начислений есть детальная информация
И у этой детальной информации количество записей по лс = количеству лс, которые попадают в условия расчета лс
Когда пользователь подтверждает эту запись начислений
Тогда у этой записи начислений Состояние "Подтверждено"
И у каждого лс из детальной информации есть Протокол расчета за текущий период
