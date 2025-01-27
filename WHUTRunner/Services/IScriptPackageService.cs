using System.Collections.Generic;
using System.Threading.Tasks;
using WHUTRunner.Models;

namespace WHUTRunner.Services
{
    /// <summary>
    /// 脚本包服务接口，用于管理脚本包的获取、安装和卸载
    /// </summary>
    public interface IScriptPackageService
    {
        /// <summary>
        /// 获取可用的脚本包列表
        /// </summary>
        /// <returns>脚本包列表</returns>
        Task<IEnumerable<ScriptPackage>> GetAvailablePackagesAsync();

        /// <summary>
        /// 获取已安装的脚本包列表
        /// </summary>
        /// <returns>已安装的脚本包列表</returns>
        Task<IEnumerable<ScriptPackage>> GetInstalledPackagesAsync();

        /// <summary>
        /// 安装指定的脚本包
        /// </summary>
        /// <param name="package">要安装的脚本包</param>
        /// <returns>安装是否成功</returns>
        Task<bool> InstallPackageAsync(ScriptPackage package);

        /// <summary>
        /// 卸载指定的脚本包
        /// </summary>
        /// <param name="package">要卸载的脚本包</param>
        /// <returns>卸载是否成功</returns>
        Task<bool> UninstallPackageAsync(ScriptPackage package);

        /// <summary>
        /// 更新指定的脚本包
        /// </summary>
        /// <param name="package">要更新的脚本包</param>
        /// <returns>更新是否成功</returns>
        Task<bool> UpdatePackageAsync(ScriptPackage package);

        /// <summary>
        /// 检查脚本包更新
        /// </summary>
        /// <param name="package">要检查的脚本包</param>
        /// <returns>是否有可用更新</returns>
        Task<bool> CheckUpdateAsync(ScriptPackage package);
    }
} 