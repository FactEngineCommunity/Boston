using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using System.Linq;
using System.Xml.Linq;
using SourcePlugins.MySQL;
using Boston.PluginInterface.Sources;
namespace SourcePlugins.MySQL
{

	public class Connection : IConnection
	{

		private string mName = null;

		private string ConnStr = null;
		internal enum QueryEnum
		{
			SchemaQuery = 1,
			TableQuery = 2,
			ColumnQuery = 3,
			RoutineSchemaQuery = 4
		}

        private string mSchemaQuery;
        private string mTableSchemaQuery;
        private string mColumnSchemaQuery;
        private string mTableNamePlaceHolder;
        private string mRoutineSchemaQuery;

        private string mTransforms;

		private List<string> mIgnoreTableNames = new List<string>();

		public Connection()
		{
            this.mSchemaQuery = this.GetQuery(QueryEnum.SchemaQuery);
		    this.mTableSchemaQuery = this.GetQuery(QueryEnum.TableQuery);
		    this.mColumnSchemaQuery = this.GetQuery(QueryEnum.ColumnQuery);
		    this.mTableNamePlaceHolder = Boston.Persistence.Source.Default_TableNamePlaceHolder;
            this.mRoutineSchemaQuery = this.GetQuery(QueryEnum.RoutineSchemaQuery);
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
				case QueryEnum.SchemaQuery:
					return ReadResource.Read("SourcePlugins.MySQL.Resources.Queries.Queries.SchemaQuery.sql");
				case QueryEnum.TableQuery:
                    return ReadResource.Read("SourcePlugins.MySQL.Resources.Queries.Queries.TableQuery.sql");
				case QueryEnum.ColumnQuery:
                    return ReadResource.Read("SourcePlugins.MySQL.Resources.Queries.Queries.ColumnQuery.sql");
				case QueryEnum.RoutineSchemaQuery:
                    return ReadResource.Read("SourcePlugins.MySQL.Resources.Queries.Queries.RoutineSchemaQuery.sql");
			}
			return null;
		}

		internal string GetTransforms()
		{
            return ReadResource.Read("SourcePlugins.MySQL.Resources.Transforms.Transforms.txt");
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
			get { return this.mRoutineSchemaQuery; }
			set { this.mRoutineSchemaQuery = value; }
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
			MySql.Data.MySqlClient.MySqlConnection Conn = new MySql.Data.MySqlClient.MySqlConnection(this.ConnStr);
			Conn.Open();
			Conn.Close();
			Conn.Dispose();
		}

