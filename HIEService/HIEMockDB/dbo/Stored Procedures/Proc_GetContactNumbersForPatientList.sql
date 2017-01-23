CREATE PROCEDURE Proc_GetContactNumbersForPatientList (@PatientList List READONLY)
AS
BEGIN
	SELECT *
	FROM PatientContactNumber PCN
	INNER JOIN @PatientList PL ON PCN.PatientID = PL.Item
END