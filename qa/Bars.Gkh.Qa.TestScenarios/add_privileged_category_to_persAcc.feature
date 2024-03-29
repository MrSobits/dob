﻿@ScenarioInTransaction
Функция: добавление категории льготы к лицевому счету
Региональный фонд - Счета - Ресстр лицевых счетов - Карточка лицевого счета - вкладка "Категория льготы"

Предыстория:
Дано пользователь добавляет новую категорию льготы к лс

@ignore
Сценарий: успешное добавление категории льготы
Дано пользователь у этой категории льготы заполняет поле Льготная категория "льгота"
И пользователь у этой категории льготы заполняет поле Действует с "текущая дата"
И пользователь у этой категории льготы заполняет поле Действует по "текущая дата"
Когда пользователь сохраняет эту категорию льготы
Тогда запись по этой категории льготы присутствует в списке категорий льгот у этого лицевого счета

@ignore
Сценарий: успешное удаление категории льготы
Дано пользователь у этой категории льготы заполняет поле Льготная категория "льгота"
И пользователь у этой категории льготы заполняет поле Действует с "текущая дата"
И пользователь у этой категории льготы заполняет поле Действует по "текущая дата"
Когда пользователь сохраняет эту категорию льготы
И пользователь удаляет эту категорию льготы
Тогда запись по этой категории льготы отсутствует в списке категорий льгот у этого лицевого счета


#GKH-3861
Сценарий: неудачное добавление категории льготы с пустым полем "Льготная категория"
Дано пользователь у этой категории льготы заполняет поле Действует с "текущая дата"
И пользователь у этой категории льготы заполняет поле Действует по "текущая дата"
Когда пользователь сохраняет эту категорию льготы
И пользователь удаляет эту категорию льготы
Тогда падает ошибка с текстом "Не заполнены обязательные поля: Льготная категория"

@ignore
#GKH-3861
Сценарий: неудачное добавление категории льготы с пустыми полями
Когда пользователь сохраняет эту категорию льготы
И пользователь удаляет эту категорию льготы
Тогда запись по этой категории льготы отсутствует в списке категорий льгот у этого лицевого счета
И падает ошибка с текстом "Не заполнены обязательные поля: Льготная категория, Действует с"
