using System.Collections.Generic;

namespace ClassifiedAds.Infrastructure.JWT;

public class TokenInfo
{
    public string user_id { get; set; }
    public string username { get; set; }
    public string firstname { get; set; }
    public string lastname { get; set; }
    public string email { get; set; }
    public IEnumerable<string> role { get; set; }
    public bool is_refresh_token { get; set; }
    public string? payload { get; set; }
}
