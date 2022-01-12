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
	PostID int,
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
	Time DATETIME,
	Content NVARCHAR(MAX),
	ChatId INT NOT NULL,
	FOREIGN KEY (SenderEmail) REFERENCES Users(EmailAddress),
	FOREIGN KEY (ChatId) REFERENCES Chats(ID)
)

CREATE TABLE BlockedEmail (
	UserEmail NVARCHAR(255) NOT NULL,
	BlockedUser NVARCHAR(255) NOT NULL,
	PRIMARY KEY (UserEmail, BlockedUser),
	FOREIGN KEY (UserEmail) REFERENCES Users(EmailAddress),
	FOREIGN KEY (BlockedUser) REFERENCES Users(EmailAddress)
)

CREATE TABLE SuspendedUser (
	SuspendedEmail NVARCHAR(255) NOT NULL,
	FOREIGN KEY (SuspendedEmail) REFERENCES Users(EmailAddress)
)

CREATE TABLE Chats (
	ID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	User1 NVARCHAR(255) NOT NULL,
	User2 NVARCHAR(255) NOT NULL,
	FOREIGN KEY (User1) REFERENCES Users(EmailAddress),
	FOREIGN KEY (User2) REFERENCES Users(EmailAddress)
)

CREATE TABLE Administrator (
	EmailAddress nvarchar(255) NOT NULL PRIMARY KEY,
	 Password nvarchar(255) NOT NULL,
	 name nvarchar(255),
	 sex nvarchar(255),
	 DateOfBirth date,
	 PhoneNumber nvarchar(255),
	 SignInStatus nvarchar(255),
	 AvatarURL nvarchar(255)
)

CREATE TABLE PostReport(
	EmailAddress nvarchar(255) NOT NULL,
	PostID int NOT NULL,
	Time datetime NOT NULL,
	Content nvarchar(max),
	PRIMARY KEY (EmailAddress, PostID),
	FOREIGN KEY (EmailAddress) REFERENCES Users(EmailAddress),
	FOREIGN KEY (PostID) REFERENCES Post(ID),
)

CREATE TABLE ReportUser (
	UserEmail nvarchar(255) NOT NULL,
	ReportedUser nvarchar(255) NOT NULL,
	Time datetime NOT NULL,
	Content nvarchar(max),
	PRIMARY KEY (UserEmail,ReportedUser),
	FOREIGN KEY (UserEmail) REFERENCES Users(EmailAddress),
	FOREIGN KEY (ReportedUser) REFERENCES Users(EmailAddress)
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

SELECT * FROM Friend WHERE UserEmail='lebuidihoa257@gmail.com'
SELECT FriendEmail, Time FROM FriendRequest WHERE UserEmail='lebuidihoa257@gmail.com'

SELECT Post.ID, Title, Content, TimeCreate, TimeModified, Author, Privacy, Users.AvatarURL, Users.name, ISNULL(LikeTable.[Like], 0) AS[Like] , ISNULL(CommentTable.[Comment], 0) AS[Comment], ISNULL(ShareTable.[Share], 0) AS[Share] 
FROM 
(SELECT Post.ID, Title, Content, TimeCreate, TimeModified, Author, Privacy FROM Post WHERE Author=@EmailAddress AND Privacy='Public'
UNION 
SELECT Post.ID, Title, Content, TimeCreate, TimeModified, Author, Privacy 
FROM Post 
WHERE Privacy='Friend' AND Author IN (SELECT FriendEmail FROM Friend WHERE UserEmail=@CurrentEmail)) AS [Post]
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
FROM PostShare GROUP BY PostShare.PostID) ShareTable 
ON Post.ID = ShareTable.PostID 
INNER JOIN Users 
ON Post.Author = Users.EmailAddress 
ORDER BY Post.TimeCreate DESC


SELECT Post.ID, Title, Content, TimeCreate, TimeModified, Author, Privacy 
FROM Post 
WHERE Privacy!='Only Me' AND Author IN (SELECT FriendEmail FROM Friend WHERE UserEmail='lebuidihoa257@gmail.com')

