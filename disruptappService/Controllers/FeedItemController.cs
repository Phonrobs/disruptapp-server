using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;
using Microsoft.Azure.Mobile.Server;
using disruptappService.DataObjects;
using disruptappService.Models;
using System;
using Microsoft.WindowsAzure.Storage;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage.Blob;

namespace disruptappService.Controllers
{
    public class FeedItemController : TableController<FeedItem>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            disruptappContext context = new disruptappContext();
            DomainManager = new EntityDomainManager<FeedItem>(context, Request);
        }

        // GET tables/FeedItem
        public IQueryable<FeedItem> GetAllFeedItems()
        {
            return Query();
        }

        // GET tables/FeedItem/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<FeedItem> GetFeedItem(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/FeedItem/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<FeedItem> PatchFeedItem(string id, Delta<FeedItem> patch)
        {
            return UpdateAsync(id, patch);
        }

        // POST tables/FeedItem
        public async Task<IHttpActionResult> PostFeedItem(FeedItem item)
        {
            item.postDate = DateTime.Now;

            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageAccount"));
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference("disrupt-pictures");
            
            SharedAccessBlobPolicy sasConstraints = new SharedAccessBlobPolicy();
            sasConstraints.SharedAccessExpiryTime = DateTime.UtcNow.AddHours(24);
            sasConstraints.Permissions = SharedAccessBlobPermissions.Write | SharedAccessBlobPermissions.List;

            item.sasToken = container.GetSharedAccessSignature(sasConstraints);
            item.picture = container.Uri + "/" + Guid.NewGuid().ToString();

            FeedItem current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/FeedItem/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteFeedItem(string id)
        {
            return DeleteAsync(id);
        }
    }
}