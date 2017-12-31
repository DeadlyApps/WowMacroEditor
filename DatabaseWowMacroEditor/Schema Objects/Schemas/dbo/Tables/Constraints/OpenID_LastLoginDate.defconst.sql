ALTER TABLE [dbo].[OpenID]
    ADD CONSTRAINT [OpenID_LastLoginDate] DEFAULT (getdate()) FOR [LastLoginDate];

