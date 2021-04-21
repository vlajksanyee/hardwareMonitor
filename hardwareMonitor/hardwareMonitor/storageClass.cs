using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hardwareMonitor
{
	public class StorageClass
	{
		public string model { get; set; }
		public string size { get; set; }
		public string partitions { get; set; }
		public string status { get; set; }
		public StorageClass(string m, string s, string p, string st)
		{
			this.model = m;
			this.size = s;
			this.partitions = p;
			this.status = st;
		}
	}
}
