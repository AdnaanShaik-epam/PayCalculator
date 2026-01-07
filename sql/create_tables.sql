-- Creates Employees and Attendances tables for PayCalculator

CREATE TABLE Employees (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(200) NOT NULL,
    HourlyPay DECIMAL(18,2) NULL
);

CREATE TABLE Attendances (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    EmployeeId INT NOT NULL FOREIGN KEY REFERENCES Employees(Id),
    LoginTime DATETIME2 NOT NULL,
    LogoutTime DATETIME2 NULL,
    BreakMinutes INT DEFAULT 0,
    CreatedAt DATETIME2 DEFAULT SYSUTCDATETIME(),
    CONSTRAINT CK_LogoutAfterLogin CHECK (LogoutTime IS NULL OR LogoutTime >= LoginTime)
);
