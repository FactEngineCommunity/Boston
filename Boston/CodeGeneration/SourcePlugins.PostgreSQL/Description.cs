using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using System.Linq;
using System.Xml.Linq;
namespace SourcePlugins.PostgreSQL
{
	public class Description : Boston.PluginInterface.IPluginDescription
	{

		public string LicenceInformation {
            get { return ReadResource.Read("SourcePlugins.PostgreSQL.Resources.Licence.txt"); }
		}

		public string ProductInformation {
            get { return ReadResource.Read("SourcePlugins.PostgreSQL.Resources.ProductInformation.txt"); }
		}

	}
}
