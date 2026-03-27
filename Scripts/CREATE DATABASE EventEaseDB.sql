-- ============================================
-- CRIAÇÃO DO BANCO DE DADOS
-- ============================================
CREATE DATABASE EventEaseDB;
GO

USE EventEaseDB;
GO

-- ============================================
-- TABELA DE USUÁRIOS (INSCRIÇÃO / LOGIN)
-- ============================================
CREATE TABLE Users (
    UserId INT IDENTITY(1,1) PRIMARY KEY,
    FullName NVARCHAR(150) NOT NULL,
    Email NVARCHAR(150) UNIQUE NOT NULL,
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE()
);

-- ============================================
-- TABELA DE EVENTOS
-- ============================================
CREATE TABLE Events (
    EventId INT IDENTITY(1,1) PRIMARY KEY,
    EventName NVARCHAR(200) NOT NULL,
    EventDescription NVARCHAR(MAX) NULL,
    EventDate DATE NOT NULL,
    EventLocation NVARCHAR(200) NOT NULL,
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE()
);

-- ============================================
-- TABELA DE INSCRIÇÕES EM EVENTOS
-- (Relaciona Usuários a Eventos)
-- ============================================
CREATE TABLE EventRegistrations (
    RegistrationId INT IDENTITY(1,1) PRIMARY KEY,
    UserId INT NOT NULL,
    EventId INT NOT NULL,
    RegistrationDate DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_Registration_User FOREIGN KEY (UserId) REFERENCES Users(UserId),
    CONSTRAINT FK_Registration_Event FOREIGN KEY (EventId) REFERENCES Events(EventId),
    CONSTRAINT UC_User_Event UNIQUE (UserId, EventId)  -- evita inscrição duplicada
);

-- ============================================
-- TABELA DE SESSÕES (STATE MANAGEMENT)
-- ============================================
CREATE TABLE UserSessions (
    SessionId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    UserId INT NOT NULL,
    SessionData NVARCHAR(MAX) NULL, -- pode armazenar JSON com estados diversos
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
    ExpiresAt DATETIME NOT NULL,
    CONSTRAINT FK_Session_User FOREIGN KEY (UserId) REFERENCES Users(UserId)
);

-- ============================================
-- TABELA DE PRESENÇA EM EVENTOS
-- ============================================
CREATE TABLE Attendance (
    AttendanceId INT IDENTITY(1,1) PRIMARY KEY,
    UserId INT NOT NULL,
    EventId INT NOT NULL,
    CheckInTime DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_Attendance_User FOREIGN KEY (UserId) REFERENCES Users(UserId),
    CONSTRAINT FK_Attendance_Event FOREIGN KEY (EventId) REFERENCES Events(EventId)
);