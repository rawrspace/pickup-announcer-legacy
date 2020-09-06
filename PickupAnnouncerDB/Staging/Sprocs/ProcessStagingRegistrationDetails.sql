﻿CREATE PROCEDURE [Staging].[ProcessStagingRegistrationDetails]
AS
BEGIN
	EXECUTE Staging.StagingRegistrationDetailsToStudent;
	EXECUTE Staging.UpdateStagingRegistrationDetailsStudentId;
	EXECUTE Staging.StagingRegistrationDetailsToVehicle;
	EXECUTE Staging.UpdateStagingRegistrationDetailsVehicleId;
	EXECUTE Staging.StagingRegistrationDetailsToStudentVehicleMap;
	EXECUTE Staging.StagingRegistrationDetailsCleanup;
END;
