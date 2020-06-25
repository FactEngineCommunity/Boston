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
	public partial class Logo : Boston.PluginInterface.Sources.ISourceDescription
	{

		public string ProviderName {
			get { return "PostgreSQL"; }
		}

		public string Description {
			get { return "PostgreSQL"; }
		}

		public System.Drawing.Image LogoImage {
			get { return this.picLogo.Image; }
		}
		public Logo()
		{
			InitializeComponent();
		}

	}
}
