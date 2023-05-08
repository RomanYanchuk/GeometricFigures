CREATE DATABASE IF NOT EXISTS figures CHARACTER SET utf8mb4 COLLATE utf8mb4_bin;
DROP USER IF EXISTS 'developer'@'%';
CREATE USER 'developer'@'%' IDENTIFIED BY 'password';
GRANT ALL PRIVILEGES ON figures.* TO 'developer'@'%';
FLUSH PRIVILEGES;
USE figures;
