CREATE TABLE [dbo].[PatientContactNumber] (
    [ContactNumberID] INT           IDENTITY (1, 1) NOT NULL,
    [PatientID]       INT           NULL,
    [ContactType]     NVARCHAR (8)  NULL,
    [ContactNumber]   NVARCHAR (32) NULL,
    PRIMARY KEY CLUSTERED ([ContactNumberID] ASC),
    FOREIGN KEY ([PatientID]) REFERENCES [dbo].[Patient] ([PatientID])
);

