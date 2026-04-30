namespace RMS.Domain.Entities
{
    public class ResetSessionToken
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public string TokenHash { get; set; }

        public DateTime ExpiryDate { get; set; }

        public bool IsUsed { get; set; }
    }
}
