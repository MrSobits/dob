﻿Функция: доработки в карточке жилого дома


Структура сценария: ограничение на выбор даты в разделе жилого дома "Протоколы и решения собственников"
Дано открыт раздел "Протоколы и решения собственников"
Дано открыта форма добавления новой записи в гриде "Протjколы"
Дано в поле "Тип протокола" значение <тип_протокола>
Дано заполнено поле "Номер"
Дано заполнено поле "Файл"
Дано заполнено поле "Количество голосов (кв.м.):"
Дано заполнено поле "Общее количество голосов (кв.м.):"
Дано заполнено поле "Доля принявших участие (%):"
Когда я заполняю поле "Дата" датой больше текущей
Тогда выводится форма с информацией <сообщение>
И запись не сохраняется

Примеры: 
| тип_протокола                                      | сообщение                                        |
| Протокол о формировании фонда капитального ремонта | Дата протокола не может быть больше текущей даты |
| Протокол о выборе управляющей организации          | Дата протокола не может быть больше текущей даты |
| Постановление Исполкома МО                         | Дата протокола не может быть больше текущей даты |


Структура сценария: Контроль ввода площадей в карточке жилого дома, раздел "Общие сведения" (ХМАО)
Дано Пользователь с ролью администратора, логин "admin", пароль "admin"
Дано тестируемая система "http://gkh-test.bars-open.ru/dev-hmao/"
Дано открыт раздел "Общие сведения"
Когда сохраняю раздел "Общие сведения"
И заполнены поля <показатель1> и <показатель2>
И их значения соответствуют правилу <правило>
То выходит <сообщение>
И введенные значения не сохраняются
Когда захожу в раздел
Если поля <показатель1> или <показатель2> не заполнены (в поле пусто)
То ошибка <сообщение> не выводится
И информация сохраняется
Если поля или одно из полей <показатель1> или <показатель2> заполнены значением "0"
То поля считаются заполненными
И происходит котроль ввода площадей

Примеры: 
| показатель1                                          | правило | показатель2                                                                                                 | сообщение                                                                                                                                                                                         |
| Площадь частной собственности                        | <=      | Общая площадь жилых и нежилых помещений                                                                     | Площадь частной собственности должна быть меньше или равна Общей площади жилых и нежилых помещений                                                                                                |
| Площадь муниципальной собственности                  | <=      | Общая площадь жилых и нежилых помещений                                                                     | Площадь муниципальной собственности должна быть меньше или равна Общей площади жилых и нежилых помещений                                                                                          |
| Площадь государственной собственности                | <=      | Общая площадь жилых и нежилых помещений                                                                     | Площадь государственной собственности должна быть меньше или равна Общей площади жилых и нежилых помещений                                                                                        |
| Площадь государственной собственности                | =       | в т.ч. жилых всего + в т.ч. нежилых помещений, функционального назначения                                   | Общая площадь жилых и нежилых помещений должна быть равна сумме двух показателей «В т.ч. жилых всего» и «В т.ч. нежилых помещений, функционального назначения                                     |
| Общая площадь жилых и нежилых помещений              | =       | Площадь частной собственности + Площадь муниципальной собственности + Площадь государственной собственности | Общая площадь жилых и нежилых помещений должна быть равна сумме трех показателей: «Площадь частной собственности», «Площадь муниципальной собственности» и «Площадь государственной собственности |
| В т.ч. жилых всего                                   | <=      | Общая площадь жилых и нежилых помещений                                                                     | Показатель «В т.ч. жилых всего» должен быть меньше или равен общей площади жилых и нежилых помещений                                                                                              |
| В т.ч. нежилых помещений, функционального назначения | <       | Общая площадь жилых и нежилых помещений                                                                     | Показатель «В т.ч. нежилых помещений, функционального назначения» должен быть меньше общей площади жилых и нежилых помещений                                                                      |
| Общая площадь жилых и нежилых помещений              | >=      | Сумма всех площадей помещений на вкладке «Сведения о помещениях»                                            | Общая площадь жилых и нежилых помещений» должна быть больше или равна сумме всех площадей помещений                                                                                               |



