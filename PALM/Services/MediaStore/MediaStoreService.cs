using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PALM.MongoDb;
using PALM.Models;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace PALM.Services.MediaStore
{
    public class MediaStoreService
    {
        //private readonly string _contentPath;

        public PhotobookManager Photobooks { get; }
        public CanvasPictureManager CanvasPictures { get; }

        public MediaStoreService(IMediaStoreDatabaseSettings settings, IWebHostEnvironment env)
        {
            //Path.Combine(_contentPath);

            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName, new MongoDatabaseSettings() { });

            Photobooks = new PhotobookManager(
                database.GetCollection<Photobook>(settings.PhotobooksCollectionName),
                env);
            CanvasPictures = new CanvasPictureManager(
                database.GetCollection<CanvasPicture>(settings.CanvasCollectionName),
                env);

        }
    }
}
