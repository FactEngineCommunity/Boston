using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using System.Linq;
using System.Xml.Linq;
namespace SourcePlugins.MySQL
{
	public class Description : Boston.PluginInterface.IPluginDescription
	{

		public string LicenceInformation {
            get { return ReadResource.Read("SourcePlugins.MySQL.Resources.Licence.txt"); }
		}

		public string ProductInformation {
            get { return ReadResource.Read("SourcePlugins.MySQL.Resources.ProductInformation.txt"); }
		}

	}
}
