IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'EmployeeDB')
BEGIN
    CREATE DATABASE EmployeeDB;
END
GO

USE EmployeeDB;
GO

IF OBJECT_ID('dbo.Phones', 'U') IS NOT NULL
    DROP TABLE dbo.Phones;
GO

IF OBJECT_ID('dbo.Employees', 'U') IS NOT NULL
    DROP TABLE dbo.Employees;
GO

CREATE TABLE Employees
(
    Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    FirstName NVARCHAR(100) NOT NULL,
    LastName NVARCHAR(100) NOT NULL,
    Email NVARCHAR(150) NOT NULL,
    DocumentNumber NVARCHAR(50) NOT NULL,
    Password NVARCHAR(255) NOT NULL,
    Role INT NOT NULL,
    Status INT NOT NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    UpdatedAt DATETIME2 NULL,
    ManagerId UNIQUEIDENTIFIER NULL
);
GO

ALTER TABLE Employees
ADD CONSTRAINT FK_Employees_Manager
FOREIGN KEY (ManagerId)
REFERENCES Employees(Id)
ON DELETE NO ACTION;
GO

CREATE TABLE Phones
(
    Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,

    EmployeeId UNIQUEIDENTIFIER NOT NULL,
    Number NVARCHAR(20) NOT NULL,
    Type INT NOT NULL,
    IsPrimary BIT NOT NULL
);
GO

ALTER TABLE Phones
ADD CONSTRAINT FK_Phones_Employees
FOREIGN KEY (EmployeeId)
REFERENCES Employees(Id)
ON DELETE CASCADE;
GO

INSERT INTO Employees ( Id, FirstName, LastName, Email, DocumentNumber, Password, Role, Status, CreatedAt, UpdatedAt, ManagerId ) VALUES ( '11111111-1111-1111-1111-111111111111', 'Admin', 'Master', 'admin@employee.com', '00000000000',  'Admin@123', 4, 1, GETUTCDATE(), NULL, NULL );