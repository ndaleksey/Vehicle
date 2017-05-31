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
  state_flags INT DEFAULT 0 NOT NULL, -- состояние выполнения задания

  PRIMARY KEY (id),
  UNIQUE (display_name)
);

CREATE TABLE obstacle -- препятствия
(
  id uuid NOT NULL,
  display_name TEXT NOT NULL,
  geometry TEXT NOT NULL, -- должно быть Polygon либо MultiPolygon в формате WKT
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

CREATE FUNCTION send_insert_notify() RETURNS TRIGGER AS
$$
DECLARE
  info XML;
BEGIN
  info =
    XMLELEMENT(NAME "notification", XMLATTRIBUTES(TG_OP as "op", TG_TABLE_NAME AS "table-name"),
      XMLELEMENT(NAME "new", XMLATTRIBUTES(NEW.id AS "id")));

  PERFORM pg_notify('nkb_vs', CAST(info AS TEXT));
  RETURN NULL;
END;
$$
LANGUAGE plpgsql;

CREATE FUNCTION send_update_notify() RETURNS TRIGGER AS
$$
DECLARE
  info XML;
BEGIN
  info =
    XMLELEMENT(NAME "notification", XMLATTRIBUTES(TG_OP as "op", TG_TABLE_NAME AS "table-name"),
      XMLELEMENT(NAME "old", XMLATTRIBUTES(OLD.id AS "id")),
      XMLELEMENT(NAME "new", XMLATTRIBUTES(NEW.id AS "id")));

  PERFORM pg_notify('nkb_vs', CAST(info AS TEXT));
  RETURN NULL;
END;
$$
LANGUAGE plpgsql;

CREATE FUNCTION send_delete_notify() RETURNS TRIGGER AS
$$
DECLARE
  info XML;
BEGIN
  info =
    XMLELEMENT(NAME "notification", XMLATTRIBUTES(TG_OP as "op", TG_TABLE_NAME AS "table-name"),
      XMLELEMENT(NAME "old", XMLATTRIBUTES(OLD.id AS "id")));

  PERFORM pg_notify('nkb_vs', CAST(info AS TEXT));
  RETURN NULL;
END;
$$
LANGUAGE plpgsql;

CREATE TRIGGER unmanned_vehicle_ai AFTER INSERT
ON unmanned_vehicle
FOR EACH ROW
EXECUTE PROCEDURE send_insert_notify();

CREATE TRIGGER unmanned_vehicle_au AFTER UPDATE
ON unmanned_vehicle
FOR EACH ROW
EXECUTE PROCEDURE send_update_notify();

CREATE TRIGGER unmanned_vehicle_ad AFTER DELETE
ON unmanned_vehicle
FOR EACH ROW
EXECUTE PROCEDURE send_delete_notify();

INSERT INTO unmanned_vehicle(id, display_name, address, x, y, heading, speed, state_flags) VALUES
  ('ff23b6ce-f2b7-4108-ad5a-039d4220ac7c', 'Танк №1', '123', 5, 65, 60, 3, 0);
