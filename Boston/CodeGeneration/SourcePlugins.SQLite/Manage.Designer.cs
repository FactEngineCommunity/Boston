namespace SourcePlugins.SQLite
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
            this.TabPage1 = new System.Windows.Forms.TabPage();
            this.Panel2 = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.btnFile = new System.Windows.Forms.Button();
            this.btnTest = new System.Windows.Forms.Button();
            this.lblFile = new System.Windows.Forms.Label();
            this.txtConnectionString = new System.Windows.Forms.TextBox();
            this.txtFile = new System.Windows.Forms.TextBox();
            this.lblConnectionString = new System.Windows.Forms.Label();
            this.Label1 = new System.Windows.Forms.Label();
            this.Label2 = new System.Windows.Forms.Label();
            this.txtColumnSchemaQuery = new Boston.UI.SQLEditor();
            this.QueryResults = new Boston.UI.QueryResults();
            this.lnkPreviewSchema = new System.Windows.Forms.LinkLabel();
            this.Panel1 = new System.Windows.Forms.Panel();
            this.GroupBox2 = new System.Windows.Forms.GroupBox();
            this.txtTableName = new System.Windows.Forms.TextBox();
            this.Label3 = new System.Windows.Forms.Label();
            this.txtTransformations = new Boston.UI.TransformationsEditor();
            this.TabPage3 = new System.Windows.Forms.TabPage();
            this.txtTableSchemaQuery = new Boston.UI.SQLEditor();
            this.splitQuery = new System.Windows.Forms.SplitContainer();
            this.Panel3 = new System.Windows.Forms.Panel();
            this.splitMain = new System.Windows.Forms.SplitContainer();
            this.TabPage2 = new System.Windows.Forms.TabPage();
            this.tcMain = new System.Windows.Forms.TabControl();
            this.ImageList1 = new System.Windows.Forms.ImageList(this.components);
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.TabPage1.SuspendLayout();
            this.Panel1.SuspendLayout();
            this.GroupBox2.SuspendLayout();
            this.TabPage3.SuspendLayout();
            this.splitQuery.Panel1.SuspendLayout();
            this.splitQuery.Panel2.SuspendLayout();
            this.splitQuery.SuspendLayout();
            this.Panel3.SuspendLayout();
            this.splitMain.Panel1.SuspendLayout();
            this.splitMain.Panel2.SuspendLayout();
            this.splitMain.SuspendLayout();
            this.TabPage2.SuspendLayout();
            this.tcMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // TabPage1
            // 
            this.TabPage1.BackColor = System.Drawing.Color.Transparent;
            this.TabPage1.Controls.Add(this.Panel2);
            this.TabPage1.Controls.Add(this.lblTitle);
            this.TabPage1.Controls.Add(this.btnFile);
            this.TabPage1.Controls.Add(this.btnTest);
            this.TabPage1.Controls.Add(this.lblFile);
            this.TabPage1.Controls.Add(this.txtConnectionString);
            this.TabPage1.Controls.Add(this.txtFile);
            this.TabPage1.Controls.Add(this.lblConnectionString);
            this.TabPage1.ImageIndex = 0;
            this.TabPage1.Location = new System.Drawing.Point(4, 4);
            this.TabPage1.Name = "TabPage1";
            this.TabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.TabPage1.Size = new System.Drawing.Size(792, 573);
            this.TabPage1.TabIndex = 0;
            this.TabPage1.Text = "Connection";
            this.TabPage1.UseVisualStyleBackColor = true;
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
            this.lblTitle.Text = "SQLite";
            // 
            // btnFile
            // 
            this.btnFile.Location = new System.Drawing.Point(402, 60);
            this.btnFile.Name = "btnFile";
            this.btnFile.Size = new System.Drawing.Size(25, 22);
            this.btnFile.TabIndex = 2;
            this.btnFile.Text = "...";
            this.btnFile.UseVisualStyleBackColor = true;
            // 
            // btnTest
            // 
            this.btnTest.Location = new System.Drawing.Point(26, 166);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(103, 23);
            this.btnTest.TabIndex = 5;
            this.btnTest.Text = "Test Connection";
            this.btnTest.UseVisualStyleBackColor = true;
            // 
            // lblFile
            // 
            this.lblFile.AutoSize = true;
            this.lblFile.Location = new System.Drawing.Point(23, 64);
            this.lblFile.Name = "lblFile";
            this.lblFile.Size = new System.Drawing.Size(26, 13);
            this.lblFile.TabIndex = 0;
            this.lblFile.Text = "File:";
            // 
            // txtConnectionString
            // 
            this.txtConnectionString.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtConnectionString.Location = new System.Drawing.Point(26, 131);
            this.txtConnectionString.Name = "txtConnectionString";
            this.txtConnectionString.Size = new System.Drawing.Size(753, 20);
            this.txtConnectionString.TabIndex = 4;
            // 
            // txtFile
            // 
            this.txtFile.Location = new System.Drawing.Point(85, 61);
            this.txtFile.Name = "txtFile";
            this.txtFile.Size = new System.Drawing.Size(317, 20);
            this.txtFile.TabIndex = 1;
            // 
            // lblConnectionString
            // 
            this.lblConnectionString.AutoSize = true;
            this.lblConnectionString.Location = new System.Drawing.Point(23, 115);
            this.lblConnectionString.Name = "lblConnectionString";
            this.lblConnectionString.Size = new System.Drawing.Size(94, 13);
            this.lblConnectionString.TabIndex = 3;
            this.lblConnectionString.Text = "Connection String:";
            // 
            // Label1
            // 
            this.Label1.BackColor = System.Drawing.Color.Transparent;
            this.Label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label1.Location = new System.Drawing.Point(0, 0);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(529, 24);
            this.Label1.TabIndex = 0;
            this.Label1.Text = "Table Schema Query";
            this.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Label2
            // 
            this.Label2.BackColor = System.Drawing.Color.White;
            this.Label2.Dock = System.Windows.Forms.DockStyle.Top;
            this.Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label2.Location = new System.Drawing.Point(0, 0);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(593, 24);
            this.Label2.TabIndex = 2;
            this.Label2.Text = "Column Schema Query";
            this.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
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
            this.txtColumnSchemaQuery.Size = new System.Drawing.Size(593, 260);
            this.txtColumnSchemaQuery.TabIndex = 5;
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
            // lnkPreviewSchema
            // 
            this.lnkPreviewSchema.BackColor = System.Drawing.Color.Transparent;
            this.lnkPreviewSchema.Dock = System.Windows.Forms.DockStyle.Right;
            this.lnkPreviewSchema.Image = ((System.Drawing.Image)(resources.GetObject("lnkPreviewSchema.Image")));
            this.lnkPreviewSchema.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lnkPreviewSchema.Location = new System.Drawing.Point(529, 0);
            this.lnkPreviewSchema.Name = "lnkPreviewSchema";
            this.lnkPreviewSchema.Size = new System.Drawing.Size(64, 24);
            this.lnkPreviewSchema.TabIndex = 5;
            this.lnkPreviewSchema.TabStop = true;
            this.lnkPreviewSchema.Text = "Preview";
            this.lnkPreviewSchema.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.toolTip1.SetToolTip(this.lnkPreviewSchema, "Preview query results (F5)");
            // 
            // Panel1
            // 
            this.Panel1.Controls.Add(this.GroupBox2);
            this.Panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.Panel1.Location = new System.Drawing.Point(0, 0);
            this.Panel1.Name = "Panel1";
            this.Panel1.Size = new System.Drawing.Size(199, 573);
            this.Panel1.TabIndex = 0;
            // 
            // GroupBox2
            // 
            this.GroupBox2.Controls.Add(this.txtTableName);
            this.GroupBox2.Controls.Add(this.Label3);
            this.GroupBox2.Location = new System.Drawing.Point(3, 3);
            this.GroupBox2.Name = "GroupBox2";
            this.GroupBox2.Size = new System.Drawing.Size(188, 87);
            this.GroupBox2.TabIndex = 1;
            this.GroupBox2.TabStop = false;
            this.GroupBox2.Text = "Column Schema Retrieval";
            // 
            // txtTableName
            // 
            this.txtTableName.Location = new System.Drawing.Point(21, 42);
            this.txtTableName.Name = "txtTableName";
            this.txtTableName.Size = new System.Drawing.Size(161, 20);
            this.txtTableName.TabIndex = 3;
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
            this.txtTableSchemaQuery.Size = new System.Drawing.Size(593, 261);
            this.txtTableSchemaQuery.TabIndex = 5;
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
            this.splitQuery.Panel1.Controls.Add(this.txtTableSchemaQuery);
            this.splitQuery.Panel1.Controls.Add(this.Panel3);
            // 
            // splitQuery.Panel2
            // 
            this.splitQuery.Panel2.Controls.Add(this.txtColumnSchemaQuery);
            this.splitQuery.Panel2.Controls.Add(this.Label2);
            this.splitQuery.Size = new System.Drawing.Size(593, 573);
            this.splitQuery.SplitterDistance = 285;
            this.splitQuery.TabIndex = 1;
            // 
            // Panel3
            // 
            this.Panel3.BackColor = System.Drawing.Color.White;
            this.Panel3.Controls.Add(this.Label1);
            this.Panel3.Controls.Add(this.lnkPreviewSchema);
            this.Panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.Panel3.Location = new System.Drawing.Point(0, 0);
            this.Panel3.Name = "Panel3";
            this.Panel3.Size = new System.Drawing.Size(593, 24);
            this.Panel3.TabIndex = 0;
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
            this.splitMain.SplitterDistance = 263;
            this.splitMain.TabIndex = 1;
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
            this.Panel1.ResumeLayout(false);
            this.GroupBox2.ResumeLayout(false);
            this.GroupBox2.PerformLayout();
            this.TabPage3.ResumeLayout(false);
            this.splitQuery.Panel1.ResumeLayout(false);
            this.splitQuery.Panel2.ResumeLayout(false);
            this.splitQuery.ResumeLayout(false);
            this.Panel3.ResumeLayout(false);
            this.splitMain.Panel1.ResumeLayout(false);
            this.splitMain.Panel2.ResumeLayout(false);
            this.splitMain.ResumeLayout(false);
            this.TabPage2.ResumeLayout(false);
            this.tcMain.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        internal System.Windows.Forms.TabPage TabPage1;
        internal System.Windows.Forms.Panel Panel2;
        internal System.Windows.Forms.Label lblTitle;
        internal System.Windows.Forms.Button btnFile;
        internal System.Windows.Forms.Button btnTest;
        internal System.Windows.Forms.Label lblFile;
        internal System.Windows.Forms.TextBox txtConnectionString;
        internal System.Windows.Forms.TextBox txtFile;
        internal System.Windows.Forms.Label lblConnectionString;
        internal System.Windows.Forms.Label Label1;
        internal System.Windows.Forms.Label Label2;
        internal Boston.UI.SQLEditor txtColumnSchemaQuery;
        internal Boston.UI.QueryResults QueryResults;
        internal System.Windows.Forms.LinkLabel lnkPreviewSchema;
        internal System.Windows.Forms.Panel Panel1;
        internal System.Windows.Forms.GroupBox GroupBox2;
        internal System.Windows.Forms.TextBox txtTableName;
        internal System.Windows.Forms.Label Label3;
        internal Boston.UI.TransformationsEditor txtTransformations;
        internal System.Windows.Forms.TabPage TabPage3;
        internal Boston.UI.SQLEditor txtTableSchemaQuery;
        internal System.Windows.Forms.SplitContainer splitQuery;
        internal System.Windows.Forms.Panel Panel3;
        internal System.Windows.Forms.SplitContainer splitMain;
        internal System.Windows.Forms.TabPage TabPage2;
        internal System.Windows.Forms.TabControl tcMain;
        internal System.Windows.Forms.ImageList ImageList1;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}
