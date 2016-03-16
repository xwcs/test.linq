using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace WF2
{
	public partial class Form3 : Form
	{
		public Form3()
		{
			InitializeComponent();

			string  xx = @"<content xmlns:type='WF2.bab_ext'>
			  <n_rpa>aa</n_rpa>
			  <n_cc>vvvv</n_cc>
			  <n_dxp>vvvvvvvvvvv</n_dxp>
			  <n_data>2016-03-17T00:00:00+01:00</n_data>
			</content>";

			string nsVal = "";

			using (XmlReader reader = XmlReader.Create(new StringReader(xx))) {
				reader.MoveToContent();
				
				if (reader.NodeType == XmlNodeType.Element && reader.Name == "content")
				{
					reader.MoveToAttribute("xmlns:type");
					nsVal = reader.Value;
                }
			}
			//deserialize
			XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
			ns.Add("type", nsVal);
			Type tt;
			object obj;
            if (xwcs.core.plgs.SPluginsLoader.getInstance().TryFindType(nsVal, out tt)) {
				XmlSerializer s = new XmlSerializer(tt, new XmlRootAttribute("content"));

				using (XmlReader reader = XmlReader.Create(new StringReader(xx)))
				{
					obj = s.Deserialize(reader);
				}


				XmlWriterSettings settings = new XmlWriterSettings();
				settings.Indent = false;
				settings.OmitXmlDeclaration = true;
				settings.Encoding = Encoding.UTF8;
				StringWriter sw = new StringWriter();
				XmlWriter writer = XmlWriter.Create(sw, settings);
				s.Serialize(writer, obj, ns);

				string ret = sw.ToString();
			}

			

		}
	}
}
