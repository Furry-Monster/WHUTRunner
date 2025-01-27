using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WHUTRunner.Models;

namespace WHUTRunner.Services
{
    public class ScriptPackageService : IScriptPackageService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;
        private readonly string _packagesDir;

        public ScriptPackageService()
        {
            _httpClient = new HttpClient();
            _baseUrl = "https://api.example.com"; // TODO: 替换为实际的API地址
            _packagesDir = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "WHUTRunner",
                "Packages"
            );

            // 确保包目录存在
            Directory.CreateDirectory(_packagesDir);
        }

        public async Task<IEnumerable<ScriptPackage>> GetAvailablePackagesAsync()
        {
            try
            {
                // TODO: 从API获取可用包列表
                // 这里暂时返回模拟数据
                return new List<ScriptPackage>
                {
                    new()
                    {
                        Id = "auto-score",
                        Name = "理工学堂一键满分",
                        Description = "自动完成所有课程作业并获得满分",
                        Author = "WHUTer",
                        Version = "1.0.0",
                        Type = "Python",
                        IconUrl = "https://example.com/icon.png",
                        DownloadCount = 1000,
                        LastUpdated = DateTime.Now.AddDays(-1)
                    },
                    new()
                    {
                        Id = "auto-watch",
                        Name = "一键看完视频",
                        Description = "自动完成所有视频观看",
                        Author = "WHUTer",
                        Version = "1.0.0",
                        Type = "Python",
                        IconUrl = "https://example.com/icon.png",
                        DownloadCount = 500,
                        LastUpdated = DateTime.Now.AddDays(-2)
                    }
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"获取可用包列表失败: {ex.Message}");
                return Array.Empty<ScriptPackage>();
            }
        }

        public async Task<IEnumerable<ScriptPackage>> GetInstalledPackagesAsync()
        {
            try
            {
                var installedPackages = new List<ScriptPackage>();
                var packageFiles = Directory.GetFiles(_packagesDir, "*.json");

                foreach (var file in packageFiles)
                {
                    var json = await File.ReadAllTextAsync(file);
                    var package = JsonConvert.DeserializeObject<ScriptPackage>(json);
                    if (package != null)
                    {
                        package.IsInstalled = true;
                        installedPackages.Add(package);
                    }
                }

                return installedPackages;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"获取已安装包列表失败: {ex.Message}");
                return Array.Empty<ScriptPackage>();
            }
        }

        public async Task<bool> InstallPackageAsync(ScriptPackage package)
        {
            try
            {
                // TODO: 从API下载脚本文件
                // 这里暂时只保存包信息
                var packagePath = Path.Combine(_packagesDir, $"{package.Id}.json");
                var json = JsonConvert.SerializeObject(package, Formatting.Indented);
                await File.WriteAllTextAsync(packagePath, json);

                package.IsInstalled = true;
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"安装包失败: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> UninstallPackageAsync(ScriptPackage package)
        {
            try
            {
                var packagePath = Path.Combine(_packagesDir, $"{package.Id}.json");
                if (File.Exists(packagePath))
                {
                    File.Delete(packagePath);
                }

                // TODO: 删除脚本文件

                package.IsInstalled = false;
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"卸载包失败: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> UpdatePackageAsync(ScriptPackage package)
        {
            try
            {
                // 先卸载旧版本
                await UninstallPackageAsync(package);
                // 安装新版本
                return await InstallPackageAsync(package);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"更新包失败: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> CheckUpdateAsync(ScriptPackage package)
        {
            try
            {
                // TODO: 从API检查更新
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"检查更新失败: {ex.Message}");
                return false;
            }
        }
    }
} 