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
using Hyper.ComponentModel;

namespace WF2
{
	public partial class Form2 : Form
	{

		test1Entities ctx;
		test1Entities1 ctx1;

		xwcs.core.ui.datalayout.DataLayoutBindingSource bs;

		public Form2()
		{

			//hyper handling
			//HyperTypeDescriptionProvider.Add(typeof(bab));

			InitializeComponent();

			ctx = new test1Entities();
			ctx1 = new test1Entities1();

			

			ctx.Database.Log = xwcs.core.manager.SLogManager.getInstance().Debug; // Console.Write;

			
			gridControl1.DataSourceChanged += (s, e) =>
			{
				gridControl1.MainView.PopulateColumns();
				(gridControl1.MainView as DevExpress.XtraGrid.Views.Grid.GridView).BestFitColumns();
			};

			BindingSource bsg = new BindingSource();
			bsg.DataSource = (from o in ctx.bab_local where (o.bab.extra != null || o.bab.extra.CompareTo("[xml,/bab/text]=scor*") == 0) select new { o.bab.ndoc, o.id, o.random, o.bab.dictionary }).ToList();
			gridControl1.DataSource = bsg;
			

			bs = new xwcs.core.ui.datalayout.DataLayoutBindingSource();
			bs.DataSource = ctx.bab.Where(s => s.id == 100).ToList();
			//bs.AddNew();
			//set something to Current
			//bs.SetProperty("Bab_Ext", new bab_ext_1());
			//(bs.Current as bab).Bab_Ext_str = "WF2.bab_ext_1\n<?xml version=\"1.0\" encoding=\"utf-16\" ?><bab_ext_1 xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"><n_rpa>ffffff</n_rpa><n_cc>ddddd</n_cc><n_dxp>aaaaaaaa</n_dxp></bab_ext_1>";
			//bs.SetProperty("Bab_Ext_str", "WF2.bab_ext_1\n<?xml version=\"1.0\" encoding=\"utf-16\" ?><bab_ext_1 xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"><n_rpa>ffffff</n_rpa><n_cc>ddddd</n_cc><n_dxp>aaaaaaaa</n_dxp></bab_ext_1>");
			//bs.SetProperty("Bab_Ext_str", "WF2.bab_ext\n<?xml version=\"1.0\" encoding=\"utf-16\" ?><bab_ext xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"><n_rpa>ffffff</n_rpa><n_cc>ddddd</n_cc><n_dxp>aaaaaaaa</n_dxp></bab_ext>");

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
