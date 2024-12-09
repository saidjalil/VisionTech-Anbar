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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Ophrys));
            pictureBox1 = new PictureBox();
            metroSetControlBox1 = new MetroSet_UI.Controls.MetroSetControlBox();
            contextMenuStrip1 = new ContextMenuStrip(components);
            button3 = new Button();
            ExportAll = new Button();
            tableLayoutPanel1 = new TableLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // pictureBox1
            // 
            pictureBox1.BackColor = Color.Transparent;
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(15, 27);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(192, 66);
            pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            pictureBox1.Click += pictureBox1_Click;
            // 
            // metroSetControlBox1
            // 
            metroSetControlBox1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            metroSetControlBox1.CloseHoverBackColor = Color.FromArgb(183, 40, 40);
            metroSetControlBox1.CloseHoverForeColor = Color.White;
            metroSetControlBox1.CloseNormalForeColor = Color.Gray;
            metroSetControlBox1.DisabledForeColor = Color.DimGray;
            metroSetControlBox1.IsDerivedStyle = true;
            metroSetControlBox1.Location = new Point(575, 2);
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
            metroSetControlBox1.TabIndex = 3;
            metroSetControlBox1.Text = "metroSetControlBox1";
            metroSetControlBox1.ThemeAuthor = "Narwin";
            metroSetControlBox1.ThemeName = "MetroLite";
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.ImageScalingSize = new Size(20, 20);
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new Size(61, 4);
            // 
            // button3
            // 
            button3.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            button3.BackColor = Color.FromArgb(42, 45, 85);
            button3.ForeColor = Color.Transparent;
            button3.Location = new Point(561, 51);
            button3.Name = "button3";
            button3.Size = new Size(64, 39);
            button3.TabIndex = 17;
            button3.Text = "+";
            button3.UseVisualStyleBackColor = false;
            button3.Click += button1_Click;
            // 
            // ExportAll
            // 
            ExportAll.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            ExportAll.Font = new Font("Microsoft Sans Serif", 14.25F, FontStyle.Regular, GraphicsUnit.Point);
            ExportAll.ForeColor = Color.FromArgb(42, 45, 85);
            ExportAll.Location = new Point(631, 50);
            ExportAll.Name = "ExportAll";
            ExportAll.Size = new Size(44, 39);
            ExportAll.TabIndex = 18;
            ExportAll.Text = "📤 ";
            ExportAll.TextAlign = ContentAlignment.MiddleRight;
            ExportAll.UseVisualStyleBackColor = true;
            ExportAll.Click += ExportAllButton_Click;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Location = new Point(15, 96);
            tableLayoutPanel1.Margin = new Padding(0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 2;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Size = new Size(660, 289);
            tableLayoutPanel1.TabIndex = 19;
            // 
            // Ophrys
            // 
            AccessibleRole = AccessibleRole.Grip;
            AutoScaleDimensions = new SizeF(10F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BorderColor = Color.FromArgb(42, 45, 85);
            BorderThickness = 5F;
            ClientSize = new Size(690, 400);
            Controls.Add(tableLayoutPanel1);
            Controls.Add(ExportAll);
            Controls.Add(button3);
            Controls.Add(metroSetControlBox1);
            Controls.Add(pictureBox1);
            Name = "Ophrys";
            ShowBorder = true;
            SmallLineColor1 = Color.Transparent;
            SmallLineColor2 = Color.Transparent;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox pictureBox1;
        private MetroSet_UI.Controls.MetroSetControlBox metroSetControlBox1;
        private ContextMenuStrip contextMenuStrip1;
        private Button button3;
        private Button ExportAll;
        private TableLayoutPanel tableLayoutPanel1;
    }
}