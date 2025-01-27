using System.Threading.Tasks;
using WHUTRunner.Models;

namespace WHUTRunner.Services
{
    /// <summary>
    /// 环境配置服务接口，用于管理脚本运行环境和依赖包
    /// </summary>
    public interface IEnvironmentService
    {
        /// <summary>
        /// 检查环境配置是否正确
        /// </summary>
        /// <param name="config">环境配置信息</param>
        /// <returns>
        /// 如果所有环境检查通过返回 true，否则返回 false
        /// </returns>
        /// <remarks>
        /// 此方法会检查：
        /// 1. Python 解释器是否可用
        /// 2. pip 包管理器是否可用
        /// 3. Node.js 运行时是否可用
        /// 4. npm 包管理器是否可用
        /// </remarks>
        Task<bool> CheckEnvironmentAsync(EnvironmentConfig config);

        /// <summary>
        /// 安装指定的 Python 包
        /// </summary>
        /// <param name="packages">要安装的包名数组，格式如：["requests==2.31.0", "pandas>=2.0.0"]</param>
        /// <returns>
        /// 如果所有包安装成功返回 true，否则返回 false
        /// </returns>
        /// <remarks>
        /// 此方法会使用 pip install 命令安装包
        /// 支持指定版本号，如：package==version
        /// 支持版本范围，如：package>=version
        /// </remarks>
        Task<bool> InstallPythonPackagesAsync(string[] packages);

        /// <summary>
        /// 安装指定的 Node.js 包
        /// </summary>
        /// <param name="packages">要安装的包名数组，格式如：["axios@1.6.2", "lodash@latest"]</param>
        /// <returns>
        /// 如果所有包安装成功返回 true，否则返回 false
        /// </returns>
        /// <remarks>
        /// 此方法会使用 npm install -g 命令全局安装包
        /// 支持指定版本号，如：package@version
        /// 支持特殊标签，如：package@latest
        /// </remarks>
        Task<bool> InstallNodePackagesAsync(string[] packages);

        /// <summary>
        /// 获取已安装的 Python 包列表
        /// </summary>
        /// <returns>
        /// 已安装的包名数组，格式如：["requests==2.31.0", "pandas==2.1.3"]
        /// 如果发生错误返回空数组
        /// </returns>
        /// <remarks>
        /// 此方法会使用 pip list 命令获取已安装的包信息
        /// 返回的包名包含版本号
        /// </remarks>
        Task<string[]> GetInstalledPythonPackagesAsync();

        /// <summary>
        /// 获取已安装的 Node.js 包列表
        /// </summary>
        /// <returns>
        /// 已安装的包名数组，格式如：["axios@1.6.2", "lodash@4.17.21"]
        /// 如果发生错误返回空数组
        /// </returns>
        /// <remarks>
        /// 此方法会使用 npm list -g 命令获取全局安装的包信息
        /// 返回的包名包含版本号
        /// </remarks>
        Task<string[]> GetInstalledNodePackagesAsync();
    }
} 