using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using System.Linq;
using System.Xml.Linq;
using Boston.PluginInterface.Sources;
namespace SourcePlugins.SQLite
{

	public class Connection : IConnection
	{

		private string mName = null;

		private string ConnStr = null;
		internal enum QueryEnum
		{
			TableQuery = 1,
			ColumnQuery = 2
		}

        private string mTableSchemaQuery;
		private string mColumnSchemaQuery;
        private string mTableNamePlaceHolder;

        private string mTransforms;

		private List<string> mIgnoreTableNames = new List<string>();

		public Connection()
		{
            this.mTableSchemaQuery = this.GetQuery(QueryEnum.TableQuery);
		    this.mColumnSchemaQuery = this.GetQuery(QueryEnum.ColumnQuery);
		    this.mTableNamePlaceHolder = Boston.Persistence.Source.Default_TableNamePlaceHolder;
            this.mTransforms = this.GetTransforms();
		}

        public Connection(string Name, string ConnectionString) : base()
		{
			this.mName = Name;
			this.ConnStr = ConnectionString;
		}

		internal string GetQuery(QueryEnum Query)
		{
			switch (Query) {
				case QueryEnum.TableQuery:
                    return ReadResource.Read("SourcePlugins.SQLite.Resources.Queries.Queries.TableQuery.sql");
				case QueryEnum.ColumnQuery:
                    return ReadResource.Read("SourcePlugins.SQLite.Resources.Queries.Queries.ColumnQuery.sql");
			}
			return null;
		}

		internal string GetTransforms()
		{
            return ReadResource.Read("SourcePlugins.SQLite.Resources.Transforms.Transforms.txt");
		}

		public string Name {
			get { return this.mName; }
			set { this.mName = value; }
		}

		public string ConnectionString {
			get { return this.ConnStr; }
			set { this.ConnStr = value; }
		}

		public string SchemaQuery {
			get { return null; }

			set { }
		}

		public string TableSchemaQuery {
			get { return this.mTableSchemaQuery; }
			set { this.mTableSchemaQuery = value; }
		}

		public string ColumnSchemaQuery {
			get { return this.mColumnSchemaQuery; }
			set { this.mColumnSchemaQuery = value; }
		}

		public string TableNamePlaceHolder {
			get { return this.mTableNamePlaceHolder; }
			set { this.mTableNamePlaceHolder = value; }
		}

		public string RoutineSchemaQuery {
			get { return null; }

			set { }
		}

		public string Transformations {
			get { return this.mTransforms; }
			set { this.mTransforms = value; }
		}

		public List<string> IgnoreTableNames {
			get { return this.mIgnoreTableNames; }
		}

		public void TestConnection()
		{
			//Using System.Data.SQLite.dll
			using (System.Data.SQLite.SQLiteConnection Conn = new System.Data.SQLite.SQLiteConnection(this.ConnStr)) {
				Conn.Open();
				Conn.Close();
				Conn.Dispose();
			}
		}

		public DataTable TestQuery(string Query)
		{
			using (System.Data.SQLite.SQLiteConnection conn = new System.Data.SQLite.SQLiteConnection(this.ConnStr)) {
				conn.Open();

				using (System.Data.SQLite.SQLiteCommand cmd = new System.Data.SQLite.SQLiteCommand()) {
					cmd.Connection = conn;
					cmd.CommandType = CommandType.Text;
					cmd.CommandText = Query;

					DataTable dt = new DataTable();
					using (System.Data.SQLite.SQLiteDataAdapter da = new System.Data.SQLite.SQLiteDataAdapter(cmd)) {
						da.Fill(dt);
					}

                    conn.Close();
                    return dt;
				}
			}
		}

