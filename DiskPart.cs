using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace USBRevertTool;

public static class DiskPart
{
    private static MainWindow _mainWindow;
    public static void Initialize(MainWindow mainWindow)
    {
        _mainWindow = mainWindow;
    }
    public static async Task<string> Getcommand(string command, int fleshNum)
    {
        
        var processInfo = new ProcessStartInfo("diskpart.exe")
        {
            RedirectStandardInput = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true,
            Verb = "runas"
        };

        using (var Process = new Process())
        {
            Process.StartInfo = processInfo;
            
            try
            { 
                Process.Start();
                
                await Process.StandardInput.WriteLineAsync(command);
                await Process.StandardInput.WriteLineAsync($"select disk {fleshNum}");
                await Process.StandardInput.WriteLineAsync("attributes disk clear readonly");
                await Process.StandardInput.WriteLineAsync("clean");
                await Process.StandardInput.WriteLineAsync("create partition primary");
                await Process.StandardInput.WriteLineAsync("format fs=ntfs quick");
                await Process.StandardInput.WriteLineAsync("exit");
                
                await Process.WaitForExitAsync();
                
                var output = await Process.StandardOutput.ReadToEndAsync();
                var errorOutput = await Process.StandardError.ReadToEndAsync();

                if (!string.IsNullOrEmpty(errorOutput))
                {   
                    Console.WriteLine("Ошибки:");
                    Console.WriteLine(errorOutput);
                    _mainWindow.TextOutput.Text = errorOutput;
                    return errorOutput;
                }
                
                Console.WriteLine(output);
                _mainWindow.TextOutput.Text = output;
                return output;

            }
            catch (Exception e)
            {
                Console.WriteLine("Произошла ошибка: " + e.Message);
            }
        }

        _mainWindow.TextOutput.Text = "!!!";
        return "";
    }
}