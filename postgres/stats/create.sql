create extension if not exists "uuid-ossp";


create table if not exists intervals(
    id uuid default uuid_generate_v4() primary key,
    title text not null,
    seconds integer not null
);
create index on intervals(id);





insert into intervals(title, seconds) values('15 минут', 1);
insert into intervals(title, seconds) values('1 час', 1);
insert into intervals(title, seconds) values('1 день', 1);
insert into intervals(title, seconds) values('1 неделя', 1);
insert into intervals(title, seconds) values('1 месяц', 1);