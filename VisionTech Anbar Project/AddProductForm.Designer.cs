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
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            textBox2 = new TextBox();
            label4 = new Label();
            textBox3 = new TextBox();
            button1 = new Button();
            metroSetControlBox1 = new MetroSet_UI.Controls.MetroSetControlBox();
            SuspendLayout();
            // 
            // textBox1
            // 
            textBox1.Location = new Point(118, 124);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(177, 27);
            textBox1.TabIndex = 0;
            // 
            // label1
            // 
            label1.Location = new Point(0, 0);
            label1.Name = "label1";
            label1.Size = new Size(100, 23);
            label1.TabIndex = 0;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(43, 127);
            label2.Name = "label2";
            label2.Size = new Size(57, 22);
            label2.TabIndex = 1;
            label2.Text = "Name";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(12, 173);
            label3.Name = "label3";
            label3.Size = new Size(100, 22);
            label3.TabIndex = 3;
            label3.Text = "Description";
            // 
            // textBox2
            // 
            textBox2.Location = new Point(118, 170);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(177, 27);
            textBox2.TabIndex = 2;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(23, 219);
            label4.Name = "label4";
            label4.Size = new Size(77, 22);
            label4.TabIndex = 5;
            label4.Text = "Quantity";
            // 
            // textBox3
            // 
            textBox3.Location = new Point(118, 216);
            textBox3.Name = "textBox3";
            textBox3.Size = new Size(177, 27);
            textBox3.TabIndex = 4;
            textBox3.KeyPress += textBox3_KeyPress;
            // 
            // button1
            // 
            button1.Location = new Point(118, 285);
            button1.Name = "button1";
            button1.Size = new Size(75, 33);
            button1.TabIndex = 6;
            button1.Text = "Add";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // metroSetControlBox1
            // 
            metroSetControlBox1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            metroSetControlBox1.CloseHoverBackColor = Color.FromArgb(183, 40, 40);
            metroSetControlBox1.CloseHoverForeColor = Color.White;
            metroSetControlBox1.CloseNormalForeColor = Color.Gray;
            metroSetControlBox1.DisabledForeColor = Color.DimGray;
            metroSetControlBox1.IsDerivedStyle = true;
            metroSetControlBox1.Location = new Point(253, 0);
            metroSetControlBox1.Margin = new Padding(3, 2, 3, 2);
            metroSetControlBox1.MaximizeBox = true;
            metroSetControlBox1.MaximizeHoverBackColor = Color.FromArgb(238, 238, 238);
            metroSetControlBox1.MaximizeHoverForeColor = Color.Gray;
            metroSetControlBox1.MaximizeNormalForeColor = Color.Gray;
            metroSetControlBox1.MinimizeBox = true;
            metroSetControlBox1.MinimizeHoverBackColor = Color.FromArgb(238, 238, 238);
            metroSetControlBox1.MinimizeHoverForeColor = Color.Gray;
            metroSetControlBox1.MinimizeNormalForeColor = Color.Gray;
            metroSetControlBox1.Name = "metroSetControlBox1";
            metroSetControlBox1.Size = new Size(100, 25);
            metroSetControlBox1.Style = MetroSet_UI.Enums.Style.Light;
            metroSetControlBox1.StyleManager = null;
            metroSetControlBox1.TabIndex = 7;
            metroSetControlBox1.Text = "metroSetControlBox1";
            metroSetControlBox1.ThemeAuthor = "Narwin";
            metroSetControlBox1.ThemeName = "MetroLite";
            // 
            // AddProductForm
            // 
            AutoScaleDimensions = new SizeF(10F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(351, 370);
            Controls.Add(metroSetControlBox1);
            Controls.Add(button1);
            Controls.Add(label4);
            Controls.Add(textBox3);
            Controls.Add(label3);
            Controls.Add(textBox2);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(textBox1);
            Name = "AddProductForm";
            Text = "AddProduct";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox textBox1;
        private Label label1;
        private Label label2;
        private Label label3;
        private TextBox textBox2;
        private Label label4;
        private TextBox textBox3;
        private Button button1;
        private MetroSet_UI.Controls.MetroSetControlBox metroSetControlBox1;
    }
}