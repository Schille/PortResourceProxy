namespace PortResourceProxy
{
    partial class PortResourceProxyMonitor
    {
        private delegate void AddConsoleLineDelegate(string myLine);
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PortResourceProxyMonitor));
            this.btnStartStop = new System.Windows.Forms.Button();
            this.btnEditRMap = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.tbPort = new System.Windows.Forms.TextBox();
            this.cbAllowQuery = new System.Windows.Forms.CheckBox();
            this.cbAllowPaths = new System.Windows.Forms.CheckBox();
            this.tbConsole = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnStartStop
            // 
            this.btnStartStop.Location = new System.Drawing.Point(12, 16);
            this.btnStartStop.Name = "btnStartStop";
            this.btnStartStop.Size = new System.Drawing.Size(95, 35);
            this.btnStartStop.TabIndex = 1;
            this.btnStartStop.Text = "Start";
            this.btnStartStop.UseVisualStyleBackColor = true;
            this.btnStartStop.Click += new System.EventHandler(this.btnStartStop_Click);
            // 
            // btnEditRMap
            // 
            this.btnEditRMap.Location = new System.Drawing.Point(113, 16);
            this.btnEditRMap.Name = "btnEditRMap";
            this.btnEditRMap.Size = new System.Drawing.Size(157, 35);
            this.btnEditRMap.TabIndex = 2;
            this.btnEditRMap.Text = "Edit Resource Map";
            this.btnEditRMap.UseVisualStyleBackColor = true;
            this.btnEditRMap.Click += new System.EventHandler(this.btnEditRMap_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 60);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Port:";
            // 
            // tbPort
            // 
            this.tbPort.Location = new System.Drawing.Point(44, 57);
            this.tbPort.MaxLength = 4;
            this.tbPort.Name = "tbPort";
            this.tbPort.Size = new System.Drawing.Size(31, 20);
            this.tbPort.TabIndex = 4;
            this.tbPort.Text = "9970";
            // 
            // cbAllowQuery
            // 
            this.cbAllowQuery.AutoSize = true;
            this.cbAllowQuery.Checked = true;
            this.cbAllowQuery.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbAllowQuery.Location = new System.Drawing.Point(81, 59);
            this.cbAllowQuery.Name = "cbAllowQuery";
            this.cbAllowQuery.Size = new System.Drawing.Size(102, 17);
            this.cbAllowQuery.TabIndex = 5;
            this.cbAllowQuery.Text = "Allow Parameter";
            this.cbAllowQuery.UseVisualStyleBackColor = true;
            // 
            // cbAllowPaths
            // 
            this.cbAllowPaths.AutoSize = true;
            this.cbAllowPaths.Checked = true;
            this.cbAllowPaths.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbAllowPaths.Location = new System.Drawing.Point(189, 59);
            this.cbAllowPaths.Name = "cbAllowPaths";
            this.cbAllowPaths.Size = new System.Drawing.Size(81, 17);
            this.cbAllowPaths.TabIndex = 6;
            this.cbAllowPaths.Text = "Allow Paths";
            this.cbAllowPaths.UseVisualStyleBackColor = true;
            // 
            // tbConsole
            // 
            this.tbConsole.Location = new System.Drawing.Point(12, 83);
            this.tbConsole.Multiline = true;
            this.tbConsole.Name = "tbConsole";
            this.tbConsole.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbConsole.Size = new System.Drawing.Size(625, 140);
            this.tbConsole.TabIndex = 0;
            // 
            // PortResourceProxyMonitor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(649, 235);
            this.Controls.Add(this.cbAllowPaths);
            this.Controls.Add(this.cbAllowQuery);
            this.Controls.Add(this.tbPort);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnEditRMap);
            this.Controls.Add(this.btnStartStop);
            this.Controls.Add(this.tbConsole);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "PortResourceProxyMonitor";
            this.Text = "PortResourceProxy";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.PortResourceProxyMonitor_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        

        #endregion

        private System.Windows.Forms.Button btnStartStop;
        private System.Windows.Forms.Button btnEditRMap;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbPort;

        public void AddConsoleLine(string myLine)
        {
            if (tbConsole.InvokeRequired)
            {
                tbConsole.Invoke(new AddConsoleLineDelegate(this.AddConsoleLine), myLine);
            }
            else
            {
                tbConsole.AppendText(myLine);
                tbConsole.AppendText("\n");
            }
            
        }

        public System.Windows.Forms.CheckBox cbAllowQuery;
        public System.Windows.Forms.CheckBox cbAllowPaths;
        private System.Windows.Forms.TextBox tbConsole;
    }
}

