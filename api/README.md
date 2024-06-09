API имеет следующий функционал:
1) процесс подтверждения почты перед регистрацией нового аккаунта;
2) авторизация с использованием JWT токенов;
3) интерфейс взаимодействия с 3D-моделями согласно функциональным
требованиям, описанным в README на главной странице репозитория;
4) автоматический сбор статистики действий пользователей;
5) интерфейс взаимодействия со статистикой для администратора

API построен на микросервисной архитектуре, что включает в себя следующие микросервисы:
1) рассылки почтовых писем (api_email);
2) взаимодействия с аккаунтами пользователей (api_users);
3) работы с клавиатурами и их составляющими (api_keyboards);
4) сбора и анализа статистики действий пользователей (api_stats).
   
![image](https://github.com/tovDmitrij/KeyLab/assets/86602542/2a7b5743-ead8-4028-a096-edd65d4bddac)

Доступ к микросервисам извне происходит через прокси-сервер на NGINX по http-протоколу (api_gateway). Микросервисы общаются между собой посредством брокера сообщений (rabbitmq). Некоторые микросервисы имеют компоненты БД PostgreSQL и БД кэширования данных Redis.

Код в микросервисах подчиняется слоистой архитектуре, которая приведена ниже.

![image](https://github.com/tovDmitrij/KeyLab/assets/86602542/8b1f2d5f-7e67-48a0-9c06-6e29c0f06a8f)

# Компоненты микросервиса api_email
<img src="https://github.com/tovDmitrij/keyboards/blob/main/docs/class/class_email_consumer.svg" />

# Компоненты микросервиса api_users
<img src="https://github.com/tovDmitrij/keyboards/blob/main/docs/class/class_verification_controller.svg" />

<img src="https://github.com/tovDmitrij/keyboards/blob/main/docs/class/class_user_controller.svg" />

# Компоненты микросервиса api_keyboards
<img src="https://github.com/tovDmitrij/keyboards/blob/main/docs/class/class_switch_controller.svg" />

<img src="https://github.com/tovDmitrij/keyboards/blob/main/docs/class/class_box_controller.svg" />

<img src="https://github.com/tovDmitrij/keyboards/blob/main/docs/class/class_keycap_controller.svg" />

<img src="https://github.com/tovDmitrij/keyboards/blob/main/docs/class/class_kit_controller.svg" />

<img src="https://github.com/tovDmitrij/keyboards/blob/main/docs/class/class_keyboard_controller.svg" />

# Компоненты микросервиса api_stats
<img src="https://github.com/tovDmitrij/keyboards/blob/main/docs/class/class_stat_consumer.svg" />

<img src="https://github.com/tovDmitrij/keyboards/blob/main/docs/class/class_interval_controller.svg" />

<img src="https://github.com/tovDmitrij/keyboards/blob/main/docs/class/class_activity_controller.svg" />

<img src="https://github.com/tovDmitrij/keyboards/blob/main/docs/class/class_stat_controller.svg" />
