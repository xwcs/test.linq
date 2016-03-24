using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors.Repository;
using Hyper.ComponentModel;
using DevExpress.Utils;
using DevExpress.Data.Linq;
using System.Data.Entity.Infrastructure;

namespace WF2
{
	public partial class Form5 : Form
	{

		

		niterEntities ctx;
		niterEntities ctxRow;

		xwcs.core.ui.datalayout.DataLayoutBindingSource bsg;
		EntityInstantFeedbackSource eifs;

		int currentRowId = -1 ;
		object currentObj;

		public Form5()
		{

			InitializeComponent();

			ctxRow = new niterEntities();
			ctxRow.Database.Log = xwcs.core.manager.SLogManager.getInstance().Debug;

			gridControl1.DataSourceChanged += (sender, e) =>
			{
				gridControl1.MainView.PopulateColumns();
				(gridControl1.MainView as DevExpress.XtraGrid.Views.Grid.GridView).BestFitColumns();
			};

			bsg = new xwcs.core.ui.datalayout.DataLayoutBindingSource();
			bsg.DataLayout = dataLayoutControl1;

			eifs = new EntityInstantFeedbackSource();
			eifs.GetQueryable += (object s, GetQueryableEventArgs e) => {
				ctx = new niterEntities();
				ctx.Database.Log = xwcs.core.manager.SLogManager.getInstance().Debug; // Console.Write;
				e.QueryableSource = (from o in ctx.labels where o.tipolabel_tipo == "classif" select o);
                e.Tag = ctx;
			};
			eifs.DismissQueryable += (s,e) => {
				((niterEntities)e.Tag).Dispose();
			};
			
			gridControl1.DataSource = eifs;
			(gridControl1.MainView as DevExpress.XtraGrid.Views.Grid.GridView).FocusedRowChanged += (sender, evt) =>
			{
				if (gridView1.IsAsyncInProgress) return;
				try
				{

					object row = gridView1.GetRow(gridView1.FocusedRowHandle);
					labels b = null;
					if (row is DevExpress.Data.NotLoadedObject) return;
					if (row is DevExpress.Data.Async.Helpers.ReadonlyThreadSafeProxyForObjectFromAnotherThread)
					{
						currentObj = ((DevExpress.Data.Async.Helpers.ReadonlyThreadSafeProxyForObjectFromAnotherThread)row).OriginalRow;
						b = (labels)currentObj;
					}
					if (b != null)
					{
						if(currentRowId != b.id) {

							currentRowId = b.id;
							bsg.DataSource = ctxRow.labels.Where(s => s.id == b.id).ToList();
							Console.WriteLine("Current row:" + currentRowId);
						}						
					}
				}
				catch (Exception ex)
				{
					Console.WriteLine("Error : " + ex.Message);
				}
				

			};
		}

		private void saveRow() {

			bool saveFailed;
			do
			{
				saveFailed = false;
				try
				{
					ctxRow.SaveChanges();
					(ctxRow as IObjectContextAdapter).ObjectContext.Refresh(System.Data.Entity.Core.Objects.RefreshMode.StoreWins, bsg.Current);// ctxRow.labels.Where(s => s.id == currentRowId));
					(ctx as IObjectContextAdapter).ObjectContext.Refresh(System.Data.Entity.Core.Objects.RefreshMode.StoreWins, currentObj); // ctx.labels.Where(s => s.id == currentRowId));
					//eifs.Refresh();
					//bsg.DataSource = ctxRow.labels.Where(s => s.id == currentRowId).ToList();
				}
				catch (DbUpdateConcurrencyException ex)
				{
					saveFailed = true;
					MessageBox.Show("Ogetto modificato dal altro utente! Record vera ricaricato!");

					// Get the current entity values and the values in the database 
					// as instances of the entity type 
					var entry = ex.Entries.Single();
					var databaseValues = entry.GetDatabaseValues();
					var databaseValuesAsLabels = (labels)databaseValues.ToObject();

					// Choose an initial set of resolved values. In this case we 
					// make the default be the values currently in the database. 
					var resolvedValuesAsLabels = (labels)databaseValues.ToObject();

					// Have the user choose what the resolved values should be 
					HaveUserResolveConcurrency((labels)entry.Entity,
											   databaseValuesAsLabels,
											   resolvedValuesAsLabels);

					// Update the original values with the database values and 
					// the current values with whatever the user choose. 
					entry.OriginalValues.SetValues(databaseValues);
					entry.CurrentValues.SetValues(resolvedValuesAsLabels);
				}
				catch (System.Data.Entity.Validation.DbEntityValidationException ve)
				{
					saveFailed = true;
                    MessageBox.Show(ve.Message);

				}
				catch (Exception ex)
				{
					saveFailed = true;
					Console.WriteLine("Save problem:" + ex.Message);
				}

			} while (saveFailed);

			
		}

		public void HaveUserResolveConcurrency(labels entity,
									   labels databaseValues,
									   labels resolvedValues)
		{
			resolvedValues = databaseValues;
			// Show the current, database, and resolved values to the user and have 
			// them update the resolved values to get the correct resolution. 
		}

		private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
		{
			saveRow();
        }
	}
}
