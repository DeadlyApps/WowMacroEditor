﻿ALTER TABLE [dbo].[Comment]
    ADD CONSTRAINT [FK_Comment_Macro] FOREIGN KEY ([MacroID]) REFERENCES [dbo].[Macro] ([MacroID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

