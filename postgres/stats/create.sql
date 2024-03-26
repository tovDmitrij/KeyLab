create extension if not exists "uuid-ossp";

create table if not exists intervals(
    id uuid default uuid_generate_v4() primary key,
    title text not null,
    seconds integer not null
);
create index on intervals(id);
copy intervals(title, seconds)
from '/postgres/stats/init/intervals.csv'
delimiter ','
csv header;

create table if not exists activities(
    id uuid default uuid_generate_v4() primary key,
    title text not null,
    tag text not null
);
create index on activities(id);
create index on activities(tag);
copy activities(title, tag)
from '/postgres/stats/init/activities.csv'
delimiter ','
csv header;