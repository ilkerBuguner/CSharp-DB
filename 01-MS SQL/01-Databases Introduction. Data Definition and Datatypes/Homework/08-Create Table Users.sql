CREATE TABLE Users (
Id INT PRIMARY KEY IDENTITY,
Username VarChar(30) UNIQUE NOT NULL,
[Password] VarChar(26) NOT NULL,
ProfilePicture VarBinary(MAX) CHECK(ProfilePicture < 72000000),
LastLoginTime DATETIME,
IsDeleted BIT
)

INSERT INTO Users(Username, [Password], ProfilePicture, LastLoginTime, IsDeleted) VALUES
('Ilko', '2434', null, null, 1),
('Pesho', '2454', null, null, 0),
('Gosho', '2414', null, null, 1),
('Sasho', '234', null, null, 0),
('Kosyo', '244', null, null, 1)
