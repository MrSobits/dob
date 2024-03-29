﻿
Функция: Доработка реестра Объектов КР


Сценарий: удаление дома из реестра объекта кр
Когда у объекта "объект кр" капитального ремонта "количество видов работ" = "0"
Тогда пользователь успешно удаляет объект "объект кр" из реестра Объектов КР


Структура сценария: сохранение удаленных объектов в "Удаленные объекты капитального ремонта"
Когда у объекта "объект кр" капитального ремонта "количество видов работ" = "0"
И пользователь успешно удаляет объект "объект кр" из реестра Объектов КР
И у удаленного объекта капитального ремонта "объект кр" отметка по программе кр "программа кр" = "На основе ДПКР"
Тогда удаленный объект капитального ремонта "объект кр" есть в списке удаленных домов в реестре "Удаленные объекты капитального ремонта"
И в реестре "Удаленные объекты капитального ремонта" в списке аттрибутов есть "<аттрибут>"

Примеры: 
| аттрибут            |
| Программа           |
| Муниципальный район |
| Адрес               |


Структура сценария: редактирование удаленного объекта кр
Дано в реестре "Удаленные объекты капитального ремонта" "количество" записей != 0
Когда пользователь редактирует удаленный объект 
И в списке аттрибутов удаленного объекта кр "объект кр" есть аттрибуты "<аттрибут>"
Тогда по удаленному объекту кр "объект кр" есть реестр "Журнал изменений"
И в реестре есть "Журнал изменений"
И в списке аттррибутов журнала есть аттрибут "Действие" = аттрибуту "Действие" из вида работ удаленного объекта кр 
И в списке аттррибутов журнала есть аттрибут "Вид работы" = аттрибуту "Вид работы" из вида работ удаленного объекта кр 
И в списке аттррибутов журнала есть аттрибут "Источник финансирвоания" = аттрибуту "Источник финансирвоания" из вида работ удаленного объекта кр
И в списке аттррибутов журнала есть аттрибут "Объем" = аттрибуту "Объем" из вида работ удаленного объекта кр 
И в списке аттррибутов журнала есть аттрибут "Сумма (руб.)" = аттрибуту "Сумма (руб.)" из вида работ удаленного объекта кр
И в списке аттррибутов журнала есть аттрибут "Год выполнения по Долгосрочной программе" = аттрибуту "Год выполнения по Долгосрочной программе" из вида работ удаленного объекта кр
И в списке аттррибутов журнала есть аттрибут "Новый год выполнения" = аттрибуту "Новый год выполнения" из вида работ удаленного объекта кр 
И в списке аттррибутов журнала есть аттрибут "Дата изменения" = аттрибуту "Дата изменения" из вида работ удаленного объекта кр
И в списке аттррибутов журнала есть аттрибут "Пользователь" = аттрибуту "Пользователь" из вида работ удаленного объекта кр 
И в списке аттррибутов журнала есть аттрибут "Причина" = аттрибуту "Причина" из вида работ удаленного объекта кр 
И в списке аттррибутов журнала есть аттрибут "Дата документа" = аттрибуту "Дата документа" из вида работ удаленного объекта кр 
И в списке аттррибутов журнала есть аттрибут "Документ (основание)" = аттрибуту "Документ (основание)" из вида работ удаленного объекта кр 
И в списке аттррибутов журнала есть аттрибут "Файл" = аттрибуту "Файл" из вида работ удаленного объекта кр 

Примеры: 
| аттрибут            |
| Объект недвижимости |
| Программа           |


Структура сценария: Восстановление удаленных объектов кр
Дано в реестре "Удаленные объекты капитального ремонта" "количество" записей != 0
Когда пользователь в реестре "Удаленные объекты капитального ремонта" выбирает объекты кр "объект кр"
И вызывает процедуру восстановления объекта кр
Тогда выбранные объекты кр "объект кр" появляются в реестре "Объекты капитального ремонта"
И выбранных объектов кр "объект кр" нет в реестре "Удаленные объекты капитального ремонта"
И у выбранного объектов кр "объект кр" в реестре "Объекты капитального ремонта" есть аттрибуты "<аттрибут>"
И в реестре объектов кр для объекта кр "объект кр" есть "Журнал изменений"

Примеры: 
| аттрибут            |
| Объект недвижимости |
| Программа           |
