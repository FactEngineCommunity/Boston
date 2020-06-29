using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using System.Linq;
using System.Xml.Linq;
using FirebirdSql.Data;
using Boston.PluginInterface.Sources;

namespace SourcePlugins.Firebird
{

	public class Connection : IConnection
	{

		private string mName = null;

		private string ConnStr = null;
		internal enum QueryEnum
		{
			SchemaQuery = 1,
			TableQuery = 2,
			ColumnQuery = 3
		}

        private string mSchemaQuery;
        private string mTableSchemaQuery;
        private string mColumnSchemaQuery;
		private string mTableNamePlaceHolder;
        private Boston.FBM.Model mBostonModel;

        private string mTransforms;

		private List<string> mIgnoreTableNames = new List<string>();

		public Connection()
		{
            this.mSchemaQuery = this.GetQuery(QueryEnum.SchemaQuery);
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
        
        public Boston.FBM.Model BostonModel
        {
            get{return this.mBostonModel;}
            set{this.mBostonModel = value;}
        }

        internal string GetQuery(QueryEnum Query)
		{
			switch (Query) {
				case QueryEnum.SchemaQuery:
                    return ReadResource.Read("SourcePlugins.Firebird.Resources.Queries.Queries.SchemaQuery.sql");
				case QueryEnum.TableQuery:
                    return ReadResource.Read("SourcePlugins.Firebird.Resources.Queries.Queries.TableQuery.sql");
				case QueryEnum.ColumnQuery:
                    return ReadResource.Read("SourcePlugins.Firebird.Resources.Queries.Queries.ColumnQuery.sql");
			}
			return null;
		}

		internal string GetTransforms()
		{
            return ReadResource.Read("SourcePlugins.Firebird.Resources.Transforms.Transforms.txt");
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
			get { return this.mSchemaQuery; }
			set { this.mSchemaQuery = value; }
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
			FirebirdSql.Data.FirebirdClient.FbConnection Conn = new FirebirdSql.Data.FirebirdClient.FbConnection(this.ConnStr);
			Conn.Open();
			Conn.Close();
			Conn.Dispose();
		}

		public DataTable TestQuery(string Query)
		{
			using (FirebirdSql.Data.FirebirdClient.FbConnection conn = new FirebirdSql.Data.FirebirdClient.FbConnection(this.ConnStr)) {
				conn.Open();

				using (FirebirdSql.Data.FirebirdClient.FbCommand cmd = new FirebirdSql.Data.FirebirdClient.FbCommand()) {
					cmd.Connection = conn;
					cmd.CommandType = CommandType.Text;
					cmd.CommandText = Query;

					DataTable dt = new DataTable();
					using (FirebirdSql.Data.FirebirdClient.FbDataAdapter da = new FirebirdSql.Data.FirebirdClient.FbDataAdapter(cmd)) {
						da.Fill(dt);
					}

                    conn.Close();
                    return dt;
				}
			}
		}

		public List<SchemaRow> GetSchema()
		{
			if (!string.IsNullOrEmpty(this.ColumnSchemaQuery)) {
				if (this.ColumnSchemaQuery.IndexOf(this.TableNamePlaceHolder) == -1) {
					throw new Exception("Required placeholder for table name: '" + this.TableNamePlaceHolder + "'.");
				}
			}

			List<SchemaRow> Schema = new List<SchemaRow>();

			using (FirebirdSql.Data.FirebirdClient.FbConnection conn = new FirebirdSql.Data.FirebirdClient.FbConnection(this.ConnStr)) {
				conn.Open();

				//Using single result set
				if (!string.IsNullOrEmpty(this.SchemaQuery)) {
					using (FirebirdSql.Data.FirebirdClient.FbCommand cmd = new FirebirdSql.Data.FirebirdClient.FbCommand()) {
						cmd.Connection = conn;
						cmd.CommandType = CommandType.Text;
						cmd.CommandText = this.SchemaQuery;

						DataTable dtSchema = new DataTable();
						using (FirebirdSql.Data.FirebirdClient.FbDataAdapter da = new FirebirdSql.Data.FirebirdClient.FbDataAdapter(cmd)) {
							da.Fill(dtSchema);
						}

						conn.Close();

						foreach (DataRow dr in dtSchema.Rows) {
							Schema.Add(this.SetColumnAttributes(dr["TABLE_NAME"].ToString(), dr["TABLE_TYPE"].ToString(), dr));
						}

						return Schema;
					}
				}

				//Way of the Table/Column

				//Retrieve table schema first
				List<SchemaRow> TableSchema = new List<SchemaRow>();
				if (!string.IsNullOrEmpty(this.TableSchemaQuery)) {
					//Using table schema query
					using (FirebirdSql.Data.FirebirdClient.FbCommand cmd = new FirebirdSql.Data.FirebirdClient.FbCommand()) {
						cmd.Connection = conn;
						cmd.CommandType = CommandType.Text;
						cmd.CommandText = this.TableSchemaQuery;

						DataTable dtTableSchema = new DataTable();
						using (FirebirdSql.Data.FirebirdClient.FbDataAdapter da = new FirebirdSql.Data.FirebirdClient.FbDataAdapter(cmd)) {
							da.Fill(dtTableSchema);
						}

						TableSchema = this.GetInitialTables(dtTableSchema);
					}

				} else {
					//Get by default using GetSchema
					DataTable dtTableSchema = new DataTable();
					dtTableSchema = conn.GetSchema(System.Data.SqlClient.SqlClientMetaDataCollectionNames.Tables);
					TableSchema = this.GetInitialTables(dtTableSchema);

				}


				//Get columns for each table
				if (!string.IsNullOrEmpty(this.ColumnSchemaQuery)) {
					//Use column schema query
					foreach (SchemaRow tsr in TableSchema) {
						DataTable dtColumnSchema = new DataTable();

						using (FirebirdSql.Data.FirebirdClient.FbCommand cmd = new FirebirdSql.Data.FirebirdClient.FbCommand()) {
							cmd.Connection = conn;
							cmd.CommandType = CommandType.Text;
							cmd.CommandText = this.ColumnSchemaQuery.Replace(this.TableNamePlaceHolder, tsr.Name);

							using (FirebirdSql.Data.FirebirdClient.FbDataAdapter da = new FirebirdSql.Data.FirebirdClient.FbDataAdapter(cmd)) {
								da.Fill(dtColumnSchema);
							}
						}

						//Get column schema
						foreach (DataRow dr in dtColumnSchema.Rows) {
							Schema.Add(this.SetColumnAttributes(tsr.Name, tsr.Type, dr));
						}
					}
				}

				conn.Close();
			}

			return Schema;
		}

		public List<SchemaRow> GetRoutineSchema()
		{
			//TODO implement
			return new List<SchemaRow>();
		}

		public List<SchemaRow> GetRoutineColumnSchema(string RoutineName, string RoutineType, bool IsProcedure, List<string> ParamList)
		{
			//TODO implement
			return new List<SchemaRow>();
		}

		private List<SchemaRow> GetInitialTables(DataTable dtTableSchema)
		{
			List<SchemaRow> TableSchema = new List<SchemaRow>();
			foreach (DataRow dr in dtTableSchema.Rows) {
				if (dr["TABLE_TYPE"].ToString().Trim().Equals("BASE TABLE", StringComparison.OrdinalIgnoreCase) | dr["TABLE_TYPE"].ToString().Trim().Equals("TABLE", StringComparison.OrdinalIgnoreCase) | dr["TABLE_TYPE"].ToString().Trim().Equals("VIEW", StringComparison.OrdinalIgnoreCase)) {
					SchemaRow tsr = new SchemaRow();
					tsr.Name = dr["TABLE_NAME"].ToString().Trim();
					tsr.Type = dr["TABLE_TYPE"].ToString().Trim();
					if (tsr.Type.Equals("TABLE", StringComparison.OrdinalIgnoreCase))
						tsr.Type = "BASE TABLE";
					TableSchema.Add(tsr);
				}
			}

			return TableSchema;
		}

		private SchemaRow SetColumnAttributes(string Table_Name, string Table_Type, DataRow dr)
		{
			SchemaRow sr = new SchemaRow();
			sr.Name = Table_Name.Trim();
			sr.Type = Table_Type.Trim();

			sr.Column_Name = dr["COLUMN_NAME"].ToString().Trim();
			sr.Data_Type = dr["DATA_TYPE"].ToString().Trim();
            sr.Ordinal_Position = Convert.ToInt64(dr["ORDINAL_POSITION"]);
            sr.Length = Convert.ToInt64((object.ReferenceEquals(dr["CHARACTER_MAXIMUM_LENGTH"], DBNull.Value) ? -1 : dr["CHARACTER_MAXIMUM_LENGTH"]));
            sr.Precision = Convert.ToInt64((object.ReferenceEquals(dr["NUMERIC_PRECISION"], DBNull.Value) ? -1 : dr["NUMERIC_PRECISION"]));
            sr.Scale = Convert.ToInt64((object.ReferenceEquals(dr["NUMERIC_SCALE"], DBNull.Value) ? -1 : dr["NUMERIC_SCALE"]));
			sr.Nullable = Convert.ToBoolean((dr["IS_NULLABLE"].ToString().Trim().Equals("YES", StringComparison.OrdinalIgnoreCase) ? true : false));
			sr.IsIdentity = Convert.ToBoolean((dr["IS_IDENTITY"].ToString().Trim().Equals("1") ? true : false));

			sr.IsTable = sr.Type.Equals("BASE TABLE", StringComparison.OrdinalIgnoreCase);
			sr.IsView = sr.Type.Equals("VIEW", StringComparison.OrdinalIgnoreCase);

			sr.IsPrimaryKey = dr["CONSTRAINT_TYPE"].ToString().Trim().Equals("PRIMARY KEY", StringComparison.OrdinalIgnoreCase);
			sr.IsForeignKey = dr["CONSTRAINT_TYPE"].ToString().Trim().Equals("FOREIGN KEY", StringComparison.OrdinalIgnoreCase);

			return sr;
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

				using (FirebirdSql.Data.FirebirdClient.FbConnection conn = new FirebirdSql.Data.FirebirdClient.FbConnection(this.ConnStr)) {
					conn.Open();

					using (FirebirdSql.Data.FirebirdClient.FbCommand cmd = new FirebirdSql.Data.FirebirdClient.FbCommand()) {
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
