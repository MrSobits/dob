﻿
Функционал: тесткейсы для раздела "Кредитные организации"
Участники процесса - Контрагенты - Кредитные организации

Предыстория: 
Дано пользователь добавляет новую кредитную организацию
И пользователь у этой кредитной организации заполняет поле Наименование "кредитная организация тест"
И пользователь у этой кредитной организации заполняет поле ИНН "6501236431"
И пользователь выбирает Адрес
| region     | city                                          | street             | houseNumber |
| kamchatka  | Камчатский край, Алеутский р-н, с. Никольское | ул. 50 лет Октября | test        |
| sahalin    | Костромское                                   | Новая              | 1211        |
| testregion | Камчатский край, Алеутский р-н, с. Никольское | ул. 50 лет Октября | test999     |

И пользователь у этой кредитной организации заполняет поле Адрес в пределах субъекта

Сценарий: успешное добавление кредитной организации
Когда пользователь сохраняет эту кредитную организацию
Тогда запись по этой кредитной организации присутствует в списке

Сценарий: успешное удаление кредитной организации
Когда пользователь сохраняет эту кредитную организацию
Когда пользователь удаляет эту кредитную организацию
Тогда запись по этой кредитной организации отсутствует в списке

Сценарий: неудачное добавление дубля кредитной организации
Когда пользователь сохраняет эту кредитную организацию
Дано пользователь добавляет новую кредитную организацию
И пользователь у этой кредитной организации заполняет поле Наименование "кредитная организация тест"
И пользователь у этой кредитной организации заполняет поле ИНН "6501236431"
И пользователь выбирает Адрес
| region     | city                                          | street             | houseNumber |
| kamchatka  | Камчатский край, Алеутский р-н, с. Никольское | ул. 50 лет Октября | test        |
| sahalin    | Костромское                                   | Новая              | 1211        |
| testregion | Камчатский край, Алеутский р-н, с. Никольское | ул. 50 лет Октября | t12est      |

И пользователь у этой кредитной организации заполняет поле Адрес в пределах субъекта
Когда пользователь сохраняет эту кредитную организацию
#Тогда запись по этой кредитной организации отсутствует в списке
Тогда падает ошибка с текстом "Кредитная организация с таким ИНН уже существует"