Структура сценария: новое поле "Всего баллов" в разделе "Конструктивные характеристики" (Москва)
Дано Пользователь с ролью администратора, логин "admin", пароль "admin"
Дано тестируемая система "http://gkh-test.bars-open.ru/dev-msk/"
Дано открыт раздел "Конструктивные характеристики"
Когда захожу в в Администрировании/Настройка прав доступа/Настройка ограничений
И выбираю роль = Администратор
И перехожу в дереве Модуль ЖКХ - Жилые дома - Поля
То в списке есть нужное нам поле <поле> с правами <права>
Когда проставляю галочки для поля <поле> на правах <права>
И возвращаюсь в раздел жилого дома "Конструктивные характеристики"
И перехожу на подвкладку "Конструктивные элементы дома"
То вижу поле "Всего баллов" над гридом конструктивных элементов
Когда редактирую вручную с клавиатуры поле "Всего баллов"
То система разрешает ввод только положительного целого значения
И сохраняет введенное вручную значение
Когда захожу в в Администрировании/Настройка прав доступа/Настройка ограничений
И выбираю роль = Администратор
И перехожу в дереве Модуль ЖКХ - Жилые дома - Поля
То в списке есть нужное нам поле <поле> с правами <права>
Когда снимаю галочки для поля <поле> на правах <права>
И возвращаюсь в раздел жилого дома "Конструктивные характеристики"
И перехожу на подвкладку "Конструктивные элементы дома"
То не вижу поля "Всего баллов"

Примеры: 
| поле         | права     |
| Всего баллов | Просмотр  |
| Всего баллов | Изменение |



Структура сценария: новое поле "Состояние" в разделе "Конструктивные характеристики" на форме КЭ (Москва)
Дано Пользователь с ролью администратора, логин "admin", пароль "admin"
Дано тестируемая система "http://gkh-test.bars-open.ru/dev-msk/"
Дано открыт раздел "Конструктивные характеристики"
Когда захожу в в Администрировании/Настройка прав доступа/Настройка ограничений
И выбираю роль = Администратор
И перехожу в дереве Модуль ЖКХ - Жилые дома - Поля
То в списке есть нужное нам поле <поле> с правами <права>
Когда проставляю галочки для поля <поле> на правах <права>
И возвращаюсь в раздел жилого дома "Конструктивные характеристики"
И открываю на редактирование любой конструктивный элемент
То вижу на форме редактирования КЭ поле "Состояние" с выпадающим списком значений
Когда прехожу к заполнению поля
То в выпадающем списке жестко привязаны значения <значение>
Когда пытаюсь ввести данные с клавиатуры
То система не дает ввобд значений с клавиатуры
И выбор возможен только из имеющихся значений <значение>
Когда захожу в в Администрировании/Настройка прав доступа/Настройка ограничений
И выбираю роль = Администратор
И перехожу в дереве Модуль ЖКХ - Жилые дома - Поля
То в списке есть нужное нам поле <поле> с правами <права>
Когда снимаю галочки для поля <поле> на правах <права>
И возвращаюсь в раздел жилого дома "Конструктивные характеристики"
И перехожу на подвкладку "Конструктивные элементы дома"
И открываю на редактирование любой конструктивный элемент
То не вижу поля "Состояние"

Примеры: 
| поле      | права     |
| Состояние | Просмотр  |
| Состояние | Изменение |

#Примеры: 
 #| значение                    |
 #| не определялось             |
 #| удовлетворительное          |
 #| неудовлетворительное        |
 #| аварийное                   |
 #| нормативное                 |
 #| ограниченно-работоспособное |
 #| работоспособное             |



 #раздел "Общие сведения"
 # для всех регионов, на примере РТ
Структура сценария: проверка историчности поля "Классификация дома"
Дано Пользователь логин "admin_tat", пароль "supergkhtat"
И тестируемая система "http://gkh-test.bars-open.ru/dev-rt"
Когда пользователь просматривает данные по классификации дома
То ему доступен просмотр истории изменений по этому полю
И в истории изменений пользователь видит информацию измененных значений по аттрибутам <аттрибуты>

Примеры: 
 | аттрибуты                     |
 | Наименование параметра        |
 | Описание измененного атрибута |
 | Значение                      |
 | Дата начала действия значения |
 | Дата установки значения       |
 | Пользователь                  |


 Сценарий: переименование поля "Общая площадь МКД" в "Общая площадь"
 Когда пользователь заходит в общии сведения по дому
 Тогда в списке полей есть поле с названием "Общая площадь" и нет поля с названием "Общая площадь МКД"


