﻿@ScenarionInTransaction
Функция: Расчет начислений

@ignore 
#https://jira.bars-open.ru/browse/GKH-4873
#должны быть созданы лс: с нулевой долей собственности
Сценарий: Расчет нулевой доли собственности для лс при нулевой доли собственности
Дано пользователь выбирает Период "2015 Февраль" 
И пользователь в реестре ЛС выбирает лицевой счет "100000378"
Когда пользователь вызывает операцию расчета лс
Тогда в реестре задач появилась задача с Наименованием "Расчет задолженности ЛС"
И у этой задачи заполнено поле Дата запуска "текущая дата"
И в течении 9 мин статус задачи стал "Успешно выполнена"
И в течении 0 мин процент выполнения задачи стал "100"
И в течении 0 мин ход выполнения задачи стал "Завершено"
Когда в Реестре неподтвержденных начислений появляется запись начислений 
И у этой записи Состояние Ожидает
И пользователь подтверждает эту запись начислений
Тогда у этой записи Состояние Подтверждено
Тогда в протоколе расчета по текущему периоду у ЛС есть запись по начислению
И у этой записи для ЛС заполнено поле С-по "01.02.2015 - 28.02.2015"
И у этой записи для ЛС заполнено поле Тариф на КР "0"
И у этой записи для ЛС заполнено поле Доля собственности "0"
И у этой записи для ЛС заполнено поле Площадь помещения "54.9"
И у этой записи для ЛС заполнено поле Количество дней "28"
И у этой записи для ЛС заполнено поле Итого "417.24"

@ignore 
#https://jira.bars-open.ru/browse/GKH-4873
#должны быть созданы лс: с нулевой общей площадью помещения
Сценарий: Расчет нулевой доли собственности для лс при нулевой площади
Дано пользователь выбирает Период "2015 Февраль" 
И пользователь в реестре ЛС выбирает лицевой счет "100000378"
Когда пользователь вызывает операцию расчета лс
Тогда в реестре задач появилась задача с Наименованием "Расчет задолженности ЛС"
И у этой задачи заполнено поле Дата запуска "текущая дата"
И в течении 9 мин статус задачи стал "Успешно выполнена"
И в течении 0 мин процент выполнения задачи стал "100"
И в течении 0 мин ход выполнения задачи стал "Завершено"
Когда в Реестре неподтвержденных начислений появляется запись начислений 
И у этой записи Состояние Ожидает
И пользователь подтверждает эту запись начислений
Тогда у этой записи Состояние Подтверждено
Тогда в протоколе расчета по текущему периоду у ЛС есть запись по начислению
И у этой записи для ЛС заполнено поле С-по "дата открытия периода начислений"
И у этой записи для ЛС заполнено поле Тариф на КР ""
И у этой записи для ЛС заполнено поле Доля собственности ""
И у этой записи для ЛС заполнено поле Площадь помещения ""
И у этой записи для ЛС заполнено поле Количество дней ""
И у этой записи для ЛС заполнено поле Итого ""

@ignore 
#https://jira.bars-open.ru/browse/GKH-4873
#должен быть создан лс с нулевым тарифом
Сценарий: Расчет нулевой доли собственности для лс при нулевом тарифе
Дано в размер взносов на кр есть запись с пустым полем Окончание периода
И у этой детальной информации есть запис по муниципальному образованию "Никольское сельское поселение"
И у этой записи по муниципальному образованию заполнено поле Размер взноса "7,8"
Когда пользователь вызывает операцию расчета лс
Тогда в реестре задач появилась задача с Наименованием "Расчет задолженности ЛС"
И у этой задачи заполнено поле Дата запуска "текущая дата"
И в течении 9 мин статус задачи стал "Успешно выполнена"
И в течении 0 мин процент выполнения задачи стал "100"
И в течении 0 мин ход выполнения задачи стал "Завершено"
Когда в Реестре неподтвержденных начислений появляется запись начислений 
И у этой записи Состояние Ожидает
И пользователь подтверждает эту запись начислений
Тогда у этой записи Состояние Подтверждено
И в протоколе расчета по текущему периоду у ЛС есть запись по начислению
И у этой записи для ЛС заполнено поле С-по "дата открытия периода начислений"
И у этой записи для ЛС заполнено поле Тариф на КР ""
И у этой записи для ЛС заполнено поле Доля собственности ""
И у этой записи для ЛС заполнено поле Площадь помещения ""
И у этой записи для ЛС заполнено поле Количество дней ""
И у этой записи для ЛС заполнено поле Итого ""
Дано у этой записи по муниципальному образованию заполнено поле Размер взноса "0,00"

