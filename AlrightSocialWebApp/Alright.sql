create table Users (
	 EmailAddress nvarchar(255) NOT NULL PRIMARY KEY,
	 Password nvarchar(255) NOT NULL,
	 name nvarchar(255),
	 sex nvarchar(255),
	 DateOfBirth date,
	 PhoneNumber nvarchar(255),
	 SignInStatus nvarchar(255),
	 AvatarURL nvarchar(255)
)

CREATE TABLE Post (
	ID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	Title NVARCHAR(255),
	Content NVARCHAR(MAX),
	TimeCreate DATETIME,
	TimeModified DATETIME,
	Author NVARCHAR(255) NOT NULL,
	Privacy NVARCHAR(255),
	ImageURL NVARCHAR(255)
)

CREATE TABLE FriendRequest (
	UserEmail NVARCHAR(255) NOT NULL,
	FriendEmail NVARCHAR(255) NOT NULL,
	Time DATETIME,
	PRIMARY KEY (UserEmail, FriendEmail),
	FOREIGN KEY (UserEmail) REFERENCES Users(EmailAddress),
	FOREIGN KEY (FriendEmail) REFERENCES Users(EmailAddress)
)

CREATE TABLE Friend (
	UserEmail NVARCHAR(255) NOT NULL,
	FriendEmail NVARCHAR(255) NOT NULL
	PRIMARY KEY (UserEmail, FriendEmail),
	FOREIGN KEY (UserEmail) REFERENCES Users(EmailAddress),
	FOREIGN KEY (FriendEmail) REFERENCES Users(EmailAddress)
)

CREATE TABLE Notification (
	ID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	UserEmail NVARCHAR(255) NOT NULL,
	Content NVARCHAR(MAX),
	PostID int NOT NULL,
	Time DATETIME,
	IsRead BIT,
	FOREIGN KEY (UserEmail) REFERENCES Users(EmailAddress),
	FOREIGN KEY (PostID) REFERENCES Post(ID)
)

CREATE TABLE PostLike (
	UserEmail NVARCHAR(255) NOT NULL,
	PostID int NOT NULL,
	NotificationID int,
	PRIMARY KEY (UserEmail, PostID),
	FOREIGN KEY (UserEmail) REFERENCES Users(EmailAddress),
	FOREIGN KEY (PostID) REFERENCES Post(ID),
	FOREIGN KEY (NotificationID) REFERENCES Notification(ID)
)

CREATE TABLE PostShare (
	ID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	UserEmail NVARCHAR(255) NOT NULL,
	PostID int NOT NULL,
	Privacy NVARCHAR(255),
	NotificationID int,
	Time Datetime,
	FOREIGN KEY (UserEmail) REFERENCES Users(EmailAddress),
	FOREIGN KEY (PostID) REFERENCES Post(ID),
	FOREIGN KEY (NotificationID) REFERENCES Notification(ID)
)

CREATE TABLE PostComment (
	ID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	UserEmail NVARCHAR(255) NOT NULL,
	PostID INT NOT NULL,
	Content NVARCHAR(MAX),
	NotificationID int,
	Time Datetime,
	FOREIGN KEY (UserEmail) REFERENCES Users(EmailAddress),
	FOREIGN KEY (PostID) REFERENCES Post(ID),
	FOREIGN KEY (NotificationID) REFERENCES Notification(ID)
)

CREATE TABLE CommentLike (
	UserEmail NVARCHAR(255) NOT NULL,
	CmtID int NOT NULL,
	NotificationID INT,
	FOREIGN KEY (UserEmail) REFERENCES Users(EmailAddress),
	FOREIGN KEY (CmtID) REFERENCES PostComment(ID),
	FOREIGN KEY (NotificationID) REFERENCES Notification(ID)
)

CREATE TABLE Message (
	ID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	SenderEmail NVARCHAR(255) NOT NULL,
	ReceiverEmail NVARCHAR(255) NOT NULL,
	Time DATETIME,
	Content NVARCHAR(MAX),
	FOREIGN KEY (SenderEmail) REFERENCES Users(EmailAddress),
	FOREIGN KEY (ReceiverEmail) REFERENCES Users(EmailAddress)
)

CREATE TABLE BlockedEmail (
	UserEmail NVARCHAR(255) NOT NULL,
	BlockedEmail NVARCHAR(255) NOT NULL,
	PRIMARY KEY (UserEmail, BlockedEmail),
	FOREIGN KEY (UserEmail) REFERENCES Users(EmailAddress),
	FOREIGN KEY (BlockedEmail) REFERENCES Users(EmailAddress)
)

CREATE TABLE SuspendedUser (
	SuspendedEmail NVARCHAR(255) NOT NULL,
	Duration int,
	FOREIGN KEY (SuspendedEmail) REFERENCES Users(EmailAddress)
)


SELECT Post.ID, Title, Content, TimeCreate, TimeModified, Author, Privacy, ISNULL(LikeTable.[Like],0) AS [Like] , ISNULL(CommentTable.[Comment],0) AS [Comment], ISNULL(ShareTable.[Share],0) AS [Share]
FROM Post 
	LEFT JOIN 
	(SELECT PostLike.PostID ID, COUNT(UserEmail) AS [Like] 
	FROM PostLike 
	GROUP BY PostLike.PostID) LikeTable 
	ON LikeTable.ID = Post.ID 
	LEFT JOIN 
	(SELECT PostComment.PostID, COUNT(UserEmail) AS [Comment]
	FROM PostComment
	GROUP BY PostComment.PostID) CommentTable
	ON Post.ID=CommentTable.PostID
	LEFT JOIN 
	(SELECT PostID, COUNT(UserEmail) AS [Share]
	FROM PostShare
	GROUP BY PostShare.PostID) ShareTable
	ON Post.ID=ShareTable.PostID
WHERE Post.ID=10

SELECT ISNULL(MAX(ID),0) AS [NUMBER] FROM NOTIFICATION
SELECT * FROM PostComment WHERE PostID=10

SELECT Post.ID, Title, Content, TimeCreate, TimeModified, Author, Privacy, Users.AvatarURL, Users.name, ISNULL(LikeTable.[Like],0) AS [Like] , ISNULL(CommentTable.[Comment],0) AS [Comment], ISNULL(ShareTable.[Share],0) AS [Share] 
FROM Post 
LEFT JOIN 
(SELECT PostLike.PostID ID, COUNT(UserEmail) AS [Like] 
FROM PostLike 
GROUP BY PostLike.PostID) LikeTable 
ON LikeTable.ID = Post.ID 
LEFT JOIN 
(SELECT PostComment.PostID, COUNT(UserEmail) AS [Comment] 
FROM PostComment 
GROUP BY PostComment.PostID) CommentTable 
ON Post.ID = CommentTable.PostID 
LEFT JOIN 
(SELECT PostID, COUNT(UserEmail) AS [Share] 
FROM PostShare 
GROUP BY PostShare.PostID) ShareTable 
ON Post.ID = ShareTable.PostID
INNER JOIN 
Users
ON Post.Author=Users.EmailAddress
WHERE Post.Privacy='Public'

SELECT COUNT(*) AS [DEM]
FROM PostLike
WHERE PostID=12 AND UserEmail='lebuidihoa257@gmail.com'