using System.IO;
using UnityEngine;

public class ReplaceFolder : MonoBehaviour
{
    public string folderToDeletePath = "E:/Projekti/Webelinx/Git/Rooms and Exits/Assets/Tacic - Unity Tools";
    public string backupToReplaceWithPath = "E:/GoogleDriveTacic/Nikola fajlovi/Resources/Tools and Scripts/Git ignorisani folderi/Tacic - Unity Tools";

    void OnValidate()
    {
        if (string.IsNullOrEmpty(folderToDeletePath))
        {
            folderToDeletePath = "E:/Projekti/Webelinx/Git/Rooms and Exits/Assets/Tacic - Unity Tools";
        }
        if (string.IsNullOrEmpty(backupToReplaceWithPath))
        {
            backupToReplaceWithPath = "E:/GoogleDriveTacic/Nikola fajlovi/Resources/Tools and Scripts/Git ignorisani folderi/Tacic - Unity Tools";
        }
    }

    void Start()
    {
        if (Directory.Exists(folderToDeletePath))
        {
            Directory.Delete(folderToDeletePath, true);
            Debug.Log("Folder deleted.");
        }
        else
        {
            Debug.Log("Folder does not exist.");
            return;
        }

        if (Directory.Exists(backupToReplaceWithPath))
        {
            DirectoryCopy(backupToReplaceWithPath, folderToDeletePath, true);
            Debug.Log("Backup folder copied to original folder.");
        }
        else
        {
            Debug.Log("Backup folder does not exist.");
        }
    }

    private static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
    {
        // Get the subdirectories for the specified directory.
        DirectoryInfo dir = new DirectoryInfo(sourceDirName);

        if (!dir.Exists)
        {
            throw new DirectoryNotFoundException("Source directory does not exist or could not be found: "
                + sourceDirName);
        }

        // If the destination directory doesn't exist, create it.
        if (!Directory.Exists(destDirName))
        {
            Directory.CreateDirectory(destDirName);
        }

        // Get the files in the directory and copy them to the new location.
        FileInfo[] files = dir.GetFiles();
        foreach (FileInfo file in files)
        {
            string temppath = Path.Combine(destDirName, file.Name);
            file.CopyTo(temppath, false);
        }

        // If copying subdirectories, copy them and their contents to new location.
        if (copySubDirs)
        {
            DirectoryInfo[] dirs = dir.GetDirectories();
            foreach (DirectoryInfo subdir in dirs)
            {
                string temppath = Path.Combine(destDirName, subdir.Name);
                DirectoryCopy(subdir.FullName, temppath, copySubDirs);
            }
        }
    }
}
