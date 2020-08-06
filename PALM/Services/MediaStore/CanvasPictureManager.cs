using Microsoft.AspNetCore.Hosting;
using MongoDB.Driver;
using PALM.Data;
using PALM.Models;
using PALM.MongoDb;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PALM.Services.MediaStore
{
    public class CanvasPictureManager : MongoDbItemManager<CanvasPicture>
    {
        private readonly string _contentPath;
        private readonly IWebHostEnvironment _env;
        private readonly IMongoCollection<CanvasPicture> _collection;
        public CanvasPictureManager(IMongoCollection<CanvasPicture> collection, IWebHostEnvironment env) : base(collection)
        {
            _env = env;
            _contentPath = Path.Combine(env.WebRootPath, "img", "content", "canvas");
            _collection = collection;
        }


        public override CanvasPicture Create(CanvasPicture item)
        {
            return base.Create(item);
        }

        public CanvasPicture Create(CanvasPicture item, MediaStreamContainer cover, MediaStreamContainer canvasPhoto)
        {
            var existingPhotobooks = FileManager.GetDirectories(_contentPath, "Canvas*");
            cover.FileName = "cover." + cover.FileName.Split(".").LastOrDefault();
            canvasPhoto.FileName = "photo." + canvasPhoto.FileName.Split(".").LastOrDefault();

            DirectoryStructureContainer dir = new DirectoryStructureContainer(
                Path.Combine(_contentPath, $"Canvas{DateTime.Now.Ticks.ToString("x")}"),
                new List<MediaStreamContainer>() { cover, canvasPhoto });

            item.Path = dir.AbsolutePath;
            item.CoverPath = dir.Files.Where(f => f.FileName.Contains("cover")).FirstOrDefault().FileName.Replace(_env.WebRootPath, string.Empty);
            item.PhotoPath = dir.Files.Where(f => f.FileName.Contains("photo")).FirstOrDefault().FileName.Replace(_env.WebRootPath, string.Empty);
            item.CreatedAtTime = DateTime.UtcNow;
            item.Priority = existingPhotobooks.Count + 1;

            FileManager.CreateDirectoryStructure(dir, true);

            //Add it to database
            base.Create(item);

            return item;
        }


        public override void Update(string id, CanvasPicture item)
        {
            base.Update(id, item);
        }

        public override void Remove(CanvasPicture item)
        {
            FileManager.DeleteDirectoryStructure(item.Path);
            base.Remove(item);
        }

        public override void Remove(string id)
        {
            FileManager.DeleteDirectoryStructure(this.Get(id).Path);
            base.Remove(id);
        }
    }
}
