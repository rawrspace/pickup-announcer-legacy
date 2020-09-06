CREATE PROCEDURE [Data].[ExportRegistrationDetails]
AS
BEGIN
	SELECT 
		vehicle.RegistrationId AS RegistrationId,
		student.FirstName AS FirstName,
		student.LastName AS LastName,
		student.Teacher AS Teacher,
		student.GradeLevel AS GradeLevel
	FROM
		Data.Student student
	INNER JOIN
		Data.StudentVehicleMap map
		ON map.StudentId = student.Id
	INNER JOIN
		Data.Vehicle vehicle
		ON vehicle.Id = map.VehicleId
END;