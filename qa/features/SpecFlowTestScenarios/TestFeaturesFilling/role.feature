﻿
Функционал: тесткейсы для раздела "Роли"
Администрирование - Настройка пользователей - Роли


Предыстория: 
Дано пользователь добавляет новую роль
И пользователь у этой роли заполняет поле Наименование "роль тест"

Сценарий: успешное добавление роли
Когда пользователь сохраняет эту роль
Тогда запись по этой роли присутствует в разделе ролей

Сценарий: успешное удаление роли
Когда пользователь сохраняет эту роль
И пользователь удаляет эту роль
Тогда запись по этой роли отсутствует в разделе ролей

Сценарий: успешное добавление дубля роли
Когда пользователь сохраняет эту роль
Дано пользователь добавляет новую роль
И пользователь у этой роли заполняет поле Наименование "роль тест"
Когда пользователь сохраняет эту роль
Тогда запись по этой роли присутствует в разделе ролей


