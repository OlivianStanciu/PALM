using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PALM.Data
{
    public static class FileManager
    {

        public static List<string> GetFiles(string directoryPath, string searchPattern = "*")
        {
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            List<string> result = new List<string>();
            foreach (var item in Directory.GetFiles(directoryPath, searchPattern).ToList<string>())
            {
                result.Add(Regex.Split(item, "wwwroot")[1]);
            }
            return result.ToList<string>();
        }


        public static List<string> GetDirectories(string directoryPath, string searchPattern = "*")
        {
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            List<string> result = new List<string>();
            foreach (var item in Directory.GetDirectories(directoryPath, searchPattern, new EnumerationOptions { MatchCasing = MatchCasing.CaseInsensitive }).ToList<string>())
            {
                result.Add(Regex.Split(item, "wwwroot")[1]);
            }
            return result.ToList<string>();
        }


        public static DirectoryStructureContainer CreateDirectoryStructure(DirectoryStructureContainer dirStructure, bool overrideIfExistent)
        {
            if (Directory.Exists(dirStructure.AbsolutePath))
            {
                if (overrideIfExistent)
                {
                    Directory.Delete(dirStructure.AbsolutePath, true);
                }
                else
                {
                    return null;
                }
            }

            try
            {
                Directory.CreateDirectory(dirStructure.AbsolutePath);
                if (dirStructure.Files?.Count() > 0)
                {
                    foreach (var file in dirStructure.Files)
                    {
                        if (!File.Exists(file.FileName)) 

                        using (var fileStream = File.Create(file.FileName))
                        {
                            file.ContentStream.Seek(0, SeekOrigin.Begin);
                            file.ContentStream.CopyTo(fileStream);
                        }
                    }
                }
                if (dirStructure.SubDirectories?.Count() > 0)
                {
                    foreach (var dir in dirStructure.SubDirectories)
                    {
                        CreateDirectoryStructure(dir, true);
                    }
                }

                return dirStructure;
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        public static void DeleteDirectoryStructure(string directoryPath)
        {
            if (Directory.Exists(directoryPath))
            {
                Directory.Delete(directoryPath, true);
            }
        }

    }
}
