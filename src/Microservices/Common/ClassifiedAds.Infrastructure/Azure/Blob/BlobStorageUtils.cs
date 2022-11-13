#nullable enable
using System;
using ClassifiedAds.CrossCuttingConcerns.Constants;
using ClassifiedAds.CrossCuttingConcerns.Exceptions;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace ClassifiedAds.Infrastructure.Azure.Blob;

public interface IBlobStorageUtils
{
    public string? GetContentJsonFromBlob(string containerName, string sourceName);
}

public class BlobStorageUtils : IBlobStorageUtils
{
    private readonly IConfiguration _configuration;

    public BlobStorageUtils(IConfiguration configuration)
    {
        this._configuration = configuration;
    }

    public string? GetContentJsonFromBlob(string containerName, string sourceName)
    {
        try
        {
            CloudBlobContainer cloudBlobContainer = ConnectAzureBlob(_configuration["BlobStorage:DefaultConnection"], containerName);
            cloudBlobContainer.CreateAsync();
            BlobContainerPermissions permissions = new BlobContainerPermissions
                {
                    PublicAccess = BlobContainerPublicAccessType.Blob
                };
            cloudBlobContainer.SetPermissionsAsync(permissions);

            CloudBlockBlob blockBlob = cloudBlobContainer.GetBlockBlobReference(sourceName);

            blockBlob.FetchAttributesAsync();
            var created = blockBlob.Properties.Created;
            var content = blockBlob.DownloadTextAsync();
            blockBlob.ExistsAsync();
            cloudBlobContainer.ExistsAsync();
            return content.Result;
        }
        catch (StorageException e)
        {
            throw new ExternalErrorException(ResponseData.THIRD_PARTY_DATA_NOT_FOUND.Code,
            ResponseData.THIRD_PARTY_DATA_NOT_FOUND.Message,
            ResponseData.THIRD_PARTY_DATA_NOT_FOUND.HttpStatus,
            e,
            "BLOB");
        }
        catch (ExternalErrorException ex)
        {
            throw ex;
        }
        catch (Exception e)
        {
            throw new ExternalErrorException(ResponseData.THIRD_PARTY_SYSTEM_ERROR.Code,
                ResponseData.THIRD_PARTY_SYSTEM_ERROR.Message,
                ResponseData.THIRD_PARTY_SYSTEM_ERROR.HttpStatus,
                e,
                "BLOB");
        }
    }

    private CloudBlobContainer ConnectAzureBlob(string connectionString, string containerName)
    {
        CloudStorageAccount storageAccount = null;
        CloudBlobClient cloudBlobClient;
        if (CloudStorageAccount.TryParse(connectionString, out storageAccount))
        {
            cloudBlobClient = storageAccount.CreateCloudBlobClient();
            return cloudBlobClient.GetContainerReference(containerName);
        }
        else
        {
            throw new ExternalErrorException(ResponseData.THIRD_PARTY_CONNECT_ERROR.Code,
                ResponseData.THIRD_PARTY_CONNECT_ERROR.Message,
                ResponseData.THIRD_PARTY_CONNECT_ERROR.HttpStatus,
                null,
                "BLOB");
        }
    }
}
