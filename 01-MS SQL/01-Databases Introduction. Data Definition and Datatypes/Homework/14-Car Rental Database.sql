CREATE DATABASE CarRental

USE CarRental

CREATE TABLE Categories (
Id INT PRIMARY KEY IDENTITY,
CategoryName NVarChar(30) NOT NULL,
DailyRate INT,
WeeklyRate INT,
MonthlyRate INT,
WeekendRate INT
)

INSERT INTO Categories (CategoryName, DailyRate, WeeklyRate, MonthlyRate, WeekendRate) VALUES
('Category 1', NULL, NULL, NULL, NULL),
('Category 2', NULL, NULL, NULL, NULL),
('Category 3', NULL, NULL, NULL, NULL)

CREATE TABLE Cars (
Id INT PRIMARY KEY IDENTITY,
PlateNumber NVarChar(10) NOT NULL,
Manufacturer NVarChar(30) NOT NULL,
Model NVarChar(30) NOT NULL,
CarYear INT NOT NULL,
CategoryId INT FOREIGN KEY REFERENCES Categories(Id),
Doors INT,
Picture VARBINARY(MAX),
Condition NVarChar(50),
Avaible BIT NOT NULL
)

INSERT INTO Cars (PlateNumber, Manufacturer, Model, CarYear, CategoryId, Doors, Picture, Condition, Avaible) VALUES
('BN1142DV', 'BMW', 'e49', 2003, 1, 4, NULL, NULL, 1),
('BN1342DV', 'Mercedes', 'benz', 2008, 1, 2, NULL, NULL, 0),
('BN4242DV', 'Renault', 'Megane', 2012, 1, 4, NULL, NULL, 1)

CREATE TABLE Employees (
Id INT PRIMARY KEY IDENTITY,
FirstName NVarChar(30) NOT NULL,
LastName NVarChar(30) NOT NULL,
Title NVarChar(30) NOT NULL,
Notes NVarChar(100)
)

INSERT INTO Employees (FirstName, LastName, Title, Notes) VALUES
('Ilker', 'Yumer', 'Web Developer', NULL),
('Pesho', 'Peshev', 'Trainer', NULL),
('Sasho', 'Sashev', 'Game Developer', NULL)

CREATE TABLE Customers (
Id INT PRIMARY KEY IDENTITY,
DriverLicenceNumber NVarChar(30) NOT NULL,
FullName NVarChar(30) NOT NULL,
[Address] NVarChar(30) NOT NULL,
City NVarChar(30) NOT NULL,
ZIPCode INT,
Notes NVarChar(100)
)

INSERT INTO Customers (DriverLicenceNumber, FullName, [Address], City, ZIPCode, Notes) VALUES
('034A239AD', 'Test Testov', 'Test Street Num.8', 'Test City', 1099, NULL),
('034A277SD', 'Admin Adminov', 'Admin Street Num.2', 'Admin City', 1019, NULL),
('233D239AD', 'Gosho Ivanov', 'Test Street Num.8', 'Test City', 1099, NULL);

CREATE TABLE RentalOrders (
Id INT PRIMARY KEY IDENTITY,
EmployeeId INT FOREIGN KEY REFERENCES Employees(Id),
CustomerId INT FOREIGN KEY REFERENCES Customers(Id),
CarId INT FOREIGN KEY REFERENCES Cars(Id),
TankLevel INT NOT NULL,
KilometrageStart INT NOT NULL,
KilometrageEnd INT NOT NULL,
TotalKilometrage INT NOT NULL,
StartDate DATE NOT NULL,
EndDate DATE NOT NULL,
TotalDays INT,
RateApplied DECIMAL(18,2) NOT NULL,
TaxRate DECIMAL(18,2) NOT NULL,
OrderStatus NVarChar(30) NOT NULL,
Notes NVarChar(100)
)

INSERT INTO RentalOrders VALUES
(1, 1, 1, 100, 10000, 10200, 200, '2019-01-01', '2019-01-05', 5, 200, 40, 'Completed', NULL),
(2, 2, 2, 70, 1000, 1100, 100, '2019-01-01', '2019-01-03', 3, 120, 40, 'Completed', NULL),
(3, 3, 3, 100, 10, 50, 40, '2019-03-04', '2019-03-05', 1, 20, 20, 'Completed', NULL);