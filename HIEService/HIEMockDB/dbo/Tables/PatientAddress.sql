CREATE TABLE [dbo].[PatientAddress] (
    [AddressID]         INT            IDENTITY (1, 1) NOT NULL,
    [PatientID]         INT            NULL,
    [StreetAddressLine] NVARCHAR (511) NULL,
    [City]              NVARCHAR (511) NULL,
    [StateOrProvince]   NVARCHAR (511) NULL,
    [County]            NVARCHAR (256) NULL,
    [PostalCode]        NVARCHAR (256) NULL,
    FOREIGN KEY ([PatientID]) REFERENCES [dbo].[Patient] ([PatientID])
);

