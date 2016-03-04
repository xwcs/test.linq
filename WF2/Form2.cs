using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors.Repository;

namespace WF2
{
	public partial class Form2 : Form
	{

		test1Entities1 ctx;
		test1Entities ctx1;
		
		public Form2()
		{
			
			InitializeComponent();

			ctx = new test1Entities1();
			ctx1 = new test1Entities();
			
			

			BindingSource bs = new BindingSource();
			bs.DataSource = ctx1.bab.Where(s => s.id == "00000100").ToList();// typeof(bab);
			//bs.AddNew();

			xwcs.core.DataLayout.DataLayoutExtender de = new xwcs.core.DataLayout.DataLayoutExtender(dataLayoutControl1);
			de.GetFieldQueryable += (sender, d) =>
			{
				switch(d.FieldName) {
					case "cc":
						d.DataSource = ctx.names.ToList();
						break;
                }
			};
			
            dataLayoutControl1.DataSource = bs;

			
		}		
	}
}
