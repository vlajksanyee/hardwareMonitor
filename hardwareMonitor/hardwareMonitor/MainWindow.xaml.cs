using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Management;
using OpenHardwareMonitor;
using OpenHardwareMonitor.Hardware;

namespace hardwareMonitor
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		Computer computer = new Computer() { CPUEnabled = true };
		string cputptremp = "";
		public MainWindow()
		{
			InitializeComponent();
			Init();
		}

		public void Init()
		{
			computer.Open();
			ManagementObjectSearcher mosCPU = new ManagementObjectSearcher("SELECT * FROM Win32_Processor");
			foreach (var item in mosCPU.Get())
			{
				cpuName.Content = item["Name"];
				cpuManufacturer.Content = item["Manufacturer"];
				cpuCores.Content = item["NumberOfCores"];
				cpuStatus.Content = item["Status"];
			}
			foreach (var item in computer.Hardware)
			{
				if (item.HardwareType == HardwareType.CPU)
				{
					item.Update();
					foreach (var i in item.Sensors)
					{
						if (i.SensorType == SensorType.Temperature)
						{
							if (i.Value != null)
							cputptremp += $"{i.Value.Value}\n";
						}
					}
				}
			}
			cpuTemp.Content = cputptremp;
			ManagementObjectSearcher mosGPU = new ManagementObjectSearcher("SELECT * FROM Win32_VideoController");
			foreach (var item in mosGPU.Get())
			{
				gpuName.Content = item["Name"];
				gpuRAM.Content = item["AdapterRAM"];
				gpuVProcessor.Content = item["VideoProcessor"];
				gpuStatus.Content = item["Status"];
			}
			ManagementObjectSearcher mosMB = new ManagementObjectSearcher("SELECT * FROM Win32_BaseBoard");
			foreach (var item in mosMB.Get())
			{
				mbName.Content = item["Product"];
				mbManufacturer.Content = item["Manufacturer"];
				mbStatus.Content = item["Status"];
			}
			ManagementObjectSearcher mosKb = new ManagementObjectSearcher("SELECT * FROM Win32_Keyboard");
			foreach (var item in mosKb.Get())
			{
				kbDesc.Content = item["Description"];
				kbStatus.Content = item["Status"];
			}
		}
	}
}
