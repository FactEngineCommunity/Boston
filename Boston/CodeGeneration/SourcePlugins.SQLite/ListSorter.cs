using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using System.Linq;
using System.Xml.Linq;
//From: http://www.codeproject.com/KB/vb/Generic_collection_sorter.aspx

using System.Reflection;
namespace SourcePlugins.SQLite
{

	public class ListSorter<T> : IComparer<T>
	{

		#region "Private Variables"
		private string _sortColumn;
			#endregion
		private bool _reverse;

		#region "Constructor"
		public ListSorter(string sortEx)
		{
			//find if we want to sort asc or desc
			if (!string.IsNullOrEmpty(sortEx)) {
				_reverse = sortEx.ToLowerInvariant().EndsWith(" desc");

				if (_reverse) {
					_sortColumn = sortEx.Substring(0, sortEx.Length - 5);
				} else {
					if (sortEx.ToLowerInvariant().EndsWith(" asc")) {
						_sortColumn = sortEx.Substring(0, sortEx.Length - 4);
					} else {
						_sortColumn = sortEx;
					}
				}

			}
		}
		#endregion

		#region "Interface Implementation"

		public int Compare(T x, T y)
		{
			//get the properties of the objects
			PropertyInfo[] propsx = x.GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
			PropertyInfo[] propsy = x.GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
			int retval = 0;

			//find the column we want to sort based on
			for (int i = 0; i <= propsx.Length - 1; i++) {
				if (_sortColumn.ToLower() == propsx[i].Name.ToLower()) {
					//find the type of column so we know how to sort
					switch (propsx[i].PropertyType.Name) {
						case "String":
							retval = Convert.ToString(propsx[i].GetValue(x, null)).CompareTo(Convert.ToString(propsy[i].GetValue(y, null)));
							break;
						case "Integer":
							retval = Convert.ToInt32(propsx[i].GetValue(x, null)).CompareTo(Convert.ToInt32(propsy[i].GetValue(y, null)));
							break;
						case "Int32":
							retval = Convert.ToInt32(propsx[i].GetValue(x, null)).CompareTo(Convert.ToInt32(propsy[i].GetValue(y, null)));
							break;
						case "Int16":
							retval = Convert.ToInt32(propsx[i].GetValue(x, null)).CompareTo(Convert.ToInt32(propsy[i].GetValue(y, null)));
							break;
						case "DateTime":
							retval = Convert.ToDateTime(propsx[i].GetValue(x, null)).CompareTo(Convert.ToDateTime(propsy[i].GetValue(y, null)));
							break;
					}

				}
			}

			if (_reverse) {
				return -1 * retval;
			} else {
				return retval;
			}

		}
		#endregion

		#region "Equal Function"

		public bool Equals(T x, T y)
		{
			PropertyInfo[] propsx = x.GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
			PropertyInfo[] propsy = y.GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
			bool retval = false;
			for (int i = 0; i <= propsx.Length - 1; i++) {
				if (_sortColumn.ToLower() == propsx[i].Name.ToLower()) {
					retval = propsx[i].GetValue(x, null).Equals(propsy[i].GetValue(y, null));
				}
			}
			return retval;
		}
		#endregion

	}
}