SELECT Post.ID, Title, Content, TimeCreate, TimeModified, Author, Privacy, Users.AvatarURL, Users.name, ISNULL(LikeTable.[Like], 0) AS[Like] , ISNULL(CommentTable.[Comment], 0) AS[Comment], ISNULL(ShareTable.[Share], 0) AS[Share]  
FROM
(SELECT Post.ID, Title, Content, TimeCreate, TimeModified, Author, Privacy 
FROM Post 
WHERE Author = @EmailAddress 
UNION 
SELECT Post.ID, Title, Content, TimeCreate, TimeModified, Author, Privacy 
FROM Post 
WHERE Privacy = 'Friend' AND Author IN(SELECT FriendEmail FROM Friend WHERE UserEmail = @EmailAddress) 
UNION 
SELECT Post.ID, Title, Content, TimeCreate, TimeModified, Author, Privacy 
FROM Post 
WHERE Privacy = 'Public' AND Author Not IN (SELECT BlockedUser FROM BlockedEmail WHERE UserEmail=@EmailAddress)) AS[Post] 
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
INNER JOIN Users 
ON Post.Author = Users.EmailAddress 
ORDER BY Post.TimeCreate DESC

SELECT COUNT(*) AS [DEM] FROM Friend WHERE UserEmail = 'lebuidihoa257@gmail.com' AND FriendEmail='asdff@gmail.com'

SELECT Id FROM Chats WHERE (User1=@User1 AND User2=@User2) OR (User2=@User1 AND User1=@User2)

SELECT FriendEmail, AvatarURL, name FROM Friend, Users WHERE UserEmail='lebuidihoa257@gmail.com' AND FriendEmail=Users.EmailAddress

SELECT TOP 1 Content, Time
FROM Message 
WHERE ChatId = 1
ORDER By Time DESC


SELECT Post.ID, Title, Content, TimeCreate, TimeModified, Author, Users.AvatarURL, Users.name, Privacy, Post.SharePrivacy, ISNULL(LikeTable.[Like],0) AS [Like] , ISNULL(CommentTable.[Comment],0) AS [Comment], ISNULL(ShareTable.[Share],0) AS [Share] 
FROM 
(SELECT Post.ID, Title, Content, TimeCreate, TimeModified, Author, Post.Privacy, ('0') AS [SharePrivacy] 
FROM Post 
WHERE Post.Author = 'lebuidihoa257@gmail.com' 
UNION 
SELECT PostShare.PostID, Title, Content, TimeCreate, TimeModified, Author, Post.Privacy, PostShare.Privacy AS [SharePrivacy] 
FROM Post, PostShare 
WHERE Post.ID = PostShare.PostID AND PostShare.UserEmail = 'lebuidihoa257@gmail.com' ) AS[Post] 
LEFT JOIN (SELECT PostLike.PostID ID, COUNT(UserEmail) AS [Like] FROM PostLike GROUP BY PostLike.PostID) LikeTable ON LikeTable.ID = Post.ID LEFT JOIN (SELECT PostComment.PostID, COUNT(UserEmail) AS [Comment] FROM PostComment GROUP BY PostComment.PostID) CommentTable ON Post.ID = CommentTable.PostID LEFT JOIN(SELECT PostID, COUNT(UserEmail) AS [Share] FROM PostShare GROUP BY PostShare.PostID) ShareTable ON Post.ID = ShareTable.PostID INNER JOIN Users ON Users.EmailAddress = Post.Author ORDER BY Post.TimeCreate DESC

