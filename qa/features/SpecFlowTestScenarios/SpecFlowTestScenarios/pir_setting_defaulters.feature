﻿
Функционал: Настройка реестра неплательщиков
Претензионная работа - Настройка - Настройка реестра неплательщиков


Структура сценария: успешное сохранение заполненных полей по реструктуризация долга
Когда пользователь заполняет поле "<аттрибут>" значением "<значение>" и сохраняет настройку реестра неплательщиков
Тогда введенное значение записываются в поле "<аттрибут>"

Примеры: 
| аттрибут                              | значение |
| Кол-во дней для приостановления права | 1        |
| Кол-во дней для расторжения           | 1        |
| Кол-во дней для приостановления права |          |
| Кол-во дней для расторжения           |          |


Структура сценария: неудачное сохранение заполненных полей по реструктуризация долга
Когда пользователь заполняет поле "<аттрибут>" значением "<значение>" и сохраняет настройку реестра неплательщиков
Тогда введенное значение записываются в поле "<аттрибут>"

Примеры: 
| аттрибут                              | значение |
| Кол-во дней для приостановления права | 1,2      |
| Кол-во дней для расторжения           | 1,2      |
| Кол-во дней для приостановления права | тест     |
| Кол-во дней для расторжения           | тест     |


Сценарий: формирование претензии когда Тип получателя = Все
Когда пользователь в настройке реестра неплательщиков в поле "Документ претензии" выбирает значение "Формировать"
И в настройке реестра неплательщиков в поле "Тип получателя" знаечние = "Все"
То в реестре неплательщиков "реестр неплательщиков" формируется претензия


Сценарий: формирование претензии когда Тип получателя = Юр.лицо
Когда пользователь в настройке реестра неплательщиков в поле "Документ претензии" выбирает значение "Формировать"
И в настройке реестра неплательщиков в поле "Тип получателя" знаечние = "Юр.лицо"
То в реестре неплательщиков "реестр неплательщиков" формируется претензия, у которых "Тип абонента" = "Юридическое лицо"
И в реестре неплательщиков "реестр неплательщиков" не формируется претензия, у которых "Тип абонента" = "Физическое лицо" по сценарию "отказ в формировании претензии"


Сценарий: формирование претензии когда Тип получателя = Физ.лицо
Когда пользователь в настройке реестра неплательщиков в поле "Документ претензии" выбирает значение "Формировать"
И в настройке реестра неплательщиков в поле "Тип получателя" знаечние = "Физ.лицо"
То в реестре неплательщиков "реестр неплательщиков" формируется претензия, у которых "Тип абонента" = "Физическое лицоо"
И в реестре неплательщиков "реестр неплательщиков" не формируется претензия, у которых "Тип абонента" = "Юридическое лиц" по сценарию "отказ в формировании претензии"


Сценарий: отказ в формировании претензии
Когда пользователь в настройке реестра неплательщиков в поле "Документ претензии" выбирает значение "Не формировать"
И пользователь формирует реестр неплательщиков из регионального фонда
То в реестре неплательщиков "реестр неплательщиков" не формируется претензия
И у записи в реестре неплательщиков претензионной работы значение статуса "статус" != "Требует формирование претензии"
И у записи в реестре неплательщиков претензионной работы значение статуса "статус" != "Сформирована претензия"
Когда пользователь редактирует запись в реестре неплательщиков и производит действие "Сформировать"
Тогда в списке аттрибутов возможных действий нет аттрибута "Претензия"
И пользователь формирует исковое заявление "Исковое заявление"