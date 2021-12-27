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
        }
        public DbSet<User> Users { get; set; }
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
        }

        public List<object> GetListOfPost(string EmailAddress, string CurrentUser)
        {
            List<object> list = new List<object>();
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = @"Data Source = localhost; Database = AlrightSocial; Integrated Security = SSPI";
            string query = "";
            if (CurrentUser == null)
            {
                query = "SELECT Post.ID, Title, Content, TimeCreate, TimeModified, Author, Privacy, ISNULL(LikeTable.[Like],0) AS [Like] , ISNULL(CommentTable.[Comment],0) AS [Comment], ISNULL(ShareTable.[Share],0) AS [Share] FROM Post LEFT JOIN (SELECT PostLike.PostID ID, COUNT(UserEmail) AS [Like] FROM PostLike GROUP BY PostLike.PostID) LikeTable ON LikeTable.ID = Post.ID LEFT JOIN (SELECT PostComment.PostID, COUNT(UserEmail) AS [Comment] FROM PostComment GROUP BY PostComment.PostID) CommentTable ON Post.ID = CommentTable.PostID LEFT JOIN (SELECT PostID, COUNT(UserEmail) AS [Share] FROM PostShare GROUP BY PostShare.PostID) ShareTable ON Post.ID = ShareTable.PostID WHERE Author = @EmailAddress AND Privacy='Public' ORDER BY Post.TimeCreate DESC";
            }
            else
            {
                if (EmailAddress == CurrentUser)
                {
                    query = "SELECT Post.ID, Title, Content, TimeCreate, TimeModified, Author, Privacy, ISNULL(LikeTable.[Like],0) AS [Like] , ISNULL(CommentTable.[Comment],0) AS [Comment], ISNULL(ShareTable.[Share],0) AS [Share] FROM Post LEFT JOIN (SELECT PostLike.PostID ID, COUNT(UserEmail) AS [Like] FROM PostLike GROUP BY PostLike.PostID) LikeTable ON LikeTable.ID = Post.ID LEFT JOIN (SELECT PostComment.PostID, COUNT(UserEmail) AS [Comment] FROM PostComment GROUP BY PostComment.PostID) CommentTable ON Post.ID = CommentTable.PostID LEFT JOIN (SELECT PostID, COUNT(UserEmail) AS [Share] FROM PostShare GROUP BY PostShare.PostID) ShareTable ON Post.ID = ShareTable.PostID WHERE Author = @EmailAddress ORDER BY Post.TimeCreate DESC";
                }
                else
                {
                    query = "SELECT Post.ID, Title, Content, TimeCreate, TimeModified, Author, Privacy, Users.AvatarURL, Users.name, ISNULL(LikeTable.[Like], 0) AS[Like] , ISNULL(CommentTable.[Comment], 0) AS[Comment], ISNULL(ShareTable.[Share], 0) AS[Share] FROM (SELECT Post.ID, Title, Content, TimeCreate, TimeModified, Author, Privacy FROM Post WHERE Author = @EmailAddress AND Privacy = 'Public' UNION SELECT Post.ID, Title, Content, TimeCreate, TimeModified, Author, Privacy FROM Post WHERE Privacy = 'Friend' AND Author IN(SELECT FriendEmail FROM Friend WHERE UserEmail = @CurrentEmail)) AS[Post] LEFT JOIN (SELECT PostLike.PostID ID, COUNT(UserEmail) AS [Like] FROM PostLike GROUP BY PostLike.PostID) LikeTable ON LikeTable.ID = Post.ID LEFT JOIN (SELECT PostComment.PostID, COUNT(UserEmail) AS [Comment] FROM PostComment GROUP BY PostComment.PostID) CommentTable ON Post.ID = CommentTable.PostID LEFT JOIN (SELECT PostID, COUNT(UserEmail) AS [Share] FROM PostShare GROUP BY PostShare.PostID) ShareTable ON Post.ID = ShareTable.PostID INNER JOIN Users ON Post.Author = Users.EmailAddress ORDER BY Post.TimeCreate DESC";
                }
            }
            var command = new SqlCommand(query, conn);
            command.Parameters.AddWithValue("EmailAddress", EmailAddress);
            if (EmailAddress != CurrentUser && CurrentUser != null)
                command.Parameters.AddWithValue("CurrentEmail", CurrentUser);
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
                        Privacy = reader["Privacy"].ToString(),
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
            string query = "SELECT Post.ID, Title, Content, TimeCreate, TimeModified, Author, Privacy, Users.AvatarURL, Users.name, ISNULL(LikeTable.[Like], 0) AS[Like] , ISNULL(CommentTable.[Comment], 0) AS[Comment], ISNULL(ShareTable.[Share], 0) AS[Share]   FROM (SELECT Post.ID, Title, Content, TimeCreate, TimeModified, Author, Privacy FROM Post WHERE Author = @EmailAddress UNION SELECT Post.ID, Title, Content, TimeCreate, TimeModified, Author, Privacy FROM Post WHERE Privacy = 'Friend' AND Author IN(SELECT FriendEmail FROM Friend WHERE UserEmail = @EmailAddress) UNION SELECT Post.ID, Title, Content, TimeCreate, TimeModified, Author, Privacy FROM Post WHERE Privacy = 'Public' AND Author Not IN(SELECT BlockedUser FROM BlockedEmail WHERE UserEmail = @EmailAddress)) AS[Post] LEFT JOIN (SELECT PostLike.PostID ID, COUNT(UserEmail) AS [Like] FROM PostLike GROUP BY PostLike.PostID) LikeTable ON LikeTable.ID = Post.ID LEFT JOIN (SELECT PostComment.PostID, COUNT(UserEmail) AS [Comment] FROM PostComment GROUP BY PostComment.PostID) CommentTable ON Post.ID = CommentTable.PostID LEFT JOIN (SELECT PostID, COUNT(UserEmail) AS [Share] FROM PostShare GROUP BY PostShare.PostID) ShareTable ON Post.ID = ShareTable.PostID INNER JOIN Users ON Post.Author = Users.EmailAddress ORDER BY Post.TimeCreate DESC";
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
            string query = "SELECT Post.ID, Title, Content, TimeCreate, TimeModified, Author, Privacy, Users.AvatarURL, Users.name, ISNULL(LikeTable.[Like], 0) AS[Like] , ISNULL(CommentTable.[Comment], 0) AS[Comment], ISNULL(ShareTable.[Share], 0) AS[Share] FROM Post LEFT JOIN (SELECT PostLike.PostID ID, COUNT(UserEmail) AS [Like] FROM PostLike GROUP BY PostLike.PostID) LikeTable ON LikeTable.ID = Post.ID LEFT JOIN (SELECT PostComment.PostID, COUNT(UserEmail) AS [Comment] FROM PostComment GROUP BY PostComment.PostID) CommentTable ON Post.ID = CommentTable.PostID LEFT JOIN (SELECT PostID, COUNT(UserEmail) AS [Share] FROM PostShare GROUP BY PostShare.PostID) ShareTable ON Post.ID = ShareTable.PostID INNER JOIN Users ON Post.Author = Users.EmailAddress WHERE Post.Privacy='Public' ORDER BY Post.TimeCreate DESC";
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
            if (temp > 0)
                return true;
            else return false;
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
        }

        public List<PostComment> GetListOfComment(int PostID)
        {
            List<PostComment> list = new List<PostComment>();
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
                    list.Add(new PostComment()
                    {
                        ID = (int)reader["ID"],
                        UserEmail = reader["UserEmail"].ToString(),
                        Content = reader["Content"].ToString(),
                        PostID = (int)reader["PostID"],
                        Time = (DateTime)reader["Time"]
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
        public List<Friend> GetListOfFriends(string EmailAddress)
        {
            List<Friend> list = new List<Friend>();
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = @"Data Source = localhost; Database = AlrightSocial; Integrated Security = SSPI";
            string query = "SELECT FriendEmail FROM Friend WHERE UserEmail=@EmailAddress";
            var command = new SqlCommand(query, conn);
            command.Parameters.AddWithValue("EmailAddress", EmailAddress);
            conn.Open();
            var reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    list.Add(new Models.Friend()
                    {
                        FriendEmail = reader["FriendEmail"].ToString()
                    });
                }
            }
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
            return list;
        }
        public DbSet<AlrightSocialWebApp.Models.FriendRequest> FriendRequest { get; set; }
        public DbSet<AlrightSocialWebApp.Models.BlockedEmail> BlockedEmail { get; set; }
    }
}
