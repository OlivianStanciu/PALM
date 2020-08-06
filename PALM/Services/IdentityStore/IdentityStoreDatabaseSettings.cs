using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PALM.Services.IdentityStore
{
    public class IdentityStoreDatabaseSettings : IIdentityStoreDatabaseSettings
    {
        public string UsersCollectionName { get; set; }
        public string RolesCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }

    public interface IIdentityStoreDatabaseSettings
    {
        string UsersCollectionName { get; set; }
        string RolesCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}
