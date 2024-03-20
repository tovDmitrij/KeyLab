create extension if not exists "uuid-ossp";


create table if not exists pages(
    id uuid default uuid_generate_v4() primary key,
    title text not null,
    tag text not null,
    description text
);
create index on pages(id);
create index on pages(tag);


create table if not exists users_activity(
    id uuid default uuid_generate_v4() primary key,
    page_id uuid not null references pages(id),
    user_id uuid not null,
    date numeric not null
);
create index on users_activity(page_id);
create index on users_activity(user_id);


insert into pages(title, tag)
values('Авторизация', 'sign_in');
insert into pages(title, tag)
values('Регистрация', 'sign_up');
insert into pages(title, tag)
values('', '');