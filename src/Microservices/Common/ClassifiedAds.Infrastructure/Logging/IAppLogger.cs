namespace ClassifiedAds.Infrastructure.Logging
{
    public interface IAppLogger
    {
        void Debug(string message, string className = null);
        void Info(string message, string className = null);
        void Error(string message, string className = null);
        void Warn(string message, string className = null);

    }
}
