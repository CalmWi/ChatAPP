
namespace ChatAPP.Models
{
    public class ChatUser
    {
        
        public string UserId { get; set; }
        public User User { get; set; }
        public int ChatId { get; set; }
        public Chat Chat { get; set; }
        public UserRole Role { get; set; }
        public override bool Equals(object? obj)
        {
            if(obj is ChatUser)
            {
                var chatUser = (ChatUser)obj;
                if(chatUser.UserId == UserId)
                    return true;

            }
            return false;
        }
    }
    
}
