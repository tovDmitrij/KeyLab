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

create table if not exists email_confirms(
    id serial primary key,
    email text not null,
    code text not null,
    expire_date numeric not null
);
create index on email_confirms(id);
create index on email_confirms(email, code, expire_date);



create table if not exists boxes(
    id integer primary key,
    x1 numeric not null,
    y1 numeric not null,
    z1 numeric not null,
    x2 numeric not null,
    y2 numeric not null,
    z2 numeric not null
);
create index on boxes(id);



create table if not exists base_types(
    id integer primary key,
    title text not null,
    tag text not null,
    description text not null
);
create index on base_types(id);
create index on base_types(tag);

create table if not exists base_boxes(
    base_type_id integer not null references base_types(id),
    box_id integer not null references boxes(id)
);
create index on base_boxes(base_type_id);
create index on base_boxes(base_type_id, box_id);

create table if not exists bases(
    id uuid default uuid_generate_v4() primary key,
    owner_id uuid not null references users(id),
    type_id integer not null references base_types(id),
    title text not null,
    link text not null
);
create index on bases(id);
create index on bases(owner_id);
create index on bases(type_id);



create table if not exists keyboards(
    id uuid default uuid_generate_v4() primary key,
    owner_id uuid not null references users(id),
    base_id uuid not null references bases(id),
    date numeric not null
);
create index on keyboards(id);
create index on keyboards(owner_id);



create table if not exists sets(
    id uuid default uuid_generate_v4() primary key,
    owner_id uuid not null references users(id),
    title text not null,
    description text not null,
    date numeric not null
);
create index on sets(id);
create index on sets(owner_id);



create table if not exists switch_types(
    id integer primary key,
    title text not null,
    tag text not null,
    description text not null
);
create index on switch_types(id);
create index on switch_types(tag);

create table if not exists switches(
    id uuid default uuid_generate_v4() primary key,
    type_id integer not null references switch_types(id),
    title text not null,
    link text not null
);
create index on switches(id);

create table if not exists keyboard_switches(
    id serial primary key,
    keyboard_id uuid not null references keyboards(id),
    switch_id uuid not null references keyboards(id),
    date numeric not null
);
create index on keyboard_switches(id);
create index on keyboard_switches(keyboard_id);



create table if not exists keycap_types(
    id integer primary key,
    title text not null,
    tag text not null,
    description text not null
);
create index on keycap_types(id);
create index on keycap_types(tag);

create table if not exists keycap_boxes(
    keycap_type_id integer not null references keycap_types(id),
    box_id integer not null references boxes(id)
);
create index on keycap_boxes(keycap_type_id);
create index on keycap_boxes(box_id);

create table if not exists keycaps(
    id uuid default uuid_generate_v4() primary key,
    type_id integer not null references keycap_types(id),
    title text not null,
    link text not null
);
create index on keycaps(id);

create table if not exists keyboard_keycaps(
    id serial primary key,
    keyboard_id uuid not null references keyboards(id),
    keycap_id uuid not null references keycaps(id),
    date numeric not null
);
create index on keyboard_keycaps(id);
create index on keyboard_keycaps(keyboard_id);

create table if not exists keycap_sets(
    id serial primary key,
    set_id uuid not null references sets(id),
    keycap_id uuid not null references keycaps(id),
    date numeric not null
);
create index on keycap_sets(id);
create index on keycap_sets(set_id);