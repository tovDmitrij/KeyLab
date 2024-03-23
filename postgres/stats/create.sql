create extension if not exists "uuid-ossp";


create table if not exists intervals(
    id uuid default uuid_generate_v4() primary key,
    title text not null,
    seconds integer not null
);
create index on intervals(id);


create table if not exists activities(
    id uuid default uuid_generate_v4() primary key,
    title text not null,
    tag text not null
);
create index on activities(id);
create index on activities(tag);




insert into intervals(title, seconds) values('15 минут', 1);
insert into intervals(title, seconds) values('1 час', 1);
insert into intervals(title, seconds) values('1 день', 1);
insert into intervals(title, seconds) values('1 неделя', 1);
insert into intervals(title, seconds) values('1 месяц', 1);


insert into activities(title, tag) values('Обновление токена', 'refresh');

insert into activities(title, tag) values('Просмотр клавиатур', 'see_keyboard');
insert into activities(title, tag) values('Редактор клавиатур', 'edit_keyboard');

insert into activities(title, tag) values('Просмотр боксов', 'see_box');
insert into activities(title, tag) values('Редактор боксов', 'edit_box');

insert into activities(title, tag) values('Просмотр кейкапов', 'see_keycap');
insert into activities(title, tag) values('Редактор кейкапов', 'edit_keycap');

insert into activities(title, tag) values('Просмотр свитчей', 'see_switch');