SELECT Post.ID, Title, Content, TimeCreate, TimeModified, Author, Privacy, Users.AvatarURL, Users.name, ISNULL(Post.SharePrivacy,0) AS [SharePrivacy], ISNULL(LikeTable.[Like], 0) AS[Like] , ISNULL(CommentTable.[Comment], 0) AS[Comment], ISNULL(ShareTable.[Share], 0) AS[Share] 
FROM 
(SELECT Post.ID, Title, Content, TimeCreate, TimeModified, Author, Post.Privacy, ('0') AS [SharePrivacy]
FROM Post 
WHERE (Post.Author='asdff@gmail.com' AND Post.Privacy = 'Public') 
UNION
SELECT Post.ID, Title, Content, TimeCreate, TimeModified, Author, Post.Privacy, PostShare.Privacy AS [SharePrivacy]
FROM Post INNER JOIN PostShare 
ON Post.ID=PostShare.PostID 
WHERE (PostShare.UserEmail='asdff@gmail.com' AND PostShare.Privacy='Public') 
UNION 
SELECT Post.ID, Title, Content, TimeCreate, TimeModified, Author, Post.Privacy, ('0') AS [SharePrivacy]
FROM Post LEFT JOIN PostShare ON Post.ID=PostShare.PostID 
WHERE (Post.Author='asdff@gmail.com' AND Post.Privacy = 'Friend' AND Author IN (SELECT FriendEmail FROM Friend WHERE UserEmail = 'lebuidihoa257@gmail.com')) 
UNION
SELECT Post.ID, Title, Content, TimeCreate, TimeModified, Author, Post.Privacy, PostShare.Privacy AS [SharePrivacy]
FROM Post INNER JOIN PostShare 
ON Post.ID=PostShare.PostID 
WHERE(PostShare.UserEmail='asdff@gmail.com' AND PostShare.Privacy='Friend' AND PostShare.UserEmail IN (SELECT FriendEmail FROM Friend WHERE UserEmail = 'lebuidihoa257@gmail.com'))) AS [Post] 
LEFT JOIN (SELECT PostLike.PostID ID, COUNT(UserEmail) AS [Like] FROM PostLike GROUP BY PostLike.PostID) LikeTable ON LikeTable.ID = Post.ID LEFT JOIN (SELECT PostComment.PostID, COUNT(UserEmail) AS [Comment] FROM PostComment GROUP BY PostComment.PostID) CommentTable ON Post.ID = CommentTable.PostID LEFT JOIN (SELECT PostID, COUNT(UserEmail) AS [Share] FROM PostShare GROUP BY PostShare.PostID) ShareTable ON Post.ID = ShareTable.PostID INNER JOIN Users ON Post.Author = Users.EmailAddress ORDER BY Post.TimeCreate DESC


SELECT Post.ID, Title, Content, TimeCreate, TimeModified, Author, Post.Privacy, Users.AvatarURL, Users.name, ISNULL(PostShare.Privacy,0) AS [SharePrivacy], ISNULL(LikeTable.[Like], 0) AS[Like] , ISNULL(CommentTable.[Comment], 0) AS[Comment], ISNULL(ShareTable.[Share], 0) AS[Share] 
FROM 
Post LEFT JOIN PostShare 
ON Post.ID=PostShare.PostID 
LEFT JOIN 
(SELECT PostLike.PostID ID, COUNT(UserEmail) AS [Like] FROM PostLike GROUP BY PostLike.PostID) LikeTable ON LikeTable.ID = Post.ID LEFT JOIN (SELECT PostComment.PostID, COUNT(UserEmail) AS [Comment] FROM PostComment GROUP BY PostComment.PostID) CommentTable ON Post.ID = CommentTable.PostID LEFT JOIN (SELECT PostID, COUNT(UserEmail) AS [Share] FROM PostShare GROUP BY PostShare.PostID) ShareTable ON Post.ID = ShareTable.PostID INNER JOIN Users ON Users.EmailAddress = Post.Author WHERE (Author = 'lebuidihoa257@gmail.com' AND Post.Privacy='Public') OR (PostShare.UserEmail='lebuidihoa257@gmail.com' AND PostShare.Privacy='Public') ORDER BY Post.TimeCreate DESC

