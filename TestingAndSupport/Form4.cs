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
using DevExpress.Utils;
using WF2.db.model1;

namespace WF2
{
	public partial class Form4 : Form
	{

		

		test1Entities ctx;
		test1Entities1 ctx1;

		BindingSource bsg;
		xwcs.core.db.binding.DataLayoutBindingSource bs;

		int currentRowId = -1;

		public Form4()
		{

			InitializeComponent();

			ctx = new test1Entities();
			ctx1 = new test1Entities1();

			

			ctx.Database.Log = xwcs.core.manager.SLogManager.getInstance().Debug; // Console.Write;

			
			gridControl1.DataSourceChanged += (sender, e) =>
			{
				gridControl1.MainView.PopulateColumns();
				(gridControl1.MainView as DevExpress.XtraGrid.Views.Grid.GridView).BestFitColumns();

			
				if (currentRowId != (bsg.Current as GridRow).id)
				{
					//save old
					ctx.SaveChanges();
					currentRowId = (bsg.Current as GridRow).id;
					// reload record
					bs.DataSource = ctx.bab.Where(s => s.id == currentRowId).ToList();

				}
			};


			bsg = new BindingSource();
			bs = new xwcs.core.db.binding.DataLayoutBindingSource();
			bs.DataLayout = dataLayoutControl1;

			dataLayoutControl1.GotFocus += (sender, evt) => {
			};

			bsg.DataSource = (from o in ctx.bab_local where (o.bab.extra != null || o.bab.extra.CompareTo("[xml,/bab/dictionary]=scor*") == 0) select new GridRow { ndoc = o.bab.ndoc, id = o.id, random = o.random, dict = o.bab.dictionary }).ToList();
			gridControl1.DataSource = bsg;
			(gridControl1.MainView as DevExpress.XtraGrid.Views.Grid.GridView).FocusedRowChanged += (sender, evt) =>
			{
				if (currentRowId != (bsg.Current as GridRow).id)
				{
					//save old
					ctx.SaveChanges();
					currentRowId = (bsg.Current as GridRow).id;
					// reload record
					bs.DataSource = ctx.bab.Where(s => s.id == currentRowId).ToList();
					//dataLayoutControl1.FindElement(sender as FrameworkElement, elem => elem is DataLayoutItem && ((DataLayoutItem)elem).Label.ToString() == "Field2")).Content.Focus();
				}
			};
			//the same BS only if not morph able
			//bsg.DataLayout = dataLayoutControl1;


			

			/*
			bsg.CurrentChanged += (sender, evt) =>
			{
				if (currentRowId != (bsg.Current as GridRow).id)
				{
					//save old
					ctx.SaveChanges();
					currentRowId = (bsg.Current as GridRow).id;
					// reload record
					bs.DataSource = ctx.bab.Where(s => s.id == currentRowId).ToList();

				}
			};
			*/
			





		}

		private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
		{
			//ctx.SaveChanges();
        }

		private void dataLayoutControl1_Enter(object sender, EventArgs e)
		{
		}
	}
}
