using Cronos;
using Sgbj.Cron;

namespace Spl.Crm.SaleOrder.Modules.MasterData.Service;

public class MasterDataScheduleService : BackgroundService
{
    private readonly IServiceProvider _service;
    private readonly IConfiguration _configuration;

    public MasterDataScheduleService(IServiceProvider service, IConfiguration configuration)
    {
        _service = service;
        _configuration = configuration;
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        
        using var timer = new CronTimer(CronExpression.Parse(_configuration["BlobStorage:DurationTrigger"], CronFormat.IncludeSeconds),TimeZoneInfo.Local);
        while (await timer.WaitForNextTickAsync(stoppingToken))
        {
            using ( var scope = _service.CreateScope() )
            {
                var calStatRepo = scope.ServiceProvider.GetRequiredService<IMasterDataService>();
                calStatRepo.ResetMasterData();
            }
        }
        
    }
}