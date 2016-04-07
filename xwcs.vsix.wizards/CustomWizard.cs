using Microsoft.VisualStudio.TemplateWizard;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace xwcs.vsix.wizards
{
	[XmlRoot("data", Namespace = "http://schemas.microsoft.com/developer/vstemplate/2005")]
	public class WizardData {


		/*
		 * Allowed kinds:
		 *		EDMX
		 *		TASTATE
		 */
		[XmlAttribute("kind")]
		public string Kind { get; set; }
	}

	class CustomWizard : IWizard
	{
		public bool Confirmed { get; private set; } = false;

		private WizardData GetData(string str) {
			XmlSerializer des = new XmlSerializer(typeof(WizardData));
			using (XmlReader reader = XmlReader.Create(new StringReader(str)))
			{
				return (WizardData)des.Deserialize(reader);
			}
		}

		public void BeforeOpeningFile(global::EnvDTE.ProjectItem projectItem)
		{
		}

		public void ProjectFinishedGenerating(global::EnvDTE.Project project)
		{
		}

		public void ProjectItemFinishedGenerating(global::EnvDTE.ProjectItem projectItem)
		{
		}

		public void RunFinished()
		{
		}

		public void RunStarted(object automationObject, Dictionary<string, string> replacementsDictionary, WizardRunKind runKind, object[] customParams)
		{
			//take data
			string wizdata;
			WizardData myData = null;

			if(!replacementsDictionary.TryGetValue("$wizarddata$", out wizdata)) {
				MessageBox.Show("Missing wizard type in template definition!");
				return;
			}else {
				myData = GetData(wizdata);
				if(myData == null) {
					MessageBox.Show("Incorect wizard type in template definition! (Xml with data not correct)");
					return;
				}
			}

			string filterStr = "";
			string filterExt = "";

			switch(myData.Kind) {
				case "EDMX":
					filterStr = "EF6 Diagram|*.edmx";
					filterExt = "edmx";
					break;
				case "TASTATE":
					filterStr = "State Diagram|*.tastate";
					filterExt = "tastate";
					break;
				default:
					MessageBox.Show("Incorect wizard type in template definition! ( EDMX | TASTATE ) is not set");
					return;
			}

			EnvDTE.DTE dte = automationObject as EnvDTE.DTE;

			EnvDTE.SelectedItem se = dte.DTE.SelectedItems.OfType<EnvDTE.SelectedItem>().First();
			EnvDTE.ProjectItem projectItem = se.ProjectItem;
			string path = projectItem.Properties.Item("FullPath").Value.ToString();

			//  Create the form
			var form = new ChooseFileForm(path, filterStr);


			//take eventual first file
			string[] files = System.IO.Directory.GetFiles(path, "*." + filterExt);
			if(files.Count() > 0) {
				form.EdmxFileName = Path.GetFileName(files[0]);
			}else {
				form.EdmxFileName = "File." + filterExt;
			}

			
			//  Show the form.
			form.ShowDialog();

			if(form.Confirmed) {
				string file = form.EdmxFileName ?? "File." + filterExt;

				//  Add the options to the replacementsDictionary.
				replacementsDictionary.Add("$InputFile$", file);
				Confirmed = true;
			}
		}

		public bool ShouldAddProjectItem(string filePath)
		{
			return Confirmed;
		}
	}
}