#должны быть созданы лс: 1й со статусом "Закрыт", 2й со статусом "Не активен"
Структура сценария: отсутствие расчета начислений для лс со статусами "Закрыт" и "Не активен"
И пользователь в реестре ЛС выбирает лицевой счет "<лс>"
Дано пользователь выбирает Период "текущий" 
Когда пользователь вызывает операцию расчета лс
Тогда в реестре задач не появилась задача

Примеры:
| лс        |
| 010045932 |
| 010124852 |

#Администрирование/Настройки приложения/Единые настройки приложения - групбокс "Расчет вести для домов, у которых способ формирования фонда"
Сценарий: отсутствие расчета начислений при отсутствии настройки в Настройке параметров
Дано пользователь в единых настройках приложения заполняет поле Счет регионального оператора "false"
И пользователь в единых настройках приложения заполняет поле Специальный счет регионального оператора "false"
И пользователь в единых настройках приложения заполняет поле Специальный счет "false"
И пользователь в единых настройках приложения заполняет поле Не выбран "false" 
Когда пользователь сохраняет настройки
Допустим пользователь выбирает Период "2015 Февраль"
Когда пользователь вызывает операцию расчета лс
Тогда в реестре задач не появилась задача
Дано пользователь в единых настройках приложения заполняет поле Счет регионального оператора "true"
И пользователь в единых настройках приложения заполняет поле Специальный счет регионального оператора "true"
И пользователь в единых настройках приложения заполняет поле Специальный счет "true"
И пользователь в единых настройках приложения заполняет поле Не выбран "true" 
Когда пользователь сохраняет настройки

@ignore
#Администрирование/Настройки приложения/Единые настройки приложения - групбокс "Расчет вести для домов, у которых способ формирования фонда"
Сценарий: расчет начислений при заполненной настройке в Настройке параметров со всеми включенными параметрами
Дано пользователь в единых настройках приложения заполняет поле Счет регионального оператора "true"
И пользователь в единых настройках приложения заполняет поле Специальный счет регионального оператора "true"
И пользователь в единых настройках приложения заполняет поле Специальный счет "true"
И пользователь в единых настройках приложения заполняет поле Не выбран "true" 
Когда пользователь вызывает операцию расчета лс
Тогда в реестре задач появилась задача с Наименованием "Расчет задолженности ЛС"
И у этой задачи заполнено поле Дата запуска "текущая дата"
И в течении 9 мин статус задачи стал "Успешно выполнена"
И в течении 0 мин процент выполнения задачи стал "100"
И в течении 0 мин ход выполнения задачи стал "Завершено"
Когда в Реестре неподтвержденных начислений появляется запись начислений 

@ignore
#Администрирование/Настройки приложения/Единые настройки приложения - групбокс "Расчет вести для домов, у которых способ формирования фонда"
#должны быть домавлены 4 лс, у которых дома с протоколами в конечном статусе со способами формирования Счет регионального оператора, Специальный счет регионального оператора, Специальный счет, Не выбран 
Структура сценария: расчет начислений при заполненной настройке в Настройке параметров по одному параметру
Дано пользователь в единых настройках приложения заполняет поле "<способ формирования фонда>" "true"
И пользователь в реестре ЛС выбирает лицевой счет "100000378"
Когда пользователь вызывает операцию расчета лс
Тогда в реестре задач появилась задача с Наименованием "Расчет задолженности ЛС"
И у этой задачи заполнено поле Дата запуска "текущая дата"
И в течении 9 мин статус задачи стал "Успешно выполнена"
И в течении 0 мин процент выполнения задачи стал "100"
И в течении 0 мин ход выполнения задачи стал "Завершено""
Когда в Реестре неподтвержденных начислений появляется запись начислений 
Тогда у этой детальной информации по начислениям отсутствует запись по ЛС "<лс>"

Примеры:
| способ формирования фонда                | лс |
| Счет регионального оператора             |    |
| Специальный счет регионального оператора |    |
| Специальный счет                         |    |
| Не выбран                                |    |

#должны быть добавлены 2 дома: 1 с сотоянием Аварийный, 2 с состоянием Снесен, и лицевые счета к ним
Структура сценария: отсутствие расчета начислений для лс, у которых дома с состояниями Аварийный и Снесен
И пользователь в реестре ЛС выбирает лицевой счет "<лс>"
Дано пользователь выбирает Период "текущий" 
Когда пользователь вызывает операцию расчета лс
Тогда в реестре задач не появилась задача

