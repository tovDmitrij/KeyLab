create extension if not exists "uuid-ossp";


create or replace function GetDefaultUserID() returns uuid as
    $$ begin
        return 'dced1acd-b907-47e0-9659-77cb2c95e0aa';
    end; $$ language plpgsql;



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
    GetDefaultUserID(), 
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



create table if not exists switches(
    id uuid default uuid_generate_v4() primary key,
    title text not null,
    description text not null,
    file_path text not null,
    sound_path text not null
);
create index on switches(id);
insert into switches(id, title, description, file_path, sound_path)
values(
    'b0ac9399-8eb1-4920-9366-82cbf7904eb1',
    'MX Blue',
    'Свитч MX Blue',
    'mxblue.glb',
    'mxblue.mp3'
);
insert into switches(id, title, description, file_path, sound_path)
values(
    'f876e294-c56b-40d3-9ac2-9f85eb532de6',
    'MX Brown',
    'Свитч MX Brown',
    'mxbrown.glb',
    'mxbrown.mp3'
);



create table if not exists box_types(
    id uuid default uuid_generate_v4() primary key,
    title text not null,
    description text not null
);
create index on box_types(id);
insert into box_types(id, title, description)
values(
    'f27d815d-8702-4853-9df8-482a95bd6aaa',
    '100%',
    'Размерность клавиатуры 100%'
);
insert into box_types(id, title, description)
values(
    '809c62fe-8c6a-4ae4-b90d-9b112cbba86d',
    '75%',
    'Размерность клавиатуры 75%'
);
insert into box_types(id, title, description)
values(
    '63a9640a-8763-4101-8294-5b37e796bb9b',
    '60%',
    'Размерность клавиатуры 60%'
);
insert into box_types(id, title, description)
values(
    '782f1e2b-5eaa-4452-ae82-0427fbecaefd',
    '40%',
    'Размерность клавиатуры 40%'
);



create table if not exists keyboards(
    id uuid default uuid_generate_v4() primary key,
    owner_id uuid not null references users(id),
    switch_type_id uuid not null references switches(id),
    box_type_id uuid not null references box_types(id),
    title text not null,
    description text,
    file_path text not null,
    creation_date numeric not null
);
create index on keyboards(id);
create index on keyboards(owner_id);
insert into keyboards(id, owner_id, switch_type_id, box_type_id, title, description, file_path, creation_date)
values(
    'd296c943-4894-484a-b0c3-9b3783accbaa',
    GetDefaultUserID(),
    'b0ac9399-8eb1-4920-9366-82cbf7904eb1',
    '63a9640a-8763-4101-8294-5b37e796bb9b',
    'Клавиатура по умолчанию №1',
    'Клавиатура размерностью 60%',
    GetDefaultUserID() || '/keyboards/60percent.glb',
    1706024855
);
insert into keyboards(id, owner_id, switch_type_id, box_type_id, title, description, file_path, creation_date)
values(
    '6e8ac55c-d2de-47d2-8794-a864d14af1ce',
    GetDefaultUserID(),
    'f876e294-c56b-40d3-9ac2-9f85eb532de6',
    '782f1e2b-5eaa-4452-ae82-0427fbecaefd',
    'Клавиатура по умолчанию №2',
    'Описание...',
    GetDefaultUserID() || '/keyboards/nonkeyboard.glb',
    1706026855
);



create table if not exists boxes(
    id uuid default uuid_generate_v4() primary key,
    owner_id uuid not null references users(id),
    type_id uuid not null references box_types(id),
    title text not null,
    description text,
    file_path text not null,
    creation_date numeric not null
);
create index on boxes(id);
create index on boxes(owner_id);
create index on boxes(type_id);
insert into boxes(id, owner_id, type_id, title, description, file_path, creation_date)
values(
    'df1c24c4-7212-4651-bff6-793ab9c4e34f',
    GetDefaultUserID(),
    '63a9640a-8763-4101-8294-5b37e796bb9b',
    'Бокс №1',
    'КоробОчка',
    GetDefaultUserID() || '/boxes/60percent.glb',
    1706026855
);
insert into boxes(id, owner_id, type_id, title, description, file_path, creation_date)
values(
    'd0a45e0a-ebe1-4190-82e6-c4563aacdd41',
    GetDefaultUserID(),
    '63a9640a-8763-4101-8294-5b37e796bb9b',
    'Бокс №2',
    'Основание со свитчами',
    GetDefaultUserID() || '/boxes/60percentswitches.glb',
    1706026855
);



create table if not exists kits(
    id uuid default uuid_generate_v4() primary key,
    owner_id uuid not null references users(id),
    title text not null,
    description text not null,
    creation_date numeric not null
);
create index on kits(id);
create index on kits(owner_id);