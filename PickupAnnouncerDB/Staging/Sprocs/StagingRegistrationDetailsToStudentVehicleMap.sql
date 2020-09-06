CREATE PROCEDURE [Staging].[StagingRegistrationDetailsToStudentVehicleMap]
AS
BEGIN
	MERGE Data.StudentVehicleMap AS target
	USING (
		SELECT DISTINCT
			StudentId,
			VehicleId
		FROM
			Staging.RegistrationDetails
		WHERE
			StudentId IS NOT NULL
			AND VehicleId IS NOT NULL
	) AS source
	ON (
		source.StudentId = target.StudentId
		AND source.VehicleId = target.VehicleId
	)
	WHEN NOT MATCHED BY TARGET THEN
		INSERT (
			StudentId,
			VehicleId
		)
		VALUES (
			source.StudentId,
			source.VehicleId
		)
	WHEN NOT MATCHED BY SOURCE THEN
		DELETE;
END;
