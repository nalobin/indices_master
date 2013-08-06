namespace Indices_Master
{
    partial class IndicesForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
			this.DataGridView = new System.Windows.Forms.DataGridView();
			((System.ComponentModel.ISupportInitialize)(this.DataGridView)).BeginInit();
			this.SuspendLayout();
			// 
			// DataGridView
			// 
			this.DataGridView.AllowUserToAddRows = false;
			this.DataGridView.AllowUserToDeleteRows = false;
			this.DataGridView.AllowUserToResizeColumns = false;
			this.DataGridView.AllowUserToResizeRows = false;
			this.DataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
			this.DataGridView.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
			this.DataGridView.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.DataGridView.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Sunken;
			dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle1.Font = new System.Drawing.Font("Verdana", 7.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
			this.DataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
			this.DataGridView.ColumnHeadersHeight = 24;
			this.DataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.DataGridView.Location = new System.Drawing.Point(0, 0);
			this.DataGridView.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.DataGridView.MultiSelect = false;
			this.DataGridView.Name = "DataGridView";
			this.DataGridView.ReadOnly = true;
			this.DataGridView.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToDisplayedHeaders;
			this.DataGridView.RowTemplate.Height = 24;
			this.DataGridView.RowTemplate.ReadOnly = true;
			this.DataGridView.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.DataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
			this.DataGridView.ShowCellErrors = false;
			this.DataGridView.ShowEditingIcon = false;
			this.DataGridView.Size = new System.Drawing.Size(742, 102);
			this.DataGridView.TabIndex = 0;
			// 
			// IndicesForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 14F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(742, 102);
			this.Controls.Add(this.DataGridView);
			this.Font = new System.Drawing.Font("Verdana", 7.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.MaximizeBox = false;
			this.Name = "IndicesForm";
			this.Opacity = 0.9;
			this.Text = "Indices Master";
			this.TopMost = true;
			this.Load += new System.EventHandler(this.IndicesForm_Load);
			((System.ComponentModel.ISupportInitialize)(this.DataGridView)).EndInit();
			this.ResumeLayout(false);

        }

        #endregion

		private System.Windows.Forms.DataGridView DataGridView;
    }
}

