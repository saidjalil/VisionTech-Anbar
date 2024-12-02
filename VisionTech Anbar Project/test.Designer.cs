using System.ComponentModel;

namespace VisionTech_Anbar_Project;

partial class test
{
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private IContainer components = null;

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
        panelDynamic = new Panel();
        btnAddTopCategory = new Button();
        SuspendLayout();
        // 
        // panelDynamic
        // 
        panelDynamic.Location = new Point(129, 82);
        panelDynamic.Name = "panelDynamic";
        panelDynamic.Size = new Size(409, 133);
        panelDynamic.TabIndex = 0;
        // 
        // btnAddTopCategory
        // 
        btnAddTopCategory.Location = new Point(555, 89);
        btnAddTopCategory.Name = "btnAddTopCategory";
        btnAddTopCategory.Size = new Size(94, 29);
        btnAddTopCategory.TabIndex = 1;
        btnAddTopCategory.Text = "button1";
        btnAddTopCategory.UseVisualStyleBackColor = true;
        // 
        // test
        // 
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(800, 450);
        Controls.Add(btnAddTopCategory);
        Controls.Add(panelDynamic);
        Name = "test";
        Text = "test";
        ResumeLayout(false);
    }

    #endregion

    private Panel panelDynamic;
    private Button btnAddTopCategory;
}