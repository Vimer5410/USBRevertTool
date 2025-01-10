using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;



namespace USBRevertTool;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        
        InitializeComponent();
        DiskPart.Initialize(this);
    }

     static int GetUsbNum()
     {
         List<string> drivesName=new List<string>();
         int UsbNum = 0;
         foreach (ManagementObject managementObject in new ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive").Get())
         {
             var mo = managementObject.GetPropertyValue("InterfaceType").ToString();
             drivesName.Add(mo);
             Console.WriteLine(mo);
         }
         foreach (var el in drivesName)
         {
             if (el.Contains("USB"))
             {
                 UsbNum= drivesName.IndexOf(el)+1;
             }
         }
         return UsbNum;
     }
    
    private async void ClearButton_OnClick(object? sender, RoutedEventArgs e)
    {
        var command = "list disk";
        var fleshNum =GetUsbNum();
        Console.WriteLine(fleshNum);
        await DiskPart.Getcommand(command, 10);
    }
}