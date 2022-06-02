using Microsoft.AspNetCore.Identity;

namespace ChatAPP.Models
{
    public class User : IdentityUser
    {
        public override string UserName { get; set; }

        public ICollection<ChatUser> Chats { get; set; }
        public User() : base()
        {
            Chats = new List<ChatUser>();
        }
        public override bool Equals(object? obj)
        {
            if (obj is User)
            {
                var user = (User)obj;
                if (user.Id == Id)
                    return true;

            }
            return false;
        }
    }
}
