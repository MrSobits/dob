﻿@ScenarioInTransaction
Функционал: тесткейсы граничных значений для добавления абонента в раздел "Реестр абонентов"
Региональный фонд - Абоненты - Реестр абонентов


Предыстория: 
Дано в реестр жилых домов добавлен новый дом
| region     | houseType       | city                                          | street             | houseNumber |
| testregion | Многоквартирный | Камчатский край, Алеутский р-н, с. Никольское | ул. 50 лет Октября | д. test     |

И у этого дома добавлено помещение
| RoomNum | Area | LivingArea | Type  | OwnershipType |
| 1       | 51   | 35         | Жилое | Частная       |

Дано добавлена организационно-правовая форма
| Name | Code | OkopfCode |
| тест | тест | тест      |

Дано добавлен контрагент с организационно правовой формой
| Name |
| тест |

@ignore 
#GKH-2669
Сценарий: неудачное добавление абонента типа Счет физ лица при незаполненных обязательных полях Имя
Дано пользователь добавляет абонента типа Счет физ.лица
И пользователь у этого абонента типа Счет физ.лица заполняет поле Фамилия "1"
И пользователь у этого абонента типа Счет физ.лица заполняет поле Серия документа "1111111"
И пользователь у этого абонента типа Счет физ.лица заполняет поле Номер документа "1"
Когда пользователь сохраняет этого абонента типа Счет физ.лица
Тогда запись по этому абоненту отсутствует в разделе абонентов типа Счет физ.лица
И падает ошибка с текстом "Не заполнены обязательные поля: Имя"

@ignore 
#GKH-2669
Сценарий: неудачное добавление абонента типа Счет физ лица при незаполненных обязательных полях Фамилия
Дано пользователь добавляет абонента типа Счет физ.лица
И пользователь у этого абонента типа Счет физ.лица заполняет поле Имя "Сергей"
И пользователь у этого абонента типа Счет физ.лица заполняет поле Серия документа "1111111"
И пользователь у этого абонента типа Счет физ.лица заполняет поле Номер документа "1"
Когда пользователь сохраняет этого абонента типа Счет физ.лица
Тогда запись по этому абоненту отсутствует в разделе абонентов типа Счет физ.лица
И падает ошибка с текстом "Не заполнены обязательные поля: Фамилия"


Сценарий: удачное добавление абонента типа Счет физ лица при вводе граничных условий в 100 знаков, Фамилия
Дано пользователь добавляет абонента типа Счет физ.лица
И пользователь у этого абонента типа Счет физ.лица заполняет поле Фамилия 100 символо "1"
И пользователь у этого абонента типа Счет физ.лица заполняет поле Серия документа "1111111"
И пользователь у этого абонента типа Счет физ.лица заполняет поле Номер документа "1"
И пользователь у этого абонента типа Счет физ.лица заполняет поле Имя "Сергей"
Когда пользователь сохраняет этого абонента типа Счет физ.лица
Тогда запись по этому абоненту присутствует в разделе абонентов типа Счет физ.лица

@ignore 
#GKH-2669
Сценарий: неудачное добавление абонента типа Счет физ лица при вводе граничных условий в 101 знаков, Фамилия
Дано пользователь добавляет абонента типа Счет физ.лица
И пользователь у этого абонента типа Счет физ.лица заполняет поле Имя "Иван"
И пользователь у этого абонента типа Счет физ.лица заполняет поле Серия документа "1111111"
И пользователь у этого абонента типа Счет физ.лица заполняет поле Номер документа "1"
И пользователь у этого абонента типа Счет физ.лица заполняет поле Фамилия 101 символо "1"
Когда пользователь сохраняет этого абонента типа Счет физ.лица
Тогда запись по этому абоненту отсутствует в разделе абонентов типа Счет физ.лица
И падает ошибка с текстом "Не заполнены обязательные поля: Фамилия"

Сценарий: удачное добавление абонента типа Счет физ лица при вводе граничных условий в 100 знаков, Имя
Дано пользователь добавляет абонента типа Счет физ.лица
И пользователь у этого абонента типа Счет физ.лица заполняет поле Фамилия "1"
И пользователь у этого абонента типа Счет физ.лица заполняет поле Серия документа "1111111"
И пользователь у этого абонента типа Счет физ.лица заполняет поле Номер документа "1"
И пользователь у этого абонента типа Счет физ.лица заполняет поле Имя 100 символов "1"
Когда пользователь сохраняет этого абонента типа Счет физ.лица
Тогда запись по этому абоненту присутствует в разделе абонентов типа Счет физ.лица

@ignore 
#GKH-2669
Сценарий: неудачное добавление абонента при вводе граничных условий в 101 знаков, Имя
Дано пользователь добавляет абонента типа Счет физ.лица
И пользователь у этого абонента типа Счет физ.лица заполняет поле Фамилия "1"
И пользователь у этого абонента типа Счет физ.лица заполняет поле Серия документа "1111111"
И пользователь у этого абонента типа Счет физ.лица заполняет поле Номер документа "1"
И пользователь у этого абонента типа Счет физ.лица заполняет поле Имя 101 символов "1"
Когда пользователь сохраняет этого абонента типа Счет физ.лица
Тогда запись по этому абоненту отсутствует в разделе абонентов типа Счет физ.лица
И падает ошибка с текстом "Не заполнены обязательные поля: Имя"

