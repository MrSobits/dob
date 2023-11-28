﻿@ScenarioInTransaction
Функционал: тесткейсы граничных значений для раздела "Заявления в суд"
Справочники - Претензионная и исковая работа - Заявления в суд

@ignore
#GKH-3354
Сценарий: создание нового заявления в суд с пустыми полями, Код
Дано пользователь добавляет новое заявление в суд
И пользователь у этого заявления в суд заполняет поле Краткое наименование "тест1"
И пользователь у этого заявления в суд заполняет поле Полное наименование "тест1"
Когда пользователь сохраняет этот заявление в суд
Тогда запись по этому заявлению в суд присутствует в справочнике заявлений в суд

Сценарий: создание нового заявления в суд с пустыми полями, Краткое наименование
Дано пользователь добавляет новое заявление в суд
И пользователь у этого заявления в суд заполняет поле Код "тест1"
И пользователь у этого заявления в суд заполняет поле Полное наименование "тест1"
Когда пользователь сохраняет этот заявление в суд
Тогда запись по этому заявлению в суд присутствует в справочнике заявлений в суд

Сценарий: создание нового заявления в суд с пустыми полями, Полное наименование
Дано пользователь добавляет новое заявление в суд
И пользователь у этого заявления в суд заполняет поле Код "тест1"
И пользователь у этого заявления в суд заполняет поле Краткое наименование "тест1"
Когда пользователь сохраняет этот заявление в суд
Тогда запись по этому заявлению в суд присутствует в справочнике заявлений в суд

Сценарий: удачное создание нового заявления в суд при вводе граничных условий в 100 знаков, Код
Дано пользователь добавляет новое заявление в суд
И пользователь у этого заявления в суд заполняет поле Краткое наименование "тест1"
И пользователь у этого заявления в суд заполняет поле Полное наименование "тест1"
И пользователь у этого заявления в суд заполняет поле Код 100 символов "1"
Когда пользователь сохраняет этот заявление в суд
Тогда запись по этому заявлению в суд присутствует в справочнике заявлений в суд

#GKH-3354
@ignore
Сценарий: неудачное создание нового заявления в суд при вводе граничных условий в 101 знаков, Код
Дано пользователь добавляет новое заявление в суд
И пользователь у этого заявления в суд заполняет поле Краткое наименование "тест1"
И пользователь у этого заявления в суд заполняет поле Полное наименование "тест1"
И пользователь у этого заявления в суд заполняет поле Код 101 символов "1""
Когда пользователь сохраняет этот заявление в суд
Тогда запись по этому заявлению в суд отсутствует в справочнике заявлений в суд
И падает ошибка с текстом "Количество знаков в поле Код не должно превышать 100 символов"
#И количество символов в поле Код = 100
#И выходит предупреждение с текстом "Код сохранен не полностью, так как превышена максимально возможная длина в 100 символов"

Сценарий: удачное создание нового заявления в суд при вводе граничных условий в 500 знаков, Краткое наименование
Дано пользователь добавляет новое заявление в суд
И пользователь у этого заявления в суд заполняет поле Код "тест1"
И пользователь у этого заявления в суд заполняет поле Полное наименование "тест1"
И пользователь у этого заявления в суд заполняет поле Краткое наименование 500 символов "1"
Когда пользователь сохраняет этот заявление в суд
Тогда запись по этому заявлению в суд присутствует в справочнике заявлений в суд

#GKH-3354
@ignore
Сценарий: неудачное создание нового заявления в суд при вводе граничных условий в 501 знаков, Краткое наименование
Дано пользователь добавляет новое заявление в суд
И пользователь у этого заявления в суд заполняет поле Код "тест1"
И пользователь у этого заявления в суд заполняет поле Полное наименование "тест1"
И пользователь у этого заявления в суд заполняет поле Краткое наименование 501 символов "1"
Когда пользователь сохраняет этот заявление в суд
Тогда запись по этому заявлению в суд отсутствует в справочнике заявлений в суд
И падает ошибка с текстом "Количество знаков в поле Краткое наименование не должно превышать 500 символов"
#И количество символов в поле Краткое наименование = 500
#И выходит предупреждение с текстом "Краткое наименование сохранено не полностью, так как превышена максимально возможная длина в 500 символов"

Сценарий: удачное создание нового заявления в суд при вводе граничных условий в 3000 знаков, Полное наименование
Дано пользователь добавляет новое заявление в суд
И пользователь у этого заявления в суд заполняет поле Код "тест1"
И пользователь у этого заявления в суд заполняет поле Краткое наименование "тест1"
И пользователь у этого заявления в суд заполняет поле Полное наименование 3000 символов "1"
Когда пользователь сохраняет этот заявление в суд
Тогда запись по этому заявлению в суд присутствует в справочнике заявлений в суд

#GKH-3354
@ignore
Сценарий: неудачное создание нового заявления в суд при вводе граничных условий в 3001 знаков, Полное наименование
Дано пользователь добавляет новое заявление в суд
И пользователь у этого заявления в суд заполняет поле Код "тест1"
И пользователь у этого заявления в суд заполняет поле Краткое наименование "тест1"
И пользователь у этого заявления в суд заполняет поле Полное наименование 3001 символов "1"
Когда пользователь сохраняет этот заявление в суд
Тогда запись по этому заявлению в суд отсутствует в справочнике заявлений в суд
И падает ошибка с текстом "Количество знаков в поле Полное наименование не должно превышать 3000 символов"
#И количество символов в поле Полное наименование = 3000
#И выходит предупреждение с текстом "Полное наименование сохранено не полностью, так как превышена максимально возможная длина в 3000 символов"