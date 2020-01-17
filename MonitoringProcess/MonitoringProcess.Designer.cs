namespace MonitoringProcess
{
    partial class MonitoringProcess
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.buttonGetProcess = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.timerProcess = new System.Windows.Forms.Timer(this.components);
            this.listViewProcess = new UserControl.ListViewNF();
            this.pid = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.name = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.userName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.description = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.button2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonGetProcess
            // 
            this.buttonGetProcess.Location = new System.Drawing.Point(13, 267);
            this.buttonGetProcess.Name = "buttonGetProcess";
            this.buttonGetProcess.Size = new System.Drawing.Size(75, 23);
            this.buttonGetProcess.TabIndex = 1;
            this.buttonGetProcess.Text = "获取进程";
            this.buttonGetProcess.UseVisualStyleBackColor = true;
            this.buttonGetProcess.Click += new System.EventHandler(this.buttonGetProcess_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(157, 267);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(280, 267);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 21);
            this.textBox1.TabIndex = 3;
            // 
            // timerProcess
            // 
            this.timerProcess.Enabled = true;
            this.timerProcess.Interval = 5000;
            this.timerProcess.Tick += new System.EventHandler(this.timerProcess_Tick);
            // 
            // listViewProcess
            // 
            this.listViewProcess.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.pid,
            this.name,
            this.userName,
            this.description});
            this.listViewProcess.FullRowSelect = true;
            this.listViewProcess.HideSelection = false;
            this.listViewProcess.Location = new System.Drawing.Point(13, 13);
            this.listViewProcess.MultiSelect = false;
            this.listViewProcess.Name = "listViewProcess";
            this.listViewProcess.Size = new System.Drawing.Size(575, 237);
            this.listViewProcess.TabIndex = 0;
            this.listViewProcess.UseCompatibleStateImageBehavior = false;
            this.listViewProcess.View = System.Windows.Forms.View.Details;
            this.listViewProcess.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.listViewProcess_ColumnClick);
            // 
            // pid
            // 
            this.pid.Tag = typeof(int);
            this.pid.Text = "PID";
            this.pid.Width = 48;
            // 
            // name
            // 
            this.name.Tag = typeof(string);
            this.name.Text = "映像名称";
            this.name.Width = 200;
            // 
            // userName
            // 
            this.userName.Tag = typeof(string);
            this.userName.Text = "用户名";
            this.userName.Width = 100;
            // 
            // description
            // 
            this.description.Tag = typeof(string);
            this.description.Text = "描述";
            this.description.Width = 200;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(417, 267);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 4;
            this.button2.Text = "清理进程";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // MonitoringProcess
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(600, 310);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.buttonGetProcess);
            this.Controls.Add(this.listViewProcess);
            this.Name = "MonitoringProcess";
            this.Text = "MonitoringProcess";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MonitoringProcess_FormClosing);
            this.Load += new System.EventHandler(this.MonitoringProcess_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private UserControl.ListViewNF listViewProcess;
        private System.Windows.Forms.Button buttonGetProcess;
        private System.Windows.Forms.ColumnHeader pid;
        private System.Windows.Forms.ColumnHeader name;
        private System.Windows.Forms.ColumnHeader userName;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ColumnHeader description;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Timer timerProcess;
        private System.Windows.Forms.Button button2;
    }
}