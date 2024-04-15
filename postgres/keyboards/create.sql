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
copy users(id, email, salt, password, nickname, registration_date)
from '/postgres/keyboards/init/users.csv'
delimiter ','
csv header;

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
copy switches(id, title, file_name, sound_name, preview_name)
from '/postgres/keyboards/init/switches.csv'
delimiter ','
csv header;

create table if not exists box_types(
    id uuid default uuid_generate_v4() primary key,
    title text not null
);
create index on box_types(id);
copy box_types(id, title)
from '/postgres/keyboards/init/box_types.csv'
delimiter ','
csv header;

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
create index on box_types(id);
copy keyboards(owner_id, switch_type_id, box_type_id, title, file_name, preview_name, creation_date)
from '/postgres/keyboards/init/keyboards.csv'
delimiter ','
csv header;

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
copy boxes(owner_id, type_id, title, file_name, preview_name, creation_date)
from '/postgres/keyboards/init/boxes.csv'
delimiter ','
csv header;

create table if not exists kits(
    id uuid default uuid_generate_v4() primary key,
    owner_id uuid not null references users(id),
    title text not null,
    creation_date numeric not null
);
create index on kits(id);
create index on kits(owner_id);
copy kits(id, owner_id, title, creation_date)
from '/postgres/keyboards/init/kits.csv'
delimiter ','
csv header;

create table if not exists keycaps(
    id uuid default uuid_generate_v4() primary key,
    kit_id uuid not null references kits(id),
    title text not null,
    file_name text not null,
    preview_name text not null,
    creation_date numeric not null
);
create index on keycaps(id);
create index on keycaps(kit_id);
copy keycaps(kit_id, title, file_name, preview_name, creation_date)
from '/postgres/keyboards/init/keycaps.csv'
delimiter ','
csv header;