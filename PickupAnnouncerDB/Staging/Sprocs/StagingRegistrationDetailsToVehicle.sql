CREATE PROCEDURE [Staging].[StagingRegistrationDetailsToVehicle]
AS
BEGIN
	MERGE Data.Vehicle AS target
	USING (
		SELECT DISTINCT
			RegistrationId
		FROM
			Staging.RegistrationDetails
	) AS source
	ON (
		source.RegistrationId = target.RegistrationId
	)
	WHEN NOT MATCHED BY TARGET THEN
		INSERT (
			RegistrationId
		)
		VALUES (
			source.RegistrationId
		)
	WHEN NOT MATCHED BY SOURCE THEN
		DELETE;
END;
