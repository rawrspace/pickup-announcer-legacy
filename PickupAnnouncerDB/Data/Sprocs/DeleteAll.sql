CREATE PROCEDURE [Data].[DeleteAll]
AS
BEGIN
	DELETE FROM Data.StudentVehicleMap
	DELETE FROM Data.Student
	DELETE FROM Data.Vehicle
END;
