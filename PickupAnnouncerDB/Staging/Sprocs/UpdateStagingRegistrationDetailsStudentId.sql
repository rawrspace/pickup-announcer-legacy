CREATE PROCEDURE [Staging].[UpdateStagingRegistrationDetailsStudentId]
AS
BEGIN
	UPDATE registration
	SET
		StudentId = student.Id
	FROM
		Staging.RegistrationDetails registration
	INNER JOIN
		Data.Student student
		ON 
			student.FirstName = registration.FirstName
			AND student.LastName = registration.LastName
			AND student.Teacher = registration.Teacher
			AND student.GradeLevel = registration.GradeLevel
END