SELECT Post.ID, Title, Content, TimeCreate, TimeModified, Author, Privacy, Users.AvatarURL, Users.name,Post.SharePrivacy,Post.SharedEmail, Post.SharedName, Post.SharedAvatar, ISNULL(ShareTime,0) AS [ShareTime], ISNULL(LikeTable.[Like], 0) AS[Like] , ISNULL(CommentTable.[Comment], 0) AS[Comment], ISNULL(ShareTable.[Share], 0) AS[Share] 
FROM 
(SELECT Post.ID, Title, Content, TimeCreate, TimeModified, Author, Post.Privacy, ('0') AS [SharePrivacy], ('0') AS [SharedEmail], ('0') AS [SharedName], ('0') AS [SharedAvatar], NULL AS [ShareTime]
FROM Post 
UNION
SELECT Post.ID, Title, Content, TimeCreate, TimeModified, Author, Post.Privacy, PostShare.Privacy AS [SharePrivacy], PostShare.UserEmail AS [SharedEmail], Users.name AS [SharedName], Users.AvatarURL AS [SharedAvatar], PostShare.Time AS [ShareTime]
FROM Post INNER JOIN PostShare 
ON Post.ID=PostShare.PostID INNER JOIN Users ON PostShare.UserEmail=Users.EmailAddress) AS [Post]
LEFT JOIN (SELECT PostLike.PostID ID, COUNT(UserEmail) AS [Like] FROM PostLike GROUP BY PostLike.PostID) LikeTable ON LikeTable.ID = Post.ID LEFT JOIN (SELECT PostComment.PostID, COUNT(UserEmail) AS [Comment] FROM PostComment GROUP BY PostComment.PostID) CommentTable ON Post.ID = CommentTable.PostID LEFT JOIN (SELECT PostID, COUNT(UserEmail) AS [Share] FROM PostShare GROUP BY PostShare.PostID) ShareTable ON Post.ID = ShareTable.PostID INNER JOIN Users ON Post.Author = Users.EmailAddress WHERE (Post.Privacy='Public' OR Post.SharePrivacy='Public') ORDER BY Post.TimeCreate DESC

