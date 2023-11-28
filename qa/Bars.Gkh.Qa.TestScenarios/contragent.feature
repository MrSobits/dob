﻿@ScenarioInTransaction
Функция: тесткейсы для раздела "Контрагенты"
Участники процесса - Контрагенты - Контрагенты


Предыстория: 
Дано добавлена организационно-правовая форма
| Name | Code | OkopfCode |
| тест | тест | тест      |

Дано пользователь добавляет нового контрагента
И пользователь у этого контрагента заполняет поле Наименование "тест"
И пользователь у этого контрагента заполняет поле Организационно-правовая форма


Сценарий: Создание контрагента
Когда пользователь сохраняет этого контрагента
Тогда запись по этому контрагенту присутствует в реестре контрагентов


Сценарий: Удаление контрагента
Когда пользователь сохраняет этого контрагента
И пользователь удаляет этого контрагента
Тогда запись по этому контрагенту отсутствует в реестре контрагентов