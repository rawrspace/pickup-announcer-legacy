CREATE TABLE [Data].[Student]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	[FirstName] VARCHAR(50) NOT NULL,
	[LastName] VARCHAR(50) NOT NULL,
	[Teacher] VARCHAR(50) NOT NULL,
	[GradeLevel] VARCHAR(50) NOT NULL, 
    CONSTRAINT [UQ_Student_Column] UNIQUE ([FirstName],[LastName],[Teacher],[GradeLevel])
)
