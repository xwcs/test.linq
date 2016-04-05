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
using WF2.db.model1;

namespace WF2
{
	public partial class Form2 : Form
	{

		test1Entities ctx;
		test1Entities1 ctx1;

		xwcs.core.db.binding.DataLayoutBindingSource bs;

		public Form2()
		{

			InitializeComponent();

			ctx = new test1Entities();
			ctx1 = new test1Entities1();

			

			ctx.Database.Log = xwcs.core.manager.SLogManager.getInstance().Debug; // Console.Write;

			
			bs = new xwcs.core.db.binding.DataLayoutBindingSource();
			bs.DataSource = ctx.bab.Where(s => s.id == 100).ToList();
			

			bs.GetFieldQueryable += (sender, d) =>
			{
				switch (d.FieldName)
				{
					case "cc":
						d.DataSource = ctx1.names.ToList();
						break;
				}
			};

			bs.DataLayout = dataLayoutControl1;			
			


		}

		private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
		{
			object snapShot = bs.Current;

			ctx.SaveChanges();

			bs.DataSource = ctx.bab.Where(s => s.id == 200).ToList();
		}
	}
}
