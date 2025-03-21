
namespace Lab_2_Polygons
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.colorPickButton = new System.Windows.Forms.Button();
            this.fillStyleComboBox = new System.Windows.Forms.ComboBox();
            this.clearButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox
            // 
            this.pictureBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pictureBox.Location = new System.Drawing.Point(168, 15);
            this.pictureBox.Margin = new System.Windows.Forms.Padding(4);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(891, 548);
            this.pictureBox.TabIndex = 0;
            this.pictureBox.TabStop = false;
            this.pictureBox.Click += new System.EventHandler(this.pictureBox1_Click);
            this.pictureBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseDown);
            // 
            // colorDialog1
            // 
            this.colorDialog1.AnyColor = true;
            this.colorDialog1.FullOpen = true;
            this.colorDialog1.SolidColorOnly = true;
            // 
            // colorPickButton
            // 
            this.colorPickButton.Location = new System.Drawing.Point(113, 79);
            this.colorPickButton.Margin = new System.Windows.Forms.Padding(4);
            this.colorPickButton.Name = "colorPickButton";
            this.colorPickButton.Size = new System.Drawing.Size(20, 16);
            this.colorPickButton.TabIndex = 2;
            this.colorPickButton.UseVisualStyleBackColor = true;
            this.colorPickButton.Click += new System.EventHandler(this.colorPickButton_click);
            // 
            // fillStyleComboBox
            // 
            this.fillStyleComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.fillStyleComboBox.FormattingEnabled = true;
            this.fillStyleComboBox.Items.AddRange(new object[] {
            "Неориентированный",
            "Ориентированный"});
            this.fillStyleComboBox.Location = new System.Drawing.Point(3, 38);
            this.fillStyleComboBox.Margin = new System.Windows.Forms.Padding(4);
            this.fillStyleComboBox.Name = "fillStyleComboBox";
            this.fillStyleComboBox.Size = new System.Drawing.Size(160, 24);
            this.fillStyleComboBox.TabIndex = 3;
            // 
            // clearButton
            // 
            this.clearButton.Location = new System.Drawing.Point(33, 118);
            this.clearButton.Margin = new System.Windows.Forms.Padding(4);
            this.clearButton.Name = "clearButton";
            this.clearButton.Size = new System.Drawing.Size(100, 28);
            this.clearButton.TabIndex = 4;
            this.clearButton.Text = "Очистить";
            this.clearButton.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 18);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(142, 16);
            this.label1.TabIndex = 5;
            this.label1.Text = "Тип многоугольника";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 79);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(97, 16);
            this.label2.TabIndex = 7;
            this.label2.Text = "Выбрать цвет";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(1067, 578);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.clearButton);
            this.Controls.Add(this.fillStyleComboBox);
            this.Controls.Add(this.colorPickButton);
            this.Controls.Add(this.pictureBox);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MinimumSize = new System.Drawing.Size(554, 285);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Закрашивание многоугольников";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.SizeChanged += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.Button colorPickButton;
        private System.Windows.Forms.ComboBox fillStyleComboBox;
        private System.Windows.Forms.Button clearButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}

