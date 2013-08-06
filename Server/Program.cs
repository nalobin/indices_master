using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Runtime.Remoting;

namespace IndicesServer
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		static void Main()
		{
			RemotingConfiguration.Configure("C:/IndicesServerConfig.xml", false);

			ServiceBase[] ServicesToRun;
			ServicesToRun = new ServiceBase[] 
			{ 
				new IndicesService() 
			};
			ServiceBase.Run(ServicesToRun);
		}
	}
}
