using System.Diagnostics;
using System.Text.Json;
using WebAppToExecutePowershell.Models;

namespace WebAppToExecutePowershell.Services
{
    public class PowerShellService
    {
        private readonly string _scriptPath;

        public PowerShellService(IWebHostEnvironment webHostEnvironment)
        {
            _scriptPath = Path.Combine(webHostEnvironment.ContentRootPath, "Scripts");
        }

        public List<string> GetAvailableScripts()
        {
            return Directory.GetFiles(_scriptPath, "*.ps1")
                          .Select(path => Path.GetFileName(path) ?? string.Empty)
                          .Where(name => !string.IsNullOrEmpty(name))
                          .ToList();
        }

        public async Task<List<PowerShellResult>> ExecutePowerShellScript(string scriptName, string? parameter = null)
        {
            var results = new List<PowerShellResult>();
            var scriptPath = Path.Combine(_scriptPath, scriptName);

            if (!File.Exists(scriptPath))
            {
                throw new FileNotFoundException($"Script {scriptName} not found");
            }

            var startInfo = new ProcessStartInfo
            {
                FileName = "pwsh",
                Arguments = string.IsNullOrEmpty(parameter)
                    ? $"-File \"{scriptPath}\""
                    : $"-File \"{scriptPath}\" -ProcessName \"{parameter}\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (var process = new Process { StartInfo = startInfo })
            {
                try
                {
                    process.Start();
                    var output = await process.StandardOutput.ReadToEndAsync();
                    var error = await process.StandardError.ReadToEndAsync();
                    await process.WaitForExitAsync();

                    if (!string.IsNullOrEmpty(error))
                    {
                        throw new Exception($"PowerShell error: {error}");
                    }

                    if (string.IsNullOrEmpty(output))
                    {
                        return results;
                    }

                    var jsonData = JsonSerializer.Deserialize<JsonElement>(output);
                    if (jsonData.ValueKind == JsonValueKind.Array)
                    {
                        foreach (var item in jsonData.EnumerateArray())
                        {
                            results.Add(new PowerShellResult
                            {
                                Name = item.GetProperty("Name").GetString() ?? string.Empty,
                                Id = item.GetProperty("Id").GetString() ?? string.Empty,
                                Status = item.GetProperty("Status").GetString() ?? string.Empty
                            });
                        }
                    }
                    else if (jsonData.ValueKind == JsonValueKind.Object)
                    {
                        // Handle single object result
                        results.Add(new PowerShellResult
                        {
                            Name = jsonData.GetProperty("Name").GetString() ?? string.Empty,
                            Id = jsonData.GetProperty("Id").GetString() ?? string.Empty,
                            Status = jsonData.GetProperty("Status").GetString() ?? string.Empty
                        });
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error executing PowerShell script: {ex.Message}");
                }
            }

            return results;
        }
    }
}
