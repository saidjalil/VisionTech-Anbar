using Serilog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VisionTech_Anbar_Project.ViewModel;
using System.Drawing;
using Image = System.Drawing.Image;

namespace VisionTech_Anbar_Project
{
    public partial class Ophrys : Form
    {
        private List<Package> Packages = new List<Package>();
        public Ophrys()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AddColumnForm addColumnForm = new AddColumnForm();
            addColumnForm.ShowDialog();
            // Example for Section 1
            Control[] section1Controls = {

            CreatePanelWithContent("Label for Section 1", "Click Me", "path_to_image.png"),
            CreatePanelWithContent("Label for Section 2", "Click Me", "path_to_image.png"),
            CreatePanelWithContent("Label for Section 3", "Click Me", "path_to_image.png"),
            CreatePanelWithContent("Label for Section 4", "Click Me", "path_to_image.png")

};

            // Add both sections to the accordion

            // Add new Package to list and UI
            if (addColumnForm.DataSaved)
            {
                AddAccordionSection("^", section1Controls, addColumnForm.NewPackage);
                //Packages.Add(addColumnForm.NewPackage);
                //AddPackageToUI(addColumnForm.NewPackage);
            }
        }
        private void AddAccordionSection(string headerText, Control[] contentControls, Package package)
        {
            // Create a panel to hold both header and content
            Panel accordionPanel = new Panel
            {
                Size = new Size(1325, 60), // Adjust the size accordingly
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Anchor = AnchorStyles.Top,
                Margin = new Padding(10)
            };

            // Create the header button (to toggle content visibility)
            Button headerButton = new Button
            {
                Text = headerText,
                Height = 30,
                Width = 40,
                Location = new Point(1275, 10), // Positioned at the top right corner
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                BackColor = Color.LightGray,
                ForeColor = Color.Black,
                Font = new Font(this.Font.FontFamily, 10, FontStyle.Bold)
            };

            // Create year label
            Label labelYear;
            labelYear = new Label();
            labelYear.Name = String.Format("LblPackageYear{0}", package.PackageId);
            labelYear.Text = package.CreatedDate.ToString();
            labelYear.Location = new Point(10, 20);
            labelYear.ForeColor = Color.Gray;
            labelYear.Font = new Font(this.Font.FontFamily, 9.5f, FontStyle.Regular);
            labelYear.Tag = package.PackageId;

            // Create the content panel (starts collapsed)
            Panel contentPanel = new Panel
            {
                Size = new Size(1325, 10), // Start collapsed
                AutoScroll = true, // Enable scrolling if content is larger
                Visible = false, // Initially collapsed
                BackColor = Color.LightGray,
                Location = new Point(0, labelYear.Bottom) // Place it directly below the header button
            };
            //Create Id label
            Label labelId;
            labelId = new Label();
            labelId.Name = String.Format("LblPackageId{0}", package.PackageId);
            labelId.Text = package.PackageId.ToString();
            labelId.Location = new Point(10, 5);
            labelId.ForeColor = Color.Black;
            labelId.Font = new Font(this.Font.FontFamily, 9.5f, FontStyle.Regular);
            labelId.AutoSize = true;
            labelId.Tag = package.PackageId;


            //Create export button
            if (package.Exported == false)
            {
                Button exportButton = new Button
                {
                    Name = "ExpBtnAction1",
                    Text = "Export",
                    Size = new Size(100, 30),
                    Location = new Point(1120, 10),
                    Tag = package.PackageId
                };
                // Add buttons to the panel
                accordionPanel.Controls.Add(exportButton);

            }
            //Create edit button
            Button editButton = new Button
            {
                Name = "EditBtnAction1",
                Text = "Edit",
                Size = new Size(100, 30),
                Location = new Point(1000, 10),
                Tag = package.PackageId
            };
            //Create Delete button
            Button deleteButton = new Button
            {
                Name = "DeleteBtnAction1",
                Text = "Delete",
                Size = new Size(100, 30),
                Location = new Point(880, 10),
                Tag = package.PackageId
            };
            //Create ProductAdd button
            Button productAddButton = new Button
            {
                Name = "AddBtnAction1",
                Text = "Add",
                Size = new Size(100, 30),
                Location = new Point(760, 10),
                Tag = package.PackageId
            };

            

            // Calculate the height for the content panel based on its controls
            int contentHeight = 0;
            foreach (var control in contentControls)
            {
                control.Dock = DockStyle.Top;
                contentPanel.Controls.Add(control);
                contentHeight += control.Height + 5; // Adjust height calculation based on the content
            }

            // Toggle content visibility when the header button is clicked
            headerButton.Click += (sender, e) =>
            {
                // Toggle visibility and adjust height of the content panel
                contentPanel.Visible = !contentPanel.Visible;
                if (contentPanel.Visible)
                {
                    contentPanel.Height = contentHeight; // Expand the content panel to fit the controls
                    accordionPanel.Height = 50 + contentPanel.Height; // Adjust the accordion panel size
                }
                else
                {
                    contentPanel.Height = 0; // Collapse the content panel
                    accordionPanel.Height = 50; // Reset the accordion height to its initial size
                }
            };

            accordionPanel.Controls.Add(editButton);
            accordionPanel.Controls.Add(productAddButton);
            accordionPanel.Controls.Add(deleteButton);

            // Add controls to panel
            accordionPanel.Controls.Add(labelId);
            accordionPanel.Controls.Add(labelYear);

            // Add the header button and content panel to the accordion panel
            accordionPanel.Controls.Add(headerButton);
            accordionPanel.Controls.Add(contentPanel);

            // Add the accordion panel to the FlowLayoutPanel
            flowLayoutPanel1.Controls.Add(accordionPanel);
        }

