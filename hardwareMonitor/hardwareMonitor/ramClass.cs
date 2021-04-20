using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hardwareMonitor
{
	public class RamClass
	{
		public string tag { get; set; }
		public string capacity { get; set; }
		public string memoryType { get; set; }
		public RamClass(string t, string c, string mt)
		{
			this.tag = t;
			this.capacity = c;
			this.memoryType = mt;
		}
	}
}
