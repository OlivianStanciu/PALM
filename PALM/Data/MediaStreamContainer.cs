using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PALM.Data
{
    public class MediaStreamContainer : IDisposable
    {
        public Stream ContentStream { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }

        public MediaStreamContainer(Stream stream)
        {
            ContentStream = stream;
            ContentStream.Seek(0, SeekOrigin.Begin);
        }



        public void Dispose()
        {
            ContentStream.Dispose();
        }
    }
}
