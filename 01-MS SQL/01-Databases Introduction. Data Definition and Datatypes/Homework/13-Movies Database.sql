CREATE DATABASE Movies

USE Movies

CREATE TABLE Directors (
Id INT PRIMARY KEY IDENTITY,
DirectorName NVarChar(30) NOT NULL,
Notes NVarChar(MAX)
)

CREATE TABLE Genres (
Id INT PRIMARY KEY IDENTITY,
GenreName NVarChar(30) NOT NULL,
Notes NVarChar(100)
)

CREATE TABLE Categories (
Id INT PRIMARY KEY IDENTITY,
CategoryName NVarChar(30) NOT NULL,
Notes NVarChar(100)
)

CREATE TABLE Movies (
Id INT PRIMARY KEY IDENTITY,
Title NVarChar(30) NOT NULL,
DirectorId INT NOT NULL FOREIGN KEY REFERENCES Directors(Id),
CopyrightYear INT,
[Length] NVarChar(50) NOT NULL,
GenreId INT NOT NULL FOREIGN KEY REFERENCES Genres(Id),
CategoryId INT NOT NULL FOREIGN KEY REFERENCES Categories(Id),
Rating INT,
Notes NVarChar(100)
)

INSERT INTO Directors(DirectorName, Notes) VALUES
('Steven Spielberg', NULL),
('Martin Scorsese', NULL),
('Alfred Hitchcock', NULL),
('Quentin Tarantino', NULL),
('Stanley Kubrick', NULL)

INSERT INTO Genres(GenreName, Notes) VALUES
('Comedy', NULL),
('Drama', NULL),
('Action', NULL),
('Thriller', NULL),
('Western', NULL)

INSERT INTO Categories(CategoryName, Notes) VALUES
('Car Racing', NULL),
('Box Matches', NULL),
('Biography', NULL),
('History', NULL),
('Prison Escape', NULL)

INSERT INTO Movies(Title, DirectorId, CopyrightYear, [Length], GenreId, CategoryId, Rating, Notes) VALUES
('Movie 0', 2, 2012, '01:50:00', 1, 2, NULL, NULL),
('Movie 1', 1, 2002, '01:50:00', 2, 1, NULL, NULL),
('Movie 2', 3, 2014, '01:50:00', 3, 3, NULL, NULL),
('Movie 3', 5, 2011, '01:50:00', 1, 4, NULL, NULL),
('Movie 4', 3, 2016, '01:50:00', 4, 5, NULL, NULL)
