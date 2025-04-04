
namespace Lab_3
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ComboBox comboBoxTMO;
        private System.Windows.Forms.Button buttonApply;
        private System.Windows.Forms.Button buttonClear;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.comboBoxTMO = new System.Windows.Forms.ComboBox();
            this.buttonApply = new System.Windows.Forms.Button();
            this.buttonClear = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.White;
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(4);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(1067, 738);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Paint += new System.Windows.Forms.PaintEventHandler(this.PictureBox1_Paint);
            this.pictureBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PictureBox1_MouseDown);
            // 
            // comboBoxTMO
            // 
            this.comboBoxTMO.Items.AddRange(new object[] {
            "Объединение",
            "Пересечение",
            "Симметричная разность",
            "Разность A\\B",
            "Разность B\\A"});
            this.comboBoxTMO.Location = new System.Drawing.Point(224, 15);
            this.comboBoxTMO.Margin = new System.Windows.Forms.Padding(4);
            this.comboBoxTMO.Name = "comboBoxTMO";
            this.comboBoxTMO.Size = new System.Drawing.Size(199, 24);
            this.comboBoxTMO.TabIndex = 2;
            this.comboBoxTMO.SelectedIndexChanged += new System.EventHandler(this.ComboBoxTMO_SelectedIndexChanged);
            // 
            // buttonApply
            // 
            this.buttonApply.Location = new System.Drawing.Point(432, 12);
            this.buttonApply.Margin = new System.Windows.Forms.Padding(4);
            this.buttonApply.Name = "buttonApply";
            this.buttonApply.Size = new System.Drawing.Size(160, 28);
            this.buttonApply.TabIndex = 3;
            this.buttonApply.Text = "Выполнить операцию";
            this.buttonApply.UseVisualStyleBackColor = true;
            this.buttonApply.Click += new System.EventHandler(this.buttonApply_Click);
            // 
            // buttonClear
            // 
            this.buttonClear.Location = new System.Drawing.Point(600, 12);
            this.buttonClear.Margin = new System.Windows.Forms.Padding(4);
            this.buttonClear.Name = "buttonClear";
            this.buttonClear.Size = new System.Drawing.Size(100, 28);
            this.buttonClear.TabIndex = 4;
            this.buttonClear.Text = "Очистить";
            this.buttonClear.UseVisualStyleBackColor = true;
            this.buttonClear.Click += new System.EventHandler(this.buttonClear_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1067, 738);
            this.Controls.Add(this.buttonClear);
            this.Controls.Add(this.buttonApply);
            this.Controls.Add(this.comboBoxTMO);
            this.Controls.Add(this.pictureBox1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Form1";
            this.Text = "Операции с полигонами";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }
    }
}

