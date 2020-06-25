using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using System.Linq;
using System.Xml.Linq;
namespace SourcePlugins.Firebird
{
	public class ReadResource
	{

		public static string Read(string resource)
		{
			using (System.IO.Stream s = typeof(ReadResource).Assembly.GetManifestResourceStream(resource)) {
				if (s == null) {
					throw new InvalidOperationException("Could not find embedded resource: " + resource);
				}
				using (System.IO.StreamReader reader = new System.IO.StreamReader(s)) {
					return reader.ReadToEnd();
				}
			}
		}

	}
}
