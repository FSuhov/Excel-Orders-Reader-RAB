using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rab_Forms
{
    public class FilePicker
    {
        public DirectoryInfo folderToRead;

        public FilePicker(string path)
        {
            folderToRead = new DirectoryInfo(path);
        }

        public List<string> ReadDirectory()
        {
            List<string> listOfFiles = new List<string>();
            try
            {
                foreach (FileSystemInfo inf in folderToRead.GetFileSystemInfos())
                {
                    if (inf.Extension == ".xlsx") listOfFiles.Add(inf.FullName);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Something went wrong: '{0}'", e);
            }
            //debug
            foreach (String elem in listOfFiles)
            {
                Console.WriteLine(elem);
            }
            return listOfFiles;
        }
    }
}
