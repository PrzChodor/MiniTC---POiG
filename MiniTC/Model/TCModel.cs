using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;

namespace MiniTC.Model
{
    class TCModel
    {
        public string[] Drives { get; set; }
        public List<string>[] Directories { get; set; }
        public List<string>[] Files { get; set; }
        public int [] Drive { get; set; }
        public string [] CurrentPath { get; set; }

        public TCModel()
        {
            Drives = Directory.GetLogicalDrives();
            foreach (var d in Drives)
                if (!new DriveInfo(d).IsReady)
                    Drives = Drives.Where(val => val != d).ToArray();
            Directories = new List<string>[2];
            Files = new List<string>[2];
            Drive = new int[2];
            CurrentPath = new string[2];
        }

        public void DirectoryChanged(string path,int panel)
        {
            CurrentPath[panel] = path;
            Directories[panel] = new List<string>();

            if (CurrentPath[panel].Substring(Path.GetPathRoot(CurrentPath[panel]).Length).Length != 0)
                Directories[panel].Add("..");

            foreach (var d in Directory.GetDirectories(CurrentPath[panel]))
            {
                var dirName = new DirectoryInfo(d).Name;
                Directories[panel].Add("<D>" + dirName);
            }

            Files[panel] = new List<string>();

            foreach (var f in Directory.GetFiles(CurrentPath[panel]))
            {
                var fileName = new FileInfo(f).Name;
                Directories[panel].Add(fileName);
            }
        }

        public void CopyFile(string source, string destination)
        {
            if (File.Exists(destination))
            {
                MessageBoxResult dialogResult = MessageBox.Show("The file: " + destination + " already exist!\nDo you want to overwrite it?", "File Exist", MessageBoxButton.YesNo);
                if (dialogResult == MessageBoxResult.Yes)
                {
                    try
                    {
                        File.Copy(source, destination, true);
                    }
                    catch (FileNotFoundException error)
                    {
                        MessageBox.Show(error.Message);
                    }
                }
            }
            else 
            {
                try
                {
                    File.Copy(source, destination);
                }
                catch (FileNotFoundException error)
                {
                    MessageBox.Show(error.Message);
                }
            }
        }
    }
}
