using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Security.Permissions;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;
using System.Security;
using System.Xml.Serialization;
using System.IO;

namespace WF2
{
	static class ExtensionMethods
	{
		public static BindingList<T> ToBindingList<T>(this IEnumerable<T> range)
		{
			return new BindingList<T>(range.ToList());
		}

		
		public static string DumpToTypedXml(this object objectInstance)
		{
			var serializer = new XmlSerializer(objectInstance.GetType());
			var sb = new StringBuilder();

			using (TextWriter writer = new StringWriter(sb))
			{
				writer.WriteLine(objectInstance.GetType().FullName);
				serializer.Serialize(writer, objectInstance);
			}

			return sb.ToString();
		}

		public static object CreateObjectFromTypedXml(this string objectData)
		{
			if (objectData.Length == 0) return null;	

			object result;

			using (TextReader reader = new StringReader(objectData))
			{
				var serializer = new XmlSerializer(Type.GetType(reader.ReadLine()));
				result = serializer.Deserialize(reader);
			}

			return result;
		}

		
	}

	/*

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

		[xwcs.core.ui.datalayout.attributes.Style(BackgrounColor = 0xFFADFF2F)]
		[Display(Name = "Id", GroupName = MG_Main + "/<IdRandom->")]
		[System.ComponentModel.ReadOnly(true)]
		public int id { get; set; }

		[xwcs.core.ui.datalayout.attributes.Style(BackgrounColor = 0xFFADFF2F)]
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
		[xwcs.core.ui.datalayout.attributes.DbLookup(DisplayMember = "Name", ValueMember = "Name")]
		public string cc { get; set; }
		[Display(Name = "DXP", GroupName = Resp)]
		public string dxp { get; set; }
		[System.ComponentModel.ReadOnly(true)]
		[Display(Name = "xml", GroupName = MainGroup + "/XML"), DataType(DataType.MultilineText)]
		public string xml { get; set; }
		[Display(AutoGenerateField = false)]
		public string extra { get; set; }

		[Display(Name = "Content", GroupName = MG_Parole), DataType(DataType.MultilineText)]
		public string content { get; set; }

		[Display(AutoGenerateField = false)]
		public bab_local bab_local { get; set; }
	}

	public class bab_ext {
		public const UInt32 greenColor = 0xff008000;

		[xwcs.core.ui.datalayout.attributes.Style(BackgrounColor = greenColor)]
		[Display(Name = "N_RPA", GroupName = "n")]
		public string n_rpa { get; set; }
		[Display(Name = "N_CC", GroupName = "n"), Required()]
		public string n_cc { get; set; }
		[xwcs.core.ui.datalayout.attributes.Style(BackgrounColor = greenColor)]
		[Display(Name = "N_DXP", GroupName = "n"), DataType(DataType.MultilineText)]
		public string n_dxp { get; set; }
		[xwcs.core.ui.datalayout.attributes.Style(BackgrounColor = 0xFFADFF2F)]
		[Display(Name = "N_Date", GroupName = "n")]
		public DateTime n_data { get; set; }
	}


	public class bab_ext_1
	{
		public const UInt32 greenColor = 0xff008000;

		[xwcs.core.ui.datalayout.attributes.Style(BackgrounColor = greenColor)]
		[Display(Name = "N_RPA", GroupName = "n")]
		public string n_rpa { get; set; }
		[Display(Name = "N_CC", GroupName = "n")]
		public string n_cc { get; set; }
		[xwcs.core.ui.datalayout.attributes.Style(BackgrounColor = greenColor)]
		[Display(Name = "N_DXP", GroupName = "n")]
		public string n_dxp { get; set; }
	}

	//[TypeDescriptionProvider(typeof(SomeTypeDescriptionProvider<bab>))]
	[MetadataType(typeof(bab_meta))]
	public partial class bab {

		[System.ComponentModel.DataAnnotations.Schema.NotMapped]
		[Display(Name = "Embeded", GroupName = "{tabs}/Embeded", Order = 4)]
		[xwcs.core.ui.datalayout.attributes.PolymorphFlag(Kind = xwcs.core.ui.datalayout.attributes.PolymorphKind.XmlSerialization, SourcePropertyName = "content")]
		public object Bab_Ext { get; set; } = null; // new bab_ext_1();

		public bab() {
			//this.P
		}

		/*

		[//System.ComponentModel.ReadOnly(true),
		 Display(Name = "Bab_Ext_str", GroupName = "{tabs}/XML_STR"), DataType(DataType.MultilineText)]
		public string Bab_Ext_str { get; set; }
		/*
			get
			{
				//obtain serialized version of Bab_Ext
				if(Bab_Ext != null) {
					return Bab_Ext.DumpToTypedXml();
				}
				return "";
			}
			set {
				// this will de-serialize
				Bab_Ext = value.CreateObjectFromTypedXml();
				
			}
		}
		* /	
	}


*/

	public partial class bab_view_data
	{
		[Key]
		[Display(Name = "n° Doc")]
		public int ndoc { get; set; }
		[Display(Name = "Id")]
		public int id { get; set; }
		[Display(Name = "Random")]
		public int random { get; set; }
		[Display(Name = "Parole", GroupName = "{tabs}")]
		public string dictionary { get; set; }
	}

	public class GridRow
	{
		public int ndoc { get; set; }
		public int id { get; set; }
		public int random { get; set; }
		public string dict { get; set; }
	}

	

	namespace db {
		[XmlInclude(typeof(TestObj))]
		public class XmlMorphable
		{

		}

		public class TestObj // : XmlMorphable
		{
			public string name { get; set; }
			public int qty { get; set; }
		}		
	}
	

}
