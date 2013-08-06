using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FinamImport;
using System.Collections;
using System.Runtime.Remoting;

namespace Indices_Master
{
    public partial class IndicesForm : Form
    {
		protected Hashtable _alert_limits;

        static System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();

		protected SharedLoader _loader;

		private void updateIndices()
		{
			Queue data = _loader.getData();

			DataGridView.Rows.Clear();

			foreach (Queue row in data)
			{
				string name = (string)row.Dequeue();
		
				string[] cells = new string[row.Count];
				int i = row.Count - 1;
				foreach(string cell in row)
				{
					cells[i--] = cell;
				}
				
				int index = DataGridView.Rows.Add(cells);
				DataGridView.Rows[index].HeaderCell.Value = name;
			}

			// раскрашиваем ячейки таблицы
			foreach (DataGridViewRow r in DataGridView.Rows)
			{
				bool alert_shown = false;

				for (int i = r.Cells.Count - 1; i >= 0; i--)
				{
					DataGridViewCell cell = r.Cells[i];
					if (cell.ColumnIndex == -1)
						continue;

					string value_string = (string)cell.Value;

					if (value_string == null || value_string.Length == 0)
						continue;

					double value = Convert.ToDouble(value_string.Replace('.', ','));

					value_string = String.Format("{0:F2}", value);

					if (value > 0)
					{
						value_string = '+' + value_string;
					}

					value_string += '%';

					cell.Value = value_string;

					int red = 0, green = 0;
					if (value < -0.01 || value > 0.01)
					{
						if (value > 0)
						{
							green = (int)(155 * value / 0.5) + 100;
							if (green > 255)
								green = 255;
						}
						else
						{
							red = (int)(-55 * value / 0.5) + 200;
							if (red > 255)
								red = 255;
						}

						cell.Style.BackColor = Color.FromArgb(0xFF, red, green, 0);
					}

					if (!alert_shown
					 && _alert_limits.Contains(r.HeaderCell.Value))
					{
						double[] limits = (double[])_alert_limits[r.HeaderCell.Value];
						double limit = (double)limits[cell.ColumnIndex];
						double limit_neg = (1 - 100 / (100 + limit)) * 100;
						if (limit != 0
						 && (value <= -limit_neg || value >= limit))
						{
							AlertForm alert_form = new AlertForm(this, "Индекс " + r.HeaderCell.Value
							 + (value < 0 ? " упал на " : " вырос на ") + value_string
							 + " за " + DataGridView.Columns[cell.ColumnIndex].Name, value < 0 ? Color.Red : Color.Green
							);

							alert_form.Show();

							alert_shown = true;
						}
					}
				}
			}

			DataGridView.ClearSelection();
			DataGridView.AutoResizeColumns();
		}

		private void timerEventProcessor(Object t_object, EventArgs event_args)
        {
			updateIndices();
        }

        public IndicesForm()
        {
            InitializeComponent();
        }

        private void IndicesForm_Load(object sender, EventArgs e)
        {
			Rectangle wa = SystemInformation.WorkingArea;
			Location = new Point();

            const int initial_rows = 6;

            int height = (int)DataGridView.Font.GetHeight() * ( initial_rows + 2 )
			 + SystemInformation.Border3DSize.Height * 2  + SystemInformation.CaptionHeight;
			int width = Size.Width;

			SetBounds(0, wa.Bottom - height, width, height);

            // RemotingConfiguration.Configure("remoting_config.xml", false);

			_loader = new SharedLoader();

			uint[] columns = Loader.getMinuteIntervals();
			columns = columns.Reverse().ToArray();

			int minutes_1_pos = -1;
			int minutes_2_pos = -1;
			int minutes_3_pos = -1;
	
			foreach (uint minutes in columns)
			{
				string name = "";
				if (minutes < 60)
					name = Convert.ToString(minutes) + " мин";
				else if (minutes < 1440)
					name = Convert.ToString(minutes / 60) + " час";
				else
					name = Convert.ToString(minutes / 1440) + " день";

				int c = DataGridView.Columns.Add(name, name);
				DataGridView.Columns[c].Frozen = true;
				DataGridView.Columns[c].Resizable = DataGridViewTriState.NotSet;
				DataGridView.Columns[c].SortMode = DataGridViewColumnSortMode.NotSortable;			

				if(minutes == 1)
					minutes_1_pos = c;
				else if (minutes == 2)
					minutes_2_pos = c;
				else if (minutes == 3)
					minutes_3_pos = c;
			}

			_alert_limits = new Hashtable();

			double[] limits = new double[columns.Length];

			if (-1 != minutes_1_pos)
				limits[minutes_1_pos] = 0.15;
			if (-1 != minutes_2_pos)
				limits[minutes_2_pos] = 0.25;
			if (-1 != minutes_3_pos)
				limits[minutes_3_pos] = 0.42;
			_alert_limits.Add("SPFUT", limits);

			limits = new double[columns.Length];

			if (-1 != minutes_1_pos)
				limits[minutes_1_pos] = 0.3;
			if (-1 != minutes_2_pos)
				limits[minutes_2_pos] = 0.5;
			if (-1 != minutes_3_pos)
				limits[minutes_3_pos] = 0.83333;
			_alert_limits.Add("MICEX", limits);

			updateIndices();

			timer.Tick += new EventHandler(timerEventProcessor);

			timer.Interval = 40000;
			timer.Start();
        }
    }
}
