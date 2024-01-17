create extension if not exists "uuid-ossp";

create table if not exists users(
    id uuid default uuid_generate_v4() primary key,
    email text not null,
    salt text not null,
    password text not null,
    nickname text not null,
    token text,
    token_expire_date numeric,
    registration_date numeric not null
);
create index on users(id);
create index on users(email);
create index on users(email, password);
create index on users(token, token_expire_date);
--password: 11111111
insert into users(id, email, salt, password, nickname, registration_date)
values(
    'dced1acd-b907-47e0-9659-77cb2c95e0aa', 
    'admin@keyboard.ru', 
    'hL36LBnKvP8whM0QFhuFQn82GSBJbPXT',
    '72653061b3aefd326e2e71f7affc9ccd1d1473fc6e35d5b4936d87c587b96dcff1729b65a57e7aaf95b964ac325fac56d7ef626cb9ea4fcad0287045176ed96e',
    'admin1',
    1705443770.40067
);

create table if not exists email_codes(
    id serial primary key,
    email text not null,
    code text not null,
    expire_date numeric not null
);
create index on email_codes(id);
create index on email_codes(email, code, expire_date);

create table if not exists keyboards(
    id uuid default uuid_generate_v4() primary key,
    owner_id uuid not null references users(id),
    title text not null,
    description text,
    file_path text not null,
    creation_date numeric not null
);
create index on keyboards(id);
create index on keyboards(owner_id);
create index on keyboards(owner_id, title);
insert into keyboards(owner_id, title, description, file_path, creation_date)
values(
    'dced1acd-b907-47e0-9659-77cb2c95e0aa',
    'Клавиатура по умолчанию №1',
    'Клавиатура размерностью 60%',
    '/files/default/keyboards/60percent.glb',
    1
);
insert into keyboards(owner_id, title, description, file_path, creation_date)
values(
    'dced1acd-b907-47e0-9659-77cb2c95e0aa',
    'Клавиатура по умолчанию №2',
    'Описание...',
    '/files/default/keyboards/nonkeyboard.glb',
    1
);