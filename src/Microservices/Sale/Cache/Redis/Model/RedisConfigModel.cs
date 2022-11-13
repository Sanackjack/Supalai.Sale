namespace Spl.Crm.SaleOrder.Cache.Redis.Model
{
    public class RedisConfigModel
    {

        //private string MasterConfigTimeExpire = "24:00:00";
        private string masterConfigTimeExpire = "480";
        private string masterConfgiSlidingExpire = "480";
        private string userTimeExpire = "480";
        private string userSlidingExpire = "480";

        public string MasterConfigTimeExpire { get => masterConfigTimeExpire; set => masterConfigTimeExpire = value; }

        public string MasterConfigSlidingExpire { get => masterConfgiSlidingExpire; set => masterConfgiSlidingExpire = value; }

        public string UserTimeExpire { get => userTimeExpire; set => userTimeExpire = value; }

        public string UserSlidingExpire { get => userSlidingExpire; set => userSlidingExpire = value; }

    }
}
