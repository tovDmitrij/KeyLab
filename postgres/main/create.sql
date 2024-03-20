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
    file_name text not null,
    sound_name text not null,
    preview_name text not null
);
create index on switches(id);


create table if not exists box_types(
    id uuid default uuid_generate_v4() primary key,
    title text not null
);
create index on box_types(id);


create table if not exists keyboards(
    id uuid default uuid_generate_v4() primary key,
    owner_id uuid not null references users(id),
    switch_type_id uuid not null references switches(id),
    box_type_id uuid not null references box_types(id),
    title text not null,
    file_name text not null,
    preview_name text not null,
    creation_date numeric not null
);
create index on keyboards(id);
create index on keyboards(owner_id);


create table if not exists boxes(
    id uuid default uuid_generate_v4() primary key,
    owner_id uuid not null references users(id),
    type_id uuid not null references box_types(id),
    title text not null,
    file_name text not null,
    preview_name text not null,
    creation_date numeric not null
);
create index on boxes(id);
create index on boxes(owner_id);
create index on boxes(type_id);


create table if not exists kits(
    id uuid default uuid_generate_v4() primary key,
    owner_id uuid not null references users(id),
    title text not null,
    creation_date numeric not null
);
create index on kits(id);
create index on kits(owner_id);



create table if not exists keycaps(
    id uuid default uuid_generate_v4() primary key,
    kit_id uuid not null references kits(id),
    title text not null,
    file_name text not null,
    preview_name text not null,
    creation_date text not null
);
create index on keycaps(id);
create index on keycaps(kit_id);





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


insert into switches(id, title, file_name, sound_name, preview_name)
values(
    '0abbfce9-8dfa-419a-8407-aca20ae26b3c',
    'MX Black',
    'mxblack.glb',
    'mxblack.mp3',
    'mxblack.jpeg'
);
insert into switches(id, title, file_name, sound_name, preview_name)
values(
    'b0ac9399-8eb1-4920-9366-82cbf7904eb1',
    'MX Blue',
    'mxblue.glb',
    'mxblue.mp3',
    'mxblue.jpeg'
);
insert into switches(id, title, file_name, sound_name, preview_name)
values(
    'f876e294-c56b-40d3-9ac2-9f85eb532de6',
    'MX Brown',
    'mxbrown.glb',
    'mxbrown.mp3',
    'mxblue.jpeg'
);
insert into switches(id, title, file_name, sound_name, preview_name)
values(
    '556eaccc-d524-4343-a4a2-6202f00f4b4d',
    'MX Red',
    'mxred.glb',
    'mxred.mp3',
    'mxred.jpeg'
);


insert into box_types(id, title)
values(
    'f27d815d-8702-4853-9df8-482a95bd6aaa',
    '100%'
);
insert into box_types(id, title)
values(
    '809c62fe-8c6a-4ae4-b90d-9b112cbba86d',
    '75%'
);
insert into box_types(id, title)
values(
    '63a9640a-8763-4101-8294-5b37e796bb9b',
    '60%'
);
insert into box_types(id, title)
values(
    '782f1e2b-5eaa-4452-ae82-0427fbecaefd',
    '40%'
);


insert into keyboards(owner_id, switch_type_id, box_type_id, title, file_name, preview_name, creation_date)
values(
    GetDefaultUserID(),
    'b0ac9399-8eb1-4920-9366-82cbf7904eb1',
    '63a9640a-8763-4101-8294-5b37e796bb9b',
    'Клавиатура по умолчанию №1',
    '60percent.glb',
    '60percent.jpeg',
    1706024855
);
insert into keyboards(owner_id, switch_type_id, box_type_id, title, file_name, preview_name, creation_date)
values(
    GetDefaultUserID(),
    'f876e294-c56b-40d3-9ac2-9f85eb532de6',
    '782f1e2b-5eaa-4452-ae82-0427fbecaefd',
    'Клавиатура по умолчанию №2',
    'nonkeyboard.glb',
    'nonkeyboard.jpeg',
    1706022855
);
insert into keyboards(owner_id, switch_type_id, box_type_id, title, file_name, preview_name, creation_date)
values(
    GetDefaultUserID(),
    'f876e294-c56b-40d3-9ac2-9f85eb532de6',
    '782f1e2b-5eaa-4452-ae82-0427fbecaefd',
    'Клавиатура по умолчанию №3',
    'anotherKeyboard.glb',
    'anotherKeyboard.jpeg',
    1706016455
);


