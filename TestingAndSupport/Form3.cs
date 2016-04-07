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

		[XmlRoot("data", Namespace = "http://schemas.microsoft.com/developer/vstemplate/2005")]
		public class WizardData
		{


			/*
			 * Allowed kinds:
			 *		EDMX
			 *		TASTATE
			 */
			[XmlElement("kind")]
			public string Kind { get; set; }
		}

		public class XmlWriterExt : XmlWriter
		{
			private XmlWriter _root;
			Type _type;
			public override WriteState WriteState
			{
				get
				{
					return _root.WriteState;
				}
			}

			public XmlWriterExt(XmlWriter root, Type type) { _root = root; _type = type; }

			public override void WriteStartAttribute(string prefix, string localName, string ns)
			{
				_root.WriteStartAttribute(prefix, localName, ns);
			}

			public override void WriteString(string text)
			{
				_root.WriteString(text);
			}

			public override void WriteEndAttribute()
			{
				_root.WriteEndAttribute();
			}

			public override void WriteStartDocument()
			{
				_root.WriteStartDocument();
            }

			public override void WriteStartDocument(bool standalone)
			{
				_root.WriteStartDocument(standalone);
			}

			public override void WriteEndDocument()
			{
				_root.WriteEndDocument();
			}

			public override void WriteDocType(string name, string pubid, string sysid, string subset)
			{
				_root.WriteDocType(name, pubid, sysid, subset);
			}

			public override void WriteStartElement(string prefix, string localName, string ns)
			{				
				_root.WriteStartElement(prefix, localName, ns);
				if (localName == "content")
				{
					_root.WriteStartAttribute("", "__content_type__", "");
					_root.WriteString(_type.FullName);
					_root.WriteEndAttribute();
				}
			}

			public override void WriteEndElement()
			{
				_root.WriteEndElement();
			}

			public override void WriteFullEndElement()
			{
				_root.WriteFullEndElement();
			}

			public override void WriteCData(string text)
			{
				_root.WriteCData(text);
			}

			public override void WriteComment(string text)
			{
				_root.WriteComment(text);
			}

			public override void WriteProcessingInstruction(string name, string text)
			{
				_root.WriteProcessingInstruction(name, text);
			}

			public override void WriteEntityRef(string name)
			{
				_root.WriteEntityRef(name);
			}

			public override void WriteCharEntity(char ch)
			{
				_root.WriteCharEntity(ch);
			}

			public override void WriteWhitespace(string ws)
			{
				_root.WriteWhitespace(ws);
			}

			public override void WriteSurrogateCharEntity(char lowChar, char highChar)
			{
				_root.WriteSurrogateCharEntity(lowChar, highChar);
			}

			public override void WriteChars(char[] buffer, int index, int count)
			{
				_root.WriteChars(buffer, index, count);
			}

			public override void WriteRaw(char[] buffer, int index, int count)
			{
				_root.WriteRaw(buffer, index, count);
			}

			public override void WriteRaw(string data)
			{
				_root.WriteRaw(data);
			}

			public override void WriteBase64(byte[] buffer, int index, int count)
			{
				_root.WriteBase64(buffer, index, count);
			}

			public override void Flush()
			{
				_root.Flush();
			}

			public override string LookupPrefix(string ns)
			{
				return _root.LookupPrefix(ns);
			}
		}


		private WizardData GetData(string str)
		{
			XmlSerializer des = new XmlSerializer(typeof(WizardData));
			using (XmlReader reader = XmlReader.Create(new StringReader(str)))
			{
				return (WizardData)des.Deserialize(reader);
			}
		}


		public Form3()
		{
			InitializeComponent();

			string testXml = "<data xmlns=\"http://schemas.microsoft.com/developer/vstemplate/2005\"><kind>EDMX</kind></data>";

			WizardData d = GetData(testXml);


			string valXml = "";

			using (StringWriter sw1 = new StringWriter())
			{
				WF2.db.TestObj ttt = new WF2.db.TestObj { name = "nnn", qty = 1 };	

				XmlWriterSettings settings = new XmlWriterSettings();
				settings.Indent = false;
				settings.OmitXmlDeclaration = true;
				settings.Encoding = Encoding.UTF8;
				XmlSerializerNamespaces nss = new XmlSerializerNamespaces();
				nss.Add("", "");

				XmlWriter writer = new XmlWriterExt(XmlWriter.Create(sw1, settings), typeof(WF2.db.TestObj));
				

				

				XmlSerializer s = new XmlSerializer(typeof(WF2.db.TestObj), new XmlRootAttribute {ElementName = "content" });
				s.Serialize(writer, ttt, nss);

				valXml = sw1.ToString();

				XmlSerializer ss = new XmlSerializer(typeof(WF2.db.TestObj), new XmlRootAttribute { ElementName = "content" });
				using (XmlReader reader = XmlReader.Create(new StringReader(valXml)))
				{
					object ooo = ss.Deserialize(reader);
				}
			}




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
