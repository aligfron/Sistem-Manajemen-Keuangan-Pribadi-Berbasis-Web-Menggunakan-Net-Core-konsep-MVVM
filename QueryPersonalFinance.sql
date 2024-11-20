create database db_personal_finance
use db_personal_finance

-------------------------

-- Tabel Users
CREATE TABLE Users (
    UserId INT PRIMARY KEY IDENTITY(1,1),
    Username NVARCHAR(50) NOT NULL,
    Password NVARCHAR(255) NOT NULL,
    Email NVARCHAR(100) NOT NULL,
    FullName NVARCHAR(100) NOT NULL,
    Role NVARCHAR(20) NOT NULL,
    CreatedBy int,
    CreatedOn DATETIME DEFAULT GETDATE(),
    ModifiedBy NVARCHAR(50),
    ModifiedOn DATETIME,
    DeletedBy NVARCHAR(50),
    IsDeleted BIT DEFAULT 0,
    DeletedOn DATETIME
);
select * from Users
-- Tabel Transactions
CREATE TABLE Transactions (
    TransactionId INT PRIMARY KEY IDENTITY(1,1),
    UserId INT,
    Amount DECIMAL(18,2) NOT NULL,
    Type NVARCHAR(20) CHECK (Type IN ('Pemasukan', 'Pengeluaran')) NOT NULL,
    Category NVARCHAR(50) NOT NULL,
    Waktu DATETIME NOT NULL,
    Sumber_gaji NVARCHAR(50) NOT NULL,
    CreatedBy NVARCHAR(50),
    CreatedOn DATETIME DEFAULT GETDATE(),
    ModifiedBy NVARCHAR(50),
    ModifiedOn DATETIME,
    DeletedBy NVARCHAR(50),
    IsDeleted BIT DEFAULT 0,
    DeletedOn DATETIME
);

-- Tabel Budget
CREATE TABLE Budget (
    BudgetId INT PRIMARY KEY IDENTITY(1,1),
    UserId INT ,
    Category NVARCHAR(50) NOT NULL,
    Amount DECIMAL(18,2) NOT NULL,
    Month INT CHECK (Month BETWEEN 1 AND 12) NOT NULL,
    Year INT NOT NULL,
    CreatedBy NVARCHAR(50),
    CreatedOn DATETIME DEFAULT GETDATE(),
    ModifiedBy NVARCHAR(50),
    ModifiedOn DATETIME,
    DeletedBy NVARCHAR(50),
    IsDeleted BIT DEFAULT 0,
    DeletedOn DATETIME
);

-- Tabel MoneySources
CREATE TABLE MoneySources (
    SourceId INT PRIMARY KEY IDENTITY(1,1),
    UserId INT ,
    SourceName NVARCHAR(50) NOT NULL,
    Amount DECIMAL(18,2) NOT NULL,
    CreatedBy NVARCHAR(50),
    CreatedOn DATETIME DEFAULT GETDATE(),
    ModifiedBy NVARCHAR(50),
    ModifiedOn DATETIME,
    DeletedBy NVARCHAR(50),
    IsDeleted BIT DEFAULT 0,
    DeletedOn DATETIME
);

-- Tabel Reports (Opsional)
CREATE TABLE Reports (
    ReportId INT PRIMARY KEY IDENTITY(1,1),
    UserId INT ,
    ReportType NVARCHAR(50) NOT NULL,
    GeneratedAt DATETIME DEFAULT GETDATE(),
    ReportData NVARCHAR(MAX),
    CreatedBy NVARCHAR(50),
    CreatedOn DATETIME DEFAULT GETDATE(),
    ModifiedBy NVARCHAR(50),
    ModifiedOn DATETIME,
    DeletedBy NVARCHAR(50),
    IsDeleted BIT DEFAULT 0,
    DeletedOn DATETIME
);
