CREATE PROCEDURE [Staging].[StagingRegistrationDetailsCleanup]
AS
BEGIN
TRUNCATE TABLE Staging.RegistrationDetails;
END;
