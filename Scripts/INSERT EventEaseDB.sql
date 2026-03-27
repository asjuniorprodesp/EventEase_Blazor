USE EventEaseDB;
GO

-- ============================================
-- INSERIR USUÁRIOS
-- ============================================
INSERT INTO Users (FullName, Email) VALUES
('Ana Carvalho', 'ana.carvalho@example.com'),
('Bruno Almeida', 'bruno.almeida@example.com'),
('Carla Oliveira', 'carla.oliveira@example.com'),
('Diego Martins', 'diego.martins@example.com');

-- ============================================
-- INSERIR EVENTOS
-- ============================================
INSERT INTO Events (EventName, EventDescription, EventDate, EventLocation) VALUES
('Summit de Inovação 2026', 'Evento corporativo focado em novas tecnologias.', '2026-05-10', 'São Paulo - SP'),
('Workshop de Liderança', 'Treinamento intensivo para líderes de equipe.', '2026-06-02', 'Rio de Janeiro - RJ'),
('Gala Anual EventEase', 'Evento social de networking e celebração.', '2026-07-20', 'Belo Horizonte - MG'),
('Tech Day 2026', 'Palestras sobre desenvolvimento e tendências de TI.', '2026-08-15', 'Curitiba - PR');

-- ============================================
-- INSERIR INSCRIÇÕES EM EVENTOS
-- (Cada usuário inscrito em eventos diferentes)
-- ============================================
INSERT INTO EventRegistrations (UserId, EventId) VALUES
(1, 1),  -- Ana no Summit
(1, 3),  -- Ana na Gala
(2, 2),  -- Bruno no Workshop
(3, 1),  -- Carla no Summit
(3, 4),  -- Carla no Tech Day
(4, 4);  -- Diego no Tech Day

-- ============================================
-- INSERIR SESSÕES DE USUÁRIOS
-- (Simulação de estado do usuário em JSON)
-- ============================================
INSERT INTO UserSessions (UserId, SessionData, ExpiresAt) VALUES
(1, '{ "theme": "dark", "lastPage": "/event/1" }', DATEADD(HOUR, 2, GETDATE())),
(2, '{ "theme": "light", "lastPage": "/event/2" }', DATEADD(HOUR, 2, GETDATE())),
(3, '{ "theme": "dark", "lastPage": "/home" }', DATEADD(HOUR, 2, GETDATE())),
(4, '{ "theme": "light", "lastPage": "/event/4" }', DATEADD(HOUR, 2, GETDATE()));

-- ============================================
-- INSERIR REGISTROS DE PRESENÇA
-- (Simula usuários fazendo check-in nos eventos)
-- ============================================
INSERT INTO Attendance (UserId, EventId, CheckInTime) VALUES
(1, 1, GETDATE()),
(2, 2, GETDATE()),
(3, 1, GETDATE()),
(3, 4, GETDATE()),
(4, 4, GETDATE());