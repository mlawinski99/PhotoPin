using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Tests
{
	public class HostEnv
	{
		protected IHostingEnvironment? HostingEnvironment { get;  set; }

		public HostEnv(IHostingEnvironment? host)
		{
			HostingEnvironment = host;
		}
	}
}
