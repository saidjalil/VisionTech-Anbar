namespace VisionTech_Anbar_Project
{
    partial class Ophrys
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Ophrys));
            ListViewItem listViewItem2 = new ListViewItem("");
            pictureBox1 = new PictureBox();
            button1 = new Button();
            listView1 = new ListView();
            columnHeader1 = new ColumnHeader();
            columnHeader2 = new ColumnHeader();
            columnHeader3 = new ColumnHeader();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // pictureBox1
            // 
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(2, 4);
            pictureBox1.Margin = new Padding(3, 4, 3, 4);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(192, 66);
            pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // button1
            // 
            button1.Location = new Point(673, 61);
            button1.Margin = new Padding(3, 4, 3, 4);
            button1.Name = "button1";
            button1.Size = new Size(42, 31);
            button1.TabIndex = 1;
            button1.Text = "+";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // listView1
            // 
            listView1.Columns.AddRange(new ColumnHeader[] { columnHeader1, columnHeader2, columnHeader3 });
            listView1.Items.AddRange(new ListViewItem[] { listViewItem2 });
            listView1.Location = new Point(64, 164);
            listView1.Margin = new Padding(3, 4, 3, 4);
            listView1.Name = "listView1";
            listView1.Size = new Size(635, 239);
            listView1.TabIndex = 2;
            listView1.UseCompatibleStateImageBehavior = false;
            // 
            // Ophrys
            // 
            AccessibleRole = AccessibleRole.Grip;
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(881, 503);
            Controls.Add(listView1);
            Controls.Add(button1);
            Controls.Add(pictureBox1);
            FormBorderStyle = FormBorderStyle.None;
            Margin = new Padding(3, 4, 3, 4);
            Name = "Ophrys";
            Text = "Ophrys";
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox pictureBox1;
        private Button button1;
        private ListView listView1;
        private ColumnHeader columnHeader1;
        private ColumnHeader columnHeader2;
        private ColumnHeader columnHeader3;
    }
}