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
            this.btnSenhas = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.chbConfirma = new System.Windows.Forms.CheckBox();
            this.txtContainer = new System.Windows.Forms.TextBox();
            this.button3 = new System.Windows.Forms.Button();
            this.btnEmbraport = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.pnlCentro = new System.Windows.Forms.Panel();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.tsStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsTerminal = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsTempo = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsContainer = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsNrRobo = new System.Windows.Forms.ToolStripStatusLabel();
            this.tmrConsulta = new System.Windows.Forms.Timer(this.components);
            this.panel1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnSenhas);
            this.panel1.Controls.Add(this.button5);
            this.panel1.Controls.Add(this.button4);
            this.panel1.Controls.Add(this.button2);
            this.panel1.Controls.Add(this.chbConfirma);
            this.panel1.Controls.Add(this.txtContainer);
            this.panel1.Controls.Add(this.button3);
            this.panel1.Controls.Add(this.btnEmbraport);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1253, 63);
            this.panel1.TabIndex = 3;
            // 
            // btnSenhas
            // 
            this.btnSenhas.Location = new System.Drawing.Point(797, 11);
            this.btnSenhas.Name = "btnSenhas";
            this.btnSenhas.Size = new System.Drawing.Size(160, 47);
            this.btnSenhas.TabIndex = 9;
            this.btnSenhas.Text = "Atualizar \r\nSenhas";
            this.btnSenhas.UseVisualStyleBackColor = true;
            this.btnSenhas.Click += new System.EventHandler(this.btnSenhas_Click);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(350, 35);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(160, 22);
            this.button5.TabIndex = 8;
            this.button5.Text = "Forçar Consulta Itapoa";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(180, 35);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(160, 22);
            this.button4.TabIndex = 7;
            this.button4.Text = "Forçar Consulta Paranagua";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(10, 35);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(160, 22);
            this.button2.TabIndex = 6;
            this.button2.Text = "Forçar Consulta VilaConde";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click_1);
            // 
            // chbConfirma
            // 
            this.chbConfirma.AutoSize = true;
            this.chbConfirma.Checked = true;
            this.chbConfirma.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chbConfirma.Location = new System.Drawing.Point(673, 27);
            this.chbConfirma.Name = "chbConfirma";
            this.chbConfirma.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.chbConfirma.Size = new System.Drawing.Size(118, 17);
            this.chbConfirma.TabIndex = 5;
            this.chbConfirma.Text = "Confirma Embarque";
            this.chbConfirma.UseVisualStyleBackColor = true;
            // 
            // txtContainer
            // 
            this.txtContainer.Location = new System.Drawing.Point(521, 24);
            this.txtContainer.Name = "txtContainer";
            this.txtContainer.Size = new System.Drawing.Size(137, 20);
            this.txtContainer.TabIndex = 4;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(350, 10);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(160, 22);
            this.button3.TabIndex = 3;
            this.button3.Text = "Forçar Consulta BTP";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
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
            this.pnlCentro.Location = new System.Drawing.Point(0, 63);
            this.pnlCentro.Name = "pnlCentro";
            this.pnlCentro.Size = new System.Drawing.Size(1253, 544);
            this.pnlCentro.TabIndex = 4;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsStatus,
            this.tsTerminal,
            this.tsTempo,
            this.tsContainer,
            this.tsNrRobo});
            this.statusStrip1.Location = new System.Drawing.Point(0, 585);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1253, 22);
            this.statusStrip1.TabIndex = 5;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // tsStatus
            // 
            this.tsStatus.AutoSize = false;
            this.tsStatus.Name = "tsStatus";
            this.tsStatus.Size = new System.Drawing.Size(300, 17);
            this.tsStatus.Text = "Status da página";
            this.tsStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tsTerminal
            // 
            this.tsTerminal.Name = "tsTerminal";
            this.tsTerminal.Padding = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.tsTerminal.Size = new System.Drawing.Size(136, 17);
            this.tsTerminal.Text = "Terminal Consultado";
            // 
            // tsTempo
            // 
            this.tsTempo.Name = "tsTempo";
            this.tsTempo.Padding = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.tsTempo.Size = new System.Drawing.Size(119, 17);
            this.tsTempo.Text = "Próxima consulta";
            // 
            // tsContainer
            // 
            this.tsContainer.Name = "tsContainer";
            this.tsContainer.Padding = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.tsContainer.Size = new System.Drawing.Size(97, 17);
            this.tsContainer.Text = "CONTAINERS";
            // 
            // tsNrRobo
            // 
            this.tsNrRobo.Name = "tsNrRobo";
            this.tsNrRobo.Size = new System.Drawing.Size(118, 17);
            this.tsNrRobo.Text = "toolStripStatusLabel1";
            // 
            // tmrConsulta
            // 
            this.tmrConsulta.Enabled = true;
            this.tmrConsulta.Interval = 15000;
            this.tmrConsulta.Tick += new System.EventHandler(this.tmrConsulta_Tick);
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
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button btnEmbraport;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Panel pnlCentro;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel tsStatus;
        private System.Windows.Forms.Timer tmrConsulta;
        private System.Windows.Forms.ToolStripStatusLabel tsContainer;
        private System.Windows.Forms.ToolStripStatusLabel tsTempo;
        private System.Windows.Forms.TextBox txtContainer;
        private System.Windows.Forms.CheckBox chbConfirma;
        private System.Windows.Forms.ToolStripStatusLabel tsTerminal;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.ToolStripStatusLabel tsNrRobo;
        private System.Windows.Forms.Button btnSenhas;
    }
}

