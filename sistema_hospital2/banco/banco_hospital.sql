create database if not exists hospital_miguelv;
use hospital;

create table if not exists pacientes (
    id int auto_increment primary key,
    nome varchar(200) not null,
    idade int not null,
    preferencial boolean not null default false
);
