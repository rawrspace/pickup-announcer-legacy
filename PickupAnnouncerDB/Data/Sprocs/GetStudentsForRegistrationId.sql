CREATE PROCEDURE [Data].[GetStudentsForRegistrationId]
	@RegistrationId int
AS
BEGIN
	SELECT
		FirstName,
		LastName,
		Teacher,
		GradeLevel
	FROM 
		Data.Student student
	INNER JOIN
		Data.StudentVehicleMap map
		ON map.StudentId = student.Id
	INNER JOIN
		Data.Vehicle vehicle
		ON vehicle.Id = map.VehicleId
	WHERE
		vehicle.RegistrationId = @RegistrationId
END;
