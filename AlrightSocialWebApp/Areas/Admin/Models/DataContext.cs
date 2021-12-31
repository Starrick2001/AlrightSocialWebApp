using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AlrightSocialWebApp.Areas.Admin.Models
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
        public List<object> GetListOfUserForAnalysis()
        {
            List<object> list = new List<object>();

            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = @"Data Source = localhost; Database = AlrightSocial; Integrated Security = SSPI";
            string query = "SELECT EmailAddress, name, sex, DateOfBirth, PhoneNumber, AvatarURL, ISNULL(Post.NumOfPost,0) AS [NumOfPost] FROM Users INNER JOIN(SELECT Author, Count(ID) AS [NumOfPost] FROM Post GROUP BY Author) AS[Post] ON Users.EmailAddress = Post.Author ORDER BY NumOfPost DESC";
            var command = new SqlCommand(query, conn);
            conn.Open();
            var reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    list.Add(new
                    {
                        AvatarURL = reader["AvatarURL"].ToString(),
                        DateOfBirth = (DateTime)reader["DateOfBirth"],
                        EmailAddress = reader["EmailAddress"].ToString(),
                        name = reader["name"].ToString(),
                        PhoneNumber = reader["PhoneNumber"].ToString(),
                        sex = reader["sex"].ToString(),
                        NumOfPost = (int)reader["NumOfPost"]
                    });

                }
                reader.Close();
            }
            conn.Close();
            return list;
        }
        public List<object> GetListOfPostForAnalysis()
        {
            List<object> list = new List<object>();

            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = @"Data Source = localhost; Database = AlrightSocial; Integrated Security = SSPI";
            string query = "SELECT Post.ID,Title, Content,Author, ISNULL(LikeTable.[Like], 0) AS[Like] , ISNULL(CommentTable.[Comment], 0) AS[Comment], ISNULL(ShareTable.[Share], 0) AS[Share]   FROM Post LEFT JOIN(SELECT PostLike.PostID ID, COUNT(UserEmail) AS [Like] FROM PostLike GROUP BY PostLike.PostID) LikeTable ON LikeTable.ID = Post.ID LEFT JOIN(SELECT PostComment.PostID, COUNT(UserEmail) AS [Comment] FROM PostComment GROUP BY PostComment.PostID) CommentTable ON Post.ID = CommentTable.PostID LEFT JOIN(SELECT PostID, COUNT(UserEmail) AS [Share] FROM PostShare GROUP BY PostShare.PostID) ShareTable ON Post.ID = ShareTable.PostID INNER JOIN Users ON Post.Author = Users.EmailAddress";
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
                        Author = reader["Author"].ToString(),
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
    }
}
