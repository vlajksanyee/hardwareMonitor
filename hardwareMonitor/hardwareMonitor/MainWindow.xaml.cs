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

namespace hardwareMonitor
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
			Init();
		}

		public void Init()
		{
			ManagementObjectSearcher mosCPU = new ManagementObjectSearcher("SELECT * FROM Win32_Processor");
			foreach (var item in mosCPU.Get())
			{
				cpuName.Content = item["Name"];
				cpuManufacturer.Content = item["Manufacturer"];
				cpuCores.Content = item["NumberOfCores"];
				cpuStatus.Content = item["Status"];		
			}
			ManagementObjectSearcher mosGPU = new ManagementObjectSearcher("SELECT * FROM Win32_VideoController");
			foreach (var item in mosGPU.Get())
			{
				gpuName.Content = item["Name"];
				gpuRAM.Content = item["AdapterRAM"];
				gpuVProcessor.Content = item["VideoProcessor"];
				gpuStatus.Content = item["Status"];
			}
		}
	}
}
