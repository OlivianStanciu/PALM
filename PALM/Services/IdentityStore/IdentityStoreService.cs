using MongoDB.Driver;
using PALM.Identity.Models;
using PALM.MongoDb;
using PALM.Services.MediaStore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PALM.Services.IdentityStore
{
    public class IdentityStoreService
    {
        public MongoDbItemManager<ApplicationUser> Users { get; set; }
        public MongoDbItemManager<ApplicationRole> Roles { get; set; }
        public IdentityStoreService(IIdentityStoreDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName, new MongoDatabaseSettings() { });

            Users = new MongoDbItemManager<ApplicationUser>(
                database.GetCollection<ApplicationUser>(settings.UsersCollectionName));
            Roles = new MongoDbItemManager<ApplicationRole>(
                database.GetCollection<ApplicationRole>(settings.RolesCollectionName));
        }
    }
}
