CREATE PROCEDURE [Config].[GetGradeLevelConfigs]
	@GradeLevelNames NVARCHAR(MAX)
AS
BEGIN
	CREATE TABLE #GradeLevels ([Name] VARCHAR(50));
	INSERT INTO #GradeLevels ([Name])
		SELECT value FROM dbo.STRING_SPLIT(@GradeLevelNames,'|');

	SELECT
		Id,
		[Name],
		BackgroundColor,
		TextColor
	FROM 
		Config.GradeLevel 
		WHERE 
			[Name] 
		IN 
			(SELECT [Name] FROM #GradeLevels)
END