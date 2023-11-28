﻿@ScenarioInTransaction
Функционал: тесткейсы для раздела "Обращения граждан"
Обращения - Обращения граждан

Предыстория:
Дано добавлена Рубрика
| Name | Code |
| name | code |

#Дано добавлена Место проблемы

#Дано добавлен Исполнитель

И в реестр жилых домов добавлен новый дом
| region     | houseType       | city                                          | street             | houseNumber |
| testregion | Многоквартирный | Камчатский край, Алеутский р-н, с. Никольское | ул. 50 лет Октября | д. test      |

Дано пользователь добавляет новое Обращение граждан
И у этого обращения заполняет поле Номер обращения "155"
И у этого обращения заполняет поле Дата обращения "11.12.2014"
И у этого обращения заполняет поле Адрес
И у этого обращения заполняет поле Рубрика
И у этого обращения заполняет поле Исполнитель
И у этого обращения заполняет поле Заявитель "Заявитель"
И у этого обращения заполняет поле Почтовый адрес заявителя "почтовый адрес Заявителя"
И у этого обращения заполняет поле Место проблемы
И у этого обращения заполняет поле Описание проблемы "1"
И у этого обращения заполняет поле Номер телефона "123"
И у этого обращения заполняет поле Email "maioa@mail.rufj123"

Сценарий: успешное сохранение Обращения граждан
Когда пользователь сохраняет новое Обращение граждан
Тогда обращение граждан присутствует в списке Обращений

Сценарий: успешное удаление Обращения граждан
Когда пользователь сохраняет новое Обращение граждан
И пользователь удаляет новое Обращение граждан
Тогда обращение граждан отсутствует в списке Обращений