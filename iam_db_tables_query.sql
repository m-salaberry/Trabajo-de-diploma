IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'USERS')
BEGIN
    CREATE TABLE USERS (
        Id UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
        Name NVARCHAR(50) NOT NULL,
        Password NVARCHAR(255) NOT NULL,
        Role NVARCHAR(50) NULL,
        IsActive BIT NOT NULL DEFAULT 1,
        CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),
        ModifiedDate DATETIME NULL,
        LastLoginDate DATETIME NULL,
        
        CONSTRAINT PK_USERS PRIMARY KEY (Id),
        CONSTRAINT UQ_USERS_Name UNIQUE (Name),
        CONSTRAINT CK_USERS_Name_NotEmpty CHECK (LEN(Name) > 0),
        CONSTRAINT CK_USERS_Password_MinLength CHECK (LEN(Password) >= 4)
    );

    CREATE INDEX IX_USERS_Name ON USERS(Name);
    CREATE INDEX IX_USERS_IsActive ON USERS(IsActive);
    
    PRINT 'Tabla USERS creada exitosamente';
END
ELSE
BEGIN
    PRINT 'Tabla USERS ya existe';
END
GO

-- ==================================================
-- TABLA: PATENTS
-- Almacena permisos atómicos (permisos individuales)
-- ==================================================

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'PATENTS')
BEGIN
    CREATE TABLE PATENTS (
        Id UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
        Name NVARCHAR(100) NOT NULL,
        Description NVARCHAR(500) NULL,
        CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),
        
        CONSTRAINT PK_PATENTS PRIMARY KEY (Id),
        CONSTRAINT UQ_PATENTS_Name UNIQUE (Name),
        CONSTRAINT CK_PATENTS_Name_NotEmpty CHECK (LEN(Name) > 0)
    );

    CREATE INDEX IX_PATENTS_Name ON PATENTS(Name);
    
    PRINT 'Tabla PATENTS creada exitosamente';
END
ELSE
BEGIN
    PRINT 'Tabla PATENTS ya existe';
END
GO

-- ==================================================
-- TABLA: FAMILIES
-- Almacena roles (agrupaciones de permisos)
-- ==================================================

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'FAMILIES')
BEGIN
    CREATE TABLE FAMILIES (
        Id UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
        Name NVARCHAR(100) NOT NULL,
        Description NVARCHAR(500) NULL,
        CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),
        ModifiedDate DATETIME NULL,
        
        CONSTRAINT PK_FAMILIES PRIMARY KEY (Id),
        CONSTRAINT UQ_FAMILIES_Name UNIQUE (Name),
        CONSTRAINT CK_FAMILIES_Name_NotEmpty CHECK (LEN(Name) > 0)
    );

    CREATE INDEX IX_FAMILIES_Name ON FAMILIES(Name);
    
    PRINT 'Tabla FAMILIES creada exitosamente';
END
ELSE
BEGIN
    PRINT 'Tabla FAMILIES ya existe';
END
GO

-- ==================================================
-- TABLA: PATENTS_FAMILIES
-- Relación N:M entre Patents (permisos) y Families (roles)
-- ==================================================

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'PATENTS_FAMILIES')
BEGIN
    CREATE TABLE PATENTS_FAMILIES (
        PatentId UNIQUEIDENTIFIER NOT NULL,
        FamilyId UNIQUEIDENTIFIER NOT NULL,
        AssignedDate DATETIME NOT NULL DEFAULT GETDATE(),
        
        CONSTRAINT PK_PATENTS_FAMILIES PRIMARY KEY (PatentId, FamilyId),
        CONSTRAINT FK_PATENTS_FAMILIES_Patent 
            FOREIGN KEY (PatentId) REFERENCES PATENTS(Id)
            ON DELETE CASCADE,
        CONSTRAINT FK_PATENTS_FAMILIES_Family 
            FOREIGN KEY (FamilyId) REFERENCES FAMILIES(Id)
            ON DELETE CASCADE
    );

    CREATE INDEX IX_PATENTS_FAMILIES_PatentId ON PATENTS_FAMILIES(PatentId);
    CREATE INDEX IX_PATENTS_FAMILIES_FamilyId ON PATENTS_FAMILIES(FamilyId);
    
    PRINT 'Tabla PATENTS_FAMILIES creada exitosamente';
END
ELSE
BEGIN
    PRINT 'Tabla PATENTS_FAMILIES ya existe';
END
GO

-- ==================================================
-- TABLA: USERS_FAMILIES
-- Relación N:M entre Users (usuarios) y Families (roles)
-- ==================================================

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'USERS_FAMILIES')
BEGIN
    CREATE TABLE USERS_FAMILIES (
        UserId UNIQUEIDENTIFIER NOT NULL,
        FamilyId UNIQUEIDENTIFIER NOT NULL,
        AssignedDate DATETIME NOT NULL DEFAULT GETDATE(),
        AssignedBy UNIQUEIDENTIFIER NULL,
        
        CONSTRAINT PK_USERS_FAMILIES PRIMARY KEY (UserId, FamilyId),
        CONSTRAINT FK_USERS_FAMILIES_User 
            FOREIGN KEY (UserId) REFERENCES USERS(Id)
            ON DELETE CASCADE,
        CONSTRAINT FK_USERS_FAMILIES_Family 
            FOREIGN KEY (FamilyId) REFERENCES FAMILIES(Id)
            ON DELETE CASCADE
    );

    CREATE INDEX IX_USERS_FAMILIES_UserId ON USERS_FAMILIES(UserId);
    CREATE INDEX IX_USERS_FAMILIES_FamilyId ON USERS_FAMILIES(FamilyId);
    
    PRINT 'Tabla USERS_FAMILIES creada exitosamente';
END
ELSE
BEGIN
    PRINT 'Tabla USERS_FAMILIES ya existe';
END