CREATE TABLE People (
Id INT PRIMARY KEY IDENTITY,
[Name] NVarChar(200) NOT NULL,
Picture VarBinary(MAX) CHECK(Picture < 2000000),
Height DECIMAL(15,2),
[Weight] DECIMAL(15,2),
Gender BIT NOT NULL,
Birthdate DATE NOT NULL,
Biography NVarChar(MAX)
)


INSERT INTO People ( [Name], Picture, Height, [Weight], Gender, Birthdate, Biography) VALUES
('Ilko', null, 1.75, 65, 1, '2000-02-23', null),
('Memo', null, 1.85, 7065, 1, '2000-06-23', null),
('Kosyo', null, 1.85, 7065, 1, '2001-06-23', null),
('Sasho', null, 1.75, 7065, 1, '2000-03-23', null),
('Pesho', null, 1.95, 7065, 1, '2001-03-23', null)

