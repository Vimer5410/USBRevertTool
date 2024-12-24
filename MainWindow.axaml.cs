using System;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace USBRevertTool;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }
    

    static async Task Getcommand(string command, int fleshNum)
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

                // Отправка команд в diskpart
                await Process.StandardInput.WriteLineAsync(command);
                await Process.StandardInput.WriteLineAsync($"select disk {fleshNum}");
                await Process.StandardInput.WriteLineAsync("attributes disk clear readonly");
                await Process.StandardInput.WriteLineAsync("clean");
                await Process.StandardInput.WriteLineAsync("create partition primary");
                await Process.StandardInput.WriteLineAsync("format fs=ntfs quick");
                
                await Process.StandardInput.WriteLineAsync("exit");

                // Ждем завершения процесса
                await Process.WaitForExitAsync();

                // Чтение вывода после завершения процесса
                var output = await Process.StandardOutput.ReadToEndAsync();
                var errorOutput = await Process.StandardError.ReadToEndAsync();

                Console.WriteLine(output);
                if (!string.IsNullOrEmpty(errorOutput))
                {
                    Console.WriteLine("Ошибки:");
                    Console.WriteLine(errorOutput);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Произошла ошибка: " + e.Message);
            }
        }
    }

    private async void ClearButton_OnClick(object? sender, RoutedEventArgs e)
    {
        var command = "list disk";
        var fleshNum = 3;
        await Getcommand(command, fleshNum);
    }
}