		public DataTable TestQuery(string Query)
		{
			using (MySql.Data.MySqlClient.MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection(this.ConnStr)) {
				conn.Open();

				using (MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand()) {
					cmd.Connection = conn;
					cmd.CommandType = CommandType.Text;
					cmd.CommandText = Query;

					DataTable dt = new DataTable();
					using (MySql.Data.MySqlClient.MySqlDataAdapter da = new MySql.Data.MySqlClient.MySqlDataAdapter(cmd)) {
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

			using (MySql.Data.MySqlClient.MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection(this.ConnStr)) {
				conn.Open();

				//Using single result set
				if (!string.IsNullOrEmpty(this.SchemaQuery)) {
					using (MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand()) {
						cmd.Connection = conn;
						cmd.CommandType = CommandType.Text;
						cmd.CommandText = this.SchemaQuery;

						DataTable dtSchema = new DataTable();
						using (MySql.Data.MySqlClient.MySqlDataAdapter da = new MySql.Data.MySqlClient.MySqlDataAdapter(cmd)) {
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
					using (MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand()) {
						cmd.Connection = conn;
						cmd.CommandType = CommandType.Text;
						cmd.CommandText = this.TableSchemaQuery;

						DataTable dtTableSchema = new DataTable();
						using (MySql.Data.MySqlClient.MySqlDataAdapter da = new MySql.Data.MySqlClient.MySqlDataAdapter(cmd)) {
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

						using (MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand()) {
							cmd.Connection = conn;
							cmd.CommandType = CommandType.Text;
							cmd.CommandText = this.ColumnSchemaQuery.Replace(this.TableNamePlaceHolder, tsr.Name);

							using (MySql.Data.MySqlClient.MySqlDataAdapter da = new MySql.Data.MySqlClient.MySqlDataAdapter(cmd)) {
								da.Fill(dtColumnSchema);
							}
						}

						//Get column schema
						foreach (DataRow dr in dtColumnSchema.Rows) {
							Schema.Add(this.SetColumnAttributes(tsr.Name, tsr.Type, dr));
						}
					}

				} else {
					//Get by default using DataReader GetSchemaTable
					//I can't figure this out either. We will not use generic get schema.
				}

				conn.Close();
			}

			return Schema;
		}

		public List<SchemaRow> GetRoutineSchema()
		{
			if (string.IsNullOrEmpty(this.SchemaQuery))
				return new List<SchemaRow>();

			using (MySql.Data.MySqlClient.MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection(this.ConnStr)) {
				conn.Open();

				List<SchemaRow> Schema = new List<SchemaRow>();
				using (MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand()) {
					cmd.Connection = conn;
					cmd.CommandType = CommandType.Text;
					cmd.CommandText = this.RoutineSchemaQuery;

					DataTable dtSchema = new DataTable();
					using (MySql.Data.MySqlClient.MySqlDataAdapter da = new MySql.Data.MySqlClient.MySqlDataAdapter(cmd)) {
						da.Fill(dtSchema);
					}

					conn.Close();

					foreach (DataRow dr in dtSchema.Rows) {
						Schema.Add(this.SetParameterAttributes(dr["ROUTINE_NAME"].ToString(), dr["ROUTINE_TYPE"].ToString(), dr));
					}

					return Schema;
				}
			}
		}

		public List<SchemaRow> GetRoutineColumnSchema(string RoutineName, string RoutineType, bool IsProcedure, List<string> ParamList)
		{
			using (MySql.Data.MySqlClient.MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection(this.ConnStr)) {
				conn.Open();

				using (MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand()) {
					System.Text.StringBuilder sb = new System.Text.StringBuilder("CALL `" + RoutineName + "` (");
					for (int i = 0; i <= ParamList.Count - 1; i++) {
						sb.Append("NULL");
						if (i < ParamList.Count - 1)
							sb.Append(", ");
					}
					sb.Append(")");

					cmd.Connection = conn;
					cmd.CommandType = CommandType.Text;
					cmd.CommandText = sb.ToString();

					DataTable dtSchema = new DataTable();
					using (MySql.Data.MySqlClient.MySqlDataAdapter da = new MySql.Data.MySqlClient.MySqlDataAdapter(cmd)) {
						da.FillSchema(dtSchema, SchemaType.Source);
					}

					conn.Close();

					return this.BuildColumns(RoutineName, RoutineType, IsProcedure, dtSchema);
				}
			}
		}

		private List<SchemaRow> GetInitialTables(DataTable dtTableSchema)
		{
			List<SchemaRow> TableSchema = new List<SchemaRow>();
			foreach (DataRow dr in dtTableSchema.Rows) {
				if (dr["TABLE_TYPE"].ToString().Equals("BASE TABLE", StringComparison.OrdinalIgnoreCase) | dr["TABLE_TYPE"].ToString().Equals("VIEW", StringComparison.OrdinalIgnoreCase)) {
					SchemaRow tsr = new SchemaRow();
					tsr.Name = dr["TABLE_NAME"].ToString();
					tsr.Type = dr["TABLE_TYPE"].ToString();
					TableSchema.Add(tsr);
				}
			}

			return TableSchema;
		}

		private SchemaRow SetColumnAttributes(string Table_Name, string Table_Type, DataRow dr)
		{
			SchemaRow sr = new SchemaRow();
			sr.Name = Table_Name;
			sr.Type = Table_Type;

			sr.Column_Name = dr["COLUMN_NAME"].ToString();
			sr.Data_Type = dr["DATA_TYPE"].ToString();
			sr.Ordinal_Position = Convert.ToInt64(dr["ORDINAL_POSITION"]);
			sr.Length = Convert.ToInt64((object.ReferenceEquals(dr["CHARACTER_MAXIMUM_LENGTH"], DBNull.Value) ? -1 : dr["CHARACTER_MAXIMUM_LENGTH"]));
            sr.Precision = Convert.ToInt64((object.ReferenceEquals(dr["NUMERIC_PRECISION"], DBNull.Value) ? -1 : dr["NUMERIC_PRECISION"]));
            sr.Scale = Convert.ToInt64((object.ReferenceEquals(dr["NUMERIC_SCALE"], DBNull.Value) ? -1 : dr["NUMERIC_SCALE"]));
			sr.Nullable = Convert.ToBoolean((dr["IS_NULLABLE"].ToString().Equals("YES", StringComparison.OrdinalIgnoreCase) ? true : false));
			sr.IsIdentity = Convert.ToBoolean((dr["IS_IDENTITY"].ToString().Equals("YES") ? true : false));

			sr.IsTable = sr.Type.Equals("BASE TABLE", StringComparison.OrdinalIgnoreCase);
			sr.IsView = sr.Type.Equals("VIEW", StringComparison.OrdinalIgnoreCase);

			sr.IsPrimaryKey = dr["CONSTRAINT_TYPE"].ToString().Equals("PRI", StringComparison.OrdinalIgnoreCase);
			sr.IsForeignKey = dr["CONSTRAINT_TYPE"].ToString().Equals("FOREIGN KEY", StringComparison.OrdinalIgnoreCase);

			return sr;
		}

		private SchemaRow SetParameterAttributes(string Routine_Name, string Routine_Type, DataRow dr)
		{
			SchemaRow sr = new SchemaRow();
			sr.Name = Routine_Name;
			sr.Type = Routine_Type;

			sr.Parameter_Name = (object.ReferenceEquals(dr["PARAMETER_NAME"], DBNull.Value) ? "" : dr["PARAMETER_NAME"]).ToString();
			sr.Parameter_Mode = (object.ReferenceEquals(dr["PARAMETER_MODE"], DBNull.Value) ? "" : dr["PARAMETER_MODE"]).ToString();
			sr.IsInMode = sr.Parameter_Mode.Equals("IN");
			sr.IsOutMode = sr.Parameter_Mode.Equals("OUT");
			sr.IsInOutMode = sr.Parameter_Mode.Equals("INOUT");

			sr.Data_Type = (object.ReferenceEquals(dr["DATA_TYPE"], DBNull.Value) ? "" : dr["DATA_TYPE"]).ToString();
            sr.Ordinal_Position = Convert.ToInt64((object.ReferenceEquals(dr["ORDINAL_POSITION"], DBNull.Value) ? 0 : dr["ORDINAL_POSITION"]));
            sr.Length = Convert.ToInt64((object.ReferenceEquals(dr["CHARACTER_MAXIMUM_LENGTH"], DBNull.Value) ? -1 : dr["CHARACTER_MAXIMUM_LENGTH"]));
            sr.Precision = Convert.ToInt64((object.ReferenceEquals(dr["NUMERIC_PRECISION"], DBNull.Value) ? -1 : dr["NUMERIC_PRECISION"]));
            sr.Scale = Convert.ToInt64((object.ReferenceEquals(dr["NUMERIC_SCALE"], DBNull.Value) ? -1 : dr["NUMERIC_SCALE"]));

			sr.IsProcedure = sr.Type.Equals("PROCEDURE", StringComparison.OrdinalIgnoreCase);
			sr.IsFunction = sr.Type.Equals("FUNCTION", StringComparison.OrdinalIgnoreCase);

			return sr;
		}

		private SqlDbType TypeToSqlDbType(Type type)
		{
			System.Data.SqlClient.SqlParameter p1 = new System.Data.SqlClient.SqlParameter();
			System.ComponentModel.TypeConverter tc = null;
			tc = System.ComponentModel.TypeDescriptor.GetConverter(p1.DbType);

			p1.DbType = (DbType)tc.ConvertFrom(type.Name);

			return p1.SqlDbType;
		}

		private List<SchemaRow> BuildColumns(string RoutineName, string RoutineType, bool IsProcedure, DataTable dtSchema)
		{
			List<SchemaRow> Schema = new List<SchemaRow>();
			for (int i = 0; i <= dtSchema.Columns.Count - 1; i++) {
				SchemaRow sr = new SchemaRow();
				sr.Name = RoutineName;
				sr.Type = RoutineType;

				sr.Column_Name = dtSchema.Columns[i].ColumnName;
				sr.Data_Type = this.TypeToSqlDbType(dtSchema.Columns[i].DataType).ToString();
				sr.Ordinal_Position = dtSchema.Columns[i].Ordinal;
				sr.Length = dtSchema.Columns[i].MaxLength;
				sr.Precision = -1;
				sr.Scale = -1;
				sr.Nullable = dtSchema.Columns[i].AllowDBNull;
				sr.IsIdentity = dtSchema.Columns[i].AutoIncrement;

				sr.IsTable = false;
				sr.IsView = false;

				sr.IsPrimaryKey = dtSchema.Columns[i].AutoIncrement;
				sr.IsForeignKey = false;

				sr.IsProcedure = IsProcedure;
				sr.IsFunction = !IsProcedure;

				Schema.Add(sr);
			}
			return Schema;
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

				using (MySql.Data.MySqlClient.MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection(this.ConnStr)) {
					conn.Open();

					using (MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand()) {
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
