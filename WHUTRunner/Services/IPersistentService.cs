using System.Threading.Tasks;
using WHUTRunner.Models;

namespace WHUTRunner.Services
{
    /// <summary>
    /// 配置服务接口，用于管理应用配置的保存和加载
    /// </summary>
    public interface IPersistentService
    {
        /// <summary>
        /// 加载环境配置
        /// </summary>
        /// <returns>环境配置对象，如果文件不存在则返回默认配置</returns>
        Task<EnvironmentConfig> LoadEnvironmentConfigAsync();

        /// <summary>
        /// 保存环境配置
        /// </summary>
        /// <param name="config">要保存的环境配置</param>
        Task SaveEnvironmentConfigAsync(EnvironmentConfig config);

        /// <summary>
        /// 加载应用设置
        /// </summary>
        /// <returns>应用设置对象，如果文件不存在则返回默认设置</returns>
        Task<AppSettings> LoadAppSettingsAsync();

        /// <summary>
        /// 保存应用设置
        /// </summary>
        /// <param name="settings">要保存的应用设置</param>
        Task SaveAppSettingsAsync(AppSettings settings);
    }
} 