using System;
using DevExpress.XtraDataLayout;

namespace xwcs.core.DataLayout.Attributes
{
	public class CustomAttribute : Attribute
	{
		public virtual void applyRetrievingAttribute(IDataLayoutExtender host, FieldRetrievingEventArgs e) { }
		public virtual void applyRetrievedAttribute(IDataLayoutExtender host, FieldRetrievedEventArgs e) { }
	}
}
