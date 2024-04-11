CREATE TABLE [dbo].[Messages]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	[ChatId] INT NOT NULL,
	[SenderId] nvarchar(450) NOT NULL,
	[Content] nvarchar(300) NOT NULL,
	[SentAt] datetime NOT NULL,
	CONSTRAINT [FK_Messages_ChatId] FOREIGN KEY (ChatId) REFERENCES Chats (Id) ON DELETE CASCADE,
	CONSTRAINT [FK_Messages_SenderId] FOREIGN KEY (SenderId) REFERENCES AspNetUsers (Id)
)
