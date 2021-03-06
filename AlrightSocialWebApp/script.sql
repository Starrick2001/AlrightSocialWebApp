USE [AlrightSocial]
GO
/****** Object:  Table [dbo].[Administrator]    Script Date: 13/01/2022 10:41:13 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Administrator](
	[EmailAddress] [nvarchar](255) NOT NULL,
	[Password] [nvarchar](255) NOT NULL,
	[name] [nvarchar](255) NULL,
	[sex] [nvarchar](255) NULL,
	[DateOfBirth] [date] NULL,
	[PhoneNumber] [nvarchar](255) NULL,
	[SignInStatus] [nvarchar](255) NULL,
	[AvatarURL] [nvarchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[EmailAddress] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BlockedEmail]    Script Date: 13/01/2022 10:41:13 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BlockedEmail](
	[UserEmail] [nvarchar](255) NOT NULL,
	[BlockedUser] [nvarchar](255) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[UserEmail] ASC,
	[BlockedUser] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Chats]    Script Date: 13/01/2022 10:41:13 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Chats](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[User1] [nvarchar](255) NOT NULL,
	[User2] [nvarchar](255) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DeletedComment]    Script Date: 13/01/2022 10:41:13 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DeletedComment](
	[ID] [int] NOT NULL,
	[UserEmail] [nvarchar](255) NOT NULL,
	[PostID] [int] NOT NULL,
	[Content] [nvarchar](max) NULL,
	[NotificationID] [int] NULL,
	[Time] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DeletedLike]    Script Date: 13/01/2022 10:41:13 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DeletedLike](
	[UserEmail] [nvarchar](255) NOT NULL,
	[PostID] [int] NOT NULL,
	[NotificationID] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[UserEmail] ASC,
	[PostID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DeletedNotification]    Script Date: 13/01/2022 10:41:13 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DeletedNotification](
	[ID] [int] NOT NULL,
	[UserEmail] [nvarchar](255) NOT NULL,
	[Content] [nvarchar](max) NULL,
	[PostID] [int] NULL,
	[Time] [datetime] NULL,
	[IsRead] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DeletedPost]    Script Date: 13/01/2022 10:41:13 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DeletedPost](
	[ID] [int] NOT NULL,
	[Title] [nvarchar](255) NULL,
	[Content] [nvarchar](max) NULL,
	[TimeCreate] [datetime] NULL,
	[TimeModified] [datetime] NULL,
	[Author] [nvarchar](255) NOT NULL,
	[Privacy] [nvarchar](255) NULL,
	[ImageURL] [nvarchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DeletedShare]    Script Date: 13/01/2022 10:41:13 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DeletedShare](
	[ID] [int] NOT NULL,
	[UserEmail] [nvarchar](255) NOT NULL,
	[PostID] [int] NOT NULL,
	[Privacy] [nvarchar](255) NULL,
	[NotificationID] [int] NULL,
	[Time] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Friend]    Script Date: 13/01/2022 10:41:13 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Friend](
	[UserEmail] [nvarchar](255) NOT NULL,
	[FriendEmail] [nvarchar](255) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[UserEmail] ASC,
	[FriendEmail] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[FriendRequest]    Script Date: 13/01/2022 10:41:13 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FriendRequest](
	[UserEmail] [nvarchar](255) NOT NULL,
	[FriendEmail] [nvarchar](255) NOT NULL,
	[Time] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[UserEmail] ASC,
	[FriendEmail] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Message]    Script Date: 13/01/2022 10:41:13 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Message](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[SenderEmail] [nvarchar](255) NOT NULL,
	[Time] [datetime] NULL,
	[Content] [nvarchar](max) NULL,
	[ChatId] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Notification]    Script Date: 13/01/2022 10:41:13 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Notification](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[UserEmail] [nvarchar](255) NOT NULL,
	[Content] [nvarchar](max) NULL,
	[PostID] [int] NULL,
	[Time] [datetime] NULL,
	[IsRead] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Post]    Script Date: 13/01/2022 10:41:13 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Post](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](255) NULL,
	[Content] [nvarchar](max) NULL,
	[TimeCreate] [datetime] NULL,
	[TimeModified] [datetime] NULL,
	[Author] [nvarchar](255) NOT NULL,
	[Privacy] [nvarchar](255) NULL,
	[ImageURL] [nvarchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PostComment]    Script Date: 13/01/2022 10:41:13 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PostComment](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[UserEmail] [nvarchar](255) NOT NULL,
	[PostID] [int] NOT NULL,
	[Content] [nvarchar](max) NULL,
	[NotificationID] [int] NULL,
	[Time] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PostLike]    Script Date: 13/01/2022 10:41:13 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PostLike](
	[UserEmail] [nvarchar](255) NOT NULL,
	[PostID] [int] NOT NULL,
	[NotificationID] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[UserEmail] ASC,
	[PostID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PostReport]    Script Date: 13/01/2022 10:41:13 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PostReport](
	[EmailAddress] [nvarchar](255) NOT NULL,
	[PostID] [int] NOT NULL,
	[Time] [datetime] NOT NULL,
	[Content] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[EmailAddress] ASC,
	[PostID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PostShare]    Script Date: 13/01/2022 10:41:13 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PostShare](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[UserEmail] [nvarchar](255) NOT NULL,
	[PostID] [int] NOT NULL,
	[Privacy] [nvarchar](255) NULL,
	[NotificationID] [int] NULL,
	[Time] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ReportUser]    Script Date: 13/01/2022 10:41:13 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ReportUser](
	[UserEmail] [nvarchar](255) NOT NULL,
	[ReportedUser] [nvarchar](255) NOT NULL,
	[Time] [datetime] NOT NULL,
	[Content] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[UserEmail] ASC,
	[ReportedUser] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SuspendedUser]    Script Date: 13/01/2022 10:41:13 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SuspendedUser](
	[SuspendedEmail] [nvarchar](255) NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 13/01/2022 10:41:13 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[EmailAddress] [nvarchar](255) NOT NULL,
	[Password] [nvarchar](255) NOT NULL,
	[name] [nvarchar](255) NULL,
	[sex] [nvarchar](255) NULL,
	[DateOfBirth] [date] NULL,
	[PhoneNumber] [nvarchar](255) NULL,
	[SignInStatus] [nvarchar](255) NULL,
	[AvatarURL] [nvarchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[EmailAddress] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[Administrator] ([EmailAddress], [Password], [name], [sex], [DateOfBirth], [PhoneNumber], [SignInStatus], [AvatarURL]) VALUES (N'hoale@gmail.com', N'$2b$10$fkC7QNG6IJTnzLlcUV03XuqwYlEaqyKTzkwH044HQ/isUEhIQmmuu', N'Hoà Lê', N'Nam', CAST(N'2022-01-01' AS Date), N'090909090', N'Offline', N'/logo/logo_alrightsocial_circle.png')
GO
INSERT [dbo].[BlockedEmail] ([UserEmail], [BlockedUser]) VALUES (N'asdff@gmail.com', N'asdf@gmail.com')
GO
SET IDENTITY_INSERT [dbo].[Chats] ON 

INSERT [dbo].[Chats] ([ID], [User1], [User2]) VALUES (6, N'lebuidihoa257@gmail.com', N'asdf@gmail.com')
INSERT [dbo].[Chats] ([ID], [User1], [User2]) VALUES (7, N'asdff@gmail.com', N'huuhieu@gmail.com')
INSERT [dbo].[Chats] ([ID], [User1], [User2]) VALUES (9, N'asdff@gmail.com', N'lebuidihoa257@gmail.com')
SET IDENTITY_INSERT [dbo].[Chats] OFF
GO
INSERT [dbo].[Friend] ([UserEmail], [FriendEmail]) VALUES (N'asdff@gmail.com', N'lebuidihoa257@gmail.com')
INSERT [dbo].[Friend] ([UserEmail], [FriendEmail]) VALUES (N'lebuidihoa257@gmail.com', N'asdff@gmail.com')
GO
SET IDENTITY_INSERT [dbo].[Message] ON 

INSERT [dbo].[Message] ([ID], [SenderEmail], [Time], [Content], [ChatId]) VALUES (42, N'lebuidihoa257@gmail.com', CAST(N'2021-12-31T22:55:49.970' AS DateTime), N'asdf', 6)
INSERT [dbo].[Message] ([ID], [SenderEmail], [Time], [Content], [ChatId]) VALUES (45, N'asdff@gmail.com', CAST(N'2022-01-04T17:11:12.937' AS DateTime), N'Chào Hiếu', 7)
INSERT [dbo].[Message] ([ID], [SenderEmail], [Time], [Content], [ChatId]) VALUES (46, N'huuhieu@gmail.com', CAST(N'2022-01-04T17:11:24.130' AS DateTime), N'Chào Hoà', 7)
INSERT [dbo].[Message] ([ID], [SenderEmail], [Time], [Content], [ChatId]) VALUES (57, N'huuhieu@gmail.com', CAST(N'2022-01-12T22:59:58.800' AS DateTime), N'1', 7)
INSERT [dbo].[Message] ([ID], [SenderEmail], [Time], [Content], [ChatId]) VALUES (58, N'asdff@gmail.com', CAST(N'2022-01-13T22:16:34.137' AS DateTime), N'2', 7)
INSERT [dbo].[Message] ([ID], [SenderEmail], [Time], [Content], [ChatId]) VALUES (59, N'asdff@gmail.com', CAST(N'2022-01-13T22:19:16.257' AS DateTime), N'3', 7)
SET IDENTITY_INSERT [dbo].[Message] OFF
GO
SET IDENTITY_INSERT [dbo].[Notification] ON 

INSERT [dbo].[Notification] ([ID], [UserEmail], [Content], [PostID], [Time], [IsRead]) VALUES (1, N'asdf@gmail.com', N'lebuidihoa257@gmail.com đã thích về bài viết của bạn.', 12, CAST(N'2021-12-26T22:36:57.403' AS DateTime), 1)
INSERT [dbo].[Notification] ([ID], [UserEmail], [Content], [PostID], [Time], [IsRead]) VALUES (2, N'asdf@gmail.com', N'lebuidihoa257@gmail.com đã bình luận về bài viết của bạn.', 12, CAST(N'2021-12-26T22:46:16.463' AS DateTime), 1)
INSERT [dbo].[Notification] ([ID], [UserEmail], [Content], [PostID], [Time], [IsRead]) VALUES (3, N'asdff@gmail.com', N'lebuidihoa257@gmail.com đã chia sẻ bài viết của bạn', 24, CAST(N'2021-12-26T22:46:16.463' AS DateTime), 1)
INSERT [dbo].[Notification] ([ID], [UserEmail], [Content], [PostID], [Time], [IsRead]) VALUES (11, N'lebuidihoa257@gmail.com', N'asdff@gmail.com đã thích về bài viết của bạn.', 26, CAST(N'2021-12-31T23:03:34.747' AS DateTime), 1)
INSERT [dbo].[Notification] ([ID], [UserEmail], [Content], [PostID], [Time], [IsRead]) VALUES (12, N'lebuidihoa257@gmail.com', N'asdff@gmail.com đã bình luận về bài viết của bạn.', 26, CAST(N'2021-12-31T23:10:58.490' AS DateTime), 1)
INSERT [dbo].[Notification] ([ID], [UserEmail], [Content], [PostID], [Time], [IsRead]) VALUES (17, N'asdf@gmail.com', N'Bài viết của bạn đã bị quản trị viên xoá', NULL, CAST(N'2022-01-01T01:31:48.173' AS DateTime), 1)
INSERT [dbo].[Notification] ([ID], [UserEmail], [Content], [PostID], [Time], [IsRead]) VALUES (20, N'asdff@gmail.com', N'huuhieu@gmail.com đã bình luận về bài viết của bạn.', 29, CAST(N'2022-01-04T17:13:45.280' AS DateTime), 1)
INSERT [dbo].[Notification] ([ID], [UserEmail], [Content], [PostID], [Time], [IsRead]) VALUES (26, N'asdff@gmail.com', N'lebuidihoa257@gmail.com đã gửi tin nhắn cho bạn', NULL, CAST(N'2022-01-04T23:11:56.897' AS DateTime), 1)
INSERT [dbo].[Notification] ([ID], [UserEmail], [Content], [PostID], [Time], [IsRead]) VALUES (27, N'asdff@gmail.com', N'lebuidihoa257@gmail.com đã thích về bài viết của bạn.', 29, CAST(N'2022-01-06T11:25:26.180' AS DateTime), 1)
INSERT [dbo].[Notification] ([ID], [UserEmail], [Content], [PostID], [Time], [IsRead]) VALUES (28, N'asdff@gmail.com', N'lebuidihoa257@gmail.com đã bình luận về bài viết của bạn.', 29, CAST(N'2022-01-06T11:26:01.773' AS DateTime), 1)
INSERT [dbo].[Notification] ([ID], [UserEmail], [Content], [PostID], [Time], [IsRead]) VALUES (29, N'asdff@gmail.com', N'lebuidihoa257@gmail.com đã chia sẻ bài viết của bạn.', 29, CAST(N'2022-01-06T11:27:02.350' AS DateTime), 1)
INSERT [dbo].[Notification] ([ID], [UserEmail], [Content], [PostID], [Time], [IsRead]) VALUES (30, N'lebuidihoa257@gmail.com', N'asdff@gmail.com đã gửi tin nhắn cho bạn', NULL, CAST(N'2022-01-06T11:37:24.017' AS DateTime), 1)
INSERT [dbo].[Notification] ([ID], [UserEmail], [Content], [PostID], [Time], [IsRead]) VALUES (31, N'lebuidihoa257@gmail.com', N'asdff@gmail.com đã gửi tin nhắn cho bạn', NULL, CAST(N'2022-01-06T11:37:48.370' AS DateTime), 1)
INSERT [dbo].[Notification] ([ID], [UserEmail], [Content], [PostID], [Time], [IsRead]) VALUES (32, N'asdff@gmail.com', N'lebuidihoa257@gmail.com đã gửi tin nhắn cho bạn', NULL, CAST(N'2022-01-06T11:38:09.790' AS DateTime), 1)
INSERT [dbo].[Notification] ([ID], [UserEmail], [Content], [PostID], [Time], [IsRead]) VALUES (33, N'lebuidihoa257@gmail.com', N'Bài viết với tiêu đề ''asdf'' của bạn đã bị quản trị viên xoá', NULL, CAST(N'2022-01-06T11:39:17.120' AS DateTime), 1)
INSERT [dbo].[Notification] ([ID], [UserEmail], [Content], [PostID], [Time], [IsRead]) VALUES (34, N'asdff@gmail.com', N'huuhieu@gmail.com đã gửi tin nhắn cho bạn', NULL, CAST(N'2022-01-10T23:21:42.383' AS DateTime), 1)
INSERT [dbo].[Notification] ([ID], [UserEmail], [Content], [PostID], [Time], [IsRead]) VALUES (35, N'asdff@gmail.com', N'lebuidihoa257@gmail.com đã bình luận về bài viết của bạn.', 29, CAST(N'2022-01-10T23:52:42.800' AS DateTime), 1)
INSERT [dbo].[Notification] ([ID], [UserEmail], [Content], [PostID], [Time], [IsRead]) VALUES (37, N'asdff@gmail.com', N'Bài viết của bạn với 1 lượt báo cáo có tiêu đề ''123'' đã bị quản trị viên xoá', NULL, CAST(N'2022-01-12T00:20:29.437' AS DateTime), 1)
INSERT [dbo].[Notification] ([ID], [UserEmail], [Content], [PostID], [Time], [IsRead]) VALUES (38, N'asdff@gmail.com', N'huuhieu@gmail.com đã gửi tin nhắn cho bạn', NULL, CAST(N'2022-01-12T22:59:58.833' AS DateTime), 1)
INSERT [dbo].[Notification] ([ID], [UserEmail], [Content], [PostID], [Time], [IsRead]) VALUES (44, N'asdff@gmail.com', N'Bài viết của bạn với 0 lượt báo cáo có tiêu đề ''1'' đã bị quản trị viên xoá', NULL, CAST(N'2022-01-13T19:05:46.973' AS DateTime), 1)
INSERT [dbo].[Notification] ([ID], [UserEmail], [Content], [PostID], [Time], [IsRead]) VALUES (45, N'asdff@gmail.com', N'Bài viết của bạn với 0 lượt báo cáo có tiêu đề ''1'' đã bị quản trị viên xoá', NULL, CAST(N'2022-01-13T19:09:12.413' AS DateTime), 1)
INSERT [dbo].[Notification] ([ID], [UserEmail], [Content], [PostID], [Time], [IsRead]) VALUES (47, N'asdff@gmail.com', N'Bài viết của bạn với 0 lượt báo cáo có tiêu đề ''123'' đã bị quản trị viên xoá', NULL, CAST(N'2022-01-13T19:13:05.687' AS DateTime), 1)
INSERT [dbo].[Notification] ([ID], [UserEmail], [Content], [PostID], [Time], [IsRead]) VALUES (49, N'asdff@gmail.com', N'Bài viết của bạn với 0 lượt báo cáo có tiêu đề ''123'' đã bị quản trị viên xoá', NULL, CAST(N'2022-01-13T19:14:44.947' AS DateTime), 1)
INSERT [dbo].[Notification] ([ID], [UserEmail], [Content], [PostID], [Time], [IsRead]) VALUES (51, N'asdff@gmail.com', N'Bài viết của bạn với 0 lượt báo cáo có tiêu đề ''123'' đã bị quản trị viên xoá', NULL, CAST(N'2022-01-13T19:17:56.457' AS DateTime), 1)
INSERT [dbo].[Notification] ([ID], [UserEmail], [Content], [PostID], [Time], [IsRead]) VALUES (52, N'asdff@gmail.com', N'lebuidihoa257@gmail.com đã thích về bài viết của bạn.', 45, CAST(N'2022-01-13T19:12:42.840' AS DateTime), 1)
INSERT [dbo].[Notification] ([ID], [UserEmail], [Content], [PostID], [Time], [IsRead]) VALUES (53, N'asdff@gmail.com', N'Bài viết của bạn với tiêu đề ''123'' đã được quản trị viên khôi phục', 45, CAST(N'2022-01-13T19:18:00.403' AS DateTime), 1)
INSERT [dbo].[Notification] ([ID], [UserEmail], [Content], [PostID], [Time], [IsRead]) VALUES (54, N'lebuidihoa257@gmail.com', N'Bài viết của bạn với 0 lượt báo cáo có tiêu đề ''bài viết nè'' đã bị quản trị viên xoá', NULL, CAST(N'2022-01-13T21:11:14.647' AS DateTime), 1)
INSERT [dbo].[Notification] ([ID], [UserEmail], [Content], [PostID], [Time], [IsRead]) VALUES (59, N'lebuidihoa257@gmail.com', N'Bài viết của bạn với 0 lượt báo cáo có tiêu đề ''Mọi người nhớ đi hiến máu nhé'' đã bị quản trị viên xoá', NULL, CAST(N'2022-01-13T21:13:10.293' AS DateTime), 1)
INSERT [dbo].[Notification] ([ID], [UserEmail], [Content], [PostID], [Time], [IsRead]) VALUES (60, N'lebuidihoa257@gmail.com', N'asdff@gmail.com đã thích về bài viết của bạn.', 50, CAST(N'2022-01-13T21:12:21.070' AS DateTime), 1)
INSERT [dbo].[Notification] ([ID], [UserEmail], [Content], [PostID], [Time], [IsRead]) VALUES (61, N'lebuidihoa257@gmail.com', N'asdff@gmail.com đã bình luận về bài viết của bạn.', 50, CAST(N'2022-01-13T21:12:23.333' AS DateTime), 1)
INSERT [dbo].[Notification] ([ID], [UserEmail], [Content], [PostID], [Time], [IsRead]) VALUES (62, N'lebuidihoa257@gmail.com', N'asdff@gmail.com đã chia sẻ bài viết của bạn.', 50, CAST(N'2022-01-13T21:12:27.207' AS DateTime), 1)
INSERT [dbo].[Notification] ([ID], [UserEmail], [Content], [PostID], [Time], [IsRead]) VALUES (63, N'lebuidihoa257@gmail.com', N'Bài viết của bạn với tiêu đề ''Mọi người nhớ đi hiến máu nhé'' đã được quản trị viên khôi phục', 50, CAST(N'2022-01-13T21:15:16.000' AS DateTime), 1)
INSERT [dbo].[Notification] ([ID], [UserEmail], [Content], [PostID], [Time], [IsRead]) VALUES (64, N'lebuidihoa257@gmail.com', N'Bài viết của bạn với 0 lượt báo cáo có tiêu đề ''bài viết nè'' đã bị quản trị viên xoá', NULL, CAST(N'2022-01-13T21:18:25.183' AS DateTime), 1)
INSERT [dbo].[Notification] ([ID], [UserEmail], [Content], [PostID], [Time], [IsRead]) VALUES (70, N'lebuidihoa257@gmail.com', N'asdff@gmail.com đã thích về bài viết của bạn.', 50, CAST(N'2022-01-13T21:38:39.983' AS DateTime), 1)
INSERT [dbo].[Notification] ([ID], [UserEmail], [Content], [PostID], [Time], [IsRead]) VALUES (71, N'asdff@gmail.com', N'Bình luận của bạn với nội dung ''asdf'' đã bị quản trị viên xoá', NULL, CAST(N'2022-01-13T22:04:30.987' AS DateTime), 1)
INSERT [dbo].[Notification] ([ID], [UserEmail], [Content], [PostID], [Time], [IsRead]) VALUES (72, N'lebuidihoa257@gmail.com', N'asdff@gmail.com đã bình luận về bài viết của bạn.', 46, CAST(N'2022-01-13T22:06:18.107' AS DateTime), 1)
INSERT [dbo].[Notification] ([ID], [UserEmail], [Content], [PostID], [Time], [IsRead]) VALUES (73, N'asdff@gmail.com', N'Bình luận của bạn với nội dung ''123'' đã bị quản trị viên xoá', NULL, CAST(N'2022-01-13T22:06:35.247' AS DateTime), 1)
INSERT [dbo].[Notification] ([ID], [UserEmail], [Content], [PostID], [Time], [IsRead]) VALUES (74, N'lebuidihoa257@gmail.com', N'asdff@gmail.com đã bình luận về bài viết của bạn.', 50, CAST(N'2022-01-13T22:08:12.800' AS DateTime), 1)
INSERT [dbo].[Notification] ([ID], [UserEmail], [Content], [PostID], [Time], [IsRead]) VALUES (75, N'lebuidihoa257@gmail.com', N'asdff@gmail.com đã bình luận về bài viết của bạn.', 50, CAST(N'2022-01-13T22:12:10.890' AS DateTime), 1)
INSERT [dbo].[Notification] ([ID], [UserEmail], [Content], [PostID], [Time], [IsRead]) VALUES (76, N'asdff@gmail.com', N'Bình luận của bạn với nội dung ''1'' đã bị quản trị viên xoá', 50, CAST(N'2022-01-13T22:12:19.850' AS DateTime), 1)
INSERT [dbo].[Notification] ([ID], [UserEmail], [Content], [PostID], [Time], [IsRead]) VALUES (77, N'huuhieu@gmail.com', N'asdff@gmail.com đã gửi tin nhắn cho bạn', NULL, CAST(N'2022-01-13T22:16:34.140' AS DateTime), 0)
INSERT [dbo].[Notification] ([ID], [UserEmail], [Content], [PostID], [Time], [IsRead]) VALUES (78, N'huuhieu@gmail.com', N'asdff@gmail.com đã gửi tin nhắn cho bạn', NULL, CAST(N'2022-01-13T22:19:16.257' AS DateTime), 0)
SET IDENTITY_INSERT [dbo].[Notification] OFF
GO
SET IDENTITY_INSERT [dbo].[Post] ON 

INSERT [dbo].[Post] ([ID], [Title], [Content], [TimeCreate], [TimeModified], [Author], [Privacy], [ImageURL]) VALUES (12, N'OUR BELOVED SUMMER', N'<h3><strong>Soo, Vi&ecirc;n ngọc mang trong m&igrave;nh nhiều vết xước...!</strong></h3>

<p><em>(Tr&ograve;n một ng&agrave;y b&agrave;i viết của Woong, m&igrave;nh xin gửi đến c&aacute;c bạn b&agrave;i viết về Soo)</em></p>

<blockquote><em>V&igrave; sao m&igrave;nh lại v&iacute; Kook Yeon Su như vi&ecirc;n ngọc?</em></blockquote>

<blockquote><em>V&igrave; Soo sinh ra trong nghịch cảnh như ngọc trong đ&aacute;, Soo lớn l&ecirc;n v&agrave; trưởng th&agrave;nh c&ugrave;ng &aacute;p lực như vi&ecirc;n ngọc phải được m&agrave;i dũa mới trở n&ecirc;n s&aacute;ng b&oacute;ng&hellip;</em></blockquote>

<blockquote><em>Đẹp đẽ, cứng c&aacute;p l&agrave; thế nhưng đ&acirc;u ai biết được b&ecirc;n trong vi&ecirc;n ngọc c&oacute; bao nhi&ecirc;u vết trầy xước&hellip;?</em></blockquote>

<p><strong><em>T&ocirc;i gh&eacute;t sự thảm hại v&igrave; muốn giấu đi l&yacute; do m&igrave;nh chăm chỉ</em></strong></p>

<p>Kh&ocirc;ng &iacute;t lần ch&uacute;ng ta của thời ấu thơ tự hỏi v&igrave; sao bạn kia học chăm chỉ đến thế, đ&oacute; thực sự l&agrave; đam m&ecirc; hay l&agrave; &aacute;p lực?</p>

<p>Với Yeon Soo, m&igrave;nh nghĩ đ&oacute; l&agrave; bởi v&igrave; &aacute;p lực&hellip; Nhưng thay v&igrave; phải t&igrave;m l&yacute; do để che giấu sự thật v&agrave; cảm thấy m&igrave;nh thật thảm hại v&igrave; phải bịa ra l&yacute; do, Yeon Soo chọn n&oacute;i rằng m&igrave;nh gh&eacute;t sự thảm hại, gh&eacute;t sự thảm hai tức l&agrave; y&ecirc;u th&iacute;ch sự chăm chỉ v&agrave; th&agrave;nh c&ocirc;ng, c&ocirc; sẽ kh&ocirc;ng c&ograve;n bị bủa v&acirc;y bởi những c&acirc;u hỏi tại sao nữa.</p>

<p>Ước mơ r&otilde; r&agrave;ng nhất của Soo ch&iacute;nh l&agrave; bảo vệ b&agrave;, mải m&ecirc; đấu tranh thực hiện ước mơ đ&oacute;, c&ocirc; đ&atilde; qu&ecirc;n mất điều ước cho bản th&acirc;n&hellip;</p>

<p><em>C&ocirc; muốn giấu đi việc m&igrave;nh chăm chỉ v&igrave; ngh&egrave;o, muốn giấu đi việc m&igrave;nh kh&ocirc;ng r&otilde; ước mơ của bản th&acirc;n, muốn mọi người h&atilde;y cứ xem c&ocirc; l&agrave; một con nhỏ k&ecirc;nh kiệu gh&eacute;t sự thảm hại..!</em></p>

<p><strong><em>Giải quyết mọi vấn đề bằng niềm ki&ecirc;u h&atilde;nh</em></strong></p>

<p>Tự ti v&igrave; kh&ocirc;ng thể h&agrave;o ph&oacute;ng với bạn b&egrave;, chỉ c&oacute; thể nhận chứ kh&ocirc;ng thể cho, Soo đ&atilde; chọn c&aacute;ch biến bản th&acirc;n th&agrave;nh người bất cần v&agrave; kh&oacute; gần, kh&ocirc;ng c&oacute; bạn b&egrave; sẽ kh&ocirc;ng đ&aacute;nh mất đi niềm ki&ecirc;u h&atilde;nh</p>

<p>Giữa hiện thực t&agrave;n khốc đ&egrave; nặng l&ecirc;n đ&ocirc;i vai yếu ớt v&agrave; một anh bạn trai đưa c&ocirc; đến những giấc mộng đẹp đẽ, c&ocirc; đ&atilde; chọn từ bỏ giấc mộng ấy, &aacute;nh mặt trời duy nhất len lỏi v&agrave;o t&acirc;m hồn lạnh lẽo của m&igrave;nh để một m&igrave;nh đối mặt với thứ hiện thực ấy. C&ocirc; chia tay m&agrave; kh&ocirc;ng hề muốn n&oacute;i l&yacute; do, c&ocirc; kh&ocirc;ng muốn &aacute;nh mặt trời ấy rồi cũng bị bao phủ bởi b&oacute;ng đ&ecirc;m m&ugrave; mịt, c&ocirc; ki&ecirc;u h&atilde;nh rằng m&igrave;nh c&oacute; thể tự c&aacute;ng đ&aacute;ng mọi thứ, c&aacute;ng đ&aacute;ng lu&ocirc;n cả việc sống thiếu anh&hellip;</p>

<p>Chỉ c&oacute; điều, c&aacute;ch giải quyết n&agrave;y t&agrave;n nhẫn với người kh&aacute;c một th&igrave; t&agrave;n nhẫn với bản th&acirc;n c&ocirc; đến mười lần&hellip;</p>

<p><strong><em>D&ugrave; mạnh mẽ đến đ&acirc;u th&igrave; cũng vẫn l&agrave; con g&aacute;i</em></strong></p>

<p>Soo vẫn hay xị mặt nh&otilde;ng nhẽo, cũng kh&ocirc;ng th&iacute;ch nhận sai như những c&ocirc; g&aacute;i kh&aacute;c v&agrave; đặc biệt lu&ocirc;n chờ đợi ba chữ &ldquo;Anh y&ecirc;u em&rdquo; từ Woong</p>

<p>Mỗi đoạn m&agrave;&nbsp;<strong><em>&ldquo;M&agrave; nh&aacute; k&ecirc; m&agrave; nh&agrave;&hellip; Giả sử như m&agrave;&rdquo;</em></strong>, Soo giả sử đủ thứ chuyện tr&aacute;i ngang tr&ecirc;n đời như em đậu đại học c&ograve;n anh rớt th&igrave; như thế n&agrave;o, em đi l&agrave;m việc ở nước ngo&agrave;i th&igrave; sao . Woong chiều Soo trả lời ch&acirc;n th&agrave;nh l&agrave; anh sẽ l&agrave;m như thế n&agrave;y, thế kia nhưng cứ đến đoạn Soo hỏi tại sao anh phải l&agrave;m như vậy - đoạn quan trọng nhất, chờ đợi một cậu chốt hạ để sưởi ấm tr&aacute;i tim đầy lo lắng của c&ocirc; thiếu nữ đang y&ecirc;u th&igrave; Woong im ru :)))</p>

<p>Bộ n&oacute;i rằng&nbsp;<strong><em>&ldquo;V&igrave; anh y&ecirc;u em&rdquo;</em></strong>&nbsp;kh&oacute; lắm hả anh, y&ecirc;u bao l&acirc;u rồi m&agrave; c&ograve;n mắc cỡ =))), n&oacute;i chứ m&igrave;nh đ&ugrave;a v&igrave; Woong thật sự kh&ocirc;ng hiểu &yacute; Soo v&agrave; cũng bực m&igrave;nh sao chị cứ giả sử ho&agrave;i!</p>

<p>Trong mắt của Woong, điểm n&agrave;y thật kỳ lạ nhưng m&igrave;nh tin trong mắt của chị em th&igrave; Yeon Soo cũng sở hữu t&acirc;m hồn mơ mộng như bao c&ocirc; g&aacute;i kh&aacute;c m&agrave; th&ocirc;i!</p>

<p><strong><em>Đ&uacute;ng người sai ho&agrave;n cảnh</em></strong></p>

<p>Hiện thực của em v&agrave; của anh kh&aacute;c nhau như thế n&agrave;o?</p>

<p>Anh bẩm sinh đ&atilde; c&oacute; một cuộc sống b&igrave;nh thường trong khi em phải cố gắng để gi&agrave;nh lấy cuộc sống b&igrave;nh thường ấy</p>

<p>Anh từ chối cơ hội du học chỉ v&igrave; anh muốn sống một cuộc sống b&igrave;nh thường nhưng em th&igrave; theo đuổi m&atilde;i chẳng chạm đến sự b&igrave;nh thường ấy, thậm ch&iacute; n&oacute; c&agrave;ng l&uacute;c c&agrave;ng xa rời em..</p>

<p>Y&ecirc;u anh gi&uacute;p em nhất thời qu&ecirc;n đi thực tại nhưng khi tỉnh giấc em lại thấy m&igrave;nh như người mộng du, kh&ocirc;ng ph&acirc;n biệt được phương hướng. Giống như &aacute;nh s&aacute;ng lu&ocirc;n rực rỡ nhất khi n&oacute; chiếu v&agrave;o nơi b&oacute;ng tối s&acirc;u thẳm, Woong l&agrave; tia nắng m&ugrave;a h&egrave; c&agrave;ng rực rỡ th&igrave; đồng nghĩa với việc t&acirc;m hồn Soo c&agrave;ng u tối, nỗi mặc cảm c&agrave;ng to dần đến cực đại.</p>

<p>Nếu vẫn tiếp tục th&igrave; em sẽ mệt mỏi lắm, th&agrave; đau khổ một lần rồi th&ocirc;i, em sẽ tự m&igrave;nh vực dậy để đối mặt với thực tại. N&oacute; vẫn tốt hơn việc hằng ng&agrave;y phải lo sợ &aacute;nh nắng của em biết rằng b&ecirc;n trong em tăm tối thế n&agrave;o, lo sợ &aacute;nh nắng ấy cũng sẽ bị b&oacute;ng tối nuốt chửng, lo sợ &aacute;nh nắng ấy chịu nhiều tổn thương v&igrave; em v&agrave; lo sợ rằng em sẽ m&atilde;i m&atilde;i mất đi sự ki&ecirc;u h&atilde;nh với anh&hellip;</p>

<p><strong><em>Soo kh&ocirc;ng &iacute;ch kỉ..!</em></strong></p>

<p>Đ&acirc;y l&agrave; c&oacute; lẽ l&agrave; điểm m&igrave;nh thấy Woong hiểu sai về Soo, cũng như b&agrave;i trước m&igrave;nh c&oacute; ph&acirc;n t&iacute;ch việc Soo hiểu sai về Woong.</p>

<blockquote><em><strong>&ldquo;Khả năng đồng cảm cũng l&agrave; tr&iacute; tuệ</strong></em></blockquote>

<blockquote><em><strong>D&acirc;n tr&iacute; thức m&agrave; lại l&agrave;m việc thiếu đồng cảm th&igrave; chẳng c&oacute; g&igrave; để tự h&agrave;o!&rdquo;</strong></em></blockquote>

<p>Soo kh&ocirc;ng hề &iacute;ch kỉ, c&ocirc; chỉ đồng cảm với những người xứng đ&aacute;ng. C&ocirc; c&oacute; thể vo tr&ograve;n tờ giấy m&igrave;nh ch&eacute;p b&agrave;i để kh&ocirc;ng phải cho những đứa hay n&oacute;i xấu sau lưng mượn nhưng sẵn s&agrave;ng d&agrave;nh ra h&agrave;ng tiếng đồng để dạy Woong học.</p>

<p>C&ocirc; c&oacute; thể nhẫn nhịn được việc trưởng ph&ograve;ng Jang sỉ nhục m&igrave;nh nhưng kh&ocirc;ng bỏ qua nếu anh ta đả k&iacute;ch Woong hay chơi kh&ocirc; m&aacute;u với đ&agrave;n anh đại học nếu d&aacute;m ăn hiếp Woong :)))</p>

<p>C&ocirc; kh&ocirc;ng d&aacute;m gửi tin nhắn động vi&ecirc;n Woong v&igrave; sợ phiền anh nhưng sẽ &acirc;m thầm viết b&agrave;i chứng m&igrave;nh Woong kh&ocirc;ng đạo tranh.</p>

<p>Soo chỉ bật kh&oacute;c hỏi b&agrave; v&igrave; sao ch&uacute;ng ta phải g&aacute;nh khoản nợ n&agrave;y chứ chưa một lần l&ecirc;n tiếng o&aacute;n tr&aacute;ch rằng ho&agrave;n cảnh đ&atilde; giết chết ước mơ của ch&aacute;u ra sao.</p>

<p>Soo kh&ocirc;ng &iacute;ch kỉ, chỉ l&agrave; c&ocirc; qu&aacute; bận rộn để c&oacute; thể c&oacute; thể đồng cảm với tất cả mọi người.</p>

<p><em>Quả b&oacute;ng cầm tr&ecirc;n tay m&igrave;nh th&igrave; m&igrave;nh thấy n&oacute; to, nhưng người kh&aacute;c nh&igrave;n v&agrave;o sẽ thấy nhỏ, quả b&oacute;ng ngh&egrave;o n&agrave;n v&agrave; nợ nần tr&ecirc;n tay Soo qu&aacute; to v&agrave; cản tầm nh&igrave;n n&ecirc;n c&ocirc; kh&ocirc;ng thể nh&igrave;n thấy được quả b&oacute;ng của tất cả mọi người&hellip; chỉ c&oacute; người cầm quả b&oacute;ng mới c&oacute; thể thật sư biết n&oacute; thật sự to v&agrave; nặng như thế n&agrave;o!</em></p>

<blockquote>Ngo&agrave;i lề, Woong ngo&agrave;i việc l&agrave; tia nắng hạ của Yeon Soo th&igrave; cũng l&agrave; tia nắng hạ của m&igrave;nh, viết bao nhi&ecirc;u b&agrave;i th&igrave; đến b&agrave;i của Woong bỗng viral, Woong h&atilde;y lu&ocirc;n ấm &aacute;p c&ugrave;ng nụ cười toả nắng m&atilde;i như thế để d&igrave;u Soo đi qua khoảng thời gian tăm tối v&agrave; gi&uacute;p đ&ocirc;i mắt Soo c&oacute; hồn trở lại!</blockquote>

<blockquote>&nbsp;</blockquote>

<p>&nbsp;</p>

<p>&nbsp;</p>
', CAST(N'2021-12-24T22:22:38.197' AS DateTime), CAST(N'2021-12-24T22:22:38.197' AS DateTime), N'asdf@gmail.com', N'Public', NULL)
INSERT [dbo].[Post] ([ID], [Title], [Content], [TimeCreate], [TimeModified], [Author], [Privacy], [ImageURL]) VALUES (18, N'asdf', N'<p>asdf</p>
', CAST(N'2021-12-24T22:41:40.857' AS DateTime), CAST(N'2021-12-24T22:41:40.860' AS DateTime), N'asdf@gmail.com', N'Friend', NULL)
INSERT [dbo].[Post] ([ID], [Title], [Content], [TimeCreate], [TimeModified], [Author], [Privacy], [ImageURL]) VALUES (24, N'Đang lau nhà mà cứ quậy phá mẹ hoài ', N'<p style="text-align:center">Ngồi trỏng chơi t&iacute; nha con</p>

<p style="text-align:center"><img alt="" height="300" src="/uploads/asdff@gmail.com/Avatar-asdff@gmail.com267821332_5278598895487956_1387629587421371943_n.png" width="300" /></p>
', CAST(N'2021-12-27T21:22:45.253' AS DateTime), CAST(N'2021-12-27T21:22:45.253' AS DateTime), N'asdff@gmail.com', N'Friend', NULL)
INSERT [dbo].[Post] ([ID], [Title], [Content], [TimeCreate], [TimeModified], [Author], [Privacy], [ImageURL]) VALUES (25, N'Én phiên bản muỗi', N'<p><img alt="" src="/uploads/asdf@gmail.com/20211227215744269988262_990537334889450_4287756626031451645_n.jpg" style="height:375px; width:300px" /></p>
', CAST(N'2021-12-27T21:58:06.140' AS DateTime), CAST(N'2021-12-27T21:58:06.140' AS DateTime), N'asdf@gmail.com', N'Friend', NULL)
INSERT [dbo].[Post] ([ID], [Title], [Content], [TimeCreate], [TimeModified], [Author], [Privacy], [ImageURL]) VALUES (26, N'2', N'<p>3</p>
', CAST(N'2021-12-27T22:12:41.297' AS DateTime), CAST(N'2022-01-04T23:16:14.777' AS DateTime), N'lebuidihoa257@gmail.com', N'Friend', NULL)
INSERT [dbo].[Post] ([ID], [Title], [Content], [TimeCreate], [TimeModified], [Author], [Privacy], [ImageURL]) VALUES (29, N'Chào 2021!!!', N'<p style="text-align:center">Ch&agrave;o 2021!!!</p>
', CAST(N'2022-01-01T03:09:32.277' AS DateTime), CAST(N'2022-01-01T03:09:32.277' AS DateTime), N'asdff@gmail.com', N'Public', NULL)
INSERT [dbo].[Post] ([ID], [Title], [Content], [TimeCreate], [TimeModified], [Author], [Privacy], [ImageURL]) VALUES (45, N'123', N'<p>321</p>
', CAST(N'2022-01-13T19:12:27.540' AS DateTime), CAST(N'2022-01-13T19:12:27.540' AS DateTime), N'asdff@gmail.com', N'Public', NULL)
INSERT [dbo].[Post] ([ID], [Title], [Content], [TimeCreate], [TimeModified], [Author], [Privacy], [ImageURL]) VALUES (46, N'Xuất sắc. Không uổng công tui đi bới nó vất vả :))', N'<p style="text-align:center"><img alt="" height="300" src="/uploads/lebuidihoa257@gmail.com/20220113210433271872599_3116595811945163_1848658051130661646_n.jpg" width="300" /></p>

<p>&nbsp;</p>

<p>&nbsp;</p>
', CAST(N'2022-01-13T21:05:23.953' AS DateTime), CAST(N'2022-01-13T21:05:23.953' AS DateTime), N'lebuidihoa257@gmail.com', N'Friend', NULL)
INSERT [dbo].[Post] ([ID], [Title], [Content], [TimeCreate], [TimeModified], [Author], [Privacy], [ImageURL]) VALUES (50, N'Mọi người nhớ đi hiến máu nhé', N'<p style="text-align:center"><img alt="" height="176" src="/uploads/lebuidihoa257@gmail.com/20220113210636271761585_140679225035946_237296098749284778_n.jpg" width="400" /></p>

<p>&nbsp;</p>
', CAST(N'2022-01-13T21:06:54.717' AS DateTime), CAST(N'2022-01-13T21:06:54.717' AS DateTime), N'lebuidihoa257@gmail.com', N'Public', NULL)
SET IDENTITY_INSERT [dbo].[Post] OFF
GO
SET IDENTITY_INSERT [dbo].[PostComment] ON 

INSERT [dbo].[PostComment] ([ID], [UserEmail], [PostID], [Content], [NotificationID], [Time]) VALUES (1, N'asdf@gmail.com', 12, N'Da mii', NULL, CAST(N'2021-12-26T22:46:00.087' AS DateTime))
INSERT [dbo].[PostComment] ([ID], [UserEmail], [PostID], [Content], [NotificationID], [Time]) VALUES (2, N'lebuidihoa257@gmail.com', 12, N'Dami', 2, CAST(N'2021-12-26T22:46:16.467' AS DateTime))
INSERT [dbo].[PostComment] ([ID], [UserEmail], [PostID], [Content], [NotificationID], [Time]) VALUES (7, N'huuhieu@gmail.com', 29, N'Chúc mừng năm mới', 20, CAST(N'2022-01-04T17:13:45.000' AS DateTime))
INSERT [dbo].[PostComment] ([ID], [UserEmail], [PostID], [Content], [NotificationID], [Time]) VALUES (8, N'lebuidihoa257@gmail.com', 29, N'123', 28, CAST(N'2022-01-06T11:26:01.777' AS DateTime))
INSERT [dbo].[PostComment] ([ID], [UserEmail], [PostID], [Content], [NotificationID], [Time]) VALUES (9, N'asdff@gmail.com', 29, N'hihihi', NULL, CAST(N'2022-01-10T22:20:30.000' AS DateTime))
INSERT [dbo].[PostComment] ([ID], [UserEmail], [PostID], [Content], [NotificationID], [Time]) VALUES (10, N'lebuidihoa257@gmail.com', 29, N'Chúc mừng năm mới', 35, CAST(N'2022-01-10T23:52:42.803' AS DateTime))
SET IDENTITY_INSERT [dbo].[PostComment] OFF
GO
INSERT [dbo].[PostLike] ([UserEmail], [PostID], [NotificationID]) VALUES (N'asdf@gmail.com', 12, NULL)
INSERT [dbo].[PostLike] ([UserEmail], [PostID], [NotificationID]) VALUES (N'asdff@gmail.com', 26, 11)
INSERT [dbo].[PostLike] ([UserEmail], [PostID], [NotificationID]) VALUES (N'asdff@gmail.com', 29, NULL)
INSERT [dbo].[PostLike] ([UserEmail], [PostID], [NotificationID]) VALUES (N'asdff@gmail.com', 45, NULL)
INSERT [dbo].[PostLike] ([UserEmail], [PostID], [NotificationID]) VALUES (N'asdff@gmail.com', 50, 70)
INSERT [dbo].[PostLike] ([UserEmail], [PostID], [NotificationID]) VALUES (N'lebuidihoa257@gmail.com', 12, 1)
INSERT [dbo].[PostLike] ([UserEmail], [PostID], [NotificationID]) VALUES (N'lebuidihoa257@gmail.com', 29, 27)
INSERT [dbo].[PostLike] ([UserEmail], [PostID], [NotificationID]) VALUES (N'lebuidihoa257@gmail.com', 45, 52)
GO
INSERT [dbo].[PostReport] ([EmailAddress], [PostID], [Time], [Content]) VALUES (N'lebuidihoa257@gmail.com', 29, CAST(N'2022-01-11T23:51:58.020' AS DateTime), N'asdf')
GO
SET IDENTITY_INSERT [dbo].[PostShare] ON 

INSERT [dbo].[PostShare] ([ID], [UserEmail], [PostID], [Privacy], [NotificationID], [Time]) VALUES (1, N'lebuidihoa257@gmail.com', 24, N'Friend', 3, CAST(N'2021-12-26T22:46:16.463' AS DateTime))
INSERT [dbo].[PostShare] ([ID], [UserEmail], [PostID], [Privacy], [NotificationID], [Time]) VALUES (5, N'lebuidihoa257@gmail.com', 29, N'Friend', 29, CAST(N'2022-01-06T11:27:02.347' AS DateTime))
INSERT [dbo].[PostShare] ([ID], [UserEmail], [PostID], [Privacy], [NotificationID], [Time]) VALUES (11, N'asdff@gmail.com', 50, N'Friend', 60, CAST(N'2022-01-13T21:12:27.207' AS DateTime))
SET IDENTITY_INSERT [dbo].[PostShare] OFF
GO
INSERT [dbo].[ReportUser] ([UserEmail], [ReportedUser], [Time], [Content]) VALUES (N'asdf@gmail.com', N'asdff@gmail.com', CAST(N'2022-01-12T00:45:19.173' AS DateTime), N'Spam')
INSERT [dbo].[ReportUser] ([UserEmail], [ReportedUser], [Time], [Content]) VALUES (N'lebuidihoa257@gmail.com', N'asdff@gmail.com', CAST(N'2022-01-12T01:11:28.587' AS DateTime), N'Ngôn từ gây thù ghét')
GO
INSERT [dbo].[SuspendedUser] ([SuspendedEmail]) VALUES (N'asdf@gmail.com')
GO
INSERT [dbo].[Users] ([EmailAddress], [Password], [name], [sex], [DateOfBirth], [PhoneNumber], [SignInStatus], [AvatarURL]) VALUES (N'asdf@gmail.com', N'$2b$10$4TDtTafzIie94TrpplaWrulxt69Rp428abF.ECygXeW2gBGUSSrG6', N'Lê Hoà', N'Nam', CAST(N'2017-07-05' AS Date), N'+10792148688', N'Offline', N'/logo/logo_alrightsocial_circle.png')
INSERT [dbo].[Users] ([EmailAddress], [Password], [name], [sex], [DateOfBirth], [PhoneNumber], [SignInStatus], [AvatarURL]) VALUES (N'asdff@gmail.com', N'$2b$10$bp.08V6KDGqNI.g76IIYDOgDmi3Q3u.un.k4jfcVu4d3rdKC/16Je', N'Dĩ Hoà', N'Nam', CAST(N'2021-12-25' AS Date), N'+10792148688', N'Online', N'/uploads/asdff@gmail.com/Avatar-asdff@gmail.com270583776_127543036418777_188482142765861458_n.jpg')
INSERT [dbo].[Users] ([EmailAddress], [Password], [name], [sex], [DateOfBirth], [PhoneNumber], [SignInStatus], [AvatarURL]) VALUES (N'huuhieu@gmail.com', N'$2b$10$43eRJrnyTfDmpTvdIsLDlukGVOrIe6GwIqf9ehIha217HWD67jNDe', N'Bùi Hữu Hiếu', N'Nam', CAST(N'2021-12-30' AS Date), N'+10773546784', N'Offline', N'/logo/logo_alrightsocial_circle.png')
INSERT [dbo].[Users] ([EmailAddress], [Password], [name], [sex], [DateOfBirth], [PhoneNumber], [SignInStatus], [AvatarURL]) VALUES (N'lebuidihoa257@gmail.com', N'$2b$10$/ycxxqQYioBuMm69Jh57E.IF2Me5Gt5/VPa7IhSjPrfRQ9LUh1uLa', N'Lê Bùi Dĩ Hoà', N'Nam', CAST(N'2001-07-25' AS Date), N'+10792148688', N'Offline', N'/uploads/lebuidihoa257@gmail.com/Avatar-lebuidihoa257@gmail.comScreenshot 2021-10-25 154739.png')
GO
ALTER TABLE [dbo].[BlockedEmail]  WITH CHECK ADD FOREIGN KEY([BlockedUser])
REFERENCES [dbo].[Users] ([EmailAddress])
GO
ALTER TABLE [dbo].[BlockedEmail]  WITH CHECK ADD FOREIGN KEY([UserEmail])
REFERENCES [dbo].[Users] ([EmailAddress])
GO
ALTER TABLE [dbo].[Chats]  WITH CHECK ADD FOREIGN KEY([User1])
REFERENCES [dbo].[Users] ([EmailAddress])
GO
ALTER TABLE [dbo].[Chats]  WITH CHECK ADD FOREIGN KEY([User2])
REFERENCES [dbo].[Users] ([EmailAddress])
GO
ALTER TABLE [dbo].[DeletedComment]  WITH CHECK ADD FOREIGN KEY([NotificationID])
REFERENCES [dbo].[DeletedNotification] ([ID])
GO
ALTER TABLE [dbo].[DeletedComment]  WITH CHECK ADD FOREIGN KEY([PostID])
REFERENCES [dbo].[DeletedPost] ([ID])
GO
ALTER TABLE [dbo].[DeletedComment]  WITH CHECK ADD FOREIGN KEY([UserEmail])
REFERENCES [dbo].[Users] ([EmailAddress])
GO
ALTER TABLE [dbo].[DeletedLike]  WITH CHECK ADD FOREIGN KEY([NotificationID])
REFERENCES [dbo].[DeletedNotification] ([ID])
GO
ALTER TABLE [dbo].[DeletedLike]  WITH CHECK ADD FOREIGN KEY([PostID])
REFERENCES [dbo].[DeletedPost] ([ID])
GO
ALTER TABLE [dbo].[DeletedLike]  WITH CHECK ADD FOREIGN KEY([UserEmail])
REFERENCES [dbo].[Users] ([EmailAddress])
GO
ALTER TABLE [dbo].[DeletedNotification]  WITH CHECK ADD FOREIGN KEY([PostID])
REFERENCES [dbo].[DeletedPost] ([ID])
GO
ALTER TABLE [dbo].[DeletedNotification]  WITH CHECK ADD FOREIGN KEY([UserEmail])
REFERENCES [dbo].[Users] ([EmailAddress])
GO
ALTER TABLE [dbo].[DeletedShare]  WITH CHECK ADD FOREIGN KEY([NotificationID])
REFERENCES [dbo].[DeletedNotification] ([ID])
GO
ALTER TABLE [dbo].[DeletedShare]  WITH CHECK ADD FOREIGN KEY([PostID])
REFERENCES [dbo].[DeletedPost] ([ID])
GO
ALTER TABLE [dbo].[DeletedShare]  WITH CHECK ADD FOREIGN KEY([UserEmail])
REFERENCES [dbo].[Users] ([EmailAddress])
GO
ALTER TABLE [dbo].[Friend]  WITH CHECK ADD FOREIGN KEY([FriendEmail])
REFERENCES [dbo].[Users] ([EmailAddress])
GO
ALTER TABLE [dbo].[Friend]  WITH CHECK ADD FOREIGN KEY([UserEmail])
REFERENCES [dbo].[Users] ([EmailAddress])
GO
ALTER TABLE [dbo].[FriendRequest]  WITH CHECK ADD FOREIGN KEY([FriendEmail])
REFERENCES [dbo].[Users] ([EmailAddress])
GO
ALTER TABLE [dbo].[FriendRequest]  WITH CHECK ADD FOREIGN KEY([UserEmail])
REFERENCES [dbo].[Users] ([EmailAddress])
GO
ALTER TABLE [dbo].[Message]  WITH CHECK ADD FOREIGN KEY([ChatId])
REFERENCES [dbo].[Chats] ([ID])
GO
ALTER TABLE [dbo].[Message]  WITH CHECK ADD FOREIGN KEY([SenderEmail])
REFERENCES [dbo].[Users] ([EmailAddress])
GO
ALTER TABLE [dbo].[Notification]  WITH CHECK ADD FOREIGN KEY([PostID])
REFERENCES [dbo].[Post] ([ID])
GO
ALTER TABLE [dbo].[Notification]  WITH CHECK ADD FOREIGN KEY([UserEmail])
REFERENCES [dbo].[Users] ([EmailAddress])
GO
ALTER TABLE [dbo].[PostComment]  WITH CHECK ADD FOREIGN KEY([NotificationID])
REFERENCES [dbo].[Notification] ([ID])
GO
ALTER TABLE [dbo].[PostComment]  WITH CHECK ADD FOREIGN KEY([PostID])
REFERENCES [dbo].[Post] ([ID])
GO
ALTER TABLE [dbo].[PostComment]  WITH CHECK ADD FOREIGN KEY([UserEmail])
REFERENCES [dbo].[Users] ([EmailAddress])
GO
ALTER TABLE [dbo].[PostLike]  WITH CHECK ADD FOREIGN KEY([NotificationID])
REFERENCES [dbo].[Notification] ([ID])
GO
ALTER TABLE [dbo].[PostLike]  WITH CHECK ADD FOREIGN KEY([PostID])
REFERENCES [dbo].[Post] ([ID])
GO
ALTER TABLE [dbo].[PostLike]  WITH CHECK ADD FOREIGN KEY([UserEmail])
REFERENCES [dbo].[Users] ([EmailAddress])
GO
ALTER TABLE [dbo].[PostReport]  WITH CHECK ADD FOREIGN KEY([EmailAddress])
REFERENCES [dbo].[Users] ([EmailAddress])
GO
ALTER TABLE [dbo].[PostReport]  WITH CHECK ADD FOREIGN KEY([PostID])
REFERENCES [dbo].[Post] ([ID])
GO
ALTER TABLE [dbo].[PostShare]  WITH CHECK ADD FOREIGN KEY([NotificationID])
REFERENCES [dbo].[Notification] ([ID])
GO
ALTER TABLE [dbo].[PostShare]  WITH CHECK ADD FOREIGN KEY([PostID])
REFERENCES [dbo].[Post] ([ID])
GO
ALTER TABLE [dbo].[PostShare]  WITH CHECK ADD FOREIGN KEY([UserEmail])
REFERENCES [dbo].[Users] ([EmailAddress])
GO
ALTER TABLE [dbo].[ReportUser]  WITH CHECK ADD FOREIGN KEY([ReportedUser])
REFERENCES [dbo].[Users] ([EmailAddress])
GO
ALTER TABLE [dbo].[ReportUser]  WITH CHECK ADD FOREIGN KEY([UserEmail])
REFERENCES [dbo].[Users] ([EmailAddress])
GO
ALTER TABLE [dbo].[SuspendedUser]  WITH CHECK ADD FOREIGN KEY([SuspendedEmail])
REFERENCES [dbo].[Users] ([EmailAddress])
GO
