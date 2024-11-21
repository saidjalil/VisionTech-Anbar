
namespace VisionTech_Anbar_Project
{
    partial class AddColumnForm
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
            button1 = new Button();
            textBox1 = new TextBox();
            label4 = new Label();
            dateTimePicker1 = new DateTimePicker();
            metroSetControlBox1 = new MetroSet_UI.Controls.MetroSetControlBox();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Location = new Point(103, 202);
            button1.Name = "button1";
            button1.Size = new Size(75, 30);
            button1.TabIndex = 0;
            button1.Text = "Add";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(103, 118);
            textBox1.Margin = new Padding(0);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(111, 27);
            textBox1.TabIndex = 6;
            textBox1.TextChanged += textBox2_TextChanged;
            textBox1.KeyPress += textBox1_KeyPress;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(80, 121);
            label4.Name = "label4";
            label4.Size = new Size(24, 22);
            label4.TabIndex = 8;
            label4.Text = "Id";
            label4.Click += label4_Click;
            // 
            // dateTimePicker1
            // 
            dateTimePicker1.Location = new Point(14, 157);
            dateTimePicker1.Name = "dateTimePicker1";
            dateTimePicker1.Size = new Size(200, 27);
            dateTimePicker1.TabIndex = 9;
            // 
            // metroSetControlBox1
            // 
            metroSetControlBox1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            metroSetControlBox1.CloseHoverBackColor = Color.FromArgb(183, 40, 40);
            metroSetControlBox1.CloseHoverForeColor = Color.White;
            metroSetControlBox1.CloseNormalForeColor = Color.Gray;
            metroSetControlBox1.DisabledForeColor = Color.DimGray;
            metroSetControlBox1.IsDerivedStyle = true;
            metroSetControlBox1.Location = new Point(145, 0);
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
            metroSetControlBox1.TabIndex = 10;
            metroSetControlBox1.Text = "metroSetControlBox1";
            metroSetControlBox1.ThemeAuthor = "Narwin";
            metroSetControlBox1.ThemeName = "MetroLite";
            // 
            // AddColumnForm
            // 
            AutoScaleDimensions = new SizeF(10F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(244, 254);
            Controls.Add(metroSetControlBox1);
            Controls.Add(dateTimePicker1);
            Controls.Add(label4);
            Controls.Add(textBox1);
            Controls.Add(button1);
            Name = "AddColumnForm";
            Text = "AddPackage";
            ResumeLayout(false);
            PerformLayout();
        }

        private void label4_Click(object sender, EventArgs e)
        {
            
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        #endregion

        private Button button1;
        private TextBox textBox1;
        private Label label4;
        private DateTimePicker dateTimePicker1;
        private MetroSet_UI.Controls.MetroSetControlBox metroSetControlBox1;
    }
}