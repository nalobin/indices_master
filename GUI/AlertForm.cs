using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Indices_Master
{
	public partial class AlertForm : Form
	{
		protected Timer _timer;
		protected Form _parent;

		private void timerEventProcessor(Object t_object, EventArgs event_args)
		{
			// закрываем форму
			Close();
			Dispose();
		}

		public AlertForm(Form parent, string text, Color back_color)
		{
			InitializeComponent();

			_parent = parent;

			Rectangle wa = SystemInformation.WorkingArea;

			int height = (int)(RichTextBox.Font.GetHeight() * 3.5)
			 + SystemInformation.Border3DSize.Height * 2 + SystemInformation.CaptionHeight;

			SetBounds(wa.Right - Size.Width, wa.Bottom - height, Size.Width, height);

			RichTextBox.Text = text;

			RichTextBox.BackColor = back_color;

			_timer = new System.Windows.Forms.Timer();

			_timer.Tick += new EventHandler(timerEventProcessor);

			_timer.Interval = 10000;
			_timer.Start();
		}

		private void RichTextBox_DoubleClick(object sender, EventArgs e)
		{
			_parent.WindowState = FormWindowState.Normal;
			_parent.Activate();
		}
	}
}
