ALTER TABLE [dbo].[OpenID]
    ADD CONSTRAINT [OpenID_CreateDate] DEFAULT (getdate()) FOR [CreateDate];

