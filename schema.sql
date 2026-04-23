USE master;
GO

IF DB_ID('BarberiaTurnosDb') IS NULL
BEGIN
    CREATE DATABASE BarberiaTurnosDb;
END
GO

USE BarberiaTurnosDb;
GO

IF OBJECT_ID('dbo.Auditoria', 'U') IS NOT NULL DROP TABLE dbo.Auditoria;
IF OBJECT_ID('dbo.Usuarios', 'U') IS NOT NULL DROP TABLE dbo.Usuarios;
IF OBJECT_ID('dbo.Roles', 'U') IS NOT NULL DROP TABLE dbo.Roles;
GO

CREATE TABLE Roles (
    IdRol INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Nombre NVARCHAR(50) NOT NULL UNIQUE
);
GO

CREATE TABLE Usuarios (
    IdUsuario INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Username NVARCHAR(50) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(200) NOT NULL,
    PasswordSalt NVARCHAR(200) NOT NULL,
    NombreCompleto NVARCHAR(MAX) NULL,
    Email NVARCHAR(MAX) NULL,
    IdRol INT NOT NULL,
    Activo BIT NOT NULL CONSTRAINT DF_Usuarios_Activo DEFAULT (1),
    Idioma NVARCHAR(5) NOT NULL CONSTRAINT DF_Usuarios_Idioma DEFAULT ('ES'),
    UltimoLogin DATETIME2 NULL,
    FechaAlta DATETIME2 NOT NULL CONSTRAINT DF_Usuarios_FechaAlta DEFAULT (SYSDATETIME()),
    CONSTRAINT FK_Usuarios_Roles FOREIGN KEY (IdRol) REFERENCES Roles(IdRol)
);
GO

CREATE TABLE Auditoria (
    IdAuditoria INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    IdUsuario INT NULL,
    Evento NVARCHAR(100) NOT NULL,
    Detalle NVARCHAR(400) NOT NULL,
    FechaHora DATETIME2 NOT NULL CONSTRAINT DF_Auditoria_FechaHora DEFAULT (SYSDATETIME()),
    CONSTRAINT FK_Auditoria_Usuarios FOREIGN KEY (IdUsuario) REFERENCES Usuarios(IdUsuario) ON DELETE SET NULL
);
GO

INSERT INTO Roles (Nombre) VALUES
('Administrador'),
('Recepcionista'),
('Barbero'),
('Cliente');
GO

INSERT INTO Usuarios (Username, PasswordHash, PasswordSalt, NombreCompleto, Email, IdRol, Activo, Idioma, UltimoLogin)
VALUES
('admin', 'zsv0t3c8Fv79yZpmbfS1stLbcpXQjrlGgOzNW1Mpm6o=', 'QmFyYmVyaWFTYWx0MjAyNg==', NULL, NULL, 1, 1, 'ES', NULL);
GO