Сценарий: удачное добавление абонента при вводе граничных условий в 100 знаков, Отчество
Дано пользователь добавляет абонента типа Счет физ.лица
И пользователь у этого абонента типа Счет физ.лица заполняет поле Фамилия "1"
И пользователь у этого абонента типа Счет физ.лица заполняет поле Имя "1"
И пользователь у этого абонента типа Счет физ.лица заполняет поле Серия документа "1111111"
И пользователь у этого абонента типа Счет физ.лица заполняет поле Номер документа "1"
И пользователь у этого абонента типа Счет физ.лица заполняет поле Отчество 100 знаков "1"
Когда пользователь сохраняет этого абонента типа Счет физ.лица
Тогда запись по этому абоненту присутствует в разделе абонентов типа Счет физ.лица

@ignore 
#GKH-2669
Сценарий: неудачное добавление абонента при вводе граничных условий в 101 знаков, Отчество
Дано пользователь добавляет абонента типа Счет физ.лица
И пользователь у этого абонента типа Счет физ.лица заполняет поле Фамилия "1"
И пользователь у этого абонента типа Счет физ.лица заполняет поле Имя "1"
И пользователь у этого абонента типа Счет физ.лица заполняет поле Серия документа "1111111"
И пользователь у этого абонента типа Счет физ.лица заполняет поле Номер документа "1"
И пользователь у этого абонента типа Счет физ.лица заполняет поле Отчество 101 знаков "1"
Когда пользователь сохраняет этого абонента типа Счет физ.лица
Тогда запись по этому абоненту отсутствует в разделе абонентов типа Счет физ.лица
И падает ошибка с текстом "Не заполнены обязательные поля: Отчество"

Структура сценария: удачное добавление абонента при вводе граничных условий в 200 знаков, Серия документа
Дано пользователь добавляет абонента типа Счет физ.лица
И пользователь у этого абонента типа Счет физ.лица заполняет поле Фамилия "1"
И пользователь у этого абонента типа Счет физ.лица заполняет поле Имя "1"
И пользователь у этого абонента типа Счет физ.лица заполняет поле Отчество "1"
И пользователь у этого абонента типа Счет физ.лица заполняет поле Тип документа "<Тип документа>"
И пользователь у этого абонента типа Счет физ.лица заполняет поле Номер документа "1"
И пользователь у этого абонента типа Счет физ.лица заполняет поле Серия документа 200 символов "1"
Когда пользователь сохраняет этого абонента типа Счет физ.лица
Тогда запись по этому абоненту присутствует в разделе абонентов типа Счет физ.лица

Примеры: 
| Тип документа            |
| Паспорт                  |
| Свидетельство о рождении |

@ignore 
#GKH-2669
Структура сценария: неудачное добавление абонента при вводе граничных условий в 201 знаков, Серия документа
Дано пользователь добавляет абонента типа Счет физ.лица
И пользователь у этого абонента типа Счет физ.лица заполняет поле Фамилия "1"
И пользователь у этого абонента типа Счет физ.лица заполняет поле Имя "1"
И пользователь у этого абонента типа Счет физ.лица заполняет поле Отчество "1"
И пользователь у этого абонента типа Счет физ.лица заполняет поле Тип документа "<Тип документа>"
И пользователь у этого абонента типа Счет физ.лица заполняет поле Номер документа "1"
И пользователь у этого абонента типа Счет физ.лица заполняет поле Серия документа 201 символов "1"
Когда пользователь сохраняет этого абонента типа Счет физ.лица
Тогда запись по этому абоненту отсутствует в разделе абонентов типа Счет физ.лица
И падает ошибка с текстом "Не заполнены обязательные поля: Серия документа"

Примеры: 
| Тип документа            |
| Паспорт                  |
| Свидетельство о рождении |

Структура сценария: удачное добавление абонента при вводе граничных условий в 200 знаков, Номер документа
Дано пользователь добавляет абонента типа Счет физ.лица
И пользователь у этого абонента типа Счет физ.лица заполняет поле Фамилия "1"
И пользователь у этого абонента типа Счет физ.лица заполняет поле Имя "1"
И пользователь у этого абонента типа Счет физ.лица заполняет поле Отчество "1"
И пользователь у этого абонента типа Счет физ.лица заполняет поле Тип документа "<Тип документа>"
И пользователь у этого абонента типа Счет физ.лица заполняет поле Серия документа "1111111"
И пользователь у этого абонента типа Счет физ.лица заполняет поле Номер документа 200 символов "1"
Когда пользователь сохраняет этого абонента типа Счет физ.лица
Тогда запись по этому абоненту присутствует в разделе абонентов типа Счет физ.лица

Примеры: 
| Тип документа            |
| Паспорт                  |
| Свидетельство о рождении |

@ignore 
#GKH-2669
Структура сценария: неудачное добавление абонента при вводе граничных условий в 201 знаков, Номер документа
Дано пользователь добавляет абонента типа Счет физ.лица
И пользователь у этого абонента типа Счет физ.лица заполняет поле Фамилия "1"
И пользователь у этого абонента типа Счет физ.лица заполняет поле Имя "1"
И пользователь у этого абонента типа Счет физ.лица заполняет поле Отчество "1"
И пользователь у этого абонента типа Счет физ.лица заполняет поле Тип документа "<Тип документа>"
И пользователь у этого абонента типа Счет физ.лица заполняет поле Серия документа "1111111"
И пользователь у этого абонента типа Счет физ.лица заполняет поле Номер документа 201 символов "1"
Когда пользователь сохраняет этого абонента типа Счет физ.лица
Тогда запись по этому абоненту отсутствует в разделе абонентов типа Счет физ.лица
И падает ошибка с текстом "Не заполнены обязательные поля: Номер документа"

Примеры: 
| Тип документа            |
| Паспорт                  |
| Свидетельство о рождении |

