CREATE PROCEDURE Proc_GetDocumentUniqueIDList (@EMPID NVARCHAR(256))
AS
BEGIN
	SELECT DocumentUniqueID,IsDeprecated
	FROM PatientDocuments PD
	INNER JOIN Patient P ON PD.PatientID = P.PatientID
	WHERE P.EMPID = @EMPID
END


select * from PatientDocuments