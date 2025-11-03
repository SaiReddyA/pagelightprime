IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'PageLightPrime')
BEGIN
    CREATE DATABASE PageLightPrime;
    PRINT 'Database PageLightPrime created.';
END
ELSE
BEGIN
    PRINT 'Database PageLightPrime already exists.';
END
GO

USE PageLightPrime;
GO

IF EXISTS (SELECT * FROM sys.types WHERE name = 'udt_ID')
    DROP TYPE udt_ID;
IF EXISTS (SELECT * FROM sys.types WHERE name = 'udt_Name')
    DROP TYPE udt_Name;
IF EXISTS (SELECT * FROM sys.types WHERE name = 'udt_Remarks')
    DROP TYPE udt_Remarks;
IF EXISTS (SELECT * FROM sys.types WHERE name = 'udt_ActiveFlag')
    DROP TYPE udt_ActiveFlag;
IF EXISTS (SELECT * FROM sys.types WHERE name = 'udt_DateTime2')
    DROP TYPE udt_DateTime2;
GO


CREATE TYPE udt_ID FROM INT NOT NULL;
CREATE TYPE udt_Name FROM NVARCHAR(200) NOT NULL;
CREATE TYPE udt_Remarks FROM NVARCHAR(500) NULL;
CREATE TYPE udt_ActiveFlag FROM BIT;
CREATE TYPE udt_DateTime2 FROM DATETIME2;
GO


IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'dbo.Country') AND type = 'U')
BEGIN
    CREATE TABLE dbo.Country
    (
        CountryId udt_ID IDENTITY(1,1) PRIMARY KEY,
        CountryName udt_Name UNIQUE,
        IsActive udt_ActiveFlag NOT NULL DEFAULT(1),
        CreatedAt udt_DateTime2 NOT NULL DEFAULT GETUTCDATE(),
        ModifiedAt udt_DateTime2 NULL
    );
    PRINT 'Table Country created.';
END
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'dbo.State') AND type = 'U')
BEGIN
    CREATE TABLE dbo.State
    (
        StateId udt_ID IDENTITY(1,1) PRIMARY KEY,
        StateName udt_Name NOT NULL,
        CountryId udt_ID NOT NULL,
        IsActive udt_ActiveFlag NOT NULL DEFAULT(1),
        CreatedAt udt_DateTime2 NOT NULL DEFAULT SYSUTCDATETIME(),
        ModifiedAt udt_DateTime2 NULL,
        CONSTRAINT FK_State_Country FOREIGN KEY (CountryId) REFERENCES dbo.Country(CountryId)
    );
    CREATE UNIQUE INDEX UQ_State_CountryId_StateName ON dbo.State (CountryId, StateName);
    PRINT 'Table State created.';
END
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'dbo.District') AND type = 'U')
BEGIN
    CREATE TABLE dbo.District
    (
        DistrictId udt_ID IDENTITY(1,1) PRIMARY KEY,
        DistrictName udt_Name NOT NULL,
        StateId udt_ID NOT NULL,
        IsActive udt_ActiveFlag NOT NULL DEFAULT(1),
        CreatedAt udt_DateTime2 NOT NULL DEFAULT GETUTCDATE(),
        ModifiedAt udt_DateTime2 NULL,
        CONSTRAINT FK_District_State FOREIGN KEY (StateId) REFERENCES dbo.State(StateId)
    );
    CREATE UNIQUE INDEX UQ_District_StateId_DistrictName ON dbo.District (StateId, DistrictName);
    PRINT 'Table District created.';
END
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'dbo.LocationMapping') AND type = 'U')
BEGIN
    CREATE TABLE dbo.LocationMapping
    (
        MappingId udt_ID IDENTITY(1,1) PRIMARY KEY,
        CountryId udt_ID NOT NULL,
        StateId udt_ID NOT NULL,
        DistrictId udt_ID NOT NULL,
        Remarks udt_Remarks NULL,
		IsActive udt_ActiveFlag NOT NULL DEFAULT(1),
        CreatedBy udt_Name NOT NULL,
        CreatedAt udt_DateTime2 NOT NULL DEFAULT GETUTCDATE(),
        ModifiedBy udt_Name NULL,
        ModifiedAt udt_DateTime2 NULL  DEFAULT GETUTCDATE(),
        CONSTRAINT FK_LocationMapping_Country FOREIGN KEY (CountryId) REFERENCES dbo.Country(CountryId),
        CONSTRAINT FK_LocationMapping_State FOREIGN KEY (StateId) REFERENCES dbo.State(StateId),
        CONSTRAINT FK_LocationMapping_District FOREIGN KEY (DistrictId) REFERENCES dbo.District(DistrictId)
    );
    PRINT 'Table LocationMapping created.';
