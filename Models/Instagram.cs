using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMWebApplication.Models
{
    public class Instagram
    {
    }

    // Users Table
    public class User
    {
        public int UserID { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public string ProfilePictureURL { get; set; }
        public string Bio { get; set; }
        public string Website { get; set; }
        public string Gender { get; set; }
        public string DateOfBirth { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime AccountCreationTimestamp { get; set; }
        public DateTime? LastLoginTimestamp { get; set; }
        public string AccountStatus { get; set; }

        public bool IsFriend { get; set; }
        public string RequestStatus { get; set; }

        
        public bool IsFollowing { get; set; }
        public bool IsFollowedBack { get; set; }
    }

    // Posts Table
    public class Post
    {
        public string Username { get; set; }
        public int PostID { get; set; }
        public int UserID { get; set; }
        public string Caption { get; set; }
        public string MediaType { get; set; }
        public string MediaURL { get; set; }
        public string Location { get; set; }
        public string ProfilePictureURL { get; set; }
        public DateTime PostCreationTimestamp { get; set; }
        public string PostStatus { get; set; }
        public int LikesCount { get; set; }
        public int CommentsCount { get; set; }
        public int SharesCount { get; set; }
        public string Visibility { get; set; }
    }

    // Comments Table
    public class Comment
    {
        public int CommentID { get; set; }
        public int PostID { get; set; }
        public int UserID { get; set; }
        public string CommentText { get; set; }
        public DateTime CommentTimestamp { get; set; }
        public string CommentStatus { get; set; }
    }

    

    // Likes Table
    public class Like
    {
        public int CommentID { get; set; }
        public int ReelID { get; set; }
        public int LikeID { get; set; }
        public int PostID { get; set; }
        public int UserID { get; set; }
        public DateTime LikeTimestamp { get; set; }
    }

    // Followers/Following Table
    public class FollowersFollowing
    {
        public int RelationshipID { get; set; }
        public int FollowerUserID { get; set; }
        public int FollowingUserID { get; set; }
        public bool IsFriend { get; set; }
        public string RequestStatus { get; set; }
        public DateTime FollowTimestamp { get; set; }
    }


    // Direct Messages Table
    public class DirectMessage
    {
        public int MessageID { get; set; }
        public int SenderUserID { get; set; }
        public int ReceiverUserID { get; set; }
        public string MessageText { get; set; }
        public DateTime MessageTimestamp { get; set; }
        public string MessageStatus { get; set; }
    }

    // Hashtags Table
    public class Hashtag
    {
        public int HashtagID { get; set; }
        public string HashtagText { get; set; }
    }

    // Tagged Users Table
    public class TaggedUser
    {
        public int TagID { get; set; }
        public int PostID { get; set; }
        public int UserID { get; set; }
        public DateTime TagTimestamp { get; set; }
    }

    // Notifications Table
    public class Notification
    {
        public int NotificationID { get; set; }
        public int UserID { get; set; }
        public string NotificationType { get; set; }
        public int? NotificationSenderUserID { get; set; }
        public int? NotificationPostID { get; set; }
        public string NotificationText { get; set; }
        public DateTime NotificationTimestamp { get; set; }
        public string NotificationStatus { get; set; }
    }

    // Reported Posts Table
    public class ReportedPost
    {
        public int ReportID { get; set; }
        public int PostID { get; set; }
        public int ReporterUserID { get; set; }
        public string ReportReason { get; set; }
        public DateTime ReportTimestamp { get; set; }
        public string ReportStatus { get; set; }
    }

    // Blocked Users Table
    public class BlockedUser
    {
        public int BlockID { get; set; }
        public int BlockingUserID { get; set; }
        public int BlockedUserID { get; set; }
        public DateTime BlockTimestamp { get; set; }
    }

    // Saved Posts Table
    public class SavedPost
    {
        public int SaveID { get; set; }
        public int UserID { get; set; }
        public int PostID { get; set; }
        public DateTime SaveTimestamp { get; set; }
    }

    // Stories Table
    public class Story
    {
        public int StoryID { get; set; }
        public int UserID { get; set; }
        public string MediaType { get; set; }
        public string MediaURL { get; set; }
        public DateTime? StoryExpiryTimestamp { get; set; }
    }

    // Explore Table
    public class Explore
    {
        public int ExploreID { get; set; }
        public int PostID { get; set; }
        public DateTime? PostTimestamp { get; set; }
        public int? PostPopularityScore { get; set; }
        public string PostSource { get; set; }
    }

    // Analytics Table
    public class Analytic
    {
        public int AnalyticID { get; set; }
        public int PostID { get; set; }
        public int ViewCount { get; set; }
        public int ImpressionCount { get; set; }
        public float EngagementRate { get; set; }
        public int Reach { get; set; }
    }

    // Settings Table
    public class Setting
    {
        public int SettingID { get; set; }
        public int UserID { get; set; }
        public string NotificationPreferences { get; set; }
        public string PrivacySettings { get; set; }
        public string AccountPreferences { get; set; }
        public string SecuritySettings { get; set; }
    }

    // Admin Table
    public class Admin
    {
        public int AdminID { get; set; }
        public string AdminUsername { get; set; }
        public string AdminPassword { get; set; }
        public string AdminRole { get; set; }
        public string AdminPermissions { get; set; }
    }

}