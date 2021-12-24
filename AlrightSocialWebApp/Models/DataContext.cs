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

        public List<object> GetListOfPost(string EmailAddress)
        {
            List<object> list = new List<object>();
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = @"Data Source = localhost; Database = AlrightSocial; Integrated Security = SSPI";
            string query = "SELECT Post.ID, Title, Content, TimeCreate, TimeModified, Author, Privacy, ISNULL(LikeTable.[Like],0) AS [Like] , ISNULL(CommentTable.[Comment],0) AS [Comment], ISNULL(ShareTable.[Share],0) AS [Share] FROM Post LEFT JOIN (SELECT PostLike.PostID ID, COUNT(UserEmail) AS [Like] FROM PostLike GROUP BY PostLike.PostID) LikeTable ON LikeTable.ID = Post.ID LEFT JOIN (SELECT PostComment.PostID, COUNT(UserEmail) AS [Comment] FROM PostComment GROUP BY PostComment.PostID) CommentTable ON Post.ID = CommentTable.PostID LEFT JOIN (SELECT PostID, COUNT(UserEmail) AS [Share] FROM PostShare GROUP BY PostShare.PostID) ShareTable ON Post.ID = ShareTable.PostID WHERE Author = @EmailAddress";
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
    }
}
