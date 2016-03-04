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

		test1Entities ctx;
		test1Entities1 ctx1;
		
		public Form2()
		{
			
			InitializeComponent();

			ctx = new test1Entities();
			ctx1 = new test1Entities1();

			ctx.Database.Log = Console.Write;
			gridControl1.DataSourceChanged += (s, e) =>
			{
				gridControl1.MainView.PopulateColumns();
				(gridControl1.MainView as DevExpress.XtraGrid.Views.Grid.GridView).BestFitColumns();
			};

			BindingSource bsg = new BindingSource();
			bsg.DataSource =(from o in ctx.bab_local where (o.bab.extra != null || o.bab.extra.CompareTo("[xml,/bab/text]=scor*") == 0) select new { o.ndoc, o.id, o.random, o.bab.dictionary }).ToList();
			gridControl1.DataSource = bsg;
			

			BindingSource bs = new BindingSource();
			bs.DataSource = ctx.bab.Where(s => s.id == "00000100").ToList();// typeof(bab);
			//bs.AddNew();

			xwcs.core.ui.datalayout.DataLayoutExtender de = new xwcs.core.ui.datalayout.DataLayoutExtender(dataLayoutControl1);
			de.GetFieldQueryable += (sender, d) =>
			{
				switch(d.FieldName) {
					case "cc":
						d.DataSource = ctx1.names.ToList();
						break;
                }
			};
			
            dataLayoutControl1.DataSource = bs;

			
		}		
	}
}