END
GO



IF OBJECT_ID('dbo.usp_GetCountries', 'P') IS NOT NULL
    DROP PROCEDURE dbo.usp_GetCountries;
GO

CREATE PROCEDURE dbo.usp_GetCountries
AS
BEGIN
    SET NOCOUNT ON;
    SELECT CountryId, CountryName FROM dbo.Country WHERE IsActive = 1 ORDER BY CountryName;
END
GO

IF OBJECT_ID('dbo.usp_GetStatesByCountry', 'P') IS NOT NULL
    DROP PROCEDURE dbo.usp_GetStatesByCountry;
GO

CREATE PROCEDURE dbo.usp_GetStatesByCountry
    @CountryId udt_ID
AS
BEGIN
    SET NOCOUNT ON;
    SELECT StateId
	 ,  StateName
	, CountryId 
    FROM dbo.State
    WHERE IsActive = 1 AND CountryId = @CountryId
    ORDER BY StateName;
END
GO


IF OBJECT_ID('dbo.usp_GetDistrictsByState', 'P') IS NOT NULL
    DROP PROCEDURE dbo.usp_GetDistrictsByState;
GO

	CREATE PROCEDURE dbo.usp_GetDistrictsByState
		@StateId udt_ID
	AS
	BEGIN
		SET NOCOUNT ON;
		SELECT DistrictId
		, DistrictName
		, StateId
		FROM dbo.District
		WHERE IsActive = 1  AND StateId = @StateId
		ORDER BY DistrictName;
	END
	GO


IF OBJECT_ID('dbo.usp_GetLocationMappings', 'P') IS NOT NULL
    DROP PROCEDURE dbo.usp_GetLocationMappings;
GO

CREATE PROCEDURE dbo.usp_GetLocationMappings
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        SELECT 
            LM.MappingId,
            LM.CountryId,
            C.CountryName,
            LM.StateId,
            S.StateName,
            LM.DistrictId,
            D.DistrictName,
            LM.Remarks,
            LM.IsActive,
            LM.CreatedBy,
            LM.CreatedAt,
            LM.ModifiedBy,
            LM.ModifiedAt
        FROM dbo.LocationMapping AS LM
        INNER JOIN dbo.Country AS C ON LM.CountryId = C.CountryId
        INNER JOIN dbo.State AS S ON LM.StateId = S.StateId
        INNER JOIN dbo.District AS D ON LM.DistrictId = D.DistrictId
        WHERE LM.IsActive = 1
        ORDER BY C.CountryName, S.StateName, D.DistrictName;
    END TRY
    BEGIN CATCH
        DECLARE @ErrMsg NVARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR(@ErrMsg, 16, 1);
    END CATCH
END
GO



IF OBJECT_ID('dbo.usp_SaveOrUpdateLocationMapping', 'P') IS NOT NULL
    DROP PROCEDURE dbo.usp_SaveOrUpdateLocationMapping;
GO

