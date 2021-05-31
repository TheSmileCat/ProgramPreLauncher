using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

namespace ChobitsMCLauncher
{
    class CheckUpdateThread
    {
        private static int redoCount = 0;
        private static List<string> controlFiles;
        public static void Run()
        {
#if Other
            //Console.WriteLine("输入控制文件目的地址：");
            //string controltarget = Console.ReadLine();
            Console.WriteLine("输入原始目的地址(末尾有“/”)：");
            string rawtarget = Console.ReadLine();
#endif
            redo:
            controlFiles = new List<string>();
            if (redoCount > 1)
            {
                reInput:
                Console.WriteLine("\r\n\r\n程序发生了多次启动重试，确定要重试吗？\r\n[Y/Esc]重试\t\t[N/Enter]继续\t\t[C]退出");
                ConsoleKeyInfo info = Console.ReadKey();
                switch (info.Key)
                {
                    case ConsoleKey.N:
                    case ConsoleKey.Enter:
                        goto launch;
                    case ConsoleKey.C:
                        Environment.Exit(0);
                        break;
                    case ConsoleKey.Y:
                    case ConsoleKey.Escape:
                        break;
                    default:
                        goto reInput;
                }
            }
#if !Other
            //程序启动“块”
            if (Tools.HTTP.GetHttpStringData("http://chobitslive.live:3080/minecraft/updater/version.json", timeout: 2000) == null)
            {
                Console.WriteLine("网络错误，尝试直接启动程序");
                goto launch;
            }
#endif
            //程序更新“块”
            {
#if !Other
                string control_file_s = Tools.HTTP.GetHttpStringData("http://chobitslive.live:3080/minecraft/updater/control.json");
#else           
                string control_file_s = Tools.HTTP.GetHttpStringData(rawtarget + "control.json");
#endif
                if (control_file_s != null)
                {
                    try
                    {
                        JArray jArray = JsonConvert.DeserializeObject(control_file_s) as JArray;
                        string[] cfs = jArray.ToObject<string[]>();
                        controlFiles.AddRange(cfs);
                    }
                    catch { }
                }
                int filed = 0;
                int done = 0;
#if !Other
                string foldsRaw = Tools.HTTP.GetHttpStringData("http://chobitslive.live:3080/minecraft/updater/publish/folds.json", timeout: 30000);
#else
                string foldsRaw = Tools.HTTP.GetHttpStringData(rawtarget + "folds.json", timeout: 30000);
#endif
                string[] folds = null;
                if (foldsRaw == null)
                {
                    Console.WriteLine("网络错误，尝试直接启动程序");
                    goto launch;
                }
                else
                {
                    try
                    {
                        JArray obj = JsonConvert.DeserializeObject(foldsRaw) as JArray;
                        folds = obj.ToObject<string[]>();
                    }
                    catch (Exception e)
                    {
                        Console.Write(e);
                    }
                }
                if (folds != null)
                {
                    UpdateMessage("正在检查更新……");
                    foreach (string f in folds)
                    {
                        //string custom_path = (AppDomain.CurrentDomain.BaseDirectory + "../.customfiles/" + f).Replace("/", "\\") + "\\";
#if !Other
                        string local_path = (AppDomain.CurrentDomain.BaseDirectory + ".updater" + f).Replace("/", "\\") + "\\";
                        string internet_path = "http://chobitslive.live:3080/minecraft/updater/publish/" + f.Replace("\\", "/") + "/";
#else
                        string local_path = (AppDomain.CurrentDomain.BaseDirectory + "files" + f).Replace("/", "\\") + "\\";
                        string internet_path = rawtarget + f.Replace("\\", "/") + "/";
#endif
                        //string folder_setting = internet_path + "fold.json";
                        try
                        {
                            //string s = Tools.HTTP.GetHttpStringData(folder_setting);
                            //var obj = JsonConvert.DeserializeObject(s) as JObject;
                            //string type = obj.GetValue("type").ToObject<string>();
                            string type = "Hash";
                            if (type == "Hash")
                            {
                                string md5sPath = internet_path + "md5s.json";
                                string md5s = Tools.HTTP.GetHttpStringData(md5sPath);
                                Dictionary<string, string> dict_internet_files = JsonConvert.DeserializeObject<Dictionary<string, string>>(md5s);
                                Dictionary<string, string> dict_local_files = new Dictionary<string, string>();
                                //Dictionary<string, string> dict_custom_files = new Dictionary<string, string>();
                                if (!Directory.Exists(local_path)) Directory.CreateDirectory(local_path);
                                string[] localFiles = Directory.GetFiles(local_path);
                                for (int i = 0; i < localFiles.Length; i++)
                                {
                                    //UpdateMessage("计算本地文件哈希值 {0} " + localFiles[i], i + 1, localFiles.Length);
                                    string local_hash = Tools.Md5.GetMD5HashFromFile(localFiles[i]);
                                    dict_local_files.Add(new FileInfo(localFiles[i]).Name, local_hash);
                                }
                                //UpdateMessage("正在由本地向上匹配文件，请稍候...");
                                //步骤1 本地向上检查
                                foreach (KeyValuePair<string, string> local in dict_local_files)
                                {
                                    try
                                    {
                                        if (!dict_internet_files.ContainsValue(local.Value))
                                        {
                                            File.Delete(local_path + local.Key);
                                            done++;
                                        }
                                    }
                                    catch
                                    {
                                        filed++;
                                    }
                                }
                                //UpdateMessage("正在由远程向下匹配文件，请稍候...");
                                //步骤2 远程向下检查
                                int dict_internet_files_enum_now = 0;
                                foreach (KeyValuePair<string, string> internet in dict_internet_files)
                                {
                                    dict_internet_files_enum_now++;
                                    if (IsControlFile(internet.Key)) continue;
                                    if (!dict_local_files.ContainsValue(internet.Value))
                                    {
                                        if (Tools.HTTP.HttpDownload(internet_path + internet.Key, local_path + internet.Key, dict_internet_files_enum_now, dict_internet_files.Count) == false) filed++;
                                        done++;
                                    }
                                }
                            }
                            else if (type == "loose")
                            {
                                string md5sPath = internet_path + "md5s.json";
                                string md5s = Tools.HTTP.GetHttpStringData(md5sPath);
                                Dictionary<string, string> dict_internet_files = JsonConvert.DeserializeObject<Dictionary<string, string>>(md5s);
                                UpdateMessage("正在由远程向下匹配文件，请稍候...");
                                //步骤2 远程向下检查
                                int dict_internet_files_enum_now = 0;
                                Dictionary<string, string> dict_local_files = new Dictionary<string, string>();
                                string[] localFiles = Directory.GetFiles(local_path);
                                for (int i = 0; i < localFiles.Length; i++)
                                {
                                    UpdateMessage("计算本地文件哈希值 {0} " + localFiles[i], i + 1, localFiles.Length);
                                    string local_hash = Tools.Md5.GetMD5HashFromFile(localFiles[i]);
                                    dict_local_files.Add(new FileInfo(localFiles[i]).Name, local_hash);
                                }
                                foreach (KeyValuePair<string, string> internet in dict_internet_files)
                                {
                                    dict_internet_files_enum_now++;
                                    if (IsControlFile(internet.Key)) continue;
                                    if (!dict_local_files.ContainsKey(internet.Key))
                                    {
                                        if (Tools.HTTP.HttpDownload(internet_path + internet.Key, local_path + internet.Key, dict_internet_files_enum_now, dict_internet_files.Count) == false) filed++;
                                        done++;
                                    }
                                    else if (!dict_local_files.ContainsValue(internet.Value))
                                    {
                                        if (Tools.HTTP.HttpDownload(internet_path + internet.Key, local_path + internet.Key, dict_internet_files_enum_now, dict_internet_files.Count) == false) filed++;
                                        done++;
                                    }
                                }
                                UpdateMessage("正在由远程向下请求删除文件，请稍候...");
                                string fo_delete_path = internet_path + "delete.json";
                                string fo_delete_s = Tools.HTTP.GetHttpStringData(fo_delete_path);
                                if (fo_delete_s != null)
                                {
                                    JArray file_list = JsonConvert.DeserializeObject(fo_delete_s) as JArray;
                                    string[] files = file_list.ToObject<string[]>();
                                    foreach (string singal_file in files)
                                    {
                                        try
                                        {
                                            if (File.Exists(local_path + singal_file)) File.Delete(local_path + singal_file);
                                            done++;
                                        }
                                        catch
                                        {
                                            filed++;
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            Console.Write(e);
                        }
                    }
                    UpdateMessage("结束操作……检查文件状态");
                    if (filed > 0)
                    {
                        reInput:
                        Console.WriteLine("\r\n\r\n有" + filed + "个文件操作失败了，你要重试一下吗？");
                        Console.WriteLine("[Y/Esc]重试\t\t[N/Enter]继续\t\t[C]退出");
                        ConsoleKeyInfo info = Console.ReadKey();
                        switch (info.Key)
                        {
                            case ConsoleKey.N:
                            case ConsoleKey.Enter:
                                goto launch;
                            case ConsoleKey.C:
                                Environment.Exit(0);
                                break;
                            case ConsoleKey.Y:
                            case ConsoleKey.Escape:
                                redoCount++;
                                goto redo;
                            default:
                                goto reInput;
                        }
                    }
                }
                if (done > 0)
                {
                    redoCount++;
                    goto redo;
                }
            }
            //启动游戏启动器 块
            launch:
            {
#if !Other
                UpdateMessage("正在等待更新程序启动...");
                System.Diagnostics.Process.Start(AppDomain.CurrentDomain.BaseDirectory + ".updater/ChobitsMCLauncher.exe");
                Thread.Sleep(1000);
                Environment.Exit(0);
#else
                Console.WriteLine("操作结束，按任意键继续");
                Console.ReadKey();
#endif
            }
        }
        private static void UpdateMessage(string message, double now, double count)
        {
            //bool a = message.Contains("{0}");
            //bool b = message.Contains("{1}");
            //if (a && b) mainWindow.UpdateStatus(string.Format(message, Math.Round(now, 2), Math.Round(count, 2)), now / count);
            //else if (a) mainWindow.UpdateStatus(string.Format(message, "(" + Math.Round(now, 2) + " / " + Math.Round(count, 2) + ")"), now / count);
            //else mainWindow.UpdateStatus(message + " (" + Math.Round(now, 2) + " / " + Math.Round(count, 2) + ")", now / count);
            Console.WriteLine("[" + Math.Round(now * 100 / count, 2) + "%] " + message);
        }
        private static void UpdateMessage(string message)
        {
            //mainWindow.UpdateStatus(message, -1);
            Console.WriteLine(message);
        }
        private static bool IsControlFile(string fileName)
        {
            return controlFiles.Contains(fileName);
        }
    }
}
