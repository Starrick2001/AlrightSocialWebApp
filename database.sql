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
