﻿ALTER TABLE [dbo].[Comment]
    ADD CONSTRAINT [PK_Comment] PRIMARY KEY CLUSTERED ([CommentID] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF);

