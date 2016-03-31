using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WF2.db.model1;

namespace WF2
{
	public partial class Form1 : Form
	{
		test1Entities ctx;
		test1Entities gridCtx;
		int currentId;

		public Form1()
		{
			InitializeComponent();

			gridControl1.Dock = DockStyle.Fill;

			ctx = new test1Entities();

			ctx.Database.Log = Console.Write;
			ctx.Database.CommandTimeout = 600;

			entityInstantFeedbackSource1.GetQueryable += EntityInstantFeedbackSource1_GetQueryable;
			entityInstantFeedbackSource1.DismissQueryable += EntityInstantFeedbackSource1_DismissQueryable;
			gridControl1.DataSourceChanged += GridControl1_DataSourceChanged;
			gridControl1.DataSource = entityInstantFeedbackSource1;

			//data layout
			dataLayoutControl1.DataSource = bindingSource1;
			dataLayoutControl1.FieldRetrieving += DataLayoutControl1_FieldRetrieving;
			dataLayoutControl1.RetrieveFields();

		}

		private void DataLayoutControl1_FieldRetrieving(object sender, DevExpress.XtraDataLayout.FieldRetrievingEventArgs e)
		{
			e.DataSourceUpdateMode = DataSourceUpdateMode.OnPropertyChanged;
			e.Handled = true;
		}

		private void GridControl1_DataSourceChanged(object sender, EventArgs e)
		{
			gridControl1.MainView.PopulateColumns();
			(gridControl1.MainView as DevExpress.XtraGrid.Views.Grid.GridView).BestFitColumns();
		}

		private void EntityInstantFeedbackSource1_DismissQueryable(object sender, DevExpress.Data.Linq.GetQueryableEventArgs e)
		{
			((test1Entities)e.Tag).Dispose();
		}

		private void EntityInstantFeedbackSource1_GetQueryable(object sender, DevExpress.Data.Linq.GetQueryableEventArgs e)
		{
			gridCtx = new test1Entities();
			gridCtx.Database.Log = Console.Write;
			e.QueryableSource = (from o in gridCtx.bab select new bab_view_data { ndoc = o.ndoc, id = o.id, random = o.random, dictionary = o.dictionary });//final;
			e.Tag = gridCtx;
		}

		private void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
		{
			if(gridView1.IsAsyncInProgress) return;
			try
			{

				object row = gridView1.GetRow(gridView1.FocusedRowHandle);
				bab_view_data b = null;
				if (row is DevExpress.Data.NotLoadedObject) return;
				if (row is DevExpress.Data.Async.Helpers.ReadonlyThreadSafeProxyForObjectFromAnotherThread)
				{
					b = (bab_view_data)((DevExpress.Data.Async.Helpers.ReadonlyThreadSafeProxyForObjectFromAnotherThread)row).OriginalRow;
				}
				if (b != null)
				{
					currentId = b.id;
					var query = ctx.bab.Where(s => s.id == b.id);
					bindingSource1.DataSource = query.ToList();
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("Error : " + ex.Message);
			}
		}

		private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
		{
			try
			{
				ctx.SaveChanges();
				Console.WriteLine("Child Saved !!!");
			}
			catch (System.Data.Entity.Infrastructure.DbUpdateConcurrencyException)
			{
				MessageBox.Show("Ogetto modificato dal altro utente!");
			}
			catch (System.Data.Entity.Validation.DbEntityValidationException ve) {
				MessageBox.Show(ve.Message);
			}catch(Exception ex) {
				Console.WriteLine("Save problem:" + ex.Message);
			}

			(gridCtx as System.Data.Entity.Infrastructure.IObjectContextAdapter).ObjectContext.Refresh(System.Data.Entity.Core.Objects.RefreshMode.StoreWins, gridCtx.bab.Where(s => s.id == currentId));
			entityInstantFeedbackSource1.Refresh();
		}

		private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
		{
			Form2 f = new Form2();

			f.Show(this);
		}
	}
}
