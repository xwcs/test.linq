using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace WF2
{
	public class bab_meta
	{
		const string MainGroup = "{tabs}";
		const string Resp = MainGroup + "/Responsabilita";
		const string MG_Main = MainGroup + "/<Doc>/Main";
		const string MG_Parole = MainGroup + "/<Doc>/Parole";
		const string MG_Testo = MainGroup + "/<Doc>/Testo";

		[Display(Name = "n° Doc", GroupName = MG_Main + "/<IdRandom->", Order = 1)]
		[System.ComponentModel.ReadOnly(true)]
		public int ndoc { get; set; }

		[xwcs.core.DataLayout.Attributes.Style(BackgrounColor = 0xFFADFF2F)]
		[Display(Name = "Id", GroupName = MG_Main + "/<IdRandom->")]
		[System.ComponentModel.ReadOnly(true)]
		public string id { get; set; }

		[xwcs.core.DataLayout.Attributes.Style(BackgrounColor = 0xFFADFF2F)]
		[Display(Name = "Random", GroupName = MG_Main + "/<IdRandom->")]
		[Range(0, 1000, ErrorMessage = "Fuori range!")]
		public int random { get; set; }
		[Display(Name = "Parole", GroupName = MG_Parole)]
		public string dictionary { get; set; }
		[Display(Name = "Testo", GroupName = MG_Parole), DataType(DataType.MultilineText)]
		public string text { get; set; }
		[Display(Name = "RPA", GroupName = Resp)]
		public string rpa { get; set; }
		[Display(Name = "CC", GroupName = Resp)]
		[xwcs.core.DataLayout.Attributes.DbLookup(DisplayMember = "Name", ValueMember = "Name")]
		public string cc { get; set; }
		[Display(Name = "DXP", GroupName = Resp)]
		public string dxp { get; set; }
		[System.ComponentModel.ReadOnly(true)]
		[Display(Name = "xml", GroupName = MainGroup + "/XML"), DataType(DataType.MultilineText)]
		public string xml { get; set; }
		[Display(AutoGenerateField = false)]
		public string extra { get; set; }
	}

	public class bab_ext {
		public const UInt32 greenColor = 0xff008000;

		[xwcs.core.DataLayout.Attributes.Style(BackgrounColor = greenColor)]
		[Display(Name = "N_RPA", GroupName = "n")]
		public string n_rpa { get; set; }
		[Display(Name = "N_CC", GroupName = "n")]
		public string n_cc { get; set; }
		[xwcs.core.DataLayout.Attributes.Style(BackgrounColor = greenColor)]
		[Display(Name = "N_DXP", GroupName = "n")]
		public string n_dxp { get; set; }
		[xwcs.core.DataLayout.Attributes.Style(BackgrounColor = 0xFFADFF2F)]
		[Display(Name = "N_Date", GroupName = "n")]
		public DateTime n_data { get; set; }
	}

	[MetadataType(typeof(bab_meta))]
	public partial class bab {

		[Display(Name = "Embeded", GroupName = "{tabs}/Embeded", Order = 4)]
		public bab_ext Bab_Ext{get ; set; }
	}




	public partial class bab_view_data
	{
		[Key]
		[Display(Name = "n° Doc")]
		public int ndoc { get; set; }
		[Display(Name = "Id")]
		public string id { get; set; }
		[Display(Name = "Random")]
		public int random { get; set; }
		[Display(Name = "Parole", GroupName = "{tabs}")]
		public string dictionary { get; set; }


	}
}
