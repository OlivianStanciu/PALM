using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using PALM.MongoDb;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PALM.Models
{
    public class PhotobookPhoto
    {
        public string FullResolutionPath { get; set; }
        public string ThumbnailPath { get; set; }
    }

    [Serializable]
    public class Photobook : IMediaItem
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Path { get; set; }
        public List<PhotobookPhoto> Photos { get; set; } = new List<PhotobookPhoto>();
        public string CoverPath { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        [Display(Name="Cover Description")]
        public string CoverDescription { get; set; }
        [Required]
        public string SubTitle { get; set; }
        public List<MediaItemAttribute> Attributes { get; set; } = new List<MediaItemAttribute>();
        [Display(Name = "Created at time")]
        public DateTime CreatedAtTime { get; set; }
        public int Priority { get; set; }
    }
}
