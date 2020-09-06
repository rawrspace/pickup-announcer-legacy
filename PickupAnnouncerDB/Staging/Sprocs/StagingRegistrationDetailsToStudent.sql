CREATE PROCEDURE [Staging].[StagingRegistrationDetailsToStudent]
AS
BEGIN
	MERGE Data.Student AS target
	USING (
		SELECT DISTINCT
			FirstName,
			LastName,
			Teacher,
			GradeLevel
		FROM
			Staging.RegistrationDetails
	) AS source
	ON (
		source.FirstName = target.FirstName
		AND source.LastName = target.LastName
		AND source.Teacher = target.Teacher
		AND source.GradeLevel = target.GradeLevel
	)
	WHEN NOT MATCHED BY TARGET THEN
		INSERT (
			FirstName,
			LastName,
			Teacher,
			GradeLevel
		)
		VALUES (
			source.FirstName,
			source.LastName,
			source.Teacher,
			source.GradeLevel
		)	
	WHEN NOT MATCHED BY SOURCE THEN
		DELETE;
END;
