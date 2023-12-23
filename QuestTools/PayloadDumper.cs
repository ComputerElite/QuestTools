using System;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.IO.Compression;
using System.Linq;
using ComputerUtils.ConsoleUi;
using ComputerUtils.FileManaging;
using ComputerUtils.Logging;
using ICSharpCode.SharpZipLib.BZip2;
using ProtoBuf;
using SevenZip.Compression.LZMA;

namespace QuestTools;

class PayloadDumper
{
    public static void AskInput()
    {
        // Check if 7z is installed
        try
        {
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = "7z",
                Arguments = "",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };
            Process.Start(startInfo).WaitForExit();
        } catch (Exception e)
        {
            Logger.Log("7z is not installed. Please install it and try again. https://www.7-zip.org/download.html");
            return;
        }
        string payloadLocation = ConsoleUiController.QuestionString("Payload location: ");
        DumpPayload(payloadLocation, PublicStaticVars.settings._resultDirectory + "img");
        ExtractPartition(PublicStaticVars.settings._resultDirectory + "img" + Path.DirectorySeparatorChar + "system.img",
            PublicStaticVars.settings._resultDirectory + "extracted" + Path.DirectorySeparatorChar + "system");
        
        // Find all apk files in the extracted system partition
        List<string> allApkFiles = GetAllApkFilesRecursive(PublicStaticVars.settings._resultDirectory + "extracted" + Path.DirectorySeparatorChar + "system");
        PublicStaticVars.settings.packageId = "tmpGetPackageId";
        FileManager.CreateDirectoryIfNotExisting(PublicStaticVars.settings._resultDirectory + "apps");
        int i = 0;
        foreach (string apk in allApkFiles)
        {
            Logger.Log("Processing " + apk);
            FileManager.RecreateDirectoryIfExisting(PublicStaticVars.settings.resultDirectory);
            string apkLoc = PublicStaticVars.settings.resultDirectory + "app.apk";
            File.Copy(apk, apkLoc);
            HorizonDecompiler.DepackApk(apkLoc);
            // Extract package id from android manifest
            string manifestLocation = PublicStaticVars.extracedApkLocation + "AndroidManifest.xml";
            string packageId = "UnknownPackageName-" + i;
            if (!File.Exists(manifestLocation))
            {
                i++;
            }
            else
            {
                string manifest = File.ReadAllText(manifestLocation);
                packageId = manifest.Split("package=\"")[1].Split("\"")[0];
            }
            Logger.Log("Apk is " + packageId);
            // Move apk
            int ii = 0;
            string tmpPackageId = packageId;
            while (Directory.Exists(PublicStaticVars.settings._resultDirectory + "apps" + Path.DirectorySeparatorChar + tmpPackageId))
            {
                tmpPackageId = packageId + "-" + ii;
                ii++;
            }
            packageId = tmpPackageId;
            string correctDir = PublicStaticVars.settings._resultDirectory + "apps" + Path.DirectorySeparatorChar +
                                packageId;
            Directory.Move(PublicStaticVars.settings.resultDirectory, correctDir);
            string correctApk = correctDir + Path.DirectorySeparatorChar + "app.apk";
            HorizonDecompiler.DecompileAPK(correctApk, packageId);
        }
    }

    public static List<string> GetAllApkFilesRecursive(string directory)
    {
        if (!Directory.Exists(directory)) return new List<string>();
        List<string> inSubDir = new List<string>();
        foreach (string dir in Directory.GetDirectories(directory))
        {
            inSubDir.AddRange(GetAllApkFilesRecursive(dir));
        }
        foreach (string file in Directory.GetFiles(directory))
        {
            if (file.EndsWith(".apk")) inSubDir.Add(file);
        }
        return inSubDir;
    }

    public static void ExtractPartition(string imageFile, string extractedDirectory)
    {
        RunProcessRedirect("7z", "x \"" + imageFile + "\" -o\"" + extractedDirectory + "\"");
    }

    public static void DumpPayload(string payloadLocation, string outputDirectory)
    {
        Logger.Log("Installing dependencies with pip");
        RunPython("", "-m pip install -r \"" + PublicStaticVars.payloadDumperLocation + "requirements.txt\"");
        Logger.Log("Dumping payload");
        RunPython(PublicStaticVars.payloadDumperLocation + "payload_dumper.py", "\"" + payloadLocation + "\" --out \"" + outputDirectory + "\"");
    }

    public static void RunPython(string script, string args)
    {
        // Run python script
        string pythonArguments = (script != "" ? "\"" + script + "\" " : "") + args;
        RunProcessRedirect("python.exe", pythonArguments);
        
    }

    public static void RunProcessRedirect(string file, string args)
    {
        ProcessStartInfo startInfo = new ProcessStartInfo
        {
            FileName = file,
            Arguments = args,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };
        Process process = new Process
        {
            StartInfo = startInfo
        };
        process.Start();
        while (!process.StandardOutput.EndOfStream)
        {
            Console.Write((char)process.StandardOutput.Read());
        }
        process.WaitForExit();
    }
}