SELECT Post.ID, Title, Content, TimeCreate, TimeModified, Author, Privacy, Users.AvatarURL, Users.name,Post.SharePrivacy,Post.SharedEmail, Post.SharedName, Post.SharedAvatar, ISNULL(ShareTime,0) AS [ShareTime], ISNULL(LikeTable.[Like], 0) AS[Like] , ISNULL(CommentTable.[Comment], 0) AS[Comment], ISNULL(ShareTable.[Share], 0) AS[Share]   
FROM 
(SELECT Post.ID, Title, Content, TimeCreate, TimeModified, Author, Post.Privacy, ('0') AS [SharePrivacy], ('0') AS [SharedEmail], ('0') AS [SharedName], ('0') AS [SharedAvatar], NULL AS [ShareTime]
FROM Post WHERE Author = 'lebuidihoa257@gmail.com' 
UNION
SELECT Post.ID, Title, Content, TimeCreate, TimeModified, Author, Post.Privacy, PostShare.Privacy AS [SharePrivacy], PostShare.UserEmail AS [SharedEmail], Users.name AS [SharedName], Users.AvatarURL AS [SharedAvatar], PostShare.Time AS [ShareTime]
FROM Post INNER JOIN PostShare 
ON Post.ID=PostShare.PostID INNER JOIN Users ON PostShare.UserEmail=Users.EmailAddress
WHERE PostShare.UserEmail = 'lebuidihoa257@gmail.com' 
UNION 
SELECT Post.ID, Title, Content, TimeCreate, TimeModified, Author, Post.Privacy, ('0') AS [SharePrivacy], ('0') AS [SharedEmail], ('0') AS [SharedName], ('0') AS [SharedAvatar], NULL AS [ShareTime]
FROM Post 
WHERE (Privacy = 'Friend' AND Author IN (SELECT FriendEmail FROM Friend WHERE UserEmail = 'lebuidihoa257@gmail.com') )
UNION
SELECT Post.ID, Title, Content, TimeCreate, TimeModified, Author, Post.Privacy, PostShare.Privacy AS [SharePrivacy], PostShare.UserEmail AS [SharedEmail], Users.name AS [SharedName], Users.AvatarURL AS [SharedAvatar], PostShare.Time AS [ShareTime]
FROM Post INNER JOIN PostShare 
ON Post.ID=PostShare.PostID INNER JOIN Users ON PostShare.UserEmail=Users.EmailAddress
WHERE (PostShare.Privacy='Friend' AND PostShare.UserEmail IN (SELECT FriendEmail FROM Friend WHERE UserEmail = 'lebuidihoa257@gmail.com')) 
UNION 
SELECT Post.ID, Title, Content, TimeCreate, TimeModified, Author, Post.Privacy, ('0') AS [SharePrivacy], ('0') AS [SharedEmail], ('0') AS [SharedName], ('0') AS [SharedAvatar], NULL AS [ShareTime]
FROM Post 
WHERE (Privacy = 'Public' AND Author Not IN (SELECT BlockedUser FROM BlockedEmail WHERE UserEmail = 'lebuidihoa257@gmail.com'))
UNION
SELECT Post.ID, Title, Content, TimeCreate, TimeModified, Author, Post.Privacy, PostShare.Privacy AS [SharePrivacy], PostShare.UserEmail AS [SharedEmail], Users.name AS [SharedName], Users.AvatarURL AS [SharedAvatar], PostShare.Time AS [ShareTime]
FROM Post INNER JOIN PostShare 
ON Post.ID=PostShare.PostID INNER JOIN Users ON PostShare.UserEmail=Users.EmailAddress
WHERE PostShare.Privacy='Public' AND PostShare.UserEmail Not IN (SELECT BlockedUser FROM BlockedEmail WHERE UserEmail = 'lebuidihoa257@gmail.com')) AS [Post]   
LEFT JOIN (SELECT PostLike.PostID ID, COUNT(UserEmail) AS [Like] FROM PostLike GROUP BY PostLike.PostID) LikeTable ON LikeTable.ID = Post.ID LEFT JOIN (SELECT PostComment.PostID, COUNT(UserEmail) AS [Comment] FROM PostComment GROUP BY PostComment.PostID) CommentTable ON Post.ID = CommentTable.PostID LEFT JOIN (SELECT PostID, COUNT(UserEmail) AS [Share] FROM PostShare GROUP BY PostShare.PostID) ShareTable ON Post.ID = ShareTable.PostID INNER JOIN Users ON Post.Author = Users.EmailAddress ORDER BY Post.TimeCreate DESC


SELECT Post.ID, Title, Content, TimeCreate, TimeModified, Author, Post.Privacy, Users.AvatarURL, Users.name, ISNULL(PostShare.Privacy, 0) AS [SharePrivacy],PostShare.UserEmail AS [SharedEmail], PostShare.name AS [SharedName], PostShare.AvatarURL AS [SharedAvatar], ISNULL(PostShare.Time,0) AS [ShareTime], ISNULL(LikeTable.[Like], 0) AS[Like] , ISNULL(CommentTable.[Comment], 0) AS[Comment], ISNULL(ShareTable.[Share], 0) AS[Share] 
FROM Post 
LEFT JOIN (SELECT * FROM PostShare INNER JOIN Users ON Users.EmailAddress=PostShare.UserEmail) AS [PostShare]
ON Post.ID = PostShare.PostID 
LEFT JOIN (SELECT PostLike.PostID ID, COUNT(UserEmail) AS [Like] FROM PostLike GROUP BY PostLike.PostID) LikeTable ON LikeTable.ID = Post.ID LEFT JOIN(SELECT PostComment.PostID, COUNT(UserEmail) AS [Comment] FROM PostComment GROUP BY PostComment.PostID) CommentTable ON Post.ID = CommentTable.PostID LEFT JOIN(SELECT PostID, COUNT(UserEmail) AS [Share] FROM PostShare GROUP BY PostShare.PostID) ShareTable ON Post.ID = ShareTable.PostID INNER JOIN Users ON Users.EmailAddress = Post.Author WHERE(Author = 'lebuidihoa257@gmail.com' AND Post.Privacy = 'Public') OR (PostShare.UserEmail = 'lebuidihoa257@gmail.com' AND PostShare.Privacy = 'Public') ORDER BY Post.TimeCreate DESC

