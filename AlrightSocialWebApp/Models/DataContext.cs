using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Microsoft.Data.SqlClient;
using AlrightSocialWebApp.Models;
using Microsoft.AspNetCore.Http;

namespace AlrightSocialWebApp.Models
{
    public class DataContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var builder = new ConfigurationBuilder()
                            .SetBasePath(Directory.GetCurrentDirectory())
                            .AddJsonFile("appsettings.json");
            var configuration = builder.Build();
            optionsBuilder.UseSqlServer(configuration["ConnectionStrings:DefaultConnection"]);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PostLike>()
                .HasKey(o => new { o.UserEmail, o.PostID });
            modelBuilder.Entity<Friend>()
               .HasKey(o => new { o.UserEmail, o.FriendEmail });
            modelBuilder.Entity<FriendRequest>()
               .HasKey(o => new { o.UserEmail, o.FriendEmail });
            modelBuilder.Entity<BlockedEmail>()
              .HasKey(o => new { o.UserEmail, o.BlockedUser });
            modelBuilder.Entity<PostReport>()
                .HasKey(o => new { o.EmailAddress, o.PostID });
            modelBuilder.Entity<ReportUser>()
                .HasKey(o => new { o.UserEmail, o.ReportedUser });
        }
        public DbSet<User> Users { get; set; }
        public DbSet<SuspendedUser> SuspendedUser { get; set; }
        public User GetUserInfo(string EmailAddress)
        {
            User user = new User();
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = @"Data Source = localhost; Database = AlrightSocial; Integrated Security = SSPI";
            string query = "SELECT * FROM USERS WHERE EmailAddress=@EmailAddress";
            var command = new SqlCommand(query, conn);
            command.Parameters.AddWithValue("EmailAddress", EmailAddress);
            conn.Open();
            var reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    user.AvatarURL = reader["AvatarURL"].ToString();
                    user.DateOfBirth = (DateTime)reader["DateOfBirth"];
                    user.EmailAddress = reader["EmailAddress"].ToString();
                    user.name = reader["name"].ToString();
                    user.Password = reader["Password"].ToString();
                    user.PhoneNumber = reader["PhoneNumber"].ToString();
                    user.sex = reader["sex"].ToString();
                    user.SignInStatus = reader["SignInStatus"].ToString();

                }
                reader.Close();
            }
            conn.Close();
            return user;
        }

        public DbSet<AlrightSocialWebApp.Models.Post> Post { get; set; }
        public int CreatePost(Post p)
        {
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = @"Data Source = localhost; Database = AlrightSocial; Integrated Security = SSPI";
            string query = "INSERT INTO Post (Title, Content, TimeCreate, TimeModified, Author, Privacy) VALUES (@Title, @Content, @TimeCreate,@TimeModified, @Author, @Privacy)";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@Title", p.Title);
            cmd.Parameters.AddWithValue("@Content", p.Content);
            cmd.Parameters.AddWithValue("@TimeCreate", p.TimeCreate);
            cmd.Parameters.AddWithValue("@TimeModified", p.TimeModified);
            cmd.Parameters.AddWithValue("@Author", p.Author);
            cmd.Parameters.AddWithValue("@Privacy", p.Privacy);
            conn.Open();
            return cmd.ExecuteNonQuery();
            conn.Close();
        }

        public void DeletePost(int PostID)
        {
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = @"Data Source = localhost; Database = AlrightSocial; Integrated Security = SSPI";
            string query = "DELETE FROM PostLike WHERE PostID=@PostID";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@PostID", PostID);
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
            query = "DELETE FROM PostComment WHERE PostID=@PostID";
            cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@PostID", PostID);
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
            query = "DELETE FROM PostShare WHERE PostID=@PostID";
            cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@PostID", PostID);
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
            query = "DELETE FROM Notification WHERE PostID=@PostID";
            cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@PostID", PostID);
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
            query = "DELETE FROM PostReport WHERE PostID=@PostID";
            cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@PostID", PostID);
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
            query = "DELETE FROM Post WHERE ID=@PostID";
            cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@PostID", PostID);
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        public List<object> GetListOfPost(string EmailAddress, string CurrentUser)
        {
            List<object> list = new List<object>();
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = @"Data Source = localhost; Database = AlrightSocial; Integrated Security = SSPI";
            string query = "";
            if (CurrentUser == null)
            {
                query = "SELECT Post.ID, Title, Content, TimeCreate, TimeModified, Author, Post.Privacy, Users.AvatarURL, Users.name, ISNULL(PostShare.Privacy, 0) AS [SharePrivacy],PostShare.UserEmail AS [SharedEmail], PostShare.name AS [SharedName], PostShare.AvatarURL AS [SharedAvatar], ISNULL(PostShare.Time,0) AS [ShareTime], ISNULL(LikeTable.[Like], 0) AS[Like] , ISNULL(CommentTable.[Comment], 0) AS[Comment], ISNULL(ShareTable.[Share], 0) AS[Share]  FROM Post LEFT JOIN(SELECT* FROM PostShare INNER JOIN Users ON Users.EmailAddress= PostShare.UserEmail) AS[PostShare] ON Post.ID = PostShare.PostID LEFT JOIN(SELECT PostLike.PostID ID, COUNT(UserEmail) AS [Like] FROM PostLike GROUP BY PostLike.PostID) LikeTable ON LikeTable.ID = Post.ID LEFT JOIN(SELECT PostComment.PostID, COUNT(UserEmail) AS [Comment] FROM PostComment GROUP BY PostComment.PostID) CommentTable ON Post.ID = CommentTable.PostID LEFT JOIN(SELECT PostID, COUNT(UserEmail) AS [Share] FROM PostShare GROUP BY PostShare.PostID) ShareTable ON Post.ID = ShareTable.PostID INNER JOIN Users ON Users.EmailAddress = Post.Author WHERE(Author = @EmailAddress AND Post.Privacy = 'Public') OR(PostShare.UserEmail = @EmailAddress AND PostShare.Privacy = 'Public') ORDER BY Post.TimeCreate DESC";
            }
            else
            {
                if (EmailAddress == CurrentUser)
                {
                    query = "SELECT Post.ID, Title, Content, TimeCreate, TimeModified, Author, Users.AvatarURL, Users.name, Privacy, Post.SharePrivacy, Post.SharedEmail, Post.SharedName, Post.SharedAvatar, ISNULL(Post.ShareTime,0) AS [ShareTime], ISNULL(LikeTable.[Like],0) AS [Like] , ISNULL(CommentTable.[Comment],0) AS [Comment], ISNULL(ShareTable.[Share],0) AS [Share]   FROM (SELECT Post.ID, Title, Content, TimeCreate, TimeModified, Author, Post.Privacy, ('0') AS[SharePrivacy], ('0') AS[SharedEmail], ('0') AS[SharedName], ('0') AS[SharedAvatar], NULL AS[ShareTime] FROM Post WHERE Post.Author = @EmailAddress UNION SELECT PostShare.PostID, Title, Content, TimeCreate, TimeModified, Author, Post.Privacy, PostShare.Privacy AS[SharePrivacy], PostShare.UserEmail AS[SharedEmail], Users.name AS[SharedName], Users.AvatarURL AS[SharedAvatar], ISNULL(PostShare.Time, 0) AS[ShareTime] FROM Post INNER JOIN PostShare ON Post.ID = PostShare.PostID INNER JOIN Users ON PostShare.UserEmail = Users.EmailAddress WHERE PostShare.UserEmail = @EmailAddress) AS[Post] LEFT JOIN(SELECT PostLike.PostID ID, COUNT(UserEmail) AS [Like] FROM PostLike GROUP BY PostLike.PostID) LikeTable ON LikeTable.ID = Post.ID LEFT JOIN(SELECT PostComment.PostID, COUNT(UserEmail) AS [Comment] FROM PostComment GROUP BY PostComment.PostID) CommentTable ON Post.ID = CommentTable.PostID LEFT JOIN(SELECT PostID, COUNT(UserEmail) AS [Share] FROM PostShare GROUP BY PostShare.PostID) ShareTable ON Post.ID = ShareTable.PostID INNER JOIN Users ON Users.EmailAddress = Post.Author ORDER BY Post.TimeCreate DESC ";
                }
                else
                {
                    query = "SELECT Post.ID, Title, Content, TimeCreate, TimeModified, Author, Privacy, Users.AvatarURL, Users.name, ISNULL(Post.SharePrivacy,0) AS [SharePrivacy], Post.SharedEmail, Post.SharedName, Post.SharedAvatar, ISNULL(Post.SharedTime,0) AS [ShareTime], ISNULL(LikeTable.[Like], 0) AS[Like] , ISNULL(CommentTable.[Comment], 0) AS[Comment], ISNULL(ShareTable.[Share], 0) AS[Share]   FROM (SELECT Post.ID, Title, Content, TimeCreate, TimeModified, Author, Post.Privacy, ('0') AS[SharePrivacy], ('0') AS[SharedEmail], ('0') AS[SharedName], ('0') AS[SharedAvatar], NULL AS[SharedTime] FROM Post WHERE(Post.Author = @EmailAddress AND Post.Privacy = 'Public') UNION SELECT Post.ID, Title, Content, TimeCreate, TimeModified, Author, Post.Privacy, PostShare.Privacy AS[SharePrivacy], PostShare.UserEmail AS[SharedEmail], Users.name AS[SharedName], Users.AvatarURL AS[SharedAvatar], PostShare.Time AS[ShareTime] FROM Post INNER JOIN PostShare ON Post.ID = PostShare.PostID INNER JOIN Users ON Users.EmailAddress = PostShare.UserEmail WHERE(PostShare.UserEmail = @EmailAddress AND PostShare.Privacy = 'Public') UNION SELECT Post.ID, Title, Content, TimeCreate, TimeModified, Author, Post.Privacy, ('0') AS[SharePrivacy], ('0') AS[SharedEmail], ('0') AS[SharedName], ('0') AS[SharedAvatar], NULL AS[SharedTime] FROM Post LEFT JOIN PostShare ON Post.ID = PostShare.PostID WHERE(Post.Author = @EmailAddress AND Post.Privacy = 'Friend' AND Author IN(SELECT FriendEmail FROM Friend WHERE UserEmail = @CurrentUser)) UNION SELECT Post.ID, Title, Content, TimeCreate, TimeModified, Author, Post.Privacy, PostShare.Privacy AS[SharePrivacy], PostShare.UserEmail AS[SharedEmail], Users.name AS[SharedName], Users.AvatarURL AS[SharedAvatar], PostShare.Time AS[ShareTime] FROM Post INNER JOIN PostShare ON Post.ID = PostShare.PostID INNER JOIN Users ON Users.EmailAddress = PostShare.UserEmail WHERE(PostShare.UserEmail = @EmailAddress AND PostShare.Privacy = 'Friend' AND PostShare.UserEmail IN(SELECT FriendEmail FROM Friend WHERE UserEmail = @CurrentUser))) AS[Post] LEFT JOIN(SELECT PostLike.PostID ID, COUNT(UserEmail) AS [Like] FROM PostLike GROUP BY PostLike.PostID) LikeTable ON LikeTable.ID = Post.ID LEFT JOIN(SELECT PostComment.PostID, COUNT(UserEmail) AS [Comment] FROM PostComment GROUP BY PostComment.PostID) CommentTable ON Post.ID = CommentTable.PostID LEFT JOIN(SELECT PostID, COUNT(UserEmail) AS [Share] FROM PostShare GROUP BY PostShare.PostID) ShareTable ON Post.ID = ShareTable.PostID INNER JOIN Users ON Post.Author = Users.EmailAddress ORDER BY Post.TimeCreate DESC";
                }
            }
            var command = new SqlCommand(query, conn);
            command.Parameters.AddWithValue("EmailAddress", EmailAddress);
            if (EmailAddress != CurrentUser && CurrentUser != null)
                command.Parameters.AddWithValue("CurrentUser", CurrentUser);
            conn.Open();
            var reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    list.Add(new
                    {
                        ID = (int)reader["ID"],
                        Title = reader["Title"].ToString(),
                        Content = reader["Content"].ToString(),
                        TimeCreate = (DateTime)reader["TimeCreate"],
                        TimeModified = (DateTime)reader["TimeModified"],
                        Author = reader["Author"].ToString(),
                        AvatarURL = reader["AvatarURL"].ToString(),
                        name = reader["name"].ToString(),
                        Privacy = reader["Privacy"].ToString(),
                        SharePrivacy = reader["SharePrivacy"].ToString(),
                        SharedEmail = reader["SharedEmail"].ToString(),
                        SharedName = reader["SharedName"].ToString(),
                        SharedAvatar = reader["SharedAvatar"].ToString(),
                        ShareTime = (DateTime)reader["ShareTime"],
                        Like = (int)reader["Like"],
                        Comment = (int)reader["Comment"],
                        Share = (int)reader["Share"],
                        isLiked = isLiked((int)reader["ID"], EmailAddress)
                    });
                }
                reader.Close();
            }
            conn.Close();
            return list;
        }
        public List<object> GetListOfPostHomePage(string EmailAddress)
        {
            List<object> list = new List<object>();
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = @"Data Source = localhost; Database = AlrightSocial; Integrated Security = SSPI";
            string query = "SELECT Post.ID, Title, Content, TimeCreate, TimeModified, Author, Privacy, Users.AvatarURL, Users.name,Post.SharePrivacy,Post.SharedEmail, Post.SharedName, Post.SharedAvatar, ISNULL(ShareTime,0) AS [ShareTime], ISNULL(LikeTable.[Like], 0) AS[Like] , ISNULL(CommentTable.[Comment], 0) AS[Comment], ISNULL(ShareTable.[Share], 0) AS[Share]    FROM (SELECT Post.ID, Title, Content, TimeCreate, TimeModified, Author, Post.Privacy, ('0') AS[SharePrivacy], ('0') AS[SharedEmail], ('0') AS[SharedName], ('0') AS[SharedAvatar], NULL AS[ShareTime] FROM Post WHERE Author = @EmailAddress UNION SELECT Post.ID, Title, Content, TimeCreate, TimeModified, Author, Post.Privacy, PostShare.Privacy AS[SharePrivacy], PostShare.UserEmail AS[SharedEmail], Users.name AS[SharedName], Users.AvatarURL AS[SharedAvatar], PostShare.Time AS[ShareTime] FROM Post INNER JOIN PostShare ON Post.ID = PostShare.PostID INNER JOIN Users ON PostShare.UserEmail = Users.EmailAddress WHERE PostShare.UserEmail = @EmailAddress UNION SELECT Post.ID, Title, Content, TimeCreate, TimeModified, Author, Post.Privacy, ('0') AS[SharePrivacy], ('0') AS[SharedEmail], ('0') AS[SharedName], ('0') AS[SharedAvatar], NULL AS[ShareTime] FROM Post WHERE(Privacy = 'Friend' AND Author IN(SELECT FriendEmail FROM Friend WHERE UserEmail = @EmailAddress)) UNION SELECT Post.ID, Title, Content, TimeCreate, TimeModified, Author, Post.Privacy, PostShare.Privacy AS[SharePrivacy], PostShare.UserEmail AS[SharedEmail], Users.name AS[SharedName], Users.AvatarURL AS[SharedAvatar], PostShare.Time AS[ShareTime] FROM Post INNER JOIN PostShare ON Post.ID = PostShare.PostID INNER JOIN Users ON PostShare.UserEmail = Users.EmailAddress WHERE(PostShare.Privacy = 'Friend' AND PostShare.UserEmail IN(SELECT FriendEmail FROM Friend WHERE UserEmail = @EmailAddress)) UNION SELECT Post.ID, Title, Content, TimeCreate, TimeModified, Author, Post.Privacy, ('0') AS[SharePrivacy], ('0') AS[SharedEmail], ('0') AS[SharedName], ('0') AS[SharedAvatar], NULL AS[ShareTime] FROM Post WHERE(Privacy = 'Public' AND Author Not IN(SELECT BlockedUser FROM BlockedEmail WHERE UserEmail = @EmailAddress)) UNION SELECT Post.ID, Title, Content, TimeCreate, TimeModified, Author, Post.Privacy, PostShare.Privacy AS[SharePrivacy], PostShare.UserEmail AS[SharedEmail], Users.name AS[SharedName], Users.AvatarURL AS[SharedAvatar], PostShare.Time AS[ShareTime] FROM Post INNER JOIN PostShare ON Post.ID = PostShare.PostID INNER JOIN Users ON PostShare.UserEmail = Users.EmailAddress WHERE PostShare.Privacy = 'Public' AND PostShare.UserEmail Not IN(SELECT BlockedUser FROM BlockedEmail WHERE UserEmail = @EmailAddress)) AS[Post] LEFT JOIN(SELECT PostLike.PostID ID, COUNT(UserEmail) AS [Like] FROM PostLike GROUP BY PostLike.PostID) LikeTable ON LikeTable.ID = Post.ID LEFT JOIN(SELECT PostComment.PostID, COUNT(UserEmail) AS [Comment] FROM PostComment GROUP BY PostComment.PostID) CommentTable ON Post.ID = CommentTable.PostID LEFT JOIN(SELECT PostID, COUNT(UserEmail) AS [Share] FROM PostShare GROUP BY PostShare.PostID) ShareTable ON Post.ID = ShareTable.PostID INNER JOIN Users ON Post.Author = Users.EmailAddress ORDER BY Post.TimeCreate DESC";
            var command = new SqlCommand(query, conn);
            command.Parameters.AddWithValue("EmailAddress", EmailAddress);
            conn.Open();
            var reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    list.Add(new
                    {
                        ID = (int)reader["ID"],
                        Title = reader["Title"].ToString(),
                        Content = reader["Content"].ToString(),
                        TimeCreate = (DateTime)reader["TimeCreate"],
                        TimeModified = (DateTime)reader["TimeModified"],
                        Author = reader["Author"].ToString(),
                        AvatarURL = reader["AvatarURL"].ToString(),
                        AuthorName = reader["name"].ToString(),
                        Privacy = reader["Privacy"].ToString(),
                        SharePrivacy = reader["SharePrivacy"].ToString(),
                        SharedEmail = reader["SharedEmail"].ToString(),
                        SharedName = reader["SharedName"].ToString(),
                        SharedAvatar = reader["SharedAvatar"].ToString(),
                        ShareTime = (DateTime)reader["ShareTime"],
                        Like = (int)reader["Like"],
                        Comment = (int)reader["Comment"],
                        Share = (int)reader["Share"],
                        isLiked = isLiked((int)reader["ID"], EmailAddress)
                    });
                }
                reader.Close();
            }
            conn.Close();
            return list;
        }
        public List<object> GetListOfPublicPost()
        {
            List<object> list = new List<object>();
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = @"Data Source = localhost; Database = AlrightSocial; Integrated Security = SSPI";
            string query = "SELECT Post.ID, Title, Content, TimeCreate, TimeModified, Author, Privacy, Users.AvatarURL, Users.name,Post.SharePrivacy,Post.SharedEmail, Post.SharedName, Post.SharedAvatar, ISNULL(ShareTime,0) AS [ShareTime], ISNULL(LikeTable.[Like], 0) AS[Like] , ISNULL(CommentTable.[Comment], 0) AS[Comment], ISNULL(ShareTable.[Share], 0) AS[Share]  FROM(SELECT Post.ID, Title, Content, TimeCreate, TimeModified, Author, Post.Privacy, ('0') AS[SharePrivacy], ('0') AS[SharedEmail], ('0') AS[SharedName], ('0') AS[SharedAvatar], NULL AS[ShareTime] FROM Post UNION SELECT Post.ID, Title, Content, TimeCreate, TimeModified, Author, Post.Privacy, PostShare.Privacy AS[SharePrivacy], PostShare.UserEmail AS[SharedEmail], Users.name AS[SharedName], Users.AvatarURL AS[SharedAvatar], PostShare.Time AS[ShareTime] FROM Post INNER JOIN PostShare ON Post.ID = PostShare.PostID INNER JOIN Users ON PostShare.UserEmail = Users.EmailAddress) AS[Post] LEFT JOIN(SELECT PostLike.PostID ID, COUNT(UserEmail) AS [Like] FROM PostLike GROUP BY PostLike.PostID) LikeTable ON LikeTable.ID = Post.ID LEFT JOIN(SELECT PostComment.PostID, COUNT(UserEmail) AS [Comment] FROM PostComment GROUP BY PostComment.PostID) CommentTable ON Post.ID = CommentTable.PostID LEFT JOIN(SELECT PostID, COUNT(UserEmail) AS [Share] FROM PostShare GROUP BY PostShare.PostID) ShareTable ON Post.ID = ShareTable.PostID INNER JOIN Users ON Post.Author = Users.EmailAddress WHERE((Post.Privacy = 'Public' AND Post.SharePrivacy = '0')OR(Post.SharePrivacy = 'Public')) ORDER BY Post.TimeCreate DESC";
            var command = new SqlCommand(query, conn);
            conn.Open();
            var reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    list.Add(new
                    {
                        ID = (int)reader["ID"],
                        Title = reader["Title"].ToString(),
                        Content = reader["Content"].ToString(),
                        TimeCreate = (DateTime)reader["TimeCreate"],
                        TimeModified = (DateTime)reader["TimeModified"],
                        Author = reader["Author"].ToString(),
                        AvatarURL = reader["AvatarURL"].ToString(),
                        AuthorName = reader["name"].ToString(),
                        Privacy = reader["Privacy"].ToString(),
                        SharePrivacy = reader["SharePrivacy"].ToString(),
                        SharedEmail = reader["SharedEmail"].ToString(),
                        SharedName = reader["SharedName"].ToString(),
                        SharedAvatar = reader["SharedAvatar"].ToString(),
                        ShareTime = (DateTime)reader["ShareTime"],
                        Like = (int)reader["Like"],
                        Comment = (int)reader["Comment"],
                        Share = (int)reader["Share"]
                    });
                }
                reader.Close();
            }
            conn.Close();
            return list;
        }

        public List<object> SearchPublicPost(string searchString)
        {
            List<object> list = new List<object>();
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = @"Data Source = localhost; Database = AlrightSocial; Integrated Security = SSPI";
            string query = "SELECT Post.ID, Title, Content, TimeCreate, TimeModified, Author, Privacy, Users.AvatarURL, Users.name,Post.SharePrivacy,Post.SharedEmail, Post.SharedName, Post.SharedAvatar, ISNULL(ShareTime,0) AS [ShareTime], ISNULL(LikeTable.[Like], 0) AS[Like] , ISNULL(CommentTable.[Comment], 0) AS[Comment], ISNULL(ShareTable.[Share], 0) AS[Share]  FROM (SELECT Post.ID, Title, Content, TimeCreate, TimeModified, Author, Post.Privacy, ('0') AS[SharePrivacy], ('0') AS[SharedEmail], ('0') AS[SharedName], ('0') AS[SharedAvatar], NULL AS[ShareTime] FROM Post UNION SELECT Post.ID, Title, Content, TimeCreate, TimeModified, Author, Post.Privacy, PostShare.Privacy AS[SharePrivacy], PostShare.UserEmail AS[SharedEmail], Users.name AS[SharedName], Users.AvatarURL AS[SharedAvatar], PostShare.Time AS[ShareTime] FROM Post INNER JOIN PostShare ON Post.ID = PostShare.PostID INNER JOIN Users ON PostShare.UserEmail = Users.EmailAddress) AS[Post] LEFT JOIN(SELECT PostLike.PostID ID, COUNT(UserEmail) AS [Like] FROM PostLike GROUP BY PostLike.PostID) LikeTable ON LikeTable.ID = Post.ID LEFT JOIN(SELECT PostComment.PostID, COUNT(UserEmail) AS [Comment] FROM PostComment GROUP BY PostComment.PostID) CommentTable ON Post.ID = CommentTable.PostID LEFT JOIN(SELECT PostID, COUNT(UserEmail) AS [Share] FROM PostShare GROUP BY PostShare.PostID) ShareTable ON Post.ID = ShareTable.PostID INNER JOIN Users ON Post.Author = Users.EmailAddress WHERE (Post.Privacy = 'Public' OR Post.SharePrivacy = 'Public') AND Title LIKE @searchString ORDER BY Post.TimeCreate DESC";
            var command = new SqlCommand(query, conn);
            command.Parameters.AddWithValue("searchString", searchString);
            conn.Open();
            var reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    list.Add(new
                    {
                        ID = (int)reader["ID"],
                        Title = reader["Title"].ToString(),
                        Content = reader["Content"].ToString(),
                        TimeCreate = (DateTime)reader["TimeCreate"],
                        TimeModified = (DateTime)reader["TimeModified"],
                        Author = reader["Author"].ToString(),
                        AvatarURL = reader["AvatarURL"].ToString(),
                        AuthorName = reader["name"].ToString(),
                        Privacy = reader["Privacy"].ToString(),
                        SharePrivacy = reader["SharePrivacy"].ToString(),
                        SharedEmail = reader["SharedEmail"].ToString(),
                        SharedName = reader["SharedName"].ToString(),
                        SharedAvatar = reader["SharedAvatar"].ToString(),
                        ShareTime = (DateTime)reader["ShareTime"],
                        Like = (int)reader["Like"],
                        Comment = (int)reader["Comment"],
                        Share = (int)reader["Share"]
                    });
                }
                reader.Close();
            }
            conn.Close();
            return list;
        }
        public List<object> SearchPost(string EmailAddress, string searchString)
        {
            List<object> list = new List<object>();
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = @"Data Source = localhost; Database = AlrightSocial; Integrated Security = SSPI";
            string query = "SELECT Post.ID, Title, Content, TimeCreate, TimeModified, Author, Privacy, Users.AvatarURL, Users.name,Post.SharePrivacy,Post.SharedEmail, Post.SharedName, Post.SharedAvatar, ISNULL(ShareTime,0) AS [ShareTime], ISNULL(LikeTable.[Like], 0) AS[Like] , ISNULL(CommentTable.[Comment], 0) AS[Comment], ISNULL(ShareTable.[Share], 0) AS[Share]    FROM (SELECT Post.ID, Title, Content, TimeCreate, TimeModified, Author, Post.Privacy, ('0') AS[SharePrivacy], ('0') AS[SharedEmail], ('0') AS[SharedName], ('0') AS[SharedAvatar], NULL AS[ShareTime] FROM Post WHERE Author = @EmailAddress UNION SELECT Post.ID, Title, Content, TimeCreate, TimeModified, Author, Post.Privacy, PostShare.Privacy AS[SharePrivacy], PostShare.UserEmail AS[SharedEmail], Users.name AS[SharedName], Users.AvatarURL AS[SharedAvatar], PostShare.Time AS[ShareTime] FROM Post INNER JOIN PostShare ON Post.ID = PostShare.PostID INNER JOIN Users ON PostShare.UserEmail = Users.EmailAddress WHERE PostShare.UserEmail = @EmailAddress UNION SELECT Post.ID, Title, Content, TimeCreate, TimeModified, Author, Post.Privacy, ('0') AS[SharePrivacy], ('0') AS[SharedEmail], ('0') AS[SharedName], ('0') AS[SharedAvatar], NULL AS[ShareTime] FROM Post WHERE(Privacy = 'Friend' AND Author IN(SELECT FriendEmail FROM Friend WHERE UserEmail = @EmailAddress)) UNION SELECT Post.ID, Title, Content, TimeCreate, TimeModified, Author, Post.Privacy, PostShare.Privacy AS[SharePrivacy], PostShare.UserEmail AS[SharedEmail], Users.name AS[SharedName], Users.AvatarURL AS[SharedAvatar], PostShare.Time AS[ShareTime] FROM Post INNER JOIN PostShare ON Post.ID = PostShare.PostID INNER JOIN Users ON PostShare.UserEmail = Users.EmailAddress WHERE(PostShare.Privacy = 'Friend' AND PostShare.UserEmail IN(SELECT FriendEmail FROM Friend WHERE UserEmail = @EmailAddress)) UNION SELECT Post.ID, Title, Content, TimeCreate, TimeModified, Author, Post.Privacy, ('0') AS[SharePrivacy], ('0') AS[SharedEmail], ('0') AS[SharedName], ('0') AS[SharedAvatar], NULL AS[ShareTime] FROM Post WHERE(Privacy = 'Public' AND Author Not IN(SELECT BlockedUser FROM BlockedEmail WHERE UserEmail = @EmailAddress)) UNION SELECT Post.ID, Title, Content, TimeCreate, TimeModified, Author, Post.Privacy, PostShare.Privacy AS[SharePrivacy], PostShare.UserEmail AS[SharedEmail], Users.name AS[SharedName], Users.AvatarURL AS[SharedAvatar], PostShare.Time AS[ShareTime] FROM Post INNER JOIN PostShare ON Post.ID = PostShare.PostID INNER JOIN Users ON PostShare.UserEmail = Users.EmailAddress WHERE PostShare.Privacy = 'Public' AND PostShare.UserEmail Not IN(SELECT BlockedUser FROM BlockedEmail WHERE UserEmail = @EmailAddress)) AS[Post] LEFT JOIN(SELECT PostLike.PostID ID, COUNT(UserEmail) AS [Like] FROM PostLike GROUP BY PostLike.PostID) LikeTable ON LikeTable.ID = Post.ID LEFT JOIN(SELECT PostComment.PostID, COUNT(UserEmail) AS [Comment] FROM PostComment GROUP BY PostComment.PostID) CommentTable ON Post.ID = CommentTable.PostID LEFT JOIN(SELECT PostID, COUNT(UserEmail) AS [Share] FROM PostShare GROUP BY PostShare.PostID) ShareTable ON Post.ID = ShareTable.PostID INNER JOIN Users ON Post.Author = Users.EmailAddress WHERE Title LIKE @searchString ORDER BY Post.TimeCreate DESC";
            var command = new SqlCommand(query, conn);
            command.Parameters.AddWithValue("searchString", searchString);
            command.Parameters.AddWithValue("EmailAddress", EmailAddress);
            conn.Open();
            var reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    list.Add(new
                    {
                        ID = (int)reader["ID"],
                        Title = reader["Title"].ToString(),
                        Content = reader["Content"].ToString(),
                        TimeCreate = (DateTime)reader["TimeCreate"],
                        TimeModified = (DateTime)reader["TimeModified"],
                        Author = reader["Author"].ToString(),
                        AvatarURL = reader["AvatarURL"].ToString(),
                        AuthorName = reader["name"].ToString(),
                        Privacy = reader["Privacy"].ToString(),
                        SharePrivacy = reader["SharePrivacy"].ToString(),
                        SharedEmail = reader["SharedEmail"].ToString(),
                        SharedName = reader["SharedName"].ToString(),
                        SharedAvatar = reader["SharedAvatar"].ToString(),
                        ShareTime = (DateTime)reader["ShareTime"],
                        Like = (int)reader["Like"],
                        Comment = (int)reader["Comment"],
                        Share = (int)reader["Share"],
                        isLiked = isLiked((int)reader["ID"], EmailAddress)
                    });
                }
                reader.Close();
            }
            conn.Close();
            return list;
        }
        public object GetPostInformation(int PostID)
        {
            object post = new Post();
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = @"Data Source = localhost; Database = AlrightSocial; Integrated Security = SSPI";
            string query = "SELECT Post.ID, Title, Content, TimeCreate, TimeModified, Author, Privacy, ISNULL(LikeTable.[Like],0) AS [Like] , ISNULL(CommentTable.[Comment],0) AS [Comment], ISNULL(ShareTable.[Share],0) AS [Share] FROM Post LEFT JOIN (SELECT PostLike.PostID ID, COUNT(UserEmail) AS [Like] FROM PostLike GROUP BY PostLike.PostID) LikeTable ON LikeTable.ID = Post.ID LEFT JOIN (SELECT PostComment.PostID, COUNT(UserEmail) AS [Comment] FROM PostComment GROUP BY PostComment.PostID) CommentTable ON Post.ID = CommentTable.PostID LEFT JOIN (SELECT PostID, COUNT(UserEmail) AS [Share] FROM PostShare GROUP BY PostShare.PostID) ShareTable ON Post.ID = ShareTable.PostID WHERE Post.ID = @PostID";
            var command = new SqlCommand(query, conn);
            command.Parameters.AddWithValue("PostID", PostID);
            conn.Open();
            var reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    post = new
                    {
                        ID = (int)reader["ID"],
                        Title = reader["Title"].ToString(),
                        Content = reader["Content"].ToString(),
                        TimeCreate = (DateTime)reader["TimeCreate"],
                        TimeModified = (DateTime)reader["TimeModified"],
                        Author = reader["Author"].ToString(),
                        Privacy = reader["Privacy"].ToString(),
                        Like = (int)reader["Like"],
                        Comment = (int)reader["Comment"],
                        Share = (int)reader["Share"]
                    };
                }
                reader.Close();
            }
            conn.Close();
            return post;
        }
        public bool isLiked(int PostID, string UserEmail)
        {
            int temp = 0;
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = @"Data Source = localhost; Database = AlrightSocial; Integrated Security = SSPI";
            string query = "SELECT COUNT(*) AS [DEM] FROM PostLike WHERE PostID = @PostID AND UserEmail = @UserEmail";
            var command = new SqlCommand(query, conn);
            command.Parameters.AddWithValue("PostID", PostID);
            command.Parameters.AddWithValue("UserEmail", UserEmail);
            conn.Open();
            var reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    temp = (int)reader["DEM"];
                }
            }
            conn.Close();
            if (temp > 0)
                return true;
            else return false;
        }
        public bool isBlocked(string UserEmail, string BlockedUser)
        {
            int temp = 0;
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = @"Data Source = localhost; Database = AlrightSocial; Integrated Security = SSPI";
            string query = "SELECT COUNT(*) AS [DEM] FROM BlockedEmail WHERE UserEmail = @UserEmail AND BlockedUser = @BlockedUser";
            var command = new SqlCommand(query, conn);
            command.Parameters.AddWithValue("UserEmail", UserEmail);
            command.Parameters.AddWithValue("BlockedUser", BlockedUser);
            conn.Open();
            var reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    temp = (int)reader["DEM"];
                }
            }
            conn.Close();
            if (temp > 0)
                return true;
            else return false;
        }
        public bool isFriended(string UserEmail, string FriendEmail)
        {
            int temp = 0;
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = @"Data Source = localhost; Database = AlrightSocial; Integrated Security = SSPI";
            string query = "SELECT COUNT(*) AS [DEM] FROM (SELECT FriendEmail FROM Friend WHERE UserEmail = @UserEmail AND FriendEmail=@FriendEmail UNION SELECT FriendEmail FROM FriendRequest WHERE FriendEmail = @UserEmail AND UserEmail=@FriendEmail) AS [A]";
            var command = new SqlCommand(query, conn);
            command.Parameters.AddWithValue("UserEmail", UserEmail);
            command.Parameters.AddWithValue("FriendEmail", FriendEmail);
            conn.Open();
            var reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    temp = (int)reader["DEM"];
                }
            }
            conn.Close();
            if (temp > 0)
                return true;
            else return false;
        }
        public bool isSuspended(string UserEmail)
        {
            int temp = 0;
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = @"Data Source = localhost; Database = AlrightSocial; Integrated Security = SSPI";
            string query = "SELECT COUNT(*) AS [DEM] FROM (SELECT SuspendedEmail FROM SuspendedUser WHERE SuspendedEmail = @UserEmail) AS [A]";
            var command = new SqlCommand(query, conn);
            command.Parameters.AddWithValue("UserEmail", UserEmail);
            conn.Open();
            var reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    temp = (int)reader["DEM"];
                }
            }
            conn.Close();
            if (temp > 0)
                return true;
            else return false;
        }

        public int findChat(string User1, string User2)
        {
            int temp = 0;
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = @"Data Source = localhost; Database = AlrightSocial; Integrated Security = SSPI";
            string query = "SELECT Id FROM Chats WHERE (User1=@User1 AND User2=@User2) OR (User2=@User1 AND User1=@User2)";
            var command = new SqlCommand(query, conn);
            command.Parameters.AddWithValue("User1", User1);
            command.Parameters.AddWithValue("User2", User2);
            conn.Open();
            var reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    temp = (int)reader["Id"];
                }
            }
            conn.Close();
            return temp;
        }

        public string findLastMessage(int ChatId)
        {
            string temp = "";
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = @"Data Source = localhost; Database = AlrightSocial; Integrated Security = SSPI";
            string query = "SELECT TOP 1 Content, Time FROM Message WHERE ChatId = @ChatId ORDER By Time DESC";
            var command = new SqlCommand(query, conn);
            command.Parameters.AddWithValue("ChatId", ChatId);
            conn.Open();
            var reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    temp = reader["Content"].ToString();
                }
            }
            conn.Close();
            return temp;
        }
        public DateTime findLastTime(int ChatId)
        {
            DateTime temp = new DateTime();
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = @"Data Source = localhost; Database = AlrightSocial; Integrated Security = SSPI";
            string query = "SELECT TOP 1 Content, Time FROM Message WHERE ChatId = @ChatId ORDER By Time DESC";
            var command = new SqlCommand(query, conn);
            command.Parameters.AddWithValue("ChatId", ChatId);
            conn.Open();
            var reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    temp = (DateTime)reader["Time"];
                }
            }
            conn.Close();
            return temp;
        }
        public DbSet<AlrightSocialWebApp.Models.PostComment> PostComment { get; set; }
        public void CreateComment(PostComment cmt)
        {
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = @"Data Source = localhost; Database = AlrightSocial; Integrated Security = SSPI";
            string query2 = "INSERT INTO PostComment (PostID, UserEmail, Content, Time) VALUES (@PostID, @UserEmail, @Content, @Time)";
            SqlCommand cmd2 = new SqlCommand(query2, conn);
            cmd2.Parameters.AddWithValue("@PostID", cmt.PostID);
            cmd2.Parameters.AddWithValue("@UserEmail", cmt.UserEmail);
            cmd2.Parameters.AddWithValue("@Content", cmt.Content);
            cmd2.Parameters.AddWithValue("@Time", DateTime.Now);
            conn.Open();
            cmd2.ExecuteNonQuery();
            conn.Close();
        }
        public void CreateComment(PostComment cmt, Post post)
        {
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = @"Data Source = localhost; Database = AlrightSocial; Integrated Security = SSPI";

            string query1 = "INSERT INTO Notification (UserEmail, Content, Time, isRead, PostID) VALUES (@UserEmail, @Content, @Time, 'false', @PostID)";
            SqlCommand cmd1 = new SqlCommand(query1, conn);
            cmd1.Parameters.AddWithValue("@UserEmail", post.Author);
            cmd1.Parameters.AddWithValue("@Content", cmt.UserEmail + " đã bình luận về bài viết của bạn.");
            cmd1.Parameters.AddWithValue("@Time", DateTime.Now);
            cmd1.Parameters.AddWithValue("@PostID", cmt.PostID);
            conn.Open();
            cmd1.ExecuteNonQuery();
            int numberofnotification = -1;
            string getnumberofnotification = "SELECT ISNULL(MAX(ID),0) AS [NUMBER] FROM NOTIFICATION";
            SqlCommand command = new SqlCommand(getnumberofnotification, conn);
            var reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    numberofnotification = (int)reader["NUMBER"];
                }
                reader.Close();
            }
            conn.Close();
            string query2 = "INSERT INTO PostComment (PostID, UserEmail, Content, NotificationID, Time) VALUES (@PostID, @UserEmail, @Content, @NotificationID, @Time)";
            SqlCommand cmd2 = new SqlCommand(query2, conn);
            cmd2.Parameters.AddWithValue("@PostID", cmt.PostID);
            cmd2.Parameters.AddWithValue("@UserEmail", cmt.UserEmail);
            cmd2.Parameters.AddWithValue("@Content", cmt.Content);
            cmd2.Parameters.AddWithValue("@Time", DateTime.Now);
            cmd2.Parameters.AddWithValue("@NotificationID", numberofnotification);
            conn.Open();
            cmd2.ExecuteNonQuery();
            conn.Close();
        }

        public void InsertLike(PostLike like)
        {
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = @"Data Source = localhost; Database = AlrightSocial; Integrated Security = SSPI";
            string query2 = "INSERT INTO PostLike (PostID, UserEmail) VALUES (@PostID, @UserEmail)";
            SqlCommand cmd2 = new SqlCommand(query2, conn);
            cmd2.Parameters.AddWithValue("@PostID", like.PostID);
            cmd2.Parameters.AddWithValue("@UserEmail", like.UserEmail);
            conn.Open();
            cmd2.ExecuteNonQuery();
            conn.Close();
        }
        public void InsertLike(PostLike like, string UserEmail)
        {
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = @"Data Source = localhost; Database = AlrightSocial; Integrated Security = SSPI";

            string query1 = "INSERT INTO Notification (UserEmail, Content, Time, isRead, PostID) VALUES (@UserEmail, @Content, @Time, 'false', @PostID)";
            SqlCommand cmd1 = new SqlCommand(query1, conn);
            cmd1.Parameters.AddWithValue("@UserEmail", UserEmail);
            cmd1.Parameters.AddWithValue("@Content", like.UserEmail + " đã thích về bài viết của bạn.");
            cmd1.Parameters.AddWithValue("@Time", DateTime.Now);
            cmd1.Parameters.AddWithValue("@PostID", like.PostID);
            conn.Open();
            cmd1.ExecuteNonQuery();
            int numberofnotification = -1;
            string getnumberofnotification = "SELECT ISNULL(MAX(ID),0) AS [NUMBER] FROM NOTIFICATION";
            SqlCommand command = new SqlCommand(getnumberofnotification, conn);
            var reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    numberofnotification = (int)reader["NUMBER"];
                }
                reader.Close();
            }
            conn.Close();
            string query2 = "INSERT INTO PostLike (PostID, UserEmail, NotificationID) VALUES (@PostID, @UserEmail, @NotificationID)";
            SqlCommand cmd2 = new SqlCommand(query2, conn);
            cmd2.Parameters.AddWithValue("@PostID", like.PostID);
            cmd2.Parameters.AddWithValue("@UserEmail", like.UserEmail);
            cmd2.Parameters.AddWithValue("@NotificationID", numberofnotification);
            conn.Open();
            cmd2.ExecuteNonQuery();
            conn.Close();
        }

        public List<object> GetListOfComment(int PostID)
        {
            List<object> list = new List<object>();
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = @"Data Source = localhost; Database = AlrightSocial; Integrated Security = SSPI";
            string query = "SELECT * FROM PostComment WHERE PostID=@PostID";
            var command = new SqlCommand(query, conn);
            command.Parameters.AddWithValue("PostID", PostID);
            conn.Open();
            var reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    list.Add(new
                    {
                        ID = (int)reader["ID"],
                        UserEmail = reader["UserEmail"].ToString(),
                        UserName = GetUserInfo(reader["UserEmail"].ToString()).name,
                        Content = reader["Content"].ToString(),
                        PostID = (int)reader["PostID"],
                        Time = (DateTime)reader["Time"],
                    });
                }
                reader.Close();
            }
            conn.Close();
            return list;
        }
        public DbSet<AlrightSocialWebApp.Models.Notification> Notification { get; set; }
        public DbSet<AlrightSocialWebApp.Models.PostLike> PostLike { get; set; }
        public DbSet<AlrightSocialWebApp.Models.Friend> Friend { get; set; }
        public List<object> GetListOfFriends(string EmailAddress)
        {
            List<object> list = new List<object>();
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = @"Data Source = localhost; Database = AlrightSocial; Integrated Security = SSPI";
            string query = "SELECT FriendEmail, AvatarURL, name, SignInStatus FROM Friend, Users WHERE UserEmail=@EmailAddress AND FriendEmail=Users.EmailAddress";
            var command = new SqlCommand(query, conn);
            command.Parameters.AddWithValue("EmailAddress", EmailAddress);
            conn.Open();
            var reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    list.Add(new
                    {
                        FriendEmail = reader["FriendEmail"].ToString(),
                        AvatarURL = reader["AvatarURL"].ToString(),
                        name = reader["name"].ToString(),
                        SignInStatus = reader["SignInStatus"].ToString(),
                        ChatId = findChat(reader["FriendEmail"].ToString(), EmailAddress),
                        LastMessage = findLastMessage(findChat(reader["FriendEmail"].ToString(), EmailAddress)),
                        LastTime = findLastTime(findChat(reader["FriendEmail"].ToString(), EmailAddress))
                    });
                }
            }
            conn.Close();
            return list;
        }

        public List<object> SearchFriend(string EmailAddress, string searchString)
        {
            List<object> list = new List<object>();
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = @"Data Source = localhost; Database = AlrightSocial; Integrated Security = SSPI";
            string query = "SELECT FriendEmail, AvatarURL, name, SignInStatus FROM Friend, Users WHERE UserEmail=@EmailAddress AND FriendEmail=Users.EmailAddress AND name LIKE @searchString";
            var command = new SqlCommand(query, conn);
            command.Parameters.AddWithValue("EmailAddress", EmailAddress);
            command.Parameters.AddWithValue("searchString", searchString);
            conn.Open();
            var reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    list.Add(new
                    {
                        FriendEmail = reader["FriendEmail"].ToString(),
                        AvatarURL = reader["AvatarURL"].ToString(),
                        name = reader["name"].ToString(),
                        SignInStatus = reader["SignInStatus"].ToString(),
                        ChatId = findChat(reader["FriendEmail"].ToString(), EmailAddress),
                        LastMessage = findLastMessage(findChat(reader["FriendEmail"].ToString(), EmailAddress)),
                        LastTime = findLastTime(findChat(reader["FriendEmail"].ToString(), EmailAddress))
                    });
                }
            }
            conn.Close();
            return list;
        }
        public List<FriendRequest> GetListOfFriendRequests(string EmailAddress)
        {
            List<FriendRequest> list = new List<FriendRequest>();
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = @"Data Source = localhost; Database = AlrightSocial; Integrated Security = SSPI";
            string query = "SELECT FriendEmail, Time FROM FriendRequest WHERE UserEmail=@EmailAddress";
            var command = new SqlCommand(query, conn);
            command.Parameters.AddWithValue("EmailAddress", EmailAddress);
            conn.Open();
            var reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    list.Add(new Models.FriendRequest()
                    {
                        FriendEmail = reader["FriendEmail"].ToString(),
                        Time = (DateTime)reader["Time"]
                    });
                }
            }
            conn.Close();
            return list;
        }
        public DbSet<AlrightSocialWebApp.Models.FriendRequest> FriendRequest { get; set; }
        public DbSet<AlrightSocialWebApp.Models.BlockedEmail> BlockedEmail { get; set; }
        public DbSet<AlrightSocialWebApp.Models.Message> Message { get; set; }
        public DbSet<AlrightSocialWebApp.Models.Chat> Chats { get; set; }
        public DbSet<AlrightSocialWebApp.Models.PostShare> PostShare { get; set; }
        public DbSet<AlrightSocialWebApp.Models.Administrator> Administrator { get; set; }
        public DbSet<AlrightSocialWebApp.Models.PostReport> PostReport { get; set; }
        public DbSet<AlrightSocialWebApp.Models.ReportUser> ReportUser { get; set; }

        public void InsertShare(PostShare share)
        {
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = @"Data Source = localhost; Database = AlrightSocial; Integrated Security = SSPI";
            string query2 = "INSERT INTO PostShare (UserEmail, PostID, Privacy, Time) VALUES (@UserEmail, @PostID, @Privacy, @Time)";
            SqlCommand cmd2 = new SqlCommand(query2, conn);
            cmd2.Parameters.AddWithValue("@PostID", share.PostID);
            cmd2.Parameters.AddWithValue("@UserEmail", share.UserEmail);
            cmd2.Parameters.AddWithValue("@Privacy", share.Privacy);
            cmd2.Parameters.AddWithValue("@Time", share.Time);
            conn.Open();
            cmd2.ExecuteNonQuery();
            conn.Close();
        }
        public void InsertShare(PostShare share, string UserEmail)
        {
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = @"Data Source = localhost; Database = AlrightSocial; Integrated Security = SSPI";

            string query1 = "INSERT INTO Notification (UserEmail, Content, Time, isRead, PostID) VALUES (@UserEmail, @Content, @Time, 'false', @PostID)";
            SqlCommand cmd1 = new SqlCommand(query1, conn);
            cmd1.Parameters.AddWithValue("@UserEmail", UserEmail);
            cmd1.Parameters.AddWithValue("@Content", share.UserEmail + " đã chia sẻ bài viết của bạn.");
            cmd1.Parameters.AddWithValue("@Time", DateTime.Now);
            cmd1.Parameters.AddWithValue("@PostID", share.PostID);
            conn.Open();
            cmd1.ExecuteNonQuery();
            int numberofnotification = -1;
            string getnumberofnotification = "SELECT ISNULL(MAX(ID),0) AS [NUMBER] FROM NOTIFICATION";
            SqlCommand command = new SqlCommand(getnumberofnotification, conn);
            var reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    numberofnotification = (int)reader["NUMBER"];
                }
                reader.Close();
            }
            conn.Close();
            string query2 = "INSERT INTO PostShare (PostID, UserEmail, NotificationID, Privacy, Time) VALUES (@PostID, @UserEmail, @NotificationID, @Privacy, @Time)";
            SqlCommand cmd2 = new SqlCommand(query2, conn);
            cmd2.Parameters.AddWithValue("@PostID", share.PostID);
            cmd2.Parameters.AddWithValue("@UserEmail", share.UserEmail);
            cmd2.Parameters.AddWithValue("@NotificationID", numberofnotification);
            cmd2.Parameters.AddWithValue("@Privacy", share.Privacy);
            cmd2.Parameters.AddWithValue("@Time", share.Time);
            conn.Open();
            cmd2.ExecuteNonQuery();
            conn.Close();
        }

        public List<object> GetListOfFriendsInteraction(string EmailAddress)
        {
            List<object> list = new List<object>();
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = @"Data Source = localhost; Database = AlrightSocial; Integrated Security = SSPI";
            string query = "SELECT AvatarURL,name, FriendEmail, ISNULL(Mess.NumOfMess,0) AS [NumOfMess], ISNULL(LikeInfor.NumOfLike,0) AS [NumOfLike], ISNULL(Cmt.NumOfComment,0) AS [NumOfComment], ISNULL(Share.NumOfShare,0) AS [NumOfShare] FROM Chats  INNER JOIN  (SELECT Chats.Id, COUNT(Message.ID) NumOfMess FROM Chats INNER JOIN Message ON Message.ChatId=Chats.Id GROUP BY Chats.Id) AS [Mess]  ON Mess.ID=Chats.Id INNER JOIN  (SELECT FriendEmail FROM Friend  WHERE UserEmail=@EmailAddress) AS [Friend] ON (User1=Friend.FriendEmail AND User2=@EmailAddress) OR (User2=Friend.FriendEmail AND User1=@EmailAddress) LEFT JOIN  (SELECT UserEmail, COUNT(*) AS [NumOfLike] FROM PostLike INNER JOIN Post ON Post.ID=PostLike.PostID WHERE Author=@EmailAddress GROUP BY UserEmail) AS [LikeInfor]  ON LikeInfor.UserEmail=Friend.FriendEmail LEFT JOIN  (SELECT UserEmail, COUNT(*) AS [NumOfComment] FROM PostComment INNER JOIN Post ON Post.ID=PostComment.PostID WHERE Author=@EmailAddress GROUP BY UserEmail) AS [Cmt] ON Cmt.UserEmail=Friend.FriendEmail LEFT JOIN  (SELECT UserEmail, COUNT(*) AS [NumOfShare] FROM PostShare INNER JOIN Post ON Post.ID=PostShare.PostID WHERE Author=@EmailAddress GROUP BY UserEmail) AS [Share] ON Share.UserEmail=Friend.FriendEmail INNER JOIN Users ON Users.EmailAddress=Friend.FriendEmail ORDER BY NumOfMess DESC, NumOfShare DESC, NumOfComment DESC, NumOfLike DESC";
            SqlCommand cmd1 = new SqlCommand(query, conn);
            conn.Open();
            cmd1.Parameters.AddWithValue("@EmailAddress", EmailAddress);
            var reader = cmd1.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    list.Add(new
                    {
                        name = reader["name"].ToString(),
                        AvatarURL = reader["AvatarURL"].ToString(),
                        FriendEmail = reader["FriendEmail"].ToString(),
                        NumOfMess = (int)reader["NumOfMess"],
                        NumOfShare = (int)reader["NumOfShare"],
                        NumOfComment = (int)reader["NumOfComment"],
                        NumOfLike = (int)reader["NumOfLike"]
                    });
                }
                reader.Close();
            }
            return list;
        }

        public int AmountOfReport(int PostID)
        {
            int temp = 0;
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = @"Data Source = localhost; Database = AlrightSocial; Integrated Security = SSPI";
            string query = "SELECT ISNULL(COUNT(EmailAddress),0) AS [Report] FROM PostReport WHERE PostID = @PostID";
            SqlCommand cmd1 = new SqlCommand(query, conn);
            conn.Open();
            cmd1.Parameters.AddWithValue("@PostID", PostID);
            var reader = cmd1.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    temp = (int)reader["Report"];
                }
            }
            return temp;
        }
    }
}
