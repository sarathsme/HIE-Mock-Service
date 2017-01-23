CREATE TABLE [dbo].[Patient] (
    [PatientID]   INT            IDENTITY (1, 1) NOT NULL,
    [FirstName]   NVARCHAR (511) NULL,
    [MiddleName]  NVARCHAR (511) NULL,
    [FamilyName]  NVARCHAR (511) NULL,
    [Gender]      CHAR (1)       NULL,
    [DateOfBirth] DATETIME       NULL,
    [SSN]         NVARCHAR (32)  NOT NULL,
    [EMPID]       NVARCHAR (256) NULL,
    [IPHRID]      NVARCHAR (32)  NULL,
    PRIMARY KEY CLUSTERED ([PatientID] ASC),
    CONSTRAINT [UniqueSSN_Patient] UNIQUE NONCLUSTERED ([SSN] ASC)
);

