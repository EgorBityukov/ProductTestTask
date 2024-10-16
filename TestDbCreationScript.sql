CREATE DATABASE TestDb;
GO

USE TestDb;
GO

CREATE TABLE Product (
    ID UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    Name NVARCHAR(255) NOT NULL,
    Description NVARCHAR(MAX) NULL,
    CONSTRAINT UQ_Product_Name UNIQUE (Name)
);
GO


CREATE NONCLUSTERED INDEX IX_Product_Name
ON Product (Name)
WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON);
GO


CREATE TABLE ProductVersion (
    ID UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    ProductID UNIQUEIDENTIFIER NOT NULL,
    Name NVARCHAR(255) NOT NULL,
    Description NVARCHAR(MAX) NULL,
    CreatingDate DATETIME NOT NULL DEFAULT GETUTCDATE(),
    Width FLOAT NOT NULL CHECK (Width > 0),
    Height FLOAT NOT NULL CHECK (Height > 0),
    Length FLOAT NOT NULL CHECK (Length > 0),
    CONSTRAINT FK_ProductVersion_Product FOREIGN KEY (ProductID) REFERENCES Product (ID) ON DELETE CASCADE
);
GO


CREATE NONCLUSTERED INDEX IX_ProductVersion_Name
ON ProductVersion (Name)
WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON);

CREATE NONCLUSTERED INDEX IX_ProductVersion_CreatingDate
ON ProductVersion (CreatingDate)
WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON);

CREATE NONCLUSTERED INDEX IX_ProductVersion_Width
ON ProductVersion (Width)
WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON);

CREATE NONCLUSTERED INDEX IX_ProductVersion_Height
ON ProductVersion (Height)
WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON);

CREATE NONCLUSTERED INDEX IX_ProductVersion_Length
ON ProductVersion (Length)
WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON);
GO


CREATE TABLE EventLog (
    ID UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    EventDate DATETIME NOT NULL DEFAULT GETUTCDATE(),
    Description NVARCHAR(MAX) NULL
);
GO


CREATE NONCLUSTERED INDEX IX_EventLog_EventDate
ON EventLog (EventDate)
WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON);
GO


CREATE TRIGGER TR_Product_Insert_Update_Delete
ON Product
AFTER INSERT, UPDATE, DELETE
AS
IF EXISTS ( SELECT 0 FROM Deleted )
BEGIN
   IF EXISTS ( SELECT 0 FROM Inserted )
   BEGIN
      INSERT  INTO EventLog (ID, Description)
      SELECT  NEWID(), CONCAT('UPDATE Product: ID:',u.ID, ' Name:',u.Name,' Description:',u.Description)
      FROM inserted as u
   END
ELSE
   BEGIN
      INSERT  INTO EventLog (ID, Description)
      SELECT  NEWID(), CONCAT('DELETE Product: ID:',u.ID, ' Name:',u.Name,' Description:',u.Description)
      FROM deleted as u
   END
   END
ELSE
   BEGIN
      INSERT  INTO EventLog (ID, Description)
      SELECT  NEWID(), CONCAT('INSERT Product: ID:',u.ID, ' Name:',u.Name,' Description:',u.Description)
      FROM inserted as u
   END
GO

CREATE TRIGGER TR_ProductVersion_Insert_Update_Delete
ON ProductVersion
AFTER INSERT, UPDATE, DELETE
AS
IF EXISTS ( SELECT 0 FROM Deleted )
BEGIN
   IF EXISTS ( SELECT 0 FROM Inserted )
   BEGIN
      INSERT  INTO EventLog (ID, Description)
      SELECT  NEWID(), CONCAT('UPDATE ProductVersion: ID:',u.ID, ' ProductID:',u.ProductID,' Name:',u.Name,' Description:'
	  ,u.Description,' CreatingDate:',u.CreatingDate,' Width:', u.Height,' Height:', u.Width,' Length:', u.Length)
      FROM inserted as u
   END
ELSE
   BEGIN
      INSERT  INTO EventLog (ID, Description)
      SELECT  NEWID(), CONCAT('DELETE ProductVersion: ID:',u.ID, ' ProductID:',u.ProductID,' Name:',u.Name,' Description:'
	  ,u.Description,' CreatingDate:',u.CreatingDate,' Width:', u.Height,' Height:', u.Width,' Length:', u.Length)
      FROM deleted as u
   END
   END
