﻿ALTER TABLE [dbo].[MacroTags]
    ADD CONSTRAINT [FK_MacroTags_Tags] FOREIGN KEY ([TagID]) REFERENCES [dbo].[Tags] ([TagID]) ON DELETE NO ACTION ON UPDATE NO ACTION;
