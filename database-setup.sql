-- Create Database
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'StudentDb')
BEGIN
    CREATE DATABASE StudentDb;
END
GO

USE StudentDb;
GO

-- Create Students Table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Students')
BEGIN
    CREATE TABLE Students (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Name NVARCHAR(100) NOT NULL,
        Email NVARCHAR(100) NOT NULL
    );
END
GO

-- Insert Sample Data (Optional)
IF NOT EXISTS (SELECT * FROM Students)
BEGIN
    INSERT INTO Students (Name, Email) VALUES 
    ('John Doe', 'john.doe@example.com'),
    ('Jane Smith', 'jane.smith@example.com');
END
GO