        // Section controls
        
        // Helper method to create a Panel with a Label, Button, and Image
        private Panel CreatePanelWithContent(string labelText, string buttonText, string imagePath)
        {
            // Create a Panel
            Panel panel = new Panel
            {
                Height = 80,  // Adjust based on the content size
                Width = 400,
                BorderStyle = BorderStyle.FixedSingle
            };

            // Create a Label
            Label label = new Label
            {
                Text = labelText,
                Location = new Point(10, 10),
                AutoSize = true
            };

            // Create a Button
            Button button = new Button
            {
                Text = buttonText,
                Location = new Point(250, 10),
                Size = new Size(100, 30)
            };

            // Create a PictureBox for the image
            PictureBox pictureBox = new PictureBox
            {
                Location = new Point(150, 10),
                Size = new Size(80, 50), // Adjust image size
                SizeMode = PictureBoxSizeMode.StretchImage // Stretch image to fit
            };

            // Load the image from file path
            if (System.IO.File.Exists(imagePath))
            {
                pictureBox.Image = Image.FromFile(imagePath);
            }
            else
            {

            }
            {
                pictureBox.BackColor = Color.Gray; // Default to gray if no image
            }

            // Add the controls to the Panel
            panel.Controls.Add(label);
            panel.Controls.Add(button);
            panel.Controls.Add(pictureBox);

            return panel;
        }



        //private void AddPackageToUI(Package package)
        //{
        //    //Create Panel
        //    Panel panel;
        //    panel = new Panel();
        //    panel.Name = String.Format("PnlPackage{0}", package.PackageId);
        //    panel.BackColor = Color.White;
        //    panel.Size = new Size(1325, 45);
        //    panel.Margin = new Padding(10);
        //    panel.Tag = package.PackageId;
        //    panel.BorderStyle = BorderStyle.FixedSingle;

