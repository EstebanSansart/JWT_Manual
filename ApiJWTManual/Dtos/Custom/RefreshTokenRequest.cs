namespace ApiJWTManual.Dtos.Custom
{
    public class RefreshTokenRequest
    {
        public string ExpiredToken { get; set; }
        public string RefreshToken { get; set; }
    }
}