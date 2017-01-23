CREATE PROCEDURE Proc_GetAddressesForPatientList (@PatientList List READONLY)
AS
BEGIN
	SELECT *
	FROM PatientAddress PA
	INNER JOIN @PatientList PL ON PA.PatientID = PL.Item
END