using ClassifiedAds.Infrastructure.Azure.Blob;
using ClassifiedAds.Infrastructure.Logging;
using Newtonsoft.Json;
using Spl.Crm.SaleOrder.Modules.MasterData.Models;

namespace Spl.Crm.SaleOrder.Modules.MasterData.Service;

public class MasterDataService  :IMasterDataService
{

    private readonly IConfiguration _configuration;
    private readonly IBlobStorageUtils _blobStorageUtils;
    private readonly IAppLogger _logger;
    private static Dictionary<string, CountryData>? _countryDatas ;
    private static Dictionary<string, string>? _projectAssetTypeDatas ;
    private static Dictionary<string, string>? _unitAssetTypeDatas ;
    private static Dictionary<string, string>? _unitStatusTypeDatas ;
    
    public MasterDataService(IConfiguration configuration, IBlobStorageUtils blobStorageUtils, IAppLogger logger)
    {
        _configuration = configuration;
        _blobStorageUtils = blobStorageUtils;
        _logger = logger;
    }

    public void InitialMasterData()
    {
        _logger.Info("InitialData Master Data Daily");

 
        //Master Country
        _countryDatas = JsonConvert.DeserializeObject<IEnumerable<CountryData>>(GetContentResultFromBlob(_configuration["BlobStorage:ContainerName"], _configuration["BlobStorage:MasterFileName:CountryCode"]))
            .ToDictionary(x => x.code, x => x);
        
        //Master ProjectAsset
        _projectAssetTypeDatas = JsonConvert.DeserializeObject<Dictionary<string, string>>(GetContentResultFromBlob(_configuration["BlobStorage:ContainerName"], _configuration["BlobStorage:MasterFileName:ProjectAssetType"]));
        
        //Master UnitAsset
        _unitAssetTypeDatas = JsonConvert.DeserializeObject<Dictionary<string, string>>(GetContentResultFromBlob(_configuration["BlobStorage:ContainerName"], _configuration["BlobStorage:MasterFileName:UnitAssetType"]));
        
        //Master UnitStatus
        _unitStatusTypeDatas = JsonConvert.DeserializeObject<Dictionary<string, string>>(GetContentResultFromBlob(_configuration["BlobStorage:ContainerName"], _configuration["BlobStorage:MasterFileName:UnitStatusType"]));
        
    }
    
    //# Method Schedule Run daily day  00 00 * * *
    public void ResetMasterData()
    { 
        try
        {
            InitialMasterData();
        }
        catch (Exception e)
        {
            _logger.Error("Schedule Clear and InitialData Master Data is fail");
        }
        
    }

    private string? GetContentResultFromBlob(string containerName, string sourceFileName)
    {
       return _blobStorageUtils.GetContentJsonFromBlob(containerName, sourceFileName);
    }

    public void CheckData()
    {
        Console.WriteLine("Check _unitAssetTypeDatas Master Data"+_unitAssetTypeDatas.Count);
        
        Console.WriteLine("Check  _unitStatusTypeDatas Master Data"+_unitStatusTypeDatas.Count);
        
    }
}