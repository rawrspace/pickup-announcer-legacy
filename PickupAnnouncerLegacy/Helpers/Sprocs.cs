namespace PickupAnnouncerLegacy.Helpers
{
    public static class Sprocs
    {
        public static readonly string ProcessStagingRegistrationDetails = "[Staging].[ProcessStagingRegistrationDetails]";
        public static readonly string ClearDatabase = "[Data].[DeleteAll]";
        public static readonly string GetStudentsForRegistrationId = "[Data].[GetStudentsForRegistrationId]";
        public static readonly string ExportRegistrationDetails = "[Data].[ExportRegistrationDetails]";
        public static readonly string AddPickupLog = "[Data].[AddPickupLog]";
        public static readonly string GetGradeLevelConfig = "[Config].[GetGradeLevelConfigs]";
    }
}