SELECT Post.ID, Title, Content, TimeCreate, TimeModified, Author, Users.AvatarURL, Users.name, Privacy, Post.SharePrivacy, Post.SharedEmail, Post.SharedName, Post.SharedAvatar, ISNULL(Post.ShareTime,0) AS [ShareTime], ISNULL(LikeTable.[Like],0) AS [Like] , ISNULL(CommentTable.[Comment],0) AS [Comment], ISNULL(ShareTable.[Share],0) AS [Share]  
FROM 
(SELECT Post.ID, Title, Content, TimeCreate, TimeModified, Author, Post.Privacy, ('0') AS[SharePrivacy], ('0') AS [SharedEmail], ('0') AS [SharedName], ('0') AS [SharedAvatar], NULL AS [ShareTime]
FROM Post WHERE Post.Author = 'lebuidihoa257@gmail.com' 
UNION 
SELECT PostShare.PostID, Title, Content, TimeCreate, TimeModified, Author, Post.Privacy, PostShare.Privacy AS [SharePrivacy], PostShare.UserEmail AS [SharedEmail], Users.name AS [SharedName], Users.AvatarURL AS [SharedAvatar], ISNULL(PostShare.Time,0) AS [ShareTime]
FROM Post INNER JOIN PostShare ON Post.ID = PostShare.PostID 
INNER JOIN Users ON PostShare.UserEmail = Users.EmailAddress WHERE PostShare.UserEmail = 'lebuidihoa257@gmail.com') AS[Post] 
LEFT JOIN(SELECT PostLike.PostID ID, COUNT(UserEmail) AS [Like] FROM PostLike GROUP BY PostLike.PostID) LikeTable ON LikeTable.ID = Post.ID LEFT JOIN(SELECT PostComment.PostID, COUNT(UserEmail) AS [Comment] FROM PostComment GROUP BY PostComment.PostID) CommentTable ON Post.ID = CommentTable.PostID LEFT JOIN(SELECT PostID, COUNT(UserEmail) AS [Share] FROM PostShare GROUP BY PostShare.PostID) ShareTable ON Post.ID = ShareTable.PostID INNER JOIN Users ON Users.EmailAddress = Post.Author ORDER BY Post.TimeCreate DESC 

