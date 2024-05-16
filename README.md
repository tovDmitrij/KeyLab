<img src="https://github.com/tovDmitrij/keyboards/blob/main/docs/preview.jpeg" />

# :keyboard: Веб-приложение для конструирования виртуальных механических клавиатур

## Описание проекта
TBD

## :notebook: Документация
**API** - см. README файл в папке /api;

**Client** - см. README файл в папке /client;

**PostgreSQL** - см. README файлы в папке /postgres.

## :rocket: Запуск (Docker)
1. Установить Docker;
2. Скачать архив с файлами [по ссылке](https://drive.google.com/drive/folders/1msr1UAy3w1_vsBq9aF4zVs5QZepKXcow?usp=sharing) и распаковать его в папку *files*;
3. В файле *configurations/production/api.json* в блоке *Email* определить *Login* и *Password* приложения, привязанного к аккаунту Google (см. [справочник Google](https://support.google.com/accounts/answer/185833?hl=en));
4. Запустить скрипт *docker-production.bash*.

*Пояснение:*
```
compose-production - необходим для запуска релизной версии ПО;
compose-dev - необходим для тестирования ПО.
```

## :computer: Технологический стек
```
NGINX
Docker
ASP.NET Core 8
Redis 7.2.4
PostgreSQL 16.0
RabbitMQ
ReactTS
ThreeJS
```