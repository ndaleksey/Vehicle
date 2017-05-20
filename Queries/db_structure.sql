DROP SCHEMA IF EXISTS nkb_vs CASCADE;

CREATE SCHEMA nkb_vs;

SET search_path TO nkb_vs;

CREATE TABLE beacons -- точки возврата
(
  id uuid NOT NULL,
  display_name TEXT NOT NULL,
  x DOUBLE PRECISION NOT NULL, -- долгота
  y DOUBLE PRECISION NOT NULL, -- широта

  PRIMARY KEY (id),
  UNIQUE (display_name)
);

CREATE TABLE commanders -- пользователи, являющиеся командирами
(
  id uuid NOT NULL,

  PRIMARY KEY (id)
);

CREATE TABLE obstacles -- препятствия
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

CREATE TABLE remote_control_vehicles -- пункты дистанционного управления
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

CREATE TABLE routes -- маршруты
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

CREATE TABLE session_beacons -- редактируемые точки возврата
(
  id uuid NOT NULL,
  beacon_id uuid NOT NULL,

  PRIMARY KEY (id, beacon_id),
  UNIQUE (beacon_id)
);

CREATE TABLE session_obstacles -- редактируемые препятствия
(
  id uuid NOT NULL,
  obstacle_id uuid NOT NULL,

  PRIMARY KEY (id, obstacle_id),
  UNIQUE (obstacle_id)
);

CREATE TABLE session_remote_control_vehicles -- редактируемые пункты дистанционного управления
(
  id uuid NOT NULL,
  remote_control_vehicle_id uuid NOT NULL,

  PRIMARY KEY (id, remote_control_vehicle_id),
  UNIQUE (remote_control_vehicle_id)
);

CREATE TABLE session_routes -- редактируемые маршруты
(
  id uuid NOT NULL,
  route_id uuid NOT NULL,

  PRIMARY KEY (id, route_id),
  UNIQUE (route_id)
);

CREATE TABLE session_unmanned_vehicles -- редактируемые робототехнические комплексы
(
  id uuid NOT NULL,
  unmanned_vehicle_id uuid NOT NULL,

  PRIMARY KEY (id, unmanned_vehicle_id),
  UNIQUE (unmanned_vehicle_id)
);

CREATE TABLE sessions -- сеансы редактирования
(
  id uuid NOT NULL,
  user_id uuid NOT NULL,
  created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP NOT NULL, -- время создания
  valid_before TIMESTAMP NOT NULL, -- время, до которого сеанс должен быть завершен

  PRIMARY KEY (id)
);

CREATE TABLE unmanned_vehicle_messages -- сообщения, полученные от робототехнических комплексов
(
  id uuid NOT NULL,
  ordinal BIGINT NOT NULL, -- порядковый номер
  received_at TIMESTAMP NOT NULL, -- время получения
  payload XML NOT NULL, -- содержание полученного сообщения

  PRIMARY KEY (id, ordinal)
);

CREATE TABLE unmanned_vehicles -- робототехнические комплексы
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

CREATE TABLE user_windows_principals -- имена пользователей для автоматического входа в домен Windows
(
  id uuid NOT NULL,
  principal_name TEXT NOT NULL,

  PRIMARY KEY (id, principal_name),
  UNIQUE (principal_name)
);

CREATE TABLE users -- пользователи (все)
(
  id uuid NOT NULL,
  display_name TEXT NOT NULL,
  password_md5 TEXT NOT NULL,

  PRIMARY KEY (id),
  UNIQUE (display_name)
);

ALTER TABLE commanders
  ADD FOREIGN KEY (id) REFERENCES users(id) ON UPDATE CASCADE ON DELETE CASCADE;

ALTER TABLE sessions
  ADD FOREIGN KEY (user_id) REFERENCES users(id) ON UPDATE RESTRICT ON DELETE RESTRICT;

ALTER TABLE session_beacons
  ADD FOREIGN KEY (id) REFERENCES sessions(id) ON UPDATE CASCADE ON DELETE CASCADE,
  ADD FOREIGN KEY (beacon_id) REFERENCES beacons(id) ON UPDATE RESTRICT ON DELETE RESTRICT;

ALTER TABLE session_obstacles
  ADD FOREIGN KEY (id) REFERENCES sessions(id) ON UPDATE CASCADE ON DELETE CASCADE,
  ADD FOREIGN KEY (obstacle_id) REFERENCES obstacles(id) ON UPDATE RESTRICT ON DELETE RESTRICT;

ALTER TABLE session_remote_control_vehicles
  ADD FOREIGN KEY (id) REFERENCES sessions(id) ON UPDATE CASCADE ON DELETE CASCADE,
  ADD FOREIGN KEY (remote_control_vehicle_id) REFERENCES remote_control_vehicles(id) ON UPDATE RESTRICT ON DELETE RESTRICT;

ALTER TABLE session_routes
  ADD FOREIGN KEY (id) REFERENCES sessions(id) ON UPDATE CASCADE ON DELETE CASCADE,
  ADD FOREIGN KEY (route_id) REFERENCES routes(id) ON UPDATE RESTRICT ON DELETE RESTRICT;

ALTER TABLE session_unmanned_vehicles
  ADD FOREIGN KEY (id) REFERENCES sessions(id) ON UPDATE CASCADE ON DELETE CASCADE,
  ADD FOREIGN KEY (unmanned_vehicle_id) REFERENCES unmanned_vehicles(id) ON UPDATE RESTRICT ON DELETE RESTRICT;

ALTER TABLE unmanned_vehicle_messages
  ADD FOREIGN KEY (id) REFERENCES unmanned_vehicles(id) ON UPDATE CASCADE ON DELETE CASCADE;

ALTER TABLE user_windows_principals
  ADD FOREIGN KEY (id) REFERENCES users(id) ON UPDATE CASCADE ON DELETE CASCADE;