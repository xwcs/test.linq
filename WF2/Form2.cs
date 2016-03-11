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

		public Form2()
		{

			//hyper handling
			//HyperTypeDescriptionProvider.Add(typeof(bab));

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
			bsg.DataSource = (from o in ctx.bab_local where (o.bab.extra != null || o.bab.extra.CompareTo("[xml,/bab/text]=scor*") == 0) select new { o.bab.ndoc, o.id, o.random, o.bab.dictionary }).ToList();
			gridControl1.DataSource = bsg;
			

			xwcs.core.ui.datalayout.DataLayoutBindingSource bs = new xwcs.core.ui.datalayout.DataLayoutBindingSource();
			//bs.DataSource = ctx.bab.Where(s => s.id == 100).ToList();

			bs.GetFieldQueryable += (sender, d) =>
			{
				switch (d.FieldName)
				{
					case "cc":
						d.DataSource = ctx1.names.ToList();
						break;
				}
			};

			/*
			DevExpress.XtraDataLayout.DataLayoutControlExt dlc = new DevExpress.XtraDataLayout.DataLayoutControlExt();
			dlc.Dock = DockStyle.Fill;
			splitContainerControl1.Panel2.Controls.Add(dlc);
			//xwcs.core.ui.datalayout.DataLayoutExtender de = new xwcs.core.ui.datalayout.DataLayoutExtender(dlc);
			*/

			bs.DataLayout = dataLayoutControl1;
			//dataLayoutControl1.AllowGeneratingNestedGroups = DevExpress.Utils.DefaultBoolean.True;

			/*
			PropertyDescriptorCollection pdc = TypeDescriptor.GetProperties(typeof(bab));
			ChainingPropertyDescriptor tdd = (ChainingPropertyDescriptor)pdc["Bab_Ext"];
			if (tdd != null)
			{
				tdd.ForcedPropertyType = typeof(bab_ext); // ((bab)bs.Current).Bab_Ext.GetType();
			}
			*/

			//dataLayoutControl1.DataSource = bs;

			//bs.AddNew();




			/*
			PropertyDescriptorCollection pdc = TypeDescriptor.GetProperties(typeof(bab));
            ChainingPropertyDescriptor tdd = (ChainingPropertyDescriptor)pdc["Bab_Ext"];
			if (tdd != null)
			{
				tdd.ForcedPropertyType = typeof(bab_ext); // ((bab)bs.Current).Bab_Ext.GetType();
			}
			*/
			bs.DataSource = ctx.bab.Where(s => s.id == 100).ToList();
			bs.DataSource = ctx.bab.Where(s => s.id == 200).ToList();


		}		
	}
}
