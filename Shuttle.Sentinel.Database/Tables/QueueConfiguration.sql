CREATE TABLE [dbo].[QueueConfiguration]
(
	[Scheme] VARCHAR(65) NOT NULL , 
    [ConfigurationName] VARCHAR(130) NOT NULL, 
    [EnvironmentName] VARCHAR(65) NOT NULL, 
    [JsonData] VARCHAR(MAX) NOT NULL, 
    PRIMARY KEY ([Scheme], [ConfigurationName], [EnvironmentName])
)
