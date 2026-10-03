-- =====================================================
-- DAT_LESSON_14_LAYOUT - Bài tập tự làm 2
-- Đinh Anh Tuấn - 2410900083
-- (Không bắt buộc chạy: ứng dụng tự tạo DB bằng EnsureCreated)
-- =====================================================
IF DB_ID(N'DatLesson14Db') IS NULL CREATE DATABASE DatLesson14Db;
GO
USE DatLesson14Db;
GO

CREATE TABLE DatCategory (
    Id          INT IDENTITY(1,1) PRIMARY KEY,
    Name        NVARCHAR(100) NOT NULL UNIQUE,
    Status      TINYINT NOT NULL DEFAULT 1,
    CreatedDate DATE NOT NULL DEFAULT CAST(GETDATE() AS date),
    Image       VARCHAR(100) NULL,
    Description NVARCHAR(350) NULL
);

CREATE TABLE DatProduct (
    Id          INT IDENTITY(1,1) PRIMARY KEY,
    Name        NVARCHAR(100) NOT NULL UNIQUE,
    Price       FLOAT NOT NULL,
    SalePrice   FLOAT NOT NULL DEFAULT 0,
    Status      TINYINT NOT NULL DEFAULT 1,
    CategoryId  INT NOT NULL FOREIGN KEY REFERENCES DatCategory(Id),
    CreatedDate DATE NOT NULL DEFAULT CAST(GETDATE() AS date),
    Image       VARCHAR(100) NULL,
    Description NVARCHAR(350) NULL
);

CREATE TABLE DatBanner (
    Id          INT IDENTITY(1,1) PRIMARY KEY,
    Name        NVARCHAR(100) NOT NULL UNIQUE,
    Status      TINYINT NOT NULL DEFAULT 1,
    Priority    INT NOT NULL DEFAULT 0,
    Image       VARCHAR(100) NULL,
    Description NVARCHAR(350) NULL
);

CREATE TABLE DatBlog (
    Id          INT IDENTITY(1,1) PRIMARY KEY,
    Name        NVARCHAR(100) NOT NULL UNIQUE,
    Status      TINYINT NOT NULL DEFAULT 1,
    CreatedDate DATE NOT NULL DEFAULT CAST(GETDATE() AS date),
    Image       VARCHAR(100) NULL,
    Description NVARCHAR(350) NULL
);
GO
