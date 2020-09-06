CREATE PROCEDURE [Staging].[UpdateStagingRegistrationDetailsVehicleId]
AS
BEGIN
	UPDATE registration
	SET
		VehicleId = vehicle.Id
	FROM
		Staging.RegistrationDetails registration
	INNER JOIN
		Data.Vehicle vehicle
		ON 
			vehicle.RegistrationId = registration.RegistrationId
END