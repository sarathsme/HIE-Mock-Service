CREATE PROCEDURE Proc_GetDocumentsForUniqueID (@UniqueIDList [StringList] readonly)
AS
BEGIN
	SELECT *
	FROM PatientDocuments PD
	INNER JOIN @UniqueIDList U ON PD.DocumentUniqueId = U.Item
END