CREATE TABLE [dbo].[PatientDocuments] (
    [DocumentUniqueId] NVARCHAR (256) NOT NULL,
    [PatientID]        INT            NULL,
    [Document]         XML            NULL,
    [IsDeprecated]     BIT            DEFAULT ((0)) NOT NULL,
    FOREIGN KEY ([PatientID]) REFERENCES [dbo].[Patient] ([PatientID])
);

