namespace TerminalRobo
{
    partial class Form1
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtContainer = new System.Windows.Forms.TextBox();
            this.btnEmbraport = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.pnlCentro = new System.Windows.Forms.Panel();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.tsStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsTerminal = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsTempo = new System.Windows.Forms.ToolStripStatusLabel();
            this.tmrPrintsTelas = new System.Windows.Forms.Timer(this.components);
            this.TimerMonitoramento = new System.Windows.Forms.Timer(this.components);
            this.panel1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.txtContainer);
            this.panel1.Controls.Add(this.btnEmbraport);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1253, 39);
            this.panel1.TabIndex = 3;
            // 
            // txtContainer
            // 
            this.txtContainer.Location = new System.Drawing.Point(358, 11);
            this.txtContainer.Name = "txtContainer";
            this.txtContainer.Size = new System.Drawing.Size(423, 20);
            this.txtContainer.TabIndex = 4;
            // 
            // btnEmbraport
            // 
            this.btnEmbraport.Location = new System.Drawing.Point(180, 10);
            this.btnEmbraport.Name = "btnEmbraport";
            this.btnEmbraport.Size = new System.Drawing.Size(160, 22);
            this.btnEmbraport.TabIndex = 2;
            this.btnEmbraport.Text = "Forçar Consulta Embraport";
            this.btnEmbraport.UseVisualStyleBackColor = true;
            this.btnEmbraport.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(10, 10);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(160, 22);
            this.button1.TabIndex = 1;
            this.button1.Text = "Forçar Consulta Santos Brasil";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // pnlCentro
            // 
            this.pnlCentro.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlCentro.Location = new System.Drawing.Point(0, 39);
            this.pnlCentro.Name = "pnlCentro";
            this.pnlCentro.Size = new System.Drawing.Size(1253, 568);
            this.pnlCentro.TabIndex = 4;
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsStatus,
            this.tsTerminal,
            this.tsTempo});
            this.statusStrip1.Location = new System.Drawing.Point(0, 581);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1253, 26);
            this.statusStrip1.TabIndex = 5;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // tsStatus
            // 
            this.tsStatus.AutoSize = false;
            this.tsStatus.Name = "tsStatus";
            this.tsStatus.Size = new System.Drawing.Size(300, 21);
            this.tsStatus.Text = "Status da página";
            this.tsStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tsTerminal
            // 
            this.tsTerminal.Name = "tsTerminal";
            this.tsTerminal.Padding = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.tsTerminal.Size = new System.Drawing.Size(136, 21);
            this.tsTerminal.Text = "Terminal Consultado";
            // 
            // tsTempo
            // 
            this.tsTempo.Name = "tsTempo";
            this.tsTempo.Padding = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.tsTempo.Size = new System.Drawing.Size(119, 21);
            this.tsTempo.Text = "Próxima consulta";
            // 
            // tmrPrintsTelas
            // 
            this.tmrPrintsTelas.Enabled = true;
            this.tmrPrintsTelas.Interval = 14400000;
            this.tmrPrintsTelas.Tick += new System.EventHandler(this.tmrPrintsTelas_Tick);
            // 
            // TimerMonitoramento
            // 
            this.TimerMonitoramento.Interval = 1800000;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1253, 607);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.pnlCentro);
            this.Controls.Add(this.panel1);
            this.Name = "Form1";
            this.Text = "Consulta Container no Terminal";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnEmbraport;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Panel pnlCentro;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel tsStatus;
        private System.Windows.Forms.ToolStripStatusLabel tsTempo;
        private System.Windows.Forms.TextBox txtContainer;
        private System.Windows.Forms.ToolStripStatusLabel tsTerminal;
        private System.Windows.Forms.Timer tmrPrintsTelas;
        private System.Windows.Forms.Timer TimerMonitoramento;
    }
}

