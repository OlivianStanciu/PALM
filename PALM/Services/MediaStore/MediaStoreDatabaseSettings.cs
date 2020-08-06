using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PALM.Services.MediaStore
{
    public class MediaStoreDatabaseSettings : IMediaStoreDatabaseSettings
    {
        public string PhotobooksCollectionName { get; set; }
        public string CanvasCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }

    public interface IMediaStoreDatabaseSettings
    {
        string PhotobooksCollectionName { get; set; }
        string CanvasCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}
