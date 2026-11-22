CREATE DATABASE IF NOT EXISTS catalogo_videojuegos
  CHARACTER SET utf8mb4
  COLLATE utf8mb4_unicode_ci;

USE catalogo_videojuegos;

DROP TABLE IF EXISTS Usuarios;

CREATE TABLE Usuarios (
    id INT AUTO_INCREMENT PRIMARY KEY,
    nombre_usuario VARCHAR(50) NOT NULL UNIQUE,
    password VARCHAR(255) NOT NULL
);

INSERT INTO Usuarios (nombre_usuario, password) VALUES
('admin', '1234'),
('gamer', 'portal2026'),
('profesor', 'apto2026');