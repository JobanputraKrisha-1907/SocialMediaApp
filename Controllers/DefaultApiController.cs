using SMWebApplication.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace SMWebApplication.Controllers
{
    public class DefaultApiController : ApiController
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["MyConnectionString"].ConnectionString;
       
        [HttpGet]
        [Route("api/DefaultApi/SearchUsers")]
        public IHttpActionResult SearchUsers(string query)
        {
            List<User> users = new List<User>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("SearchUsers", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Query", query);

                    connection.Open();
                    using (
                        SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            User user = new User
                            {
                                UserID = (int)reader["UserID"],
                                Username = reader["Username"].ToString(),
                                FullName = reader["FullName"].ToString(),
                                ProfilePictureURL = reader["ProfilePictureURL"].ToString()
                                
                            };
                            users.Add(user);
                        }
                    }
                }
            }

            return Ok(users);
        }

        [HttpGet]
        [Route("api/DefaultApi/GetAllUsersExceptLoggedInUser")]
        public List<User> GetAllUsersExceptLoggedInUser(int loggedInUserId)
        {
            List<User> userList = new List<User>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("GetAllUsersExceptLoggedInUser", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@LoggedInUserId", loggedInUserId);

                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        User user = new User
                        {
                            UserID = (int)reader["UserID"],
                            Username = reader["Username"].ToString(),
                            ProfilePictureURL = reader["ProfilePictureURL"].ToString(),
                            
                        };

                        userList.Add(user);
                    }
                }
            }

            return userList;
        }



        [HttpPost]
        [Route("api/DefaultApi/AddFriend")]
        public IHttpActionResult AddFriend(FollowersFollowing request)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("AddFriend", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@FollowerUserID", request.FollowerUserID);
                    command.Parameters.AddWithValue("@FollowingUserID", request.FollowingUserID);
                    

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }

            return Ok();
        }

        [HttpPost]
        [Route("api/DefaultApi/RemoveFriend")]
        public IHttpActionResult RemoveFriend(FollowersFollowing request)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("RemoveFriend", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@FollowerUserID", request.FollowerUserID);
                    command.Parameters.AddWithValue("@FollowingUserID", request.FollowingUserID);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }

            return Ok();
        }

        [HttpPost]
        [Route("api/DefaultApi/ConfirmFriendRequest")]
        public IHttpActionResult ConfirmFriendRequest(FollowersFollowing request)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("ConfirmFriendRequest", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@FollowerUserID", request.FollowerUserID);
                    command.Parameters.AddWithValue("@FollowingUserID", request.FollowingUserID);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }

            return Ok();
        }


        [HttpPost]
        [Route("api/DefaultApi/SubmitComment")]
        public IHttpActionResult SubmitComment(Comment model)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand("Comment", connection);
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@PostID", model.PostID);
                    command.Parameters.AddWithValue("@UserID", model.UserID);
                    command.Parameters.AddWithValue("@CommentText", model.CommentText);

                    SqlParameter CommentsCount = new SqlParameter("@CommentsCount", SqlDbType.Int);
                    CommentsCount.Direction = ParameterDirection.Output;
                    command.Parameters.Add(CommentsCount);

                    connection.Open();
                    command.ExecuteNonQuery();

                    // Retrieve the value of the output parameter
                    int commentsCountValue = Convert.ToInt32(CommentsCount.Value);

                    return Ok(new { CommentsCount = commentsCountValue });
                    // Alternatively, you can directly return the comment submitted successfully message here if you don't want to return CommentsCount
                    // return Ok("Comment submitted successfully");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return InternalServerError(new Exception("Failed to submit comment"));
            }
        }

        [HttpGet]
        [Route("api/DefaultApi/GetCommentsForPost")]
        public IHttpActionResult GetCommentsForPost(int postId)
        {
            try
            {
                List<CommentModel> comments = new List<CommentModel>();

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand("GetCommentsForPost", connection);
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@PostID", postId);

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        CommentModel comment = new CommentModel();
                        comment.CommentID = (int)reader["CommentID"]; // Add this line to retrieve the CommentID
                        comment.ProfilePic = reader["ProfilePic"].ToString();
                        comment.Username = reader["Username"].ToString();
                        comment.Text = reader["Text"].ToString();
                        comments.Add(comment);
                    }

                    reader.Close();
                }

                return Ok(comments);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return InternalServerError(new Exception("Failed to get comments for post"));
            }
        }



        public class CommentModel
    {
            public int CommentID { get; set; }
            public string ProfilePic { get; set; }
        public string Username { get; set; }
        public string Text { get; set; }
    }

        [HttpGet]
        [Route("api/DefaultApi/GetAllfollowers")]
        public List<User> GetAllfollowers(int loggedInUserId)
        {
            List<User> userList = new List<User>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("GetFollowersForUser", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@UserID", loggedInUserId);

                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        User user = new User
                        {
                            UserID = (int)reader["UserID"],
                            Username = reader["Username"].ToString(),
                            ProfilePictureURL = reader["ProfilePictureURL"].ToString(),
                            
                        };

                        userList.Add(user);
                    }
                }
            }

            return userList;
        }

        [HttpGet]
        [Route("api/DefaultApi/GetAllfollowings")]
        public List<User> GetAllfollowings(int loggedInUserId)
        {
            List<User> userList = new List<User>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("GetFollowingsForUser", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@UserID", loggedInUserId);

                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        User user = new User
                        {
                            UserID = (int)reader["UserID"],
                            Username = reader["Username"].ToString(),
                            ProfilePictureURL = reader["ProfilePictureURL"].ToString(),
                            
                        };

                        userList.Add(user);
                    }
                }
            }

            return userList;
        }



        [HttpGet]
        [Route("api/DefaultApi/notifications")]
        public IHttpActionResult GetNotifications(int userId)
        {
            List<string> notifications = new List<string>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("FetchNotifications", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@YourUserID", userId);

                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string notificationMessage = reader["NotificationMessage"].ToString();
                            notifications.Add(notificationMessage);
                        }
                    }
                }
            }

            return Ok(notifications);
        }


        [HttpPost]
        [Route("api/DefaultApi/AddComment")]
        public IHttpActionResult AddComment([FromBody] CommentData commentData)
        {
            try
            {
                // Perform validation if necessary

                int commentId = 0; // Initialize to default value
                string userProfileImageUrl = null;
                string username = null;

                // Insert the comment into the database and fetch additional data
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand("AddComment", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@ParentCommentId", commentData.ParentCommentId);
                        command.Parameters.AddWithValue("@CommentText", commentData.Comment);
                        command.Parameters.AddWithValue("@UserID", commentData.UserId);
                        command.Parameters.AddWithValue("@PostID", commentData.PostID);// Pass the UserID
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                commentId = Convert.ToInt32(reader["CommentID"]);
                                userProfileImageUrl = reader["UserProfileImageUrl"].ToString();
                                username = reader["Username"].ToString();
                            }
                            // No need for else condition since commentId, userProfileImageUrl, and username 
                            // are already initialized to default values
                        }
                    }
                }

                // Check if any data was returned
                if (commentId == 0 || userProfileImageUrl == null || username == null)
                {
                    throw new Exception("No data returned from AddComment stored procedure.");
                }

                return Ok(new
                {
                    success = true,
                    commentId = commentId,
                    userProfileImageUrl = userProfileImageUrl,
                    username = username,
                    commentText = commentData.Comment
                });
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                Console.WriteLine("Error adding comment: " + ex.Message);
                return InternalServerError(new Exception("Failed to add comment."));
            }
        }






        [HttpGet]
        [Route("api/DefaultApi/GetCommentsForPostWithReplies")]
        public IHttpActionResult GetCommentsForPostWithReplies(int postId)
        {
            try
            {
                List<CommentData> comments = new List<CommentData>();

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand("GetCommentsForPostWithReplies", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@PostID", postId);
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                CommentData comment = new CommentData
                                {
                                    UserId = (int)reader["UserID"],
                                    CommentID = Convert.ToInt32(reader["CommentID"]),
                                    ProfilePic = reader["ProfilePic"].ToString(),
                                    Username = reader["Username"].ToString(),
                                    Text = reader["Text"].ToString(),
                                    ParentCommentId = reader["ParentCommentId"] == DBNull.Value ? null : (int?)reader["ParentCommentId"],
                                    TotalLikesCount = Convert.ToInt32(reader["TotalLikesCount"]) // Add total likes count
                                };
                                comments.Add(comment);
                            }
                        }
                    }
                }

                return Ok(comments);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error fetching comments:", ex.Message);
                return InternalServerError(new Exception("Failed to fetch comments."));
            }
        }

        public class CommentData
        {
            public int CommentID { get; set; }
            public string ProfilePic { get; set; }
            public string Username { get; set; }
            public string Text { get; set; }
            public int? ParentCommentId { get; set; }
            public int TotalLikesCount { get; set; } // Total likes count
            public string Comment { get; set; }
            public int UserId { get; set; }
            public int PostID { get; set; }
        }


        [HttpGet]
        [Route("api/DefaultApi/GetAllChatUsers")]
        public IHttpActionResult GetAllChatUsers()
        {
            List<User> users = new List<User>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("GetAllChatUsers", conn)) // Call the stored procedure
                {
                    cmd.CommandType = CommandType.StoredProcedure; // Specify that it's a stored procedure
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        users.Add(new User
                        {
                            UserID = (int)reader["UserID"],
                            Username = reader["Username"].ToString(),
                            ProfilePictureURL = reader["ProfilePictureURL"].ToString()
                        });
                    }
                }
            }

            return Ok(users);
        }



    }
}