        //    //Create Id label
        //    Label labelId;
        //    labelId = new Label();
        //    labelId.Name = String.Format("LblPackageId{0}", package.PackageId);
        //    labelId.Text = package.PackageId.ToString();
        //    labelId.Location = new Point(10, 10);
        //    labelId.ForeColor = Color.Black;
        //    labelId.Font = new Font(this.Font.FontFamily, 9.5f, FontStyle.Regular);
        //    labelId.AutoSize = true;
        //    labelId.Tag = package.PackageId;


        //    //Create export button
        //    if(package.Exported == false)
        //    {
        //        Button exportButton = new Button
        //        {
        //            Name = "ExpBtnAction1",
        //            Text = "Export",
        //            Size = new Size(100, 30),
        //            Location = new Point(1120, 10),
        //            Tag = package.PackageId
        //        };
        //        // Add buttons to the panel
        //        panel.Controls.Add(exportButton);

        //    }
        //    //Create edit button
        //    Button editButton = new Button
        //    {
        //        Name = "EditBtnAction1",
        //        Text = "Edit",
        //        Size = new Size(100, 30),
        //        Location = new Point(1000, 10),
        //        Tag = package.PackageId
        //    };
        //    //Create Delete button
        //    Button deleteButton = new Button
        //    {
        //        Name = "DeleteBtnAction1",
        //        Text = "Delete",
        //        Size = new Size(100, 30),
        //        Location = new Point(880, 10),
        //        Tag = package.PackageId
        //    };
        //    //Create ProductAdd button
        //    Button productAddButton = new Button
        //    {
        //        Name = "AddBtnAction1",
        //        Text = "Add",
        //        Size = new Size(100, 30),
        //        Location = new Point(760, 10),
        //        Tag = package.PackageId
        //    };



        //    // Create year label
        //    Label labelYear;
        //    labelYear = new Label();
        //    labelYear.Name = String.Format("LblPackageYear{0}", package.PackageId);
        //    labelYear.Text = package.CreatedDate.ToString();
        //    labelYear.Location = new Point(1235, 20);
        //    labelYear.ForeColor = Color.Gray;
        //    labelYear.Font = new Font(this.Font.FontFamily, 9.5f, FontStyle.Regular);
        //    labelYear.Tag = package.PackageId;


        //    // Add buttons to the panel
        //    panel.Controls.Add(editButton);
        //    panel.Controls.Add(productAddButton);
        //    panel.Controls.Add(deleteButton);



        //    // Add controls to panel
        //    panel.Controls.Add(labelId);
        //    panel.Controls.Add(labelYear);

        //    // Add panel to flowlayoutpanel
        //    flowLayoutPanel1.Controls.Add(panel);
        //}

        //private void AddAccordionSection(string headerText, Control[] contentControls, Package package)
        //{

        //    // Create a panel to hold both header and content
        //    Panel accordionPanel = new Panel
        //    {
        //        //Size = new Size(1325, 50), // Adjust the size accordingly
        //        Width = flowLayoutPanel1.Width - 20,
        //        Anchor = AnchorStyles.Top | AnchorStyles.Bottom,
        //        BackColor = Color.White,
        //        BorderStyle = BorderStyle.FixedSingle,
        //        AutoSize = true,
        //        AutoSizeMode = AutoSizeMode.GrowAndShrink,
        //        Margin = new Padding(10)
        //    };

        //    // Create the header button (to toggle content visibility)
        //    Button headerButton = new Button
        //    {
        //        Text = headerText,
        //        Dock = DockStyle.Right,
        //        //Height = 10,
        //        //FlatStyle = FlatStyle.Flat,
        //        BackColor = Color.LightGray,
        //        ForeColor = Color.Black,
        //        Font = new Font(this.Font.FontFamily, 10, FontStyle.Bold)
        //    };

        //    // Create the content panel
        //    Panel contentPanel = new Panel
        //    {
        //        Dock = DockStyle.Fill,
        //       // Height = 0, // Start collapsed
        //        AutoScroll = true, // Enable scrolling if content is larger
        //        Visible = false, // Initially collapsed
        //        AutoSize = true,
        //        AutoSizeMode = AutoSizeMode.GrowAndShrink,
        //        BackColor = Color.LightGray
        //    };

