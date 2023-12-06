using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Specialized;
using Forum.BLL.Services.Communication;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forum.BLL.Services
{
    //public class BlobHostService : IImageHostService
    //{
    //    private readonly BlobServiceClient _blobServiceClient;

    //    public BlobHostService(BlobServiceClient blobServiceClient)
    //    {
    //        _blobServiceClient = blobServiceClient;
    //    }
    //    public async Task<ImageHostResponse> SaveImageAsync(string base64string)
    //    {
    //        var blobContainerService = _blobServiceClient.GetBlobContainerClient("images");
    //        var bytes = Convert.FromBase64String(base64string);
    //        var block = blobContainerService.GetBlockBlobClient("image_" + Guid.NewGuid().ToString() + ".jpg");

    //        using (var stream = new MemoryStream(bytes))
    //        {
    //            await block.UploadAsync(stream);
    //        }
    //        return new ImageHostResponse { data = new Data { display_url = block.Uri.AbsoluteUri }, success = true };
    //    }
    //}
}
