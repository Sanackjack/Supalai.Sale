namespace Spl.Crm.SaleOrder.Cache.Redis.Model
{
    public class RedisConfigModel
    {
        private string defaultTimeExpire = "480";
        private string defaultSlidingExpire = "60";
        private string masterConfigAbsoluteExpiration = "24:00:00";
        private string userTimeExpire = "480";
        private string userSlidingExpire = "480";

        public string DefaultTimeExpire { get => defaultTimeExpire; set => defaultTimeExpire = value; }

        public string DefaultSlidingExpire { get => defaultSlidingExpire; set => defaultSlidingExpire = value; }

        public string MasterConfigAbsoluteExpiration { get => masterConfigAbsoluteExpiration; set => masterConfigAbsoluteExpiration = value; }

        public string UserTimeExpire { get => userTimeExpire; set => userTimeExpire = value; }

        public string UserSlidingExpire { get => userSlidingExpire; set => userSlidingExpire = value; }

    }
}
