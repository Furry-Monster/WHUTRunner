using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using WHUTRunner.Models;

namespace WHUTRunner.Services
{
    public class EnvironmentService : IEnvironmentService
    {
        public async Task<bool> CheckEnvironmentAsync(EnvironmentConfig config)
        {
            try
            {
                // 检查 Python
                var pythonVersion = await RunProcessAsync(config.PythonPath, "--version");
                if (string.IsNullOrEmpty(pythonVersion)) return false;

                // 检查 pip
                var pipVersion = await RunProcessAsync(config.PipPath, "--version");
                if (string.IsNullOrEmpty(pipVersion)) return false;

                // 检查 Node.js
                var nodeVersion = await RunProcessAsync(config.NodePath, "--version");
                if (string.IsNullOrEmpty(nodeVersion)) return false;

                // 检查 npm
                var npmVersion = await RunProcessAsync(config.NpmPath, "--version");
                if (string.IsNullOrEmpty(npmVersion)) return false;

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> InstallPythonPackagesAsync(string[] packages)
        {
            try
            {
                foreach (var package in packages)
                {
                    var result = await RunProcessAsync("pip", $"install {package}");
                    if (string.IsNullOrEmpty(result)) return false;
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> InstallNodePackagesAsync(string[] packages)
        {
            try
            {
                foreach (var package in packages)
                {
                    var result = await RunProcessAsync("npm", $"install -g {package}");
                    if (string.IsNullOrEmpty(result)) return false;
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<string[]> GetInstalledPythonPackagesAsync()
        {
            try
            {
                var output = await RunProcessAsync("pip", "list");
                var packages = new List<string>();
                var lines = output.Split('\n');
                
                foreach (var line in lines.Skip(2)) // 跳过头部
                {
                    var match = Regex.Match(line, @"^(\S+)\s+(\S+)");
                    if (match.Success)
                    {
                        packages.Add($"{match.Groups[1].Value}=={match.Groups[2].Value}");
                    }
                }

                return packages.ToArray();
            }
            catch
            {
                return Array.Empty<string>();
            }
        }

        public async Task<string[]> GetInstalledNodePackagesAsync()
        {
            try
            {
                var output = await RunProcessAsync("npm", "list -g --depth=0");
                var packages = new List<string>();
                var lines = output.Split('\n');

                foreach (var line in lines.Skip(1)) // 跳过第一行
                {
                    var match = Regex.Match(line, @"└── (\S+)@(\S+)");
                    if (match.Success)
                    {
                        packages.Add($"{match.Groups[1].Value}@{match.Groups[2].Value}");
                    }
                }

                return packages.ToArray();
            }
            catch
            {
                return Array.Empty<string>();
            }
        }

        /// <summary>
        /// 开启一个新线程，异步运行终端命令
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="arguments"></param>
        /// <returns></returns>
        private async Task<string> RunProcessAsync(string fileName, string arguments)
        {
            using var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = fileName,
                    Arguments = arguments,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true
                }
            };

            try
            {
                process.Start();
                var output = await process.StandardOutput.ReadToEndAsync();
                await process.WaitForExitAsync();
                return output;
            }
            catch
            {
                return string.Empty;
            }
        }
    }
} 