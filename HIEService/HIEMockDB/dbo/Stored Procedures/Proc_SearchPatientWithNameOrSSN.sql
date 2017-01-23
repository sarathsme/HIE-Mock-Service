CREATE PROCEDURE Proc_SearchPatientWithNameOrSSN (
	@SSN NVARCHAR(32) = NULL
	,@FirstName NVARCHAR(256) = NULL
	,@FamilyName NVARCHAR(256) = NULL
	)
AS
BEGIN
	IF (@SSN IS NOT NULL)
	BEGIN
		SELECT *
		FROM Patient
		WHERE SSN = @SSN
	END
	ELSE
	BEGIN
		SELECT *
		FROM Patient
		WHERE Firstname = @FirstName
			AND familyname = @FamilyName
	END
END