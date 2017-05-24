DROP SCHEMA IF EXISTS nkb_vs CASCADE;

CREATE SCHEMA nkb_vs;

SET search_path TO nkb_vs;

CREATE TABLE beacon -- точки возврата
(
  id uuid NOT NULL,
  display_name TEXT NOT NULL,
  x DOUBLE PRECISION NOT NULL, -- долгота
  y DOUBLE PRECISION NOT NULL, -- широта

  PRIMARY KEY (id),
  UNIQUE (display_name)
);

CREATE TABLE mission -- маршрутное задание
(
  id uuid NOT NULL,
  display_name TEXT NOT NULL,
  description XML NOT NULL,

  PRIMARY KEY (id),
  UNIQUE (display_name)
);

CREATE TABLE obstacle -- препятствия
(
  id uuid NOT NULL,
  display_name TEXT NOT NULL,
  geometry XML NOT NULL,
  x_min DOUBLE PRECISION NOT NULL, -- западная долгота
  y_min DOUBLE PRECISION NOT NULL, -- южная широта
  x_max DOUBLE PRECISION NOT NULL, -- восточная долгота
  y_max DOUBLE PRECISION NOT NULL, -- северная широта

  PRIMARY KEY (id),
  UNIQUE (display_name)
);

CREATE TABLE remote_control_vehicle -- пункты дистанционного управления
(
  id uuid NOT NULL,
  display_name TEXT NOT NULL,
  address TEXT NOT NULL, -- номер, используемый для установки связи
  x DOUBLE PRECISION NOT NULL, -- долгота
  y DOUBLE PRECISION NOT NULL, -- широта
  heading DOUBLE PRECISION NOT NULL, -- курсовой угол в градусах по часовой стрелке, 0 соответствует северу
  speed DOUBLE PRECISION DEFAULT 0.0 NOT NULL, -- скорость в километрах в час
  state_flags INT DEFAULT 0 NOT NULL, -- работоспособность разных систем
  
  PRIMARY KEY (id),
  UNIQUE (display_name)
);

CREATE TABLE route -- маршруты
(
  id uuid NOT NULL,
  display_name TEXT NOT NULL,
  segments XML NOT NULL,
  x_min DOUBLE PRECISION NOT NULL, -- западная долгота
  y_min DOUBLE PRECISION NOT NULL, -- южная широта
  x_max DOUBLE PRECISION NOT NULL, -- восточная долгота
  y_max DOUBLE PRECISION NOT NULL, -- северная широта

  PRIMARY KEY (id),
  UNIQUE (display_name)
);

CREATE TABLE symbol -- условно-графические обозначения
(
  id uuid NOT NULL,
  display_name TEXT NOT NULL,
  geometry XML NOT NULL,
  type_name TEXT NOT NULL, -- название типа символа в соответствии с классификатором
  x_min DOUBLE PRECISION NOT NULL, -- западная долгота
  y_min DOUBLE PRECISION NOT NULL, -- южная широта
  x_max DOUBLE PRECISION NOT NULL, -- восточная долгота
  y_max DOUBLE PRECISION NOT NULL, -- северная широта

  PRIMARY KEY (id),
  UNIQUE (display_name)
);

CREATE TABLE unmanned_vehicle_message -- сообщения, полученные от робототехнических комплексов
(
  id uuid NOT NULL,
  unmanned_vehicle_id uuid NOT NULL,
  ordinal BIGINT NOT NULL, -- порядковый номер
  received_at TIMESTAMP NOT NULL, -- время получения
  payload XML NOT NULL, -- содержание полученного сообщения

  PRIMARY KEY (id),
  UNIQUE (unmanned_vehicle_id, ordinal)
);

CREATE TABLE unmanned_vehicle -- робототехнические комплексы
(
  id uuid NOT NULL,
  display_name TEXT NOT NULL,
  address TEXT NOT NULL, -- номер, используемый для установки связи
  x DOUBLE PRECISION NOT NULL, -- долгота
  y DOUBLE PRECISION NOT NULL, -- широта
  heading DOUBLE PRECISION NOT NULL, -- курсовой угол в градусах по часовой стрелке, 0 соответствует северу
  speed DOUBLE PRECISION DEFAULT 0.0 NOT NULL, -- скорость в километрах в час
  state_flags INT DEFAULT 0 NOT NULL, -- работоспособность разных систем
  
  PRIMARY KEY (id),
  UNIQUE (display_name),
  UNIQUE (address)
);

ALTER TABLE unmanned_vehicle_message
  ADD FOREIGN KEY (unmanned_vehicle_id) REFERENCES unmanned_vehicle(id) ON UPDATE CASCADE ON DELETE CASCADE;

CREATE INDEX unmanned_vehicle_message_received_at ON unmanned_vehicle_message (unmanned_vehicle_id, received_at);