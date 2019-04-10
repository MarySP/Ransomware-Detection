using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Security.Permissions;
using Manger;


public class ConsoleApp1
{

    public static object Win32Processes { get; private set; }
    public static string filename = @"C:\Users\CSC72\Desktop\Master";

    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    static void Main()
    {
        
        
        using (FileSystemWatcher watcher = new FileSystemWatcher())
        {
            watcher.Path = filename;

            // Watch for changes in LastAccess and LastWrite times, and
            // the renaming of files or directories.
            watcher.NotifyFilter = NotifyFilters.LastAccess
                                 | NotifyFilters.LastWrite
                                 | NotifyFilters.FileName
                                 | NotifyFilters.DirectoryName;

            // Only watch text files.
            watcher.Filter = "*.*";

            // Add event handlers.
            watcher.Changed += OnChanged;
            watcher.Created += OnChanged;
            watcher.Deleted += OnChanged;
            watcher.Renamed += OnRenamed;

            // Begin watching.
            watcher.EnableRaisingEvents = true;
            string[] lines = { "First line", "Second line", "Third line" };
            System.IO.File.WriteAllLines(filename+"//test.txt", lines);
            Console.ReadKey();
        }
    }
    private static void OnChanged(object source, FileSystemEventArgs e)
    {
        List<Process> processes = FileUtil.GetProcess(e.FullPath);
        foreach (Process i in processes)
        {
            i.Kill();
            Console.WriteLine(i.ProcessName);
        }
        Console.WriteLine($"File: {e.FullPath} {e.ChangeType}");
    }

    private static void OnRenamed(object source, RenamedEventArgs e)
    {
        List<Process> processes = FileUtil.GetProcess(e.FullPath);
        foreach (Process i in processes)
        {
            i.Kill();
            Console.WriteLine(i.ProcessName);
        }
        Console.WriteLine($"File: {e.OldFullPath} renamed to {e.FullPath}");
    }
    

 
        
    
        

  
}