Примеры:
| лс        |
| 140014141 |
| 140014153 |

@ignore
#должны быть 4 дома с протоколами, у которых статус не переведен в конечный
Структура сценария: неудачный расчет начислений при заполненной настройке с протоколом НЕ в конечном статусе
Дано пользователь в единых настройках приложения заполняет поле Счет регионального оператора "true"
И пользователь в единых настройках приложения заполняет поле Специальный счет регионального оператора "true"
И пользователь в единых настройках приложения заполняет поле Специальный счет "true"
И пользователь в единых настройках приложения заполняет поле Не выбран "true" 
Когда пользователь сохраняет настройки
И пользователь вызывает операцию расчета лс для ЛС "<лс>"
Тогда в реестре задач не появилась задача

Примеры:
| лс |
|    |
|    |
|    |
|    |

@ignore
#должны быть 4 дома с протоколами, у которых статус переведен в конечный, но даты протоколов 
Структура сценария: неудачный расчет начислений при заполненной настройке с протоколом в конечном статусе, но с датой вступления в силу > текущей даты
Дано пользователь в единых настройках приложения заполняет поле Счет регионального оператора "true"
И пользователь в единых настройках приложения заполняет поле Специальный счет регионального оператора "true"
И пользователь в единых настройках приложения заполняет поле Специальный счет "true"
И пользователь в единых настройках приложения заполняет поле Не выбран "true" 
Когда пользователь сохраняет настройки
И пользователь вызывает операцию расчета лс для ЛС "<лс>"
Тогда в реестре задач не появилась задача

Примеры:
| лс |
|    |
|    |
|    |
|    |

#должны быть 1 дом с протоколам, у которого статус переведен в конечный, дата актуальная, а ведение лс = собственниками
Сценарий: неудачный расчет начислений при заполненной настройке с протоколом, в котором ведение лс = Собственниками
Дано пользователь в единых настройках приложения заполняет поле Счет регионального оператора "true"
И пользователь в единых настройках приложения заполняет поле Специальный счет регионального оператора "true"
И пользователь в единых настройках приложения заполняет поле Специальный счет "true"
И пользователь в единых настройках приложения заполняет поле Не выбран "true" 
Когда пользователь сохраняет настройки
Дано пользователь выбирает Период "2015 Февраль" 
И пользователь в реестре ЛС выбирает лицевой счет "100000378"

Когда пользователь вызывает операцию расчета лс
Тогда в реестре задач не появилась задача

@ignore
#должны быть 1 дом с протоколам, 1 ркц, в этот ркц добавлен лс от этого дома, и проставлена галочка в CheckBox "РКЦ проводит начисления"
Сценарий: неудачный расчет начислений для лс, добавленных в ркц с "РКЦ проводит начисления" = true
Дано пользователь в единых настройках приложения заполняет поле Счет регионального оператора "true"
И пользователь в единых настройках приложения заполняет поле Специальный счет регионального оператора "true"
И пользователь в единых настройках приложения заполняет поле Специальный счет "true"
И пользователь в единых настройках приложения заполняет поле Не выбран "true" 
Когда пользователь сохраняет настройки
Дано пользователь выбирает Период "2015 Февраль" 
И пользователь в реестре ЛС выбирает лицевой счет "100000378"
И пользователь проверяет количество задач в разделе Задачи
Когда пользователь вызывает операцию расчета лс
Тогда в реестре задач не появилась задача

@ignore
#должны быть 1 дом с протоколам, и у этого дома в CheckBox "Дом не участвует в программе КР" стоит галочка
Сценарий: неудачный расчет начислений для лс, не участвующих а программе КР
Дано пользователь выбирает Период "2015 Февраль" 
И пользователь в единых настройках приложения заполняет поле Счет регионального оператора "true"
И пользователь в единых настройках приложения заполняет поле Специальный счет регионального оператора "true"
И пользователь в единых настройках приложения заполняет поле Специальный счет "true"
И пользователь в единых настройках приложения заполняет поле Не выбран "true"
Когда пользователь сохраняет настройки 
Допустим пользователь в реестре ЛС выбирает лицевой счет "100000378"
И пользователь проверяет количество задач в разделе Задачи
Когда пользователь вызывает операцию расчета лс
Тогда в реестре задач не появилась задача