insert into boxes(owner_id, type_id, title, file_name, preview_name, creation_date)
values(
    GetDefaultUserID(),
    '63a9640a-8763-4101-8294-5b37e796bb9b',
    'Бокс №1',
    '60percent.glb',
    '60percent.jpeg',
    1706026855
);
insert into boxes(owner_id, type_id, title, file_name, preview_name, creation_date)
values(
    GetDefaultUserID(),
    '63a9640a-8763-4101-8294-5b37e796bb9b',
    'Бокс №2',
    '60percentswitches.glb',
    '60percentswitches.jpeg',
    1706026855
);


insert into kits(id, owner_id, title, creation_date)
values(
    'd296c943-4894-484a-b0c3-9b3783accbaa',
    GetDefaultUserID(),
    'Базовый набор',
    1706026855
);
insert into keycaps(kit_id, title, file_name, preview_name, creation_date)
values(
    'd296c943-4894-484a-b0c3-9b3783accbaa',
    'Backspace',
    'backspace.glb',
    'backspace.jpeg',
    1706026855
);
insert into keycaps(kit_id, title, file_name, preview_name, creation_date)
values(
    'd296c943-4894-484a-b0c3-9b3783accbaa',
    'Capslock',
    'capslock.glb',
    'capslock.jpeg',
    1706026855
);
insert into keycaps(kit_id, title, file_name, preview_name, creation_date)
values(
    'd296c943-4894-484a-b0c3-9b3783accbaa',
    'Other',
    'ctrl_alt_win_others.glb',
    'ctrl_alt_win_others.jpeg',
    1706026855
);
insert into keycaps(kit_id, title, file_name, preview_name, creation_date)
values(
    'd296c943-4894-484a-b0c3-9b3783accbaa',
    'Default',
    'default.glb',
    'default.jpeg',
    1706026855
);
insert into keycaps(kit_id, title, file_name, preview_name, creation_date)
values(
    'd296c943-4894-484a-b0c3-9b3783accbaa',
    'Enter',
    'enter.glb',
    'enter.jpeg',
    1706026855
);
insert into keycaps(kit_id, title, file_name, preview_name, creation_date)
values(
    'd296c943-4894-484a-b0c3-9b3783accbaa',
    'LeftShift',
    'left_shift.glb',
    'left_shift.jpeg',
    1706026855
);
insert into keycaps(kit_id, title, file_name, preview_name, creation_date)
values(
    'd296c943-4894-484a-b0c3-9b3783accbaa',
    'RightShift',
    'right_shift.glb',
    'right_shift.jpeg',
    1706026855
);
insert into keycaps(kit_id, title, file_name, preview_name, creation_date)
values(
    'd296c943-4894-484a-b0c3-9b3783accbaa',
    'Slashes',
    'slashes.glb',
    'slashes.jpeg',
    1706026855
);
insert into keycaps(kit_id, title, file_name, preview_name, creation_date)
values(
    'd296c943-4894-484a-b0c3-9b3783accbaa',
    'Space',
    'space.glb',
    'space.jpeg',
    1706026855
);
insert into keycaps(kit_id, title, file_name, preview_name, creation_date)
values(
    'd296c943-4894-484a-b0c3-9b3783accbaa',
    'Tab',
    'tab.glb',
    'tab.jpeg',
    1706026855
);