		public List<SchemaRow> GetSchema()
		{
			if (this.ColumnSchemaQuery.IndexOf(this.TableNamePlaceHolder) == -1) {
				throw new Exception("Required placeholder for table name: '" + this.TableNamePlaceHolder + "'. Eg: " + this.GetQuery(QueryEnum.ColumnQuery));
			}

			List<SchemaRow> Schema = new List<SchemaRow>();

			//Using System.Data.SQLite.dll
			using (System.Data.SQLite.SQLiteConnection conn = new System.Data.SQLite.SQLiteConnection(this.ConnStr)) {
				conn.Open();

				DataTable dtTableSchema = new DataTable();
				using (System.Data.SQLite.SQLiteCommand cmd = new System.Data.SQLite.SQLiteCommand()) {
					cmd.Connection = conn;
					cmd.CommandType = CommandType.Text;
					cmd.CommandText = this.TableSchemaQuery;

					using (System.Data.SQLite.SQLiteDataAdapter da = new System.Data.SQLite.SQLiteDataAdapter(cmd)) {
						da.Fill(dtTableSchema);
					}
				}

				List<SchemaRow> TableSchema = new List<SchemaRow>();
				foreach (DataRow dr in dtTableSchema.Rows) {
					SchemaRow tsr = new SchemaRow();
					tsr.Name = dr["name"].ToString();
					tsr.Type = dr["type"].ToString();
					TableSchema.Add(tsr);
				}

				foreach (SchemaRow tsr in TableSchema) {
					DataTable dtColumnSchema = new DataTable();
					using (System.Data.SQLite.SQLiteCommand cmd = new System.Data.SQLite.SQLiteCommand()) {
						cmd.Connection = conn;
						cmd.CommandType = CommandType.Text;
						cmd.CommandText = this.ColumnSchemaQuery.Replace(this.TableNamePlaceHolder, tsr.Name);


						using (System.Data.SQLite.SQLiteDataAdapter da = new System.Data.SQLite.SQLiteDataAdapter(cmd)) {
							da.Fill(dtColumnSchema);
						}
					}

					List<SchemaRow> ColSchema = new List<SchemaRow>();
					foreach (DataRow dr in dtColumnSchema.Rows) {
						SchemaRow sr = new SchemaRow();
						sr.Name = tsr.Name;
						sr.Type = tsr.Type;

						sr.Column_Name = dr["name"].ToString();
						sr.Data_Type = dr["type"].ToString();
                        sr.Ordinal_Position = Convert.ToInt64(dr["cid"]);
						sr.Ordinal_Position += 1;
						//Ordinal positions are zero based in SQLite, make it one based.
						sr.Length = -1;
						sr.Precision = -1;
						sr.Scale = -1;
						sr.Nullable = Convert.ToBoolean((dr["notnull"].ToString().Equals("1") ? true : false));
						sr.IsIdentity = Convert.ToBoolean((dr["pk"].ToString().Equals("1") ? true : false));

						sr.IsTable = sr.Type.Equals("TABLE", StringComparison.OrdinalIgnoreCase);
						sr.IsView = sr.Type.Equals("VIEW", StringComparison.OrdinalIgnoreCase);
						sr.IsPrimaryKey = sr.IsIdentity;
						sr.IsForeignKey = false;

						ColSchema.Add(sr);
					}

					//Sort by ordinal position (just in case)
					ListSorter<SchemaRow> sorter = new ListSorter<SchemaRow>("Ordinal_Position asc");
					ColSchema.Sort(sorter);

					//Add to return
                    foreach (SchemaRow sr in ColSchema)
                    {
						Schema.Add(sr);
					}
				}

				conn.Close();
			}


			return Schema;
		}

		public List<SchemaRow> GetRoutineSchema()
		{
			return new List<SchemaRow>();
		}

		public List<SchemaRow> GetRoutineColumnSchema(string RoutineName, string RoutineType, bool IsProcedure, List<string> ParamList)
		{
			return new List<SchemaRow>();
		}

		public List<string> GetTables()
		{
			List<SchemaRow> schema = this.GetSchema();
			List<string> tables = new List<string>();
			string lastTable = "";
			foreach (SchemaRow sr in schema) {
				if (!sr.Name.Equals(lastTable) & sr.IsTable) {
					lastTable = sr.Name;
					tables.Add(sr.Name);
				}
			}
			return tables;
		}

		public IConnection CreateCopy(string Name, string Connectionstring, string SchemaQuery, string TableSchemaQuery, string ColumnSchemaQuery, string TableNamePlaceHolder, string RoutineSchemaQuery, string Transformations, List<string> IgnoreTableNames)
		{
			Connection copy = new Connection(Name, Connectionstring);
			copy.SchemaQuery = SchemaQuery;
			copy.TableSchemaQuery = TableSchemaQuery;
			copy.ColumnSchemaQuery = ColumnSchemaQuery;
			copy.TableNamePlaceHolder = TableNamePlaceHolder;
			copy.RoutineSchemaQuery = RoutineSchemaQuery;
			copy.Transformations = Transformations;
			foreach (string tbl in IgnoreTableNames) {
				copy.IgnoreTableNames.Add(tbl);
			}
			return copy;
		}

		public void RunScriptFile(string ScriptFile)
		{
			if (!System.IO.File.Exists(ScriptFile))
				throw new Exception("File '" + ScriptFile + "' does not exist.");

			System.IO.StreamReader sr = null;
			try {
				sr = new System.IO.StreamReader(ScriptFile, false);

				using (System.Data.SQLite.SQLiteConnection conn = new System.Data.SQLite.SQLiteConnection(this.ConnStr)) {
					conn.Open();

					using (System.Data.SQLite.SQLiteCommand cmd = new System.Data.SQLite.SQLiteCommand()) {
						cmd.Connection = conn;
						cmd.CommandType = CommandType.Text;
						cmd.CommandText = sr.ReadToEnd();

						cmd.ExecuteNonQuery();
					}

					conn.Close();
				}

			} catch (Exception ex) {
				throw ex;

			} finally {
				sr.Close();
				sr.Dispose();

			}
		}

	}
}
