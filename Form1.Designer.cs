
namespace CircleDetect
{
    partial class Form_Principal
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.buttonBrowse = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.b_prim = new System.Windows.Forms.Button();
            this.b_kruskal = new System.Windows.Forms.Button();
            this.b_grafo = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.Prim_n = new System.Windows.Forms.Label();
            this.Kruskaln = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.sGrafos = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.listBox2 = new System.Windows.Forms.ListBox();
            this.listBox3 = new System.Windows.Forms.ListBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Cursor = System.Windows.Forms.Cursors.Cross;
            this.pictureBox1.Location = new System.Drawing.Point(41, 27);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(1123, 794);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseClick);
            // 
            // buttonBrowse
            // 
            this.buttonBrowse.Location = new System.Drawing.Point(252, 895);
            this.buttonBrowse.Name = "buttonBrowse";
            this.buttonBrowse.Size = new System.Drawing.Size(75, 23);
            this.buttonBrowse.TabIndex = 1;
            this.buttonBrowse.Text = "Cargar";
            this.buttonBrowse.UseVisualStyleBackColor = true;
            this.buttonBrowse.Click += new System.EventHandler(this.buttonBrowse_Click);
            // 
            // button2
            // 
            this.button2.Enabled = false;
            this.button2.Location = new System.Drawing.Point(873, 895);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 2;
            this.button2.Text = "Grafo";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 15;
            this.listBox1.Location = new System.Drawing.Point(1201, 64);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(335, 214);
            this.listBox1.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(1311, 27);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(128, 15);
            this.label2.TabIndex = 6;
            this.label2.Text = "Orden por ID (Original)";
            // 
            // b_prim
            // 
            this.b_prim.Enabled = false;
            this.b_prim.Location = new System.Drawing.Point(1201, 798);
            this.b_prim.Name = "b_prim";
            this.b_prim.Size = new System.Drawing.Size(185, 23);
            this.b_prim.TabIndex = 12;
            this.b_prim.Text = "Ver ARM Prim";
            this.b_prim.UseVisualStyleBackColor = true;
            this.b_prim.Click += new System.EventHandler(this.b_prim_Click);
            // 
            // b_kruskal
            // 
            this.b_kruskal.Enabled = false;
            this.b_kruskal.Location = new System.Drawing.Point(1486, 798);
            this.b_kruskal.Name = "b_kruskal";
            this.b_kruskal.Size = new System.Drawing.Size(178, 23);
            this.b_kruskal.TabIndex = 13;
            this.b_kruskal.Text = "Ver ARM Kruskal";
            this.b_kruskal.UseVisualStyleBackColor = true;
            this.b_kruskal.Click += new System.EventHandler(this.b_kruskal_Click);
            // 
            // b_grafo
            // 
            this.b_grafo.Enabled = false;
            this.b_grafo.Location = new System.Drawing.Point(1364, 877);
            this.b_grafo.Name = "b_grafo";
            this.b_grafo.Size = new System.Drawing.Size(146, 41);
            this.b_grafo.TabIndex = 14;
            this.b_grafo.Text = "Ver Grafo";
            this.b_grafo.UseVisualStyleBackColor = true;
            this.b_grafo.Click += new System.EventHandler(this.b_grafo_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(1201, 647);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(94, 15);
            this.label3.TabIndex = 15;
            this.label3.Text = "Arboles de Prim:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(1201, 678);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(107, 15);
            this.label1.TabIndex = 16;
            this.label1.Text = "Arboles de Kruskal:";
            // 
            // Prim_n
            // 
            this.Prim_n.AutoSize = true;
            this.Prim_n.Location = new System.Drawing.Point(1311, 647);
            this.Prim_n.Name = "Prim_n";
            this.Prim_n.Size = new System.Drawing.Size(0, 15);
            this.Prim_n.TabIndex = 17;
            // 
            // Kruskaln
            // 
            this.Kruskaln.AutoSize = true;
            this.Kruskaln.Location = new System.Drawing.Point(1314, 678);
            this.Kruskaln.Name = "Kruskaln";
            this.Kruskaln.Size = new System.Drawing.Size(0, 15);
            this.Kruskaln.TabIndex = 18;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(1201, 712);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(112, 15);
            this.label4.TabIndex = 19;
            this.label4.Text = "Subgrafos Conexos:";
            // 
            // sGrafos
            // 
            this.sGrafos.AutoSize = true;
            this.sGrafos.Location = new System.Drawing.Point(1319, 712);
            this.sGrafos.Name = "sGrafos";
            this.sGrafos.Size = new System.Drawing.Size(0, 15);
            this.sGrafos.TabIndex = 20;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(1274, 329);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(88, 15);
            this.label5.TabIndex = 23;
            this.label5.Text = "Arboles Kruskal";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(1698, 329);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(75, 15);
            this.label6.TabIndex = 24;
            this.label6.Text = "Arboles Prim";
            // 
            // listBox2
            // 
            this.listBox2.FormattingEnabled = true;
            this.listBox2.ItemHeight = 15;
            this.listBox2.Location = new System.Drawing.Point(1201, 352);
            this.listBox2.Name = "listBox2";
            this.listBox2.Size = new System.Drawing.Size(238, 184);
            this.listBox2.TabIndex = 25;
            // 
            // listBox3
            // 
            this.listBox3.FormattingEnabled = true;
            this.listBox3.ItemHeight = 15;
            this.listBox3.Location = new System.Drawing.Point(1625, 352);
            this.listBox3.Name = "listBox3";
            this.listBox3.Size = new System.Drawing.Size(238, 184);
            this.listBox3.TabIndex = 26;
            // 
            // Form_Principal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(1904, 1041);
            this.Controls.Add(this.listBox3);
            this.Controls.Add(this.listBox2);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.sGrafos);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.Kruskaln);
            this.Controls.Add(this.Prim_n);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.b_grafo);
            this.Controls.Add(this.b_kruskal);
            this.Controls.Add(this.b_prim);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.buttonBrowse);
            this.Controls.Add(this.pictureBox1);
            this.Name = "Form_Principal";
            this.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Text = "Detectar Circulo";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button buttonBrowse;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button b_prim;
        private System.Windows.Forms.Button b_kruskal;
        private System.Windows.Forms.Button b_grafo;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label Prim_n;
        private System.Windows.Forms.Label Kruskaln;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label sGrafos;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ListBox listBox2;
        private System.Windows.Forms.ListBox listBox3;
    }
}

