CREATE TABLE [dbo].[ChatMembers]
(
	[Id] INT NOT NULL PRIMARY KEY,
	[ChatId] INT NOT NULL,
	[UserId] nvarchar(450) NOT NULL,
	CONSTRAINT [FK_ChatMembers_ChatId] FOREIGN KEY (ChatId) REFERENCES Chats (Id),
	CONSTRAINT [FK_ChatMembers_UserId] FOREIGN KEY (UserId) REFERENCES AspNetUsers (Id)
)
