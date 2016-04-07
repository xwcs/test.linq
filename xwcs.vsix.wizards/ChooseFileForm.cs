using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace xwcs.vsix.wizards
{
	public partial class ChooseFileForm : Form
	{
		/*
		 */
		public string EdmxFileName { get { return textBox1.Text; } set { textBox1.Text = value; } }
		protected string _rootPath;
		protected string _filter;
		public bool Confirmed { get; set; } = false;

		public ChooseFileForm(string rootPath, string filter)
		{
			_rootPath = rootPath;
			_filter = filter;
			InitializeComponent();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			
			openFileDialog1.InitialDirectory = _rootPath;
			openFileDialog1.Filter = _filter;
			openFileDialog1.FileName = EdmxFileName;

			if(DialogResult.OK == openFileDialog1.ShowDialog()) {
				EdmxFileName = Path.GetFileName(openFileDialog1.FileName);
			}			
		}

		private void button2_Click(object sender, EventArgs e)
		{
			Confirmed = true;
			this.Close();
		}
	}
}