SELECT Post.ID, Title, Content, TimeCreate, TimeModified, Author, Privacy, Users.AvatarURL, Users.name, ISNULL(Post.SharePrivacy,0) AS [SharePrivacy], Post.SharedEmail, Post.SharedName, Post.SharedAvatar, ISNULL(Post.SharedTime,0) AS [ShareTime], ISNULL(LikeTable.[Like], 0) AS[Like] , ISNULL(CommentTable.[Comment], 0) AS[Comment], ISNULL(ShareTable.[Share], 0) AS[Share]  
FROM 
(SELECT Post.ID, Title, Content, TimeCreate, TimeModified, Author, Post.Privacy, ('0') AS [SharePrivacy], ('0') AS [SharedEmail], ('0') AS [SharedName], ('0') AS [SharedAvatar], NULL AS [SharedTime]
FROM Post WHERE(Post.Author = 'lebuidihoa257@gmail.com' AND Post.Privacy = 'Public') 
UNION 
SELECT Post.ID, Title, Content, TimeCreate, TimeModified, Author, Post.Privacy, PostShare.Privacy AS[SharePrivacy], PostShare.UserEmail AS [SharedEmail], Users.name AS [SharedName], Users.AvatarURL AS [SharedAvatar], PostShare.Time AS [ShareTime]
FROM Post INNER JOIN PostShare ON Post.ID = PostShare.PostID INNER JOIN Users ON Users.EmailAddress=PostShare.UserEmail
WHERE(PostShare.UserEmail = 'lebuidihoa257@gmail.com' AND PostShare.Privacy = 'Public') 
UNION 
SELECT Post.ID, Title, Content, TimeCreate, TimeModified, Author, Post.Privacy, ('0') AS[SharePrivacy], ('0') AS [SharedEmail], ('0') AS [SharedName], ('0') AS [SharedAvatar], NULL AS [SharedTime]
FROM Post LEFT JOIN PostShare ON Post.ID = PostShare.PostID 
WHERE(Post.Author = 'lebuidihoa257@gmail.com' AND Post.Privacy = 'Friend' AND Author IN(SELECT FriendEmail FROM Friend WHERE UserEmail = 'asdff@gmail.com')) 
UNION 
SELECT Post.ID, Title, Content, TimeCreate, TimeModified, Author, Post.Privacy, PostShare.Privacy AS[SharePrivacy], PostShare.UserEmail AS [SharedEmail], Users.name AS [SharedName], Users.AvatarURL AS [SharedAvatar], PostShare.Time AS [ShareTime]
FROM Post INNER JOIN PostShare ON Post.ID = PostShare.PostID INNER JOIN Users ON Users.EmailAddress=PostShare.UserEmail
WHERE(PostShare.UserEmail = 'lebuidihoa257@gmail.com' AND PostShare.Privacy = 'Friend' AND PostShare.UserEmail IN(SELECT FriendEmail FROM Friend WHERE UserEmail = 'asdff@gmail.com'))) AS[Post] 
LEFT JOIN(SELECT PostLike.PostID ID, COUNT(UserEmail) AS [Like] FROM PostLike GROUP BY PostLike.PostID) LikeTable ON LikeTable.ID = Post.ID LEFT JOIN(SELECT PostComment.PostID, COUNT(UserEmail) AS [Comment] FROM PostComment GROUP BY PostComment.PostID) CommentTable ON Post.ID = CommentTable.PostID LEFT JOIN(SELECT PostID, COUNT(UserEmail) AS [Share] FROM PostShare GROUP BY PostShare.PostID) ShareTable ON Post.ID = ShareTable.PostID INNER JOIN Users ON Post.Author = Users.EmailAddress ORDER BY Post.TimeCreate DESC 


SELECT name, FriendEmail, ISNULL(Mess.NumOfMess,0) AS [NumOfMess], ISNULL(LikeInfor.NumOfLike,0) AS [NumOfLike], ISNULL(Cmt.NumOfComment,0) AS [NumOfComment], ISNULL(Share.NumOfShare,0) AS [NumOfShare]
FROM Chats 
	INNER JOIN 
		(SELECT Chats.Id, COUNT(Message.ID) NumOfMess
		FROM Chats INNER JOIN Message ON Message.ChatId=Chats.Id
		GROUP BY Chats.Id) AS [Mess] 
	ON Mess.ID=Chats.Id
	INNER JOIN 
		(SELECT FriendEmail
		FROM Friend 
		WHERE UserEmail='lebuidihoa257@gmail.com') AS [Friend]
	ON (User1=Friend.FriendEmail AND User2='lebuidihoa257@gmail.com') OR (User2=Friend.FriendEmail AND User1='lebuidihoa257@gmail.com')
	LEFT JOIN 
		(SELECT UserEmail, COUNT(*) AS [NumOfLike]
		FROM PostLike INNER JOIN Post ON Post.ID=PostLike.PostID
		WHERE Author='lebuidihoa257@gmail.com'
		GROUP BY UserEmail) AS [LikeInfor] 
	ON LikeInfor.UserEmail=Friend.FriendEmail
	LEFT JOIN 
		(SELECT UserEmail, COUNT(*) AS [NumOfComment]
		FROM PostComment INNER JOIN Post ON Post.ID=PostComment.PostID
		WHERE Author='lebuidihoa257@gmail.com'
		GROUP BY UserEmail) AS [Cmt]
	ON Cmt.UserEmail=Friend.FriendEmail
	LEFT JOIN 
		(SELECT UserEmail, COUNT(*) AS [NumOfShare]
		FROM PostShare INNER JOIN Post ON Post.ID=PostShare.PostID
		WHERE Author='lebuidihoa257@gmail.com'
		GROUP BY UserEmail) AS [Share]
	ON Share.UserEmail=Friend.FriendEmail
	INNER JOIN Users ON Users.EmailAddress=Friend.FriendEmail
