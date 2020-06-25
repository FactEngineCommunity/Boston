namespace SourcePlugins.Firebird
{
    partial class Manage
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Manage));
            this.lnkPreviewSchema = new System.Windows.Forms.LinkLabel();
            this.TabPage1 = new System.Windows.Forms.TabPage();
            this.lblPort = new System.Windows.Forms.Label();
            this.txtRole = new System.Windows.Forms.TextBox();
            this.lblRole = new System.Windows.Forms.Label();
            this.lblCharset = new System.Windows.Forms.Label();
            this.cboCharset = new System.Windows.Forms.ComboBox();
            this.btnFile = new System.Windows.Forms.Button();
            this.lblFile = new System.Windows.Forms.Label();
            this.txtFile = new System.Windows.Forms.TextBox();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.Panel2 = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.btnTest = new System.Windows.Forms.Button();
            this.lblServer = new System.Windows.Forms.Label();
            this.txtConnectionString = new System.Windows.Forms.TextBox();
            this.txtServer = new System.Windows.Forms.TextBox();
            this.lblConnectionString = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.lblUsername = new System.Windows.Forms.Label();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.splitTableColumn = new System.Windows.Forms.SplitContainer();
            this.txtTableSchemaQuery = new Boston.UI.SQLEditor();
            this.lblTableSchemaQuery = new System.Windows.Forms.Label();
            this.txtColumnSchemaQuery = new Boston.UI.SQLEditor();
            this.lblColumnSchemaQuery = new System.Windows.Forms.Label();
            this.lblSchemaQuery = new System.Windows.Forms.Label();
            this.txtSchemaQuery = new Boston.UI.SQLEditor();
            this.Panel3 = new System.Windows.Forms.Panel();
            this.Label3 = new System.Windows.Forms.Label();
            this.TabPage2 = new System.Windows.Forms.TabPage();
            this.splitMain = new System.Windows.Forms.SplitContainer();
            this.splitQuery = new System.Windows.Forms.SplitContainer();
            this.QueryResults = new Boston.UI.QueryResults();
            this.Panel1 = new System.Windows.Forms.Panel();
            this.grpApproach = new System.Windows.Forms.GroupBox();
            this.rbApproachSingle = new System.Windows.Forms.RadioButton();
            this.rbApproachTableColumn = new System.Windows.Forms.RadioButton();
            this.grpColumnSchema = new System.Windows.Forms.GroupBox();
            this.txtTableName = new System.Windows.Forms.TextBox();
            this.grpTableSchema = new System.Windows.Forms.GroupBox();
            this.rbTableDefault = new System.Windows.Forms.RadioButton();
            this.rbTableQuery = new System.Windows.Forms.RadioButton();
            this.tcMain = new System.Windows.Forms.TabControl();
            this.TabPage3 = new System.Windows.Forms.TabPage();
            this.txtTransformations = new Boston.UI.TransformationsEditor();
            this.ImageList1 = new System.Windows.Forms.ImageList(this.components);
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.TabPage1.SuspendLayout();
            this.splitTableColumn.Panel1.SuspendLayout();
            this.splitTableColumn.Panel2.SuspendLayout();
            this.splitTableColumn.SuspendLayout();
            this.Panel3.SuspendLayout();
            this.TabPage2.SuspendLayout();
            this.splitMain.Panel1.SuspendLayout();
            this.splitMain.Panel2.SuspendLayout();
            this.splitMain.SuspendLayout();
            this.splitQuery.Panel1.SuspendLayout();
            this.splitQuery.Panel2.SuspendLayout();
            this.splitQuery.SuspendLayout();
            this.Panel1.SuspendLayout();
            this.grpApproach.SuspendLayout();
            this.grpColumnSchema.SuspendLayout();
            this.grpTableSchema.SuspendLayout();
            this.tcMain.SuspendLayout();
            this.TabPage3.SuspendLayout();
            this.SuspendLayout();
            // 
            // lnkPreviewSchema
            // 
            this.lnkPreviewSchema.BackColor = System.Drawing.Color.Transparent;
            this.lnkPreviewSchema.Dock = System.Windows.Forms.DockStyle.Right;
            this.lnkPreviewSchema.Image = ((System.Drawing.Image)(resources.GetObject("lnkPreviewSchema.Image")));
            this.lnkPreviewSchema.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lnkPreviewSchema.Location = new System.Drawing.Point(529, 0);
            this.lnkPreviewSchema.Name = "lnkPreviewSchema";
            this.lnkPreviewSchema.Size = new System.Drawing.Size(64, 24);
            this.lnkPreviewSchema.TabIndex = 4;
            this.lnkPreviewSchema.TabStop = true;
            this.lnkPreviewSchema.Text = "Preview";
            this.lnkPreviewSchema.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.toolTip1.SetToolTip(this.lnkPreviewSchema, "Preview query results (F5)");
            // 
            // TabPage1
            // 
            this.TabPage1.BackColor = System.Drawing.Color.Transparent;
            this.TabPage1.Controls.Add(this.lblPort);
            this.TabPage1.Controls.Add(this.txtRole);
            this.TabPage1.Controls.Add(this.lblRole);
            this.TabPage1.Controls.Add(this.lblCharset);
            this.TabPage1.Controls.Add(this.cboCharset);
            this.TabPage1.Controls.Add(this.btnFile);
            this.TabPage1.Controls.Add(this.lblFile);
            this.TabPage1.Controls.Add(this.txtFile);
            this.TabPage1.Controls.Add(this.txtPort);
            this.TabPage1.Controls.Add(this.Panel2);
            this.TabPage1.Controls.Add(this.lblTitle);
            this.TabPage1.Controls.Add(this.btnTest);
            this.TabPage1.Controls.Add(this.lblServer);
            this.TabPage1.Controls.Add(this.txtConnectionString);
            this.TabPage1.Controls.Add(this.txtServer);
            this.TabPage1.Controls.Add(this.lblConnectionString);
            this.TabPage1.Controls.Add(this.txtPassword);
            this.TabPage1.Controls.Add(this.lblPassword);
            this.TabPage1.Controls.Add(this.lblUsername);
            this.TabPage1.Controls.Add(this.txtUsername);
            this.TabPage1.ImageIndex = 0;
            this.TabPage1.Location = new System.Drawing.Point(4, 4);
            this.TabPage1.Name = "TabPage1";
            this.TabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.TabPage1.Size = new System.Drawing.Size(792, 573);
            this.TabPage1.TabIndex = 0;
            this.TabPage1.Text = "Connection";
            this.TabPage1.UseVisualStyleBackColor = true;
            // 
            // lblPort
            // 
            this.lblPort.AutoSize = true;
            this.lblPort.Location = new System.Drawing.Point(17, 108);
            this.lblPort.Name = "lblPort";
            this.lblPort.Size = new System.Drawing.Size(29, 13);
            this.lblPort.TabIndex = 5;
            this.lblPort.Text = "Port:";
            // 
            // txtRole
            // 
            this.txtRole.Location = new System.Drawing.Point(252, 183);
            this.txtRole.Name = "txtRole";
            this.txtRole.Size = new System.Drawing.Size(144, 20);
            this.txtRole.TabIndex = 14;
            // 
            // lblRole
            // 
            this.lblRole.AutoSize = true;
            this.lblRole.Location = new System.Drawing.Point(214, 186);
            this.lblRole.Name = "lblRole";
            this.lblRole.Size = new System.Drawing.Size(32, 13);
            this.lblRole.TabIndex = 13;
            this.lblRole.Text = "Role:";
            // 
            // lblCharset
            // 
            this.lblCharset.AutoSize = true;
            this.lblCharset.Location = new System.Drawing.Point(17, 186);
            this.lblCharset.Name = "lblCharset";
            this.lblCharset.Size = new System.Drawing.Size(46, 13);
            this.lblCharset.TabIndex = 11;
            this.lblCharset.Text = "Charset:";
            // 
            // cboCharset
            // 
            this.cboCharset.FormattingEnabled = true;
            this.cboCharset.Items.AddRange(new object[] {
            "NONE",
            "Arabic",
            "ASCII",
            "BIG_5",
            "DOS437",
            "DOS850",
            "DOS860",
            "DOS861",
            "DOS863",
            "DOS865",
            "EUCJ_0208",
            "GB_2312",
            "ISO8859_1",
            "ISO8859_2",
            "KSC_5601",
            "ISO2022-JP",
            "SJIS_0208",
            "UNICODE_FSS",
            "UTF8",
            "WIN1250",
            "WIN1251",
            "WIN1252",
            "WIN1253",
            "WIN1254",
            "WIN1254",
            "WIN1257"});
            this.cboCharset.Location = new System.Drawing.Point(79, 183);
            this.cboCharset.Name = "cboCharset";
            this.cboCharset.Size = new System.Drawing.Size(120, 21);
            this.cboCharset.TabIndex = 12;
            this.cboCharset.Text = "NONE";
            // 
            // btnFile
            // 
            this.btnFile.Location = new System.Drawing.Point(396, 80);
            this.btnFile.Name = "btnFile";
            this.btnFile.Size = new System.Drawing.Size(25, 22);
            this.btnFile.TabIndex = 4;
            this.btnFile.Text = "...";
            this.btnFile.UseVisualStyleBackColor = true;
            // 
            // lblFile
            // 
            this.lblFile.AutoSize = true;
            this.lblFile.Location = new System.Drawing.Point(17, 84);
            this.lblFile.Name = "lblFile";
            this.lblFile.Size = new System.Drawing.Size(51, 13);
            this.lblFile.TabIndex = 2;
            this.lblFile.Text = "File Path:";
            // 
            // txtFile
            // 
            this.txtFile.Location = new System.Drawing.Point(79, 81);
            this.txtFile.Name = "txtFile";
            this.txtFile.Size = new System.Drawing.Size(317, 20);
            this.txtFile.TabIndex = 3;
            // 
            // txtPort
            // 
            this.txtPort.Location = new System.Drawing.Point(79, 105);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(75, 20);
            this.txtPort.TabIndex = 6;
            this.txtPort.Text = "3050";
            // 
            // Panel2
            // 
            this.Panel2.BackColor = System.Drawing.Color.Silver;
            this.Panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.Panel2.Location = new System.Drawing.Point(3, 33);
            this.Panel2.Name = "Panel2";
            this.Panel2.Size = new System.Drawing.Size(786, 1);
            this.Panel2.TabIndex = 0;
            // 
            // lblTitle
            // 
            this.lblTitle.BackColor = System.Drawing.Color.White;
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.ForeColor = System.Drawing.Color.DimGray;
            this.lblTitle.Location = new System.Drawing.Point(3, 3);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Padding = new System.Windows.Forms.Padding(6, 6, 0, 0);
            this.lblTitle.Size = new System.Drawing.Size(786, 30);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Firebird";
            // 
            // btnTest
            // 
            this.btnTest.Location = new System.Drawing.Point(20, 294);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(103, 23);
            this.btnTest.TabIndex = 17;
            this.btnTest.Text = "Test Connection";
            this.btnTest.UseVisualStyleBackColor = true;
            // 
            // lblServer
            // 
            this.lblServer.AutoSize = true;
            this.lblServer.Location = new System.Drawing.Point(17, 58);
            this.lblServer.Name = "lblServer";
            this.lblServer.Size = new System.Drawing.Size(41, 13);
            this.lblServer.TabIndex = 0;
            this.lblServer.Text = "Server:";
            // 
            // txtConnectionString
            // 
            this.txtConnectionString.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtConnectionString.Location = new System.Drawing.Point(20, 259);
            this.txtConnectionString.Name = "txtConnectionString";
            this.txtConnectionString.Size = new System.Drawing.Size(753, 20);
            this.txtConnectionString.TabIndex = 16;
            // 
            // txtServer
            // 
            this.txtServer.Location = new System.Drawing.Point(79, 55);
            this.txtServer.Name = "txtServer";
            this.txtServer.Size = new System.Drawing.Size(317, 20);
            this.txtServer.TabIndex = 1;
            // 
            // lblConnectionString
            // 
            this.lblConnectionString.AutoSize = true;
            this.lblConnectionString.Location = new System.Drawing.Point(17, 243);
            this.lblConnectionString.Name = "lblConnectionString";
            this.lblConnectionString.Size = new System.Drawing.Size(94, 13);
            this.lblConnectionString.TabIndex = 15;
            this.lblConnectionString.Text = "Connection String:";
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(79, 157);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(317, 20);
            this.txtPassword.TabIndex = 10;
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new System.Drawing.Point(17, 160);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(56, 13);
            this.lblPassword.TabIndex = 9;
            this.lblPassword.Text = "Password:";
            // 
            // lblUsername
            // 
            this.lblUsername.AutoSize = true;
            this.lblUsername.Location = new System.Drawing.Point(17, 134);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Size = new System.Drawing.Size(58, 13);
            this.lblUsername.TabIndex = 7;
            this.lblUsername.Text = "Username:";
            // 
            // txtUsername
            // 
            this.txtUsername.Location = new System.Drawing.Point(79, 131);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(317, 20);
            this.txtUsername.TabIndex = 8;
            // 
            // splitTableColumn
            // 
            this.splitTableColumn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitTableColumn.Location = new System.Drawing.Point(0, 0);
            this.splitTableColumn.Name = "splitTableColumn";
            this.splitTableColumn.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitTableColumn.Panel1
            // 
            this.splitTableColumn.Panel1.Controls.Add(this.txtTableSchemaQuery);
            this.splitTableColumn.Panel1.Controls.Add(this.lblTableSchemaQuery);
            // 
            // splitTableColumn.Panel2
            // 
            this.splitTableColumn.Panel2.Controls.Add(this.txtColumnSchemaQuery);
            this.splitTableColumn.Panel2.Controls.Add(this.lblColumnSchemaQuery);
            this.splitTableColumn.Size = new System.Drawing.Size(593, 286);
            this.splitTableColumn.SplitterDistance = 140;
            this.splitTableColumn.TabIndex = 1;
            // 
            // txtTableSchemaQuery
            // 
            this.txtTableSchemaQuery.BackColor = System.Drawing.SystemColors.Window;
            this.txtTableSchemaQuery.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtTableSchemaQuery.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtTableSchemaQuery.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtTableSchemaQuery.Location = new System.Drawing.Point(0, 24);
            this.txtTableSchemaQuery.Name = "txtTableSchemaQuery";
            this.txtTableSchemaQuery.ReadOnly = false;
            this.txtTableSchemaQuery.SelectedText = "";
            this.txtTableSchemaQuery.SelectionLength = 0;
            this.txtTableSchemaQuery.SelectionStart = 0;
            this.txtTableSchemaQuery.Size = new System.Drawing.Size(593, 116);
            this.txtTableSchemaQuery.TabIndex = 5;
            // 
            // lblTableSchemaQuery
            // 
            this.lblTableSchemaQuery.BackColor = System.Drawing.Color.White;
            this.lblTableSchemaQuery.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTableSchemaQuery.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTableSchemaQuery.Location = new System.Drawing.Point(0, 0);
            this.lblTableSchemaQuery.Name = "lblTableSchemaQuery";
            this.lblTableSchemaQuery.Size = new System.Drawing.Size(593, 24);
            this.lblTableSchemaQuery.TabIndex = 0;
            this.lblTableSchemaQuery.Text = "Table Schema Query";
            this.lblTableSchemaQuery.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtColumnSchemaQuery
            // 
            this.txtColumnSchemaQuery.BackColor = System.Drawing.SystemColors.Window;
            this.txtColumnSchemaQuery.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtColumnSchemaQuery.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtColumnSchemaQuery.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtColumnSchemaQuery.Location = new System.Drawing.Point(0, 24);
            this.txtColumnSchemaQuery.Name = "txtColumnSchemaQuery";
            this.txtColumnSchemaQuery.ReadOnly = false;
            this.txtColumnSchemaQuery.SelectedText = "";
            this.txtColumnSchemaQuery.SelectionLength = 0;
            this.txtColumnSchemaQuery.SelectionStart = 0;
            this.txtColumnSchemaQuery.Size = new System.Drawing.Size(593, 118);
            this.txtColumnSchemaQuery.TabIndex = 5;
            // 
            // lblColumnSchemaQuery
            // 
            this.lblColumnSchemaQuery.BackColor = System.Drawing.Color.White;
            this.lblColumnSchemaQuery.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblColumnSchemaQuery.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblColumnSchemaQuery.Location = new System.Drawing.Point(0, 0);
            this.lblColumnSchemaQuery.Name = "lblColumnSchemaQuery";
            this.lblColumnSchemaQuery.Size = new System.Drawing.Size(593, 24);
            this.lblColumnSchemaQuery.TabIndex = 0;
            this.lblColumnSchemaQuery.Text = "Column Schema Query";
            this.lblColumnSchemaQuery.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblSchemaQuery
            // 
            this.lblSchemaQuery.BackColor = System.Drawing.Color.Transparent;
            this.lblSchemaQuery.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSchemaQuery.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSchemaQuery.Location = new System.Drawing.Point(0, 0);
            this.lblSchemaQuery.Name = "lblSchemaQuery";
            this.lblSchemaQuery.Size = new System.Drawing.Size(529, 24);
            this.lblSchemaQuery.TabIndex = 2;
            this.lblSchemaQuery.Text = "Schema Query";
            this.lblSchemaQuery.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtSchemaQuery
            // 
            this.txtSchemaQuery.BackColor = System.Drawing.SystemColors.Window;
            this.txtSchemaQuery.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSchemaQuery.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSchemaQuery.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtSchemaQuery.Location = new System.Drawing.Point(0, 24);
            this.txtSchemaQuery.Name = "txtSchemaQuery";
            this.txtSchemaQuery.ReadOnly = false;
            this.txtSchemaQuery.SelectedText = "";
            this.txtSchemaQuery.SelectionLength = 0;
            this.txtSchemaQuery.SelectionStart = 0;
            this.txtSchemaQuery.Size = new System.Drawing.Size(593, 259);
            this.txtSchemaQuery.TabIndex = 5;
            // 
            // Panel3
            // 
            this.Panel3.BackColor = System.Drawing.Color.White;
            this.Panel3.Controls.Add(this.lblSchemaQuery);
            this.Panel3.Controls.Add(this.lnkPreviewSchema);
            this.Panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.Panel3.Location = new System.Drawing.Point(0, 0);
            this.Panel3.Name = "Panel3";
            this.Panel3.Size = new System.Drawing.Size(593, 24);
            this.Panel3.TabIndex = 2;
            // 
            // Label3
            // 
            this.Label3.AutoSize = true;
            this.Label3.Location = new System.Drawing.Point(18, 26);
            this.Label3.Name = "Label3";
            this.Label3.Size = new System.Drawing.Size(127, 13);
            this.Label3.TabIndex = 2;
            this.Label3.Text = "Table name place-holder:";
            // 
            // TabPage2
            // 
            this.TabPage2.Controls.Add(this.splitMain);
            this.TabPage2.Controls.Add(this.Panel1);
            this.TabPage2.ImageIndex = 1;
            this.TabPage2.Location = new System.Drawing.Point(4, 4);
            this.TabPage2.Margin = new System.Windows.Forms.Padding(0);
            this.TabPage2.Name = "TabPage2";
            this.TabPage2.Size = new System.Drawing.Size(792, 573);
            this.TabPage2.TabIndex = 1;
            this.TabPage2.Text = "Meta Data";
            this.TabPage2.UseVisualStyleBackColor = true;
            // 
            // splitMain
            // 
            this.splitMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitMain.Location = new System.Drawing.Point(199, 0);
            this.splitMain.Name = "splitMain";
            this.splitMain.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitMain.Panel1
            // 
            this.splitMain.Panel1.Controls.Add(this.splitQuery);
            // 
            // splitMain.Panel2
            // 
            this.splitMain.Panel2.Controls.Add(this.QueryResults);
            this.splitMain.Panel2Collapsed = true;
            this.splitMain.Size = new System.Drawing.Size(593, 573);
            this.splitMain.TabIndex = 1;
            // 
            // splitQuery
            // 
            this.splitQuery.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitQuery.Location = new System.Drawing.Point(0, 0);
            this.splitQuery.Name = "splitQuery";
            this.splitQuery.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitQuery.Panel1
            // 
            this.splitQuery.Panel1.Controls.Add(this.txtSchemaQuery);
            this.splitQuery.Panel1.Controls.Add(this.Panel3);
            // 
            // splitQuery.Panel2
            // 
            this.splitQuery.Panel2.Controls.Add(this.splitTableColumn);
            this.splitQuery.Size = new System.Drawing.Size(593, 573);
            this.splitQuery.SplitterDistance = 283;
            this.splitQuery.TabIndex = 1;
            // 
            // QueryResults
            // 
            this.QueryResults.Dock = System.Windows.Forms.DockStyle.Fill;
            this.QueryResults.Location = new System.Drawing.Point(0, 0);
            this.QueryResults.Messages = "";
            this.QueryResults.Name = "QueryResults";
            this.QueryResults.Size = new System.Drawing.Size(150, 46);
            this.QueryResults.TabIndex = 0;
            // 
            // Panel1
            // 
            this.Panel1.Controls.Add(this.grpApproach);
            this.Panel1.Controls.Add(this.grpColumnSchema);
            this.Panel1.Controls.Add(this.grpTableSchema);
            this.Panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.Panel1.Location = new System.Drawing.Point(0, 0);
            this.Panel1.Name = "Panel1";
            this.Panel1.Size = new System.Drawing.Size(199, 573);
            this.Panel1.TabIndex = 0;
            // 
            // grpApproach
            // 
            this.grpApproach.Controls.Add(this.rbApproachSingle);
            this.grpApproach.Controls.Add(this.rbApproachTableColumn);
            this.grpApproach.Location = new System.Drawing.Point(4, 3);
            this.grpApproach.Name = "grpApproach";
            this.grpApproach.Size = new System.Drawing.Size(188, 87);
            this.grpApproach.TabIndex = 0;
            this.grpApproach.TabStop = false;
            this.grpApproach.Text = "Approach";
            // 
            // rbApproachSingle
            // 
            this.rbApproachSingle.AutoSize = true;
            this.rbApproachSingle.Checked = true;
            this.rbApproachSingle.Location = new System.Drawing.Point(21, 21);
            this.rbApproachSingle.Name = "rbApproachSingle";
            this.rbApproachSingle.Size = new System.Drawing.Size(99, 17);
            this.rbApproachSingle.TabIndex = 0;
            this.rbApproachSingle.TabStop = true;
            this.rbApproachSingle.Text = "Single result set";
            this.rbApproachSingle.UseVisualStyleBackColor = true;
            // 
            // rbApproachTableColumn
            // 
            this.rbApproachTableColumn.AutoSize = true;
            this.rbApproachTableColumn.Location = new System.Drawing.Point(21, 44);
            this.rbApproachTableColumn.Name = "rbApproachTableColumn";
            this.rbApproachTableColumn.Size = new System.Drawing.Size(132, 17);
            this.rbApproachTableColumn.TabIndex = 1;
            this.rbApproachTableColumn.Text = "Table/Column retrieval";
            this.rbApproachTableColumn.UseVisualStyleBackColor = true;
            // 
            // grpColumnSchema
            // 
            this.grpColumnSchema.Controls.Add(this.txtTableName);
            this.grpColumnSchema.Controls.Add(this.Label3);
            this.grpColumnSchema.Location = new System.Drawing.Point(4, 189);
            this.grpColumnSchema.Name = "grpColumnSchema";
            this.grpColumnSchema.Size = new System.Drawing.Size(188, 87);
            this.grpColumnSchema.TabIndex = 2;
            this.grpColumnSchema.TabStop = false;
            this.grpColumnSchema.Text = "Column Schema Retrieval Method";
            // 
            // txtTableName
            // 
            this.txtTableName.Location = new System.Drawing.Point(21, 42);
            this.txtTableName.Name = "txtTableName";
            this.txtTableName.Size = new System.Drawing.Size(161, 20);
            this.txtTableName.TabIndex = 3;
            // 
            // grpTableSchema
            // 
            this.grpTableSchema.Controls.Add(this.rbTableDefault);
            this.grpTableSchema.Controls.Add(this.rbTableQuery);
            this.grpTableSchema.Location = new System.Drawing.Point(4, 96);
            this.grpTableSchema.Name = "grpTableSchema";
            this.grpTableSchema.Size = new System.Drawing.Size(188, 87);
            this.grpTableSchema.TabIndex = 1;
            this.grpTableSchema.TabStop = false;
            this.grpTableSchema.Text = "Table Schema Retrieval Method";
            // 
            // rbTableDefault
            // 
            this.rbTableDefault.AutoSize = true;
            this.rbTableDefault.Checked = true;
            this.rbTableDefault.Location = new System.Drawing.Point(21, 21);
            this.rbTableDefault.Name = "rbTableDefault";
            this.rbTableDefault.Size = new System.Drawing.Size(62, 17);
            this.rbTableDefault.TabIndex = 0;
            this.rbTableDefault.TabStop = true;
            this.rbTableDefault.Text = "Generic";
            this.rbTableDefault.UseVisualStyleBackColor = true;
            // 
            // rbTableQuery
            // 
            this.rbTableQuery.AutoSize = true;
            this.rbTableQuery.Location = new System.Drawing.Point(21, 44);
            this.rbTableQuery.Name = "rbTableQuery";
            this.rbTableQuery.Size = new System.Drawing.Size(53, 17);
            this.rbTableQuery.TabIndex = 1;
            this.rbTableQuery.Text = "Query";
            this.rbTableQuery.UseVisualStyleBackColor = true;
            // 
            // tcMain
            // 
            this.tcMain.Alignment = System.Windows.Forms.TabAlignment.Bottom;
            this.tcMain.Controls.Add(this.TabPage1);
            this.tcMain.Controls.Add(this.TabPage2);
            this.tcMain.Controls.Add(this.TabPage3);
            this.tcMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcMain.ImageList = this.ImageList1;
            this.tcMain.Location = new System.Drawing.Point(0, 0);
            this.tcMain.Name = "tcMain";
            this.tcMain.SelectedIndex = 0;
            this.tcMain.Size = new System.Drawing.Size(800, 600);
            this.tcMain.TabIndex = 1;
            // 
            // TabPage3
            // 
            this.TabPage3.Controls.Add(this.txtTransformations);
            this.TabPage3.ImageIndex = 2;
            this.TabPage3.Location = new System.Drawing.Point(4, 4);
            this.TabPage3.Name = "TabPage3";
            this.TabPage3.Size = new System.Drawing.Size(792, 573);
            this.TabPage3.TabIndex = 2;
            this.TabPage3.Text = "Transformations";
            this.TabPage3.UseVisualStyleBackColor = true;
            // 
            // txtTransformations
            // 
            this.txtTransformations.BackColor = System.Drawing.SystemColors.Window;
            this.txtTransformations.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtTransformations.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtTransformations.Location = new System.Drawing.Point(0, 0);
            this.txtTransformations.Name = "txtTransformations";
            this.txtTransformations.ReadOnly = false;
            this.txtTransformations.SelectedText = "";
            this.txtTransformations.SelectionLength = 0;
            this.txtTransformations.SelectionStart = 0;
            this.txtTransformations.Size = new System.Drawing.Size(792, 573);
            this.txtTransformations.TabIndex = 1;
            // 
            // ImageList1
            // 
            this.ImageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ImageList1.ImageStream")));
            this.ImageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.ImageList1.Images.SetKeyName(0, "connection16x16.png");
            this.ImageList1.Images.SetKeyName(1, "tblmeta16x16.png");
            this.ImageList1.Images.SetKeyName(2, "transform16x16.png");
            // 
            // toolTip1
            // 
            this.toolTip1.AutoPopDelay = 5000;
            this.toolTip1.InitialDelay = 200;
            this.toolTip1.ReshowDelay = 100;
            // 
            // Manage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tcMain);
            this.Name = "Manage";
            this.Size = new System.Drawing.Size(800, 600);
            this.TabPage1.ResumeLayout(false);
            this.TabPage1.PerformLayout();
            this.splitTableColumn.Panel1.ResumeLayout(false);
            this.splitTableColumn.Panel2.ResumeLayout(false);
            this.splitTableColumn.ResumeLayout(false);
            this.Panel3.ResumeLayout(false);
            this.TabPage2.ResumeLayout(false);
            this.splitMain.Panel1.ResumeLayout(false);
            this.splitMain.Panel2.ResumeLayout(false);
            this.splitMain.ResumeLayout(false);
            this.splitQuery.Panel1.ResumeLayout(false);
            this.splitQuery.Panel2.ResumeLayout(false);
            this.splitQuery.ResumeLayout(false);
            this.Panel1.ResumeLayout(false);
            this.grpApproach.ResumeLayout(false);
            this.grpApproach.PerformLayout();
            this.grpColumnSchema.ResumeLayout(false);
            this.grpColumnSchema.PerformLayout();
            this.grpTableSchema.ResumeLayout(false);
            this.grpTableSchema.PerformLayout();
            this.tcMain.ResumeLayout(false);
            this.TabPage3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        internal System.Windows.Forms.LinkLabel lnkPreviewSchema;
        internal System.Windows.Forms.TabPage TabPage1;
        internal System.Windows.Forms.Label lblPort;
        internal System.Windows.Forms.TextBox txtRole;
        internal System.Windows.Forms.Label lblRole;
        internal System.Windows.Forms.Label lblCharset;
        internal System.Windows.Forms.ComboBox cboCharset;
        internal System.Windows.Forms.Button btnFile;
        internal System.Windows.Forms.Label lblFile;
        internal System.Windows.Forms.TextBox txtFile;
        internal System.Windows.Forms.TextBox txtPort;
        internal System.Windows.Forms.Panel Panel2;
        internal System.Windows.Forms.Label lblTitle;
        internal System.Windows.Forms.Button btnTest;
        internal System.Windows.Forms.Label lblServer;
        internal System.Windows.Forms.TextBox txtConnectionString;
        internal System.Windows.Forms.TextBox txtServer;
        internal System.Windows.Forms.Label lblConnectionString;
        internal System.Windows.Forms.TextBox txtPassword;
        internal System.Windows.Forms.Label lblPassword;
        internal System.Windows.Forms.Label lblUsername;
        internal System.Windows.Forms.TextBox txtUsername;
        internal System.Windows.Forms.SplitContainer splitTableColumn;
        internal Boston.UI.SQLEditor txtTableSchemaQuery;
        internal System.Windows.Forms.Label lblTableSchemaQuery;
        internal Boston.UI.SQLEditor txtColumnSchemaQuery;
        internal System.Windows.Forms.Label lblColumnSchemaQuery;
        internal System.Windows.Forms.Label lblSchemaQuery;
        internal Boston.UI.SQLEditor txtSchemaQuery;
        internal System.Windows.Forms.Panel Panel3;
        internal System.Windows.Forms.Label Label3;
        internal System.Windows.Forms.TabPage TabPage2;
        internal System.Windows.Forms.SplitContainer splitMain;
        internal System.Windows.Forms.SplitContainer splitQuery;
        internal Boston.UI.QueryResults QueryResults;
        internal System.Windows.Forms.Panel Panel1;
        internal System.Windows.Forms.GroupBox grpApproach;
        internal System.Windows.Forms.RadioButton rbApproachSingle;
        internal System.Windows.Forms.RadioButton rbApproachTableColumn;
        internal System.Windows.Forms.GroupBox grpColumnSchema;
        internal System.Windows.Forms.TextBox txtTableName;
        internal System.Windows.Forms.GroupBox grpTableSchema;
        internal System.Windows.Forms.RadioButton rbTableDefault;
        internal System.Windows.Forms.RadioButton rbTableQuery;
        internal System.Windows.Forms.TabControl tcMain;
        internal System.Windows.Forms.TabPage TabPage3;
        internal Boston.UI.TransformationsEditor txtTransformations;
        internal System.Windows.Forms.ImageList ImageList1;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}
