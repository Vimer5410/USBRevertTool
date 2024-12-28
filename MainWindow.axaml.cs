using System;
using System.Diagnostics;
using System.IO;
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

     static void GetUsbNum()
     {
         DriveInfo[] driveInfo = DriveInfo.GetDrives();
         foreach (var drive in driveInfo)
         {
            string driveName=drive.DriveType == DriveType.Removable ? drive.Name : null;
         }

     }
    
    private async void ClearButton_OnClick(object? sender, RoutedEventArgs e)
    {
        var command = "list disk";
        var fleshNum = 3;
        GetUsbNum();
    }
}