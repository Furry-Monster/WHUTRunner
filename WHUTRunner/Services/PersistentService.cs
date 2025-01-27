using System;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WHUTRunner.Models;

namespace WHUTRunner.Services
{
    public class PersistentService : IPersistentService
    {
        private readonly string _configPath;
        private readonly string _envConfigFile;
        private readonly string _appSettingsFile;

        public PersistentService()
        {
            _configPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), // <-- 获取%APPDATA%路径
                "WHUTRunner"
            );
            _envConfigFile = Path.Combine(_configPath, "environment.json");
            _appSettingsFile = Path.Combine(_configPath, "settings.json");

            // 确保配置目录存在
            Directory.CreateDirectory(_configPath);
        }

        public async Task<EnvironmentConfig> LoadEnvironmentConfigAsync()
        {
            try
            {
                if (File.Exists(_envConfigFile))
                {
                    var json = await File.ReadAllTextAsync(_envConfigFile);
                    return JsonConvert.DeserializeObject<EnvironmentConfig>(json) ?? new EnvironmentConfig();
                }
            }
            catch (Exception ex)
            {
                // TODO: 添加日志记录
                Console.WriteLine($"加载环境配置失败: {ex.Message}");
            }

            return new EnvironmentConfig();
        }

        public async Task SaveEnvironmentConfigAsync(EnvironmentConfig config)
        {
            try
            {
                var json = JsonConvert.SerializeObject(config, Formatting.Indented);
                await File.WriteAllTextAsync(_envConfigFile, json);
            }
            catch (Exception ex)
            {
                // TODO: 添加日志记录
                Console.WriteLine($"保存环境配置失败: {ex.Message}");
                throw;
            }
        }

        public async Task<AppSettings> LoadAppSettingsAsync()
        {
            try
            {
                if (File.Exists(_appSettingsFile))
                {
                    var json = await File.ReadAllTextAsync(_appSettingsFile);
                    return JsonConvert.DeserializeObject<AppSettings>(json) ?? new AppSettings();
                }
            }
            catch (Exception ex)
            {
                // TODO: 添加日志记录
                Console.WriteLine($"加载应用设置失败: {ex.Message}");
            }

            return new AppSettings();
        }

        public async Task SaveAppSettingsAsync(AppSettings settings)
        {
            try
            {
                var json = JsonConvert.SerializeObject(settings, Formatting.Indented);
                await File.WriteAllTextAsync(_appSettingsFile, json);
            }
            catch (Exception ex)
            {
                // TODO: 添加日志记录
                Console.WriteLine($"保存应用设置失败: {ex.Message}");
                throw;
            }
        }
    }
}