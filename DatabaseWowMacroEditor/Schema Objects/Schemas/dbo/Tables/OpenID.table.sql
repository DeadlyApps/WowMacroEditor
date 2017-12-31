CREATE TABLE [dbo].[OpenID] (
    [UserID]                  UNIQUEIDENTIFIER NOT NULL,
    [OpenIdClaimedIdentifier] NVARCHAR (250)   NOT NULL,
    [LastLoginDate]           DATETIME         NOT NULL,
    [CreateDate]              DATETIME         NOT NULL
);

