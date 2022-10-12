namespace Domain.Security
{
    public class TokenConfiguration
    {
        public String Audience { get; set; }
        public String Issuer { get; set; }
        public int Seconds { get; set; }

    }
}
