﻿CREATE TABLE [Data].[StudentVehicleMap]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	[StudentId] INT NOT NULL CONSTRAINT FK_StudentId_Student_Id FOREIGN KEY REFERENCES [Data].[Student](Id) ON DELETE CASCADE,
	[VehicleId] INT NOT NULL CONSTRAINT FK_VehicleId_Vehicle_Id FOREIGN KEY REFERENCES [Data].[Vehicle](Id) ON DELETE CASCADE
)
