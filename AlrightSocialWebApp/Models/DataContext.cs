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

        public List<Post> ListOfPost(string EmailAddress)
        {
            List<Post> list = new List<Post>();
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = @"Data Source = localhost; Database = AlrightSocial; Integrated Security = SSPI";
            string query = "SELECT * FROM Post WHERE Author=@EmailAddress";
            var command = new SqlCommand(query, conn);
            command.Parameters.AddWithValue("EmailAddress", EmailAddress);
            conn.Open();
            var reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    list.Add(new Post()
                    {
                        ID = (int)reader["ID"],
                        Title = reader["Title"].ToString(),
                        Content = reader["Content"].ToString(),
                        TimeCreate = (DateTime)reader["TimeCreate"],
                        TimeModified = (DateTime)reader["TimeModified"],
                        Author = reader["Author"].ToString(),
                        Privacy = reader["Privacy"].ToString()
                    });
                }
                reader.Close();
            }
            conn.Close();
            return list;
        }
    }
}
