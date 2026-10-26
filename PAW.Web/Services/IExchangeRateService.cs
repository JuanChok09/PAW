namespace PAW.Web.Services
{
    public interface IExchangeRateService
    {
        Task<string> GetExchangeRateAsync();
    }
}