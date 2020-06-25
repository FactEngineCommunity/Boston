using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SourcePlugins.SQLite
{
    public partial class Manage : UserControl, Boston.PluginInterface.Sources.IManageSource
    {

        public event Boston.PluginInterface.Sources.IManageSource.ValueChangedEventHandler ValueChanged;
        public delegate void ValueChangedEventHandler(object value);
        public event Boston.PluginInterface.Sources.IManageSource.SaveEventHandler Save;
        public delegate void SaveEventHandler();

        public Manage()
        {
            InitializeComponent();

            this.Load += new System.EventHandler(this.Manage_Load);

            this.txtFile.TextChanged += new System.EventHandler(this.txtConnParts_TextChanged);
            this.txtConnectionString.TextChanged += new System.EventHandler(this.txtConnectionString_TextChanged);
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);

            this.btnFile.Click += new System.EventHandler(this.btnFile_Click);

            this.lnkPreviewSchema.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkPreviewSchema_LinkClicked);

            this.txtTableSchemaQuery.KeyDown += new Boston.UI.SQLEditor.KeyDownEventHandler(this.txtTableSchemaQuery_KeyDown);
            this.txtTableSchemaQuery.TextChanged += new Boston.UI.SQLEditor.TextChangedEventHandler(this.txtTableSchemaQuery_TextChanged);
            this.txtTableSchemaQuery.SavePress += new Boston.UI.SQLEditor.SavePressEventHandler(this.SavePress);
            this.txtColumnSchemaQuery.TextChanged += new Boston.UI.SQLEditor.TextChangedEventHandler(this.txtColumnSchemaQuery_TextChanged);
            this.txtColumnSchemaQuery.SavePress += new Boston.UI.SQLEditor.SavePressEventHandler(this.SavePress);

            this.txtTableName.TextChanged += new System.EventHandler(this.txtTableName_TextChanged);

            this.txtTransformations.SavePress += new Boston.UI.TransformationsEditor.SavePressEventHandler(this.SavePress);
            this.txtTransformations.TextChanged += new Boston.UI.TransformationsEditor.TextChangedEventHandler(this.txtTransformations_TextChanged);
        }


        public void Setup()
        {
            if (this.txtTableSchemaQuery.Text.Length == 0)
            {
                this.txtTableSchemaQuery.Text = new Connection("", "").GetQuery(Connection.QueryEnum.TableQuery);
            }
            if (this.txtColumnSchemaQuery.Text.Length == 0)
            {
                this.txtColumnSchemaQuery.Text = new Connection("", "").GetQuery(Connection.QueryEnum.ColumnQuery);
            }
            if (this.txtTableName.Text.Length == 0)
            {
                this.txtTableName.Text = Boston.Persistence.Source.Default_TableNamePlaceHolder;
            }
            this.splitMain.Panel2Collapsed = true;
        }

        private void TestConn()
        {
            Connection conn = new Connection("", this.txtConnectionString.Text);
            conn.TestConnection();
        }


        #region "Properties"

        public string ConnectionString
        {
            get { return this.txtConnectionString.Text; }
            set { this.txtConnectionString.Text = value; }
        }

        public bool SingleResultApproach
        {
            get { return false; }
            set { }
        }

        public bool TableSchemaGeneric
        {
            get { return false; }
            set { }
        }

        public bool ColumnSchemaGeneric
        {
            get { return false; }

            set { }
        }

        public string SchemaQuery
        {
            get { return null; }
            set { }
        }

        public string TableSchemaQuery
        {
            get { return this.txtTableSchemaQuery.Text; }
            set { this.txtTableSchemaQuery.Text = value; }
        }

        public string ColumnSchemaQuery
        {
            get { return this.txtColumnSchemaQuery.Text; }
            set { this.txtColumnSchemaQuery.Text = value; }
        }

        public string TableName
        {
            get { return this.txtTableName.Text; }
            set { this.txtTableName.Text = value; }
        }

        public string RoutineSchemaQuery
        {
            get { return null; }
            set { }
        }

        public string Transformations
        {
            get { return this.txtTransformations.Text; }
            set { this.txtTransformations.Text = value; }
        }

        #endregion


        #region "Events"

        private void Manage_Load(object sender, EventArgs e)
        {

        }

        private void SavePress()
        {
            if (Save != null)
            {
                Save();
            }
        }

        private void txtTableSchemaQuery_TextChanged(object sender, System.EventArgs e)
        {
            if (ValueChanged != null)
            {
                ValueChanged(this.txtTableSchemaQuery.Text);
            }
        }


        private void txtColumnSchemaQuery_TextChanged(object sender, System.EventArgs e)
        {
            if (ValueChanged != null)
            {
                ValueChanged(this.txtColumnSchemaQuery.Text);
            }
        }

        private void txtTableName_TextChanged(object sender, System.EventArgs e)
        {
            if (ValueChanged != null)
            {
                ValueChanged(this.txtTableName.Text);
            }
        }

        private void txtTransformations_TextChanged(object sender, System.EventArgs e)
        {
            if (ValueChanged != null)
            {
                ValueChanged(this.txtTransformations.Text);
            }
        }
               
        #endregion


        #region "Connection guff"

        private void BuildConnectionString()
        {
            this.txtConnectionString.Text = "Data Source=" + this.txtFile.Text + ";Version=3;";
            //Using System.Data.SQLite.dll
            //Me.txtConnectionString.Text = "Version=3,uri=file:" & Me.txtFile.Text          //Using Community.CsharpSqlite.SQLiteClient
            //Me.txtConnectionString.Text = Me.txtFile.Text                                  //Using System.Data.SQLite.dll
        }

        private void txtConnParts_TextChanged(object sender, EventArgs e)
        {
            this.BuildConnectionString();
        }

        private void txtConnectionString_TextChanged(object sender, EventArgs e)
        {
            if (ValueChanged != null)
            {
                ValueChanged(((TextBox)sender).Text);
            }
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                this.TestConn();

                this.Cursor = Cursors.Default;
                MessageBox.Show("Test connection successful.", "Metadrone", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);

            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                MessageBox.Show(ex.Message, "Metadrone", MessageBoxButtons.OK, MessageBoxIcon.Stop, MessageBoxDefaultButton.Button1);

            }
        }

        private void btnFile_Click(System.Object sender, System.EventArgs e)
        {
            System.Windows.Forms.OpenFileDialog dlg = new System.Windows.Forms.OpenFileDialog();
            if (dlg.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                this.txtFile.Text = dlg.FileName;
            }
        }

        #endregion


        #region "Query Testing"

        private void lnkPreviewSchema_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.TestQuery();
        }

        private void txtTableSchemaQuery_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
            {
                this.TestQuery();
                this.txtTableSchemaQuery.Focus();
            }
        }

        private void TestQuery()
        {
            this.splitMain.Panel2Collapsed = false;

            this.Cursor = Cursors.WaitCursor;

            try
            {
                this.QueryResults.PrepareSourceLoad();
                Connection dt = new Connection("", this.txtConnectionString.Text);
                this.QueryResults.SetSource(dt.TestQuery(this.TableSchemaQuery));

            }
            catch (Exception ex)
            {
                this.QueryResults.Messages = ex.Message;

            }

            this.Cursor = Cursors.Default;
        }

        #endregion

    }
}
