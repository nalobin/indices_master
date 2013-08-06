using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using FinamImport;

namespace IndicesServer
{
	public partial class IndicesService : ServiceBase
	{
		public IndicesService()
		{
			InitializeComponent();
		}

		protected override void OnStart(string[] args)
		{
		}

		protected override void OnStop()
		{
		}
	}
}
