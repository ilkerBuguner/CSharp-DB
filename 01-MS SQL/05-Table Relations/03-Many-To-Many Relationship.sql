CREATE TABLE Students(
StudentID INT PRIMARY KEY IDENTITY,
[Name] NVARCHAR(30) NOT NULL
)

INSERT INTO Students VALUES
('Mila'),
('Toni'),
('Ron')

CREATE TABLE Exams(
ExamID INT PRIMARY KEY NOT NULL,
[Name] NVARCHAR(30) NOT NULL
)

INSERT INTO Exams (ExamID, [Name]) VALUES
(101, 'SpringMVC'),
(102, 'Neo4j'),
(103, 'Oracle 11g')

CREATE TABLE StudentsExams (
StudentID INT FOREIGN KEY REFERENCES Students(StudentID),
ExamID INT FOREIGN KEY REFERENCES Exams(ExamID)
CONSTRAINT PK_StudentExams PRIMARY KEY (StudentID, ExamID)
)

INSERT INTO StudentsExams([StudentID], [ExamID]) VALUES
(1, 101),
(1, 102),
(2, 101),
(3, 103),
(2, 102),
(2, 103);