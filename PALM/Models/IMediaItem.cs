using PALM.MongoDb;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PALM.Models
{
    public class MediaItemAttribute
    {
        [Required]
        public string Key { get; set; }
        [Required]
        public string Value { get; set; }
    }
    public interface IMediaItem : IMongoDbItem
    {
        public string Path { get; set; }
        public string Title { get; set; }
        //public string SubTitle { get; set; }

        //public string Description { get; set; }
        //public List<MediaItemAttribute> Attributes { get; set; }

        public DateTime CreatedAtTime { get; set; }

    }
}
