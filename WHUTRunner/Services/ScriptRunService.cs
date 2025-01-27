using System;
using System.Diagnostics;
using System.Threading.Tasks;
using WHUTRunner.Models;

namespace WHUTRunner.Services
{
    public class ScriptRunService : IScriptRunService
    {
        private Process? _currentProcess;

        public async Task<bool> RunAsync(ScriptItem script)
        {
            if (_currentProcess != null && !_currentProcess.HasExited)
            {
                throw new InvalidOperationException("A script is already running");
            }

            try
            {
                var startInfo = new ProcessStartInfo
                {
                    FileName = script.Type.ToLower() switch
                    {
                        "python" => "python",
                        "javascript" => "node",
                        _ => throw new NotSupportedException($"Script type {script.Type} is not supported")
                    },
                    Arguments = script.Path,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true,
                    WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory
                };

                // 添加参数
                foreach (var param in script.Parameters)
                {
                    startInfo.ArgumentList.Add($"--{param.Key}");
                    startInfo.ArgumentList.Add(param.Value);
                }

                _currentProcess = new Process { StartInfo = startInfo };

                // 添加输出处理
                _currentProcess.OutputDataReceived += (sender, args) =>
                {
                    if (!string.IsNullOrEmpty(args.Data))
                    {
                        // TODO: 处理脚本输出
                        Console.WriteLine($"Script output: {args.Data}");
                    }
                };

                _currentProcess.ErrorDataReceived += (sender, args) =>
                {
                    if (!string.IsNullOrEmpty(args.Data))
                    {
                        // TODO: 处理脚本错误
                        Console.WriteLine($"Script error: {args.Data}");
                    }
                };

                _currentProcess.Start();
                _currentProcess.BeginOutputReadLine();
                _currentProcess.BeginErrorReadLine();

                await _currentProcess.WaitForExitAsync();
                var exitCode = _currentProcess.ExitCode;
                _currentProcess = null;

                return exitCode == 0;
            }
            catch (Exception ex)
            {
                // TODO: 处理异常
                Console.WriteLine($"Error running script: {ex.Message}");
                _currentProcess?.Kill(true);
                _currentProcess = null;
                throw;
            }
        }

        public Task StopAsync()
        {
            try
            {
                if (_currentProcess != null && !_currentProcess.HasExited)
                {
                    _currentProcess.Kill(true);
                    _currentProcess = null;
                }
                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                // TODO: 处理异常
                Console.WriteLine($"Error stopping script: {ex.Message}");
                throw;
            }
        }
    }
} 