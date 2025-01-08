namespace VisionTech_Anbar_Project
{
    partial class AddProductForm
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
            textBox1 = new TextBox();
            label2 = new Label();
            label3 = new Label();
            label5 = new Label();
            label6 = new Label();
            label7 = new Label();
            panelDynamic = new Panel();
            comboBox1 = new ComboBox();
            comboBox2 = new ComboBox();
            label1 = new Label();
            SuspendLayout();
            // 
            // textBox1
            // 
            textBox1.BackColor = SystemColors.HighlightText;
            textBox1.ForeColor = SystemColors.InfoText;
            textBox1.Location = new Point(45, 81);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(177, 32);
            textBox1.TabIndex = 0;
            textBox1.TextChanged += textBox1_TextChanged;
            textBox1.KeyPress += textBox1_KeyPress;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.BackColor = Color.Transparent;
            label2.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point);
            label2.ForeColor = Color.DimGray;
            label2.Location = new Point(45, 54);
            label2.Name = "label2";
            label2.Size = new Size(74, 25);
            label2.TabIndex = 1;
            label2.Text = "Barkod";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.BackColor = Color.Transparent;
            label3.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point);
            label3.ForeColor = Color.DimGray;
            label3.Location = new Point(78, 145);
            label3.Name = "label3";
            label3.Size = new Size(106, 25);
            label3.TabIndex = 3;
            label3.Text = "Kateqoriya";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.BackColor = Color.Transparent;
            label5.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point);
            label5.ForeColor = Color.DimGray;
            label5.Location = new Point(387, 127);
            label5.Name = "label5";
            label5.Size = new Size(132, 25);
            label5.TabIndex = 9;
            label5.Text = "Məhsulun Adı";
            // 

            // label6
            // 
            label6.AutoSize = true;
            label6.BackColor = Color.Transparent;
            label6.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point);
            label6.ForeColor = Color.DimGray;
            label6.Location = new Point(781, 165);
            label6.Name = "label6";
            label6.Size = new Size(61, 25);
            label6.TabIndex = 11;
            label6.Text = "Daimi";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.BackColor = Color.Transparent;
            label7.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point);
            label7.ForeColor = Color.DimGray;
            label7.Location = new Point(387, 248);
            label7.Name = "label7";
            label7.Size = new Size(183, 25);
            label7.TabIndex = 14;
            label7.Text = "Əlavə barkod üçün:";
            // 
            // panelDynamic
            // 
            panelDynamic.Location = new Point(15, 173);
            panelDynamic.Name = "panelDynamic";
            panelDynamic.Size = new Size(305, 320);
            panelDynamic.TabIndex = 19;
            // 
            // comboBox1
            // 
            comboBox1.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            comboBox1.AutoCompleteSource = AutoCompleteSource.ListItems;
            comboBox1.FormattingEnabled = true;
            comboBox1.Location = new Point(375, 161);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(177, 34);
            comboBox1.TabIndex = 20;
            // 
            // comboBox2
            // 
            comboBox2.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            comboBox2.AutoCompleteSource = AutoCompleteSource.ListItems;
            comboBox2.FormattingEnabled = true;
            comboBox2.Location = new Point(578, 161);
            comboBox2.Name = "comboBox2";
            comboBox2.Size = new Size(151, 34);
            comboBox2.TabIndex = 22;
            comboBox2.SelectedIndexChanged += ComboBoxBrands_SelectedIndexChanged;
           // comboBox2.KeyDown += ComboBoxBrands_KeyDown;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = Color.Transparent;
            label1.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point);
            label1.ForeColor = Color.DimGray;
            label1.Location = new Point(615, 127);
            label1.Name = "label1";
            label1.Size = new Size(64, 25);
            label1.TabIndex = 23;
            label1.Text = "Brand";
            // 
            // AddProductForm
            // 
            AllowResize = false;
            AutoScaleDimensions = new SizeF(13F, 26F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Transparent;
            BackgroundColor = Color.AliceBlue;
            BorderColor = Color.FromArgb(42, 45, 85);
            BorderThickness = 0F;
            ClientSize = new Size(1178, 726);
            Controls.Add(panelDynamic);
            Controls.Add(label1);
            Controls.Add(comboBox2);
            Controls.Add(comboBox1);
            Controls.Add(label7);
            Controls.Add(label6);
            Controls.Add(checkBox1);
            Controls.Add(label5);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(textBox1);
            HeaderColor = Color.Transparent;
            Name = "AddProductForm";
            RightToLeftLayout = true;
            ShowBorder = true;
            SmallLineColor1 = Color.Transparent;
            SmallLineColor2 = Color.Transparent;
            TextColor = Color.Black;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox textBox1;
        private Label label2;
        private Label label3;
        private Button button1;
        private Button button2;
        private Label label5;
        private CheckBox checkBox1;
        private Label label6;
        private Label label7;
        private Panel panelDynamic;
        private ComboBox comboBox1;
        private ComboBox comboBox2;
        private MetroSet_UI.Controls.MetroSetControlBox metroSetControlBox1;
        private Label label1;
    }
}