ELSE
   BEGIN
      INSERT  INTO EventLog (ID, Description)
      SELECT  NEWID(), CONCAT('INSERT ProductVersion: ID:',u.ID, ' ProductID:',u.ProductID,' Name:',u.Name,' Description:'
	  ,u.Description,' CreatingDate:',u.CreatingDate,' Width:', u.Height,' Height:', u.Width,' Length:', u.Length)
      FROM inserted as u
   END
GO


CREATE FUNCTION GetProductVersions (
    @ProductName NVARCHAR(255),
    @ProductVersionName NVARCHAR(255),
    @MinVolume FLOAT,
    @MaxVolume FLOAT
)
RETURNS TABLE
AS
RETURN
(
    SELECT 
	   p.[ID]			AS ID
      ,p.[Name]			AS ProductName
      ,p.[Description]	AS ProductDescription
	  ,pv.[ID]			AS ProductVersionID
      ,pv.[ProductID]	AS ProductID
      ,pv.[Name]		AS ProductVersionName
      ,pv.[Description] AS ProductVersionDescription
      ,pv.[CreatingDate]
      ,pv.[Width]
      ,pv.[Height]
      ,pv.[Length]
    FROM Product p
    JOIN ProductVersion pv ON p.ID = pv.ProductID
    WHERE p.Name LIKE '%' + @ProductName + '%'
      AND pv.Name LIKE '%' + @ProductVersionName + '%'
      AND (@MinVolume IS NULL OR @MinVolume>=(pv.Width * pv.Height * pv.Length)) 
	  AND (@MaxVolume IS NULL OR @MaxVolume<=(pv.Width * pv.Height * pv.Length))
);
GO



INSERT INTO Product (ID, Name, Description)
VALUES
    (NEWID(), 'Product 1', 'Description for Product 1'),
    (NEWID(), 'Product 2', 'Description for Product 2'),
    (NEWID(), 'Product 3', 'Description for Product 3'),
    (NEWID(), 'Product 4', NULL),
    (NEWID(), 'Product 5', 'Description for Product 5');

INSERT INTO ProductVersion (ID, ProductID, Name, Description, CreatingDate, Width, Height, Length)
VALUES
    (NEWID(), (SELECT ID FROM Product WHERE Name = 'Product 1'), 'Version 1.1', 'Description for Version 1.1', GETDATE(), 10, 20, 30),
    (NEWID(), (SELECT ID FROM Product WHERE Name = 'Product 1'), 'Version 1.2', 'Description for Version 1.2', GETDATE(), 15, 25, 35),
    (NEWID(), (SELECT ID FROM Product WHERE Name = 'Product 1'), 'Version 1.3', NULL, GETDATE(), 20, 30, 40), 

    (NEWID(), (SELECT ID FROM Product WHERE Name = 'Product 2'), 'Version 2.1', 'Description for Version 2.1', GETDATE(), 40, 50, 60),
    (NEWID(), (SELECT ID FROM Product WHERE Name = 'Product 2'), 'Version 2.2', 'Description for Version 2.2', GETDATE(), 45, 55, 65),

    (NEWID(), (SELECT ID FROM Product WHERE Name = 'Product 3'), 'Version 3.1', 'Description for Version 3.1', GETDATE(), 30, 40, 50),
    (NEWID(), (SELECT ID FROM Product WHERE Name = 'Product 3'), 'Version 3.2', NULL, GETDATE(), 35, 45, 55), 

    (NEWID(), (SELECT ID FROM Product WHERE Name = 'Product 4'), 'Version 4.1', 'Description for Version 4.1', GETDATE(), 50, 60, 70),

    (NEWID(), (SELECT ID FROM Product WHERE Name = 'Product 5'), 'Version 5.1', 'Description for Version 5.1', GETDATE(), 60, 70, 80),
    (NEWID(), (SELECT ID FROM Product WHERE Name = 'Product 5'), 'Version 5.2', 'Description for Version 5.2', GETDATE(), 65, 75, 85);
GO