CREATE PROCEDURE dbo.usp_SaveOrUpdateLocationMapping
(
    @MappingId udt_ID = NULL,
    @CountryId udt_ID,
    @StateId udt_ID,
    @DistrictId udt_ID,
    @Remarks udt_Remarks = NULL,
    @UserName udt_Name
)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRAN;

        IF NOT EXISTS (SELECT 1 FROM dbo.Country WHERE IsActive = 1 AND CountryId = @CountryId  )
            RAISERROR('Invalid CountryId', 16, 1);

        IF NOT EXISTS (SELECT 1 FROM dbo.State WHERE IsActive = 1 AND  StateId = @StateId AND CountryId = @CountryId)
            RAISERROR('Invalid StateId for given CountryId', 16, 1);

        IF NOT EXISTS (SELECT 1 FROM dbo.District WHERE IsActive = 1 AND  DistrictId = @DistrictId AND StateId = @StateId)
            RAISERROR('Invalid DistrictId for given StateId', 16, 1);

        IF @MappingId IS NULL OR @MappingId = 0
        BEGIN
            INSERT INTO dbo.LocationMapping (CountryId, StateId, DistrictId, Remarks, CreatedBy, CreatedAt)
            VALUES (@CountryId, @StateId, @DistrictId, @Remarks, @UserName, GETUTCDATE());

            DECLARE @NewId INT = SCOPE_IDENTITY();
            SELECT @NewId AS MappingId, 'Inserted' AS ActionResult;
        END
        ELSE
        BEGIN
            IF NOT EXISTS (SELECT 1 FROM dbo.LocationMapping WHERE IsActive = 1 AND MappingId = @MappingId)
                RAISERROR('Mapping record not found', 16, 1);

            UPDATE dbo.LocationMapping
            SET CountryId = @CountryId,
                StateId = @StateId,
                DistrictId = @DistrictId,
                Remarks = @Remarks,
                ModifiedBy = @UserName,
                ModifiedAt = SYSUTCDATETIME()
            WHERE MappingId = @MappingId;

            SELECT @MappingId AS MappingId, 'Updated' AS ActionResult;
        END

        COMMIT TRAN;
    END TRY
    BEGIN CATCH
        IF XACT_STATE() <> 0
            ROLLBACK TRAN;

        DECLARE @ErrMsg NVARCHAR(4000) = ERROR_MESSAGE();
        DECLARE @ErrSeverity INT = ERROR_SEVERITY();
        RAISERROR('Error in usp_SaveOrUpdateLocationMapping: %s', @ErrSeverity, 1, @ErrMsg);
    END CATCH
END
GO


IF OBJECT_ID('dbo.usp_DeleteLocationMapping', 'P') IS NOT NULL
    DROP PROCEDURE dbo.usp_DeleteLocationMapping;
GO

CREATE PROCEDURE dbo.usp_DeleteLocationMapping
    @MappingId udt_ID
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRAN;

        IF NOT EXISTS (SELECT 1 FROM dbo.LocationMapping WHERE  IsActive = 1 AND MappingId = @MappingId)
        BEGIN
            RAISERROR('Mapping not found.', 16, 1);
            ROLLBACK TRAN;
            RETURN;
        END

        UPDATE dbo.LocationMapping SET IsActive = 0 WHERE IsActive = 1 AND MappingId = @MappingId;

        COMMIT TRAN;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRAN;

        DECLARE @ErrMsg NVARCHAR(4000), @ErrSeverity INT;
        SELECT @ErrMsg = ERROR_MESSAGE(), @ErrSeverity = ERROR_SEVERITY();
        RAISERROR(@ErrMsg, @ErrSeverity, 1);
    END CATCH
END
GO



-- Country Data
INSERT INTO dbo.Country (CountryName) VALUES
('India'),
('United States'),
('Australia');

-- State Data
INSERT INTO dbo.State (StateName, CountryId) VALUES
('Telangana', 1),
('Andhra Pradesh', 1),
('California', 2),
('Texas', 2),
('Victoria', 3),
('Queensland', 3);

-- District Data
INSERT INTO dbo.District (DistrictName, StateId) VALUES
('Hyderabad', 1),
('Warangal', 1),
('Vijayawada', 2),
('Visakhapatnam', 2),
('Los Angeles', 3),
('San Francisco', 3),
('Dallas', 4),
('Houston', 4),
('Melbourne', 5),
('Geelong', 5),
('Brisbane', 6),
('Gold Coast', 6);
SELECT * FROM dbo.Country;
SELECT * FROM dbo.State;
SELECT * FROM dbo.District;
SELECT * FROM dbo.LocationMapping;

---- Location Mapping Data (for dependent dropdown demo)
--INSERT INTO dbo.LocationMapping (CountryId, StateId, DistrictId)
--VALUES
--(1, 1, 1), -- India - Telangana - Hyderabad
--(1, 2, 3), -- India - Andhra Pradesh - Vijayawada
--(2, 3, 5), -- USA - California - Los Angeles
--(3, 5, 9); -- Australia - Victoria - Melbourne


