namespace Indices_Master
{
	partial class AlertForm
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
			this.RichTextBox = new System.Windows.Forms.RichTextBox();
			this.SuspendLayout();
			// 
			// RichTextBox
			// 
			this.RichTextBox.BackColor = System.Drawing.SystemColors.Control;
			this.RichTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.RichTextBox.Font = new System.Drawing.Font("Verdana", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.RichTextBox.Location = new System.Drawing.Point(0, 0);
			this.RichTextBox.Name = "RichTextBox";
			this.RichTextBox.ReadOnly = true;
			this.RichTextBox.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
			this.RichTextBox.Size = new System.Drawing.Size(178, 57);
			this.RichTextBox.TabIndex = 0;
			this.RichTextBox.Text = "";
			this.RichTextBox.DoubleClick += new System.EventHandler(this.RichTextBox_DoubleClick);
			this.RichTextBox.Click += new System.EventHandler(this.RichTextBox_DoubleClick);
			// 
			// AlertForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(178, 57);
			this.Controls.Add(this.RichTextBox);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "AlertForm";
			this.Opacity = 0.9;
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "Оповещение";
			this.TopMost = true;
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.RichTextBox RichTextBox;
	}
}