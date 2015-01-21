
CREATE TABLE [dbo].[MobileConnectUserMappings] (
    [Id] [int] NOT NULL IDENTITY,
    [MobileConnectUserId] [nvarchar](255) NOT NULL,
    [ActiveDirectoryUserId] [nvarchar](255) NOT NULL,
    [IsDeleted] [bit] NOT NULL,
    [CreationDate] [datetime] NOT NULL,
    [ModificationDate] [datetime] NOT NULL,
    CONSTRAINT [PK_dbo.MobileConnectUserMappings] PRIMARY KEY ([Id])
)

CREATE INDEX [IX_MobileConnectUserId] ON [dbo].[MobileConnectUserMappings]([MobileConnectUserId])
