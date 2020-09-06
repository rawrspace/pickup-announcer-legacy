CREATE TABLE [Staging].[RegistrationDetails]
(
	[RegistrationId] INT NOT NULL,
	[FirstName] VARCHAR(50) NOT NULL,
	[LastName] VARCHAR(50) NOT NULL,
	[Teacher] VARCHAR(50) NOT NULL,
	[GradeLevel] VARCHAR(50) NOT NULL,
	[StudentId] INT FOREIGN KEY REFERENCES Data.Student(Id),
	[VehicleId] INT FOREIGN KEY REFERENCES Data.Vehicle(Id)
)
