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
		string[] SizeSuffixes = { "bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };
		List<RamClass> ramDevices = new List<RamClass>();
		List<StorageClass> storageDevices = new List<StorageClass>();
		public MainWindow()
		{
			InitializeComponent();
			computer.Open();
			computer.CPUEnabled = true;
			Init();
		}

		private string SizeSuffix(Int64 value)
		{
			if (value < 0) { return "-" + SizeSuffix(-value); }
			if (value == 0) { return "0.0 bytes"; }

			int ss = (int)Math.Log(value, 1024);
			decimal adjustedSize = (decimal)value / (1L << (ss * 10));

			return string.Format("{0:n1} {1}", adjustedSize, SizeSuffixes[ss]);
		}

		public void Init()
		{
			computer.Open();
			//Get CPU Info
			ManagementObjectSearcher mosCPU = new ManagementObjectSearcher("SELECT * FROM Win32_Processor");
			foreach (var item in mosCPU.Get())
			{
				cpuName.Content = item["Name"];
				cpuManufacturer.Content = item["Manufacturer"];
				cpuCores.Content = item["NumberOfCores"];
				cpuStatus.Content = item["Status"];
			}
			SensorsCPU();

			//Get Memory Info
			ManagementObjectSearcher mosMemory = new ManagementObjectSearcher("SELECT * FROM Win32_PhysicalMemory");
			foreach (var item in mosMemory.Get())
			{
				if (item != null)
					ramDevices.Add(new RamClass(item["Tag"].ToString(), SizeSuffix(Convert.ToInt64(item["Capacity"])).ToString(), item["MemoryType"].ToString()));
			}
			foreach (var item in ramDevices)
			{
				memoryCB.Items.Add(item.tag);
			}

			//Get GPU Info
			ManagementObjectSearcher mosGPU = new ManagementObjectSearcher("SELECT * FROM Win32_VideoController");
			foreach (var item in mosGPU.Get())
			{
				gpuName.Content = item["Name"];
				gpuRAM.Content = item["AdapterRAM"];
				gpuVProcessor.Content = item["VideoProcessor"];
				gpuStatus.Content = item["Status"];
			}
			SensorsGPU();

			//Get Motherboard Info
			ManagementObjectSearcher mosMB = new ManagementObjectSearcher("SELECT * FROM Win32_BaseBoard");
			foreach (var item in mosMB.Get())
			{
				mbName.Content = item["Product"];
				mbManufacturer.Content = item["Manufacturer"];
				mbStatus.Content = item["Status"];
			}

			//Get Storage Info
			ManagementObjectSearcher mosStorage = new ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive");
			foreach (var item in mosStorage.Get())
			{
				if (item != null)
					storageDevices.Add(new StorageClass(item["Model"].ToString(), SizeSuffix(Convert.ToInt64(item["Size"])).ToString(), item["Partitions"].ToString(), item["Status"].ToString()));
			}
			foreach (var item in storageDevices)
			{
				storageCB.Items.Add(item.model);
			}
		}

		public void SensorsCPU()
		{
			string cpuTemperature = "";
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
								cpuTemperature = $"{i.Value.Value}°C";
						}
					}
				}
			}
			cpuTemp.Content = cpuTemperature;
		}

		public void SensorsGPU()
		{
			string gpuTemperature = "";
			foreach (var item in computer.Hardware)
			{
				if (item.HardwareType == HardwareType.GpuNvidia || item.HardwareType == HardwareType.GpuAti)
				{
					item.Update();
					foreach (var i in item.Sensors)
					{
						if (i.SensorType == SensorType.Temperature)
						{
							if (i.Value != null)
								gpuTemperature = $"{i.Value.Value}°C";
						}
					}
				}
			}
			gpuTemp.Content = gpuTemperature;
		}

		private void MemoryCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			RamClass selectRam = ramDevices.Where(x => x.tag == memoryCB.SelectedItem.ToString()).First();
			memoryTag.Content = selectRam.tag;
			memoryCapacity.Content = selectRam.capacity;
			if (selectRam.memoryType == "20")
				memoryType.Content = "DDR";
			else if (selectRam.memoryType == "21")
				memoryType.Content = "DDR2";
			else if (selectRam.memoryType == "24")
				memoryType.Content = "DDR3";
			else if (selectRam.memoryType == "26")
				memoryType.Content = "DDR4";
		}

		private void StorageCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			StorageClass selectStorage = storageDevices.Where(x => x.model == storageCB.SelectedItem.ToString()).First();
			storageModel.Content = selectStorage.model;
			storageSize.Content = selectStorage.size;
			storagePartitions.Content = selectStorage.partitions;
			storageStatus.Content = selectStorage.status;
		}
	}
}
