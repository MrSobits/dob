﻿
Функционал: тесткейсы граничных значений для раздела "Виды работ текущего ремонта"
Справочники - Жилищно-коммунальное хозяйство - Виды работ текущего ремонта


Сценарий: неудачное добавление вида работы текущего ремонта при незаполненных обязательных полях
Дано пользователь добавляет новый вид работы текущего ремонта
Когда пользователь сохраняет этот вид работы текущего ремонта
Тогда запись по этому виду работы текущего ремонта не сохраняется и падает ошибка с текстом "Не заполнены обязательные поля: Код Наименование Ед. измерения"

Сценарий: удачное добавление вида работы текущего ремонта при вводе граничных условий в 300 знаков, Код
Дано добавлена единица измерения
| Name | ShortName | Description |
| тест | тест      | тест        |

Дано пользователь добавляет новый вид работы текущего ремонта
И пользователь у этого вида работы текущего ремонта заполняет поле Код 300 символов "1"
И пользователь у этого вида работы текущего ремонта заполняет поле Наименование "вид работы текущего ремонта тест"
И пользователь у этого вида работы текущего ремонта заполняет поле Ед. измерения
Когда пользователь сохраняет этот вид работы текущего ремонта
Тогда запись по этому виду работы текущего ремонта присутствует в справочнике видов работ текущего ремонта

Сценарий: неудачное добавление вида работы текущего ремонта при вводе граничных условий в 301 знаков, Код
Дано добавлена единица измерения
| Name | ShortName | Description |
| тест | тест      | тест        |

Дано пользователь добавляет новый вид работы текущего ремонта
И пользователь у этого вида работы текущего ремонта заполняет поле Код 301 символов "1"
И пользователь у этого вида работы текущего ремонта заполняет поле Наименование "вид работы текущего ремонта тест"
И пользователь у этого вида работы текущего ремонта заполняет поле Ед. измерения
Когда пользователь сохраняет этот вид работы текущего ремонта
Тогда запись по этому виду работы текущего ремонта не сохраняется и падает ошибка с текстом "Не заполнены обязательные поля: Код"

Сценарий: удачное добавление вида работы текущего ремонта при вводе граничных условий в 300 знаков, Наименование
Дано добавлена единица измерения
| Name | ShortName | Description |
| тест | тест      | тест        |

Дано пользователь добавляет новый вид работы текущего ремонта
И пользователь у этого вида работы текущего ремонта заполняет поле Код "вид работы текущего ремонта тест"
И пользователь у этого вида работы текущего ремонта заполняет поле Наименование 300 символов "1"
И пользователь у этого вида работы текущего ремонта заполняет поле Ед. измерения
Когда пользователь сохраняет этот вид работы текущего ремонта
Тогда запись по этому виду работы текущего ремонта присутствует в справочнике видов работ текущего ремонта

Сценарий: неудачное добавление вида работы текущего ремонта при вводе граничных условий в 301 знаков, Наименование
Дано добавлена единица измерения
| Name | ShortName | Description |
| тест | тест      | тест        |

Дано пользователь добавляет новый вид работы текущего ремонта
И пользователь у этого вида работы текущего ремонта заполняет поле Код "вид работы текущего ремонта тест"
И пользователь у этого вида работы текущего ремонта заполняет поле Наименование 301 символов "1"
И пользователь у этого вида работы текущего ремонта заполняет поле Ед. измерения
Когда пользователь сохраняет этот вид работы текущего ремонта
Тогда запись по этому виду работы текущего ремонта не сохраняется и падает ошибка с текстом "Не заполнены обязательные поля: Наименование"
















