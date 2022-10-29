namespace ClassifiedAds.Infrastructure.JWT;

public class TokenInfo
{
    public string user_id { get; set; }
    public string is_refresh_token { get; set; }

    public TokenStatus TokenStatus;

}
