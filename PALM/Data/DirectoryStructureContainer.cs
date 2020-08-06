using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PALM.Data
{
    public class DirectoryStructureContainer
    {
        public readonly string AbsolutePath;
        public List<MediaStreamContainer> Files { get; set; } = new List<MediaStreamContainer>();
        public List<DirectoryStructureContainer> SubDirectories { get; set; } = new List<DirectoryStructureContainer>();

        public DirectoryStructureContainer(string absolutePath)
        {
            if (absolutePath.EndsWith(Path.DirectorySeparatorChar)) 
            {
                absolutePath = absolutePath.Substring(0, absolutePath.Length - 1);
            }
            this.AbsolutePath = absolutePath;
        }

        public DirectoryStructureContainer(string absolutePath, List<MediaStreamContainer> files) : this(absolutePath)
        {
            if (files?.Count > 0)
            {
                this.Files ??= new List<MediaStreamContainer>();
                foreach (var file in files)
                {
                    file.FileName = Path.Combine(this.AbsolutePath, GetRelativePath(file.FileName));
                    this.Files.Add(file);
                }
            }
        }

        public DirectoryStructureContainer(string absolutePath, List<MediaStreamContainer> files, List<DirectoryStructureContainer> subDirectories) : this(absolutePath, files)
        {
            if (subDirectories?.Count > 0)
            {
                this.SubDirectories ??= new List<DirectoryStructureContainer>();
                foreach (var subDir in subDirectories)
                {
                    this.SubDirectories.Add(new DirectoryStructureContainer($"{Path.Combine(this.AbsolutePath, GetRelativePath(subDir.AbsolutePath))}", subDir.Files, subDir.SubDirectories));
                }
            }
        }
        

        public DirectoryStructureContainer GetDirectory(string directoryName)
        {
            return this.SubDirectories.Where(s => s.AbsolutePath.Contains(directoryName, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
        }

        private string GetRelativePath(string absolutePath)
        {
            if (absolutePath.Contains(Path.DirectorySeparatorChar))
            {
                var temp = absolutePath.Split(Path.DirectorySeparatorChar);
                absolutePath = temp[temp.Length - 1];
            }
            return absolutePath;
        }
    }
}