ORDER BY NumOfMess, NumOfShare, NumOfComment, NumOfLike DESC

SELECT EmailAddress, name, sex, DateOfBirth, PhoneNumber, AvatarURL, ISNULL(Post.NumOfPost,0) AS [NumOfPost]
FROM Users INNER JOIN (SELECT Author, Count(ID) AS [NumOfPost] FROM Post GROUP BY Author) AS [Post] ON Users.EmailAddress=Post.Author

SELECT Post.ID,Title, Content,Author, ISNULL(LikeTable.[Like], 0) AS[Like] , ISNULL(CommentTable.[Comment], 0) AS[Comment], ISNULL(ShareTable.[Share], 0) AS[Share]  
FROM Post 
LEFT JOIN(SELECT PostLike.PostID ID, COUNT(UserEmail) AS [Like] FROM PostLike GROUP BY PostLike.PostID) LikeTable ON LikeTable.ID = Post.ID LEFT JOIN(SELECT PostComment.PostID, COUNT(UserEmail) AS [Comment] FROM PostComment GROUP BY PostComment.PostID) CommentTable ON Post.ID = CommentTable.PostID LEFT JOIN(SELECT PostID, COUNT(UserEmail) AS [Share] FROM PostShare GROUP BY PostShare.PostID) ShareTable ON Post.ID = ShareTable.PostID INNER JOIN Users ON Post.Author = Users.EmailAddress

SELECT Post.ID, Title, Content, TimeCreate, TimeModified, Author, Privacy, Users.AvatarURL, Users.name,Post.SharePrivacy,Post.SharedEmail, Post.SharedName, Post.SharedAvatar, ISNULL(ShareTime,0) AS [ShareTime], ISNULL(LikeTable.[Like], 0) AS[Like] , ISNULL(CommentTable.[Comment], 0) AS[Comment], ISNULL(ShareTable.[Share], 0) AS[Share]  
FROM (SELECT Post.ID, Title, Content, TimeCreate, TimeModified, Author, Post.Privacy, ('0') AS[SharePrivacy], ('0') AS[SharedEmail], ('0') AS[SharedName], ('0') AS[SharedAvatar], NULL AS[ShareTime] FROM Post UNION SELECT Post.ID, Title, Content, TimeCreate, TimeModified, Author, Post.Privacy, PostShare.Privacy AS[SharePrivacy], PostShare.UserEmail AS[SharedEmail], Users.name AS[SharedName], Users.AvatarURL AS[SharedAvatar], PostShare.Time AS[ShareTime] FROM Post INNER JOIN PostShare ON Post.ID = PostShare.PostID INNER JOIN Users ON PostShare.UserEmail = Users.EmailAddress) AS[Post] LEFT JOIN(SELECT PostLike.PostID ID, COUNT(UserEmail) AS [Like] FROM PostLike GROUP BY PostLike.PostID) LikeTable ON LikeTable.ID = Post.ID LEFT JOIN(SELECT PostComment.PostID, COUNT(UserEmail) AS [Comment] FROM PostComment GROUP BY PostComment.PostID) CommentTable ON Post.ID = CommentTable.PostID LEFT JOIN(SELECT PostID, COUNT(UserEmail) AS [Share] FROM PostShare GROUP BY PostShare.PostID) ShareTable ON Post.ID = ShareTable.PostID INNER JOIN Users ON Post.Author = Users.EmailAddress WHERE((Post.Privacy = 'Public' AND Post.SharePrivacy = '0')OR (Post.SharePrivacy = 'Public')) ORDER BY Post.TimeCreate DESC