        //    //Create Id label
        //    Label labelId;
        //    labelId = new Label();
        //    labelId.Name = String.Format("LblPackageId{0}", package.PackageId);
        //    labelId.Text = package.PackageId.ToString();
        //    labelId.Location = new Point(10, 5);
        //    labelId.ForeColor = Color.Black;
        //    labelId.Font = new Font(this.Font.FontFamily, 9.5f, FontStyle.Regular);
        //    labelId.AutoSize = true;
        //    labelId.Tag = package.PackageId;


        //    //Create export button
        //    if (package.Exported == false)
        //    {
        //        Button exportButton = new Button
        //        {
        //            Name = "ExpBtnAction1",
        //            Text = "Export",
        //            Size = new Size(100, 30),
        //            Location = new Point(1120, 10),
        //            Tag = package.PackageId
        //        };
        //        // Add buttons to the panel
        //        accordionPanel.Controls.Add(exportButton);

        //    }
        //    //Create edit button
        //    Button editButton = new Button
        //    {
        //        Name = "EditBtnAction1",
        //        Text = "Edit",
        //        Size = new Size(100, 30),
        //        Location = new Point(1000, 10),
        //        Tag = package.PackageId
        //    };
        //    //Create Delete button
        //    Button deleteButton = new Button
        //    {
        //        Name = "DeleteBtnAction1",
        //        Text = "Delete",
        //        Size = new Size(100, 30),
        //        Location = new Point(880, 10),
        //        Tag = package.PackageId
        //    };
        //    //Create ProductAdd button
        //    Button productAddButton = new Button
        //    {
        //        Name = "AddBtnAction1",
        //        Text = "Add",
        //        Size = new Size(100, 30),
        //        Location = new Point(760, 10),
        //        Tag = package.PackageId
        //    };

        //    // Create year label
        //    Label labelYear;
        //    labelYear = new Label();
        //    labelYear.Name = String.Format("LblPackageYear{0}", package.PackageId);
        //    labelYear.Text = package.CreatedDate.ToString();
        //    labelYear.Location = new Point(10, 20);
        //    labelYear.ForeColor = Color.Gray;
        //    labelYear.Font = new Font(this.Font.FontFamily, 9.5f, FontStyle.Regular);
        //    labelYear.Tag = package.PackageId;

        //    // Add controls to the content panel
        //    foreach (var control in contentControls)
        //    {
        //        contentPanel.Controls.Add(control);
        //    }
        //    // Toggle content visibility when the header button is clicked
        //    headerButton.Click += (sender, e) =>
        //    {
        //        // Toggle visibility and height of the content panel
        //        contentPanel.Visible = !contentPanel.Visible;
        //        if (contentPanel.Visible)
        //        {
        //            //contentPanel.Height = contentControls.Length * 30; // Adjust based on content size
        //           // accordionPanel.Height += contentPanel.Height; // Expand accordion
        //        }
        //        else
        //        {
        //           // accordionPanel.Height = 50; // Collapse accordion
        //        }
        //    };

        //    accordionPanel.Controls.Add(editButton);
        //    accordionPanel.Controls.Add(productAddButton);
        //    accordionPanel.Controls.Add(deleteButton);

        //    // Add controls to panel
        //    accordionPanel.Controls.Add(labelId);
        //    accordionPanel.Controls.Add(labelYear);

        //    // Add the header and content panels to the accordion panel
        //    accordionPanel.Controls.Add(contentPanel);
        //    accordionPanel.Controls.Add(headerButton);

        //    // Add the accordion panel to the flowLayoutPanel
        //    flowLayoutPanel1.Controls.Add(accordionPanel);
        //}

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void metroSetBadge1_Click(object sender, EventArgs e)
        {

        }
    }
}
