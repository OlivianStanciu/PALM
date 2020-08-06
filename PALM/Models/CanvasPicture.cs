using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PALM.Models
{
    [Serializable]
    public class CanvasPicture : IMediaItem
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Path { get; set; }
        public string CoverPath { get; set; }
        public string PhotoPath { get; set; }
        [Required]
        public string Title { get; set; }
        [Display(Name = "Created at time")]
        public DateTime CreatedAtTime { get; set; }
        public int Priority { get; set; }
    }
}
