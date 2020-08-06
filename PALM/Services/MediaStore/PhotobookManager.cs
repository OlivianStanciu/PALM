using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PALM.MongoDb;
using PALM.Models;
using MongoDB.Driver;
using System.IO;
using Microsoft.AspNetCore.Http;
using PALM.Data;
using PALM.ImageOperations;
using Microsoft.AspNetCore.Hosting;

namespace PALM.Services.MediaStore
{
    public class PhotobookManager : MongoDbItemManager<Photobook>
    {
        private readonly string _contentPath;
        private readonly IWebHostEnvironment _env;
        private readonly IMongoCollection<Photobook> _collection;
        public PhotobookManager(IMongoCollection<Photobook> collection, IWebHostEnvironment env) : base(collection)
        {
            _env = env;
            _contentPath = Path.Combine(env.WebRootPath, "img", "content", "photobooks");
            _collection = collection;
        }

        public override Photobook Create(Photobook item) => base.Create(item);

        public Photobook Create(Photobook item, MediaStreamContainer cover, IEnumerable<MediaStreamContainer> files)
        {

            var existingPhotobooks = FileManager.GetDirectories(_contentPath, "Photobook*");
            cover.FileName = "cover." + cover.FileName.Split(".").LastOrDefault();

            DirectoryStructureContainer dir = new DirectoryStructureContainer(
                Path.Combine(_contentPath, $"Photobook{DateTime.Now.Ticks.ToString("x")}"), 
                new List<MediaStreamContainer>() { cover }, 
                new List<DirectoryStructureContainer>()
                {
                    new DirectoryStructureContainer("Photos")
                });


            item.Path = dir.AbsolutePath;
            item.CoverPath = dir.Files[0].FileName.Replace(_env.WebRootPath, string.Empty);
            item.CreatedAtTime = DateTime.UtcNow;
            item.Photos ??= new List<PhotobookPhoto>();
            item.Priority = existingPhotobooks.Count + 1;

            var photoDir = dir.GetDirectory("Photos");
            int photoCounter = 1;
            foreach (var file in files)
            {
                var thumbFile = new MediaStreamContainer(ImageProcessor.ResizeImage(file.ContentStream, ThumbnailDimensionProvider.Width, ThumbnailDimensionProvider.Height))
                {
                    ContentType = file.ContentType,
                    FileName = $"{file.FileName.Split(".")[^2]}_thumb.{file.FileName.Split(".")[^1]}"
                };
                photoDir.SubDirectories.Add(
                    new DirectoryStructureContainer(Path.Combine(photoDir.AbsolutePath, $"Photo{photoCounter}"),
                    new List<MediaStreamContainer>() { file, thumbFile }));
                photoCounter++;

                item.Photos.Add(
                    new PhotobookPhoto()
                    {
                        FullResolutionPath = file.FileName.Replace(_env.WebRootPath, string.Empty),
                        ThumbnailPath = thumbFile.FileName.Replace(_env.WebRootPath, string.Empty)
                    });
            }

            FileManager.CreateDirectoryStructure(dir, true);

            //Add it to database
            base.Create(item);

            return item;

        }

        public override void Update(string id, Photobook item)
        {
            base.Update(id, item);
        }

        public override void Remove(Photobook item)
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
