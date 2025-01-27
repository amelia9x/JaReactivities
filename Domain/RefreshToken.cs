namespace Domain
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public AppUser AppUser { get; set; }
        public string Token { get; set; }
        public DateTime Expires { get; set; } = DateTime.UtcNow.AddDays(7);
        public bool IsExpire => DateTime.UtcNow >= Expires;
        public DateTime? Revokes { get; set; }
        public bool IsActive => Revokes == null && !IsExpire;
    }
}