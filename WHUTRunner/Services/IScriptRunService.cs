using System.Threading.Tasks;
using WHUTRunner.Models;

namespace WHUTRunner.Services;

/// <summary>
/// 脚本运行器服务接口，用于管理和执行脚本
/// </summary>
public interface IScriptRunService
{
    /// <summary>
    /// 异步运行指定的脚本
    /// </summary>
    /// <param name="config">脚本配置信息，包含脚本路径、类型和参数等</param>
    /// <returns>
    /// 如果脚本执行成功返回 true，否则返回 false
    /// </returns>
    /// <remarks>
    /// 此方法会：
    /// 1. 根据脚本类型选择对应的运行时（Python/Node.js）
    /// 2. 使用命令行参数传递配置的参数
    /// 3. 捕获脚本的标准输出和错误输出
    /// 4. 等待脚本执行完成并返回结果
    /// 
    /// 如果已有脚本正在运行，将抛出 InvalidOperationException
    /// </remarks>
    Task<bool> RunAsync(ScriptItem config);

    /// <summary>
    /// 停止当前正在运行的脚本
    /// </summary>
    /// <returns>
    /// 表示异步操作的任务
    /// </returns>
    /// <remarks>
    /// 此方法会：
    /// 1. 强制终止当前运行的进程
    /// 2. 清理相关资源
    /// 3. 重置运行状态
    /// 
    /// 如果没有脚本在运行，此方法不会有任何效果
    /// </remarks>
    Task StopAsync();
} 