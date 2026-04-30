namespace RMS.Domain.Entities
{
    public class UserProvider
    {
        public int Id { get; set; }

        public string Provider { get; set; }
        public string ProviderId { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }
    }
}
