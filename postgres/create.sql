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