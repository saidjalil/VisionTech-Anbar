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
using System.Drawing;
using Image = System.Drawing.Image;
using MetroSet_UI.Forms;
using VisionTech_Anbar_Project.Utilts;
using VisionTech_Anbar_Project.Services;
using VisionTech_Anbar_Project.Entities;

namespace VisionTech_Anbar_Project
{
    public partial class Ophrys : MetroSetForm
    {
        private readonly PackageService packageService;
        TableLayoutPanel mainTableLayoutPanel;
        public Ophrys()
        {
            packageService = new PackageService(new ());

            InitializeComponent();
            SetupMainTableLayoutPanel();
            InitializeItems();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            AddColumnForm addColumnForm = new AddColumnForm();
            addColumnForm.ShowDialog();
            // Example for Section 1
            //            Control[] section1Controls = {

            //            CreatePanelWithContent("Label for Section 1", "Click Me", "path_to_image.png"),
            //            CreatePanelWithContent("Label for Section 2", "Click Me", "path_to_image.png"),
            //            CreatePanelWithContent("Label for Section 3", "Click Me", "path_to_image.png"),
            //            CreatePanelWithContent("Label for Section 4", "Click Me", "path_to_image.png")

            //};

            // Add both sections to the accordion

            // Add new Package to list and UI
            if (addColumnForm.DataSaved)
            {
                //CreateItemPanel(addColumnForm.NewPackage);
                //JsonManager.AddPackage(addColumnForm.NewPackage);
                await packageService.CreatePackageAsync(addColumnForm.NewPackage);
                RestartPage();
                InitializeItems();
                //AddAccordionSection("^", section1Controls, addColumnForm.NewPackage);
                //InitializeItems(addColumnForm.NewPackage);
                //Packages.Add(addColumnForm.NewPackage);
                //AddPackageToUI(addColumnForm.NewPackage);
            }
        }
        private void SetupMainTableLayoutPanel()
        {
            mainTableLayoutPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                Padding = new Padding(0, 30, 0, 0), // Add padding from the top
                AutoScroll = true,
                ColumnCount = 1, // Single column to arrange items vertically
            };
            this.Controls.Add(mainTableLayoutPanel);
        }

        private async void InitializeItems()
        {
            var data = await packageService.GetAllPackageWithNavigation();
            foreach (var item in data)
            {
                Panel itemPanel = CreateItemPanel(item);
                mainTableLayoutPanel.RowCount++;
                mainTableLayoutPanel.Controls.Add(itemPanel, 0, mainTableLayoutPanel.RowCount - 1); // Add item to new row

                var products = item.PackageProducts.Select(pp => new PackageProduct
                {
                    Product = pp.Product,
                    Quantity = pp.Quantity
                });
                foreach (var product in products)
                {
                    Panel subItemsPanel = CreateSubItemsPanel(product.Product, product.Quantity);
                    mainTableLayoutPanel.RowCount++;
                    mainTableLayoutPanel.Controls.Add(subItemsPanel, 0, mainTableLayoutPanel.RowCount - 1); // Add subitems below item
                    subItemsPanel.Visible = false; // Initially hidden
                }
            }
        }

        public void RestartPage()
        {
            mainTableLayoutPanel.Controls.Clear();
        }

        private Panel CreateItemPanel(Package package)
        {
            // Create the main panel for the item
            Panel itemPanel = new Panel
            {
                Height = 60, // Set explicit height for the main item
                BorderStyle = BorderStyle.None,
                Dock = DockStyle.Top,
                Margin = new Padding(5) // Add some margin between items
            };

            // Label to display item text
            Label itemLabel = new Label
            {
                Text = package.Warehouse.WarehouseName.ToString(),
                AutoSize = true,
                Location = new System.Drawing.Point(5, 15)
            };

            // Create a FlowLayoutPanel to hold all buttons in a single line
            FlowLayoutPanel buttonPanel = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.LeftToRight, // Align buttons horizontally
                Dock = DockStyle.Right,
                AutoSize = true,
                WrapContents = false, // Prevent buttons from wrapping to the next line
                Margin = new Padding(5)
            };

            // Button to expand/collapse subitems
            Button expandButton = new Button
            {
                Text = "▽",
                Width = 30,
                Height = 40,
                Margin = new Padding(5)
            };
            expandButton.Click += (s, e) => ToggleSubItems(itemPanel);

            // Add Delete, Add, Edit, and Export buttons
            Button deleteButton = new Button
            {
                Text = "🗑",
                Tag = package.Id,
                ForeColor = Color.Red,
                Width = 50,
                Height = 40,
                Margin = new Padding(5)
            };

            deleteButton.Click += DeleteButton_Click;
            Button addButton = new Button
            {
                Text = "➕",
                Font = new Font("Arial", 10, FontStyle.Bold),
                BackColor = Color.FromArgb(42, 45, 85),
                ForeColor = Color.Transparent,
                Tag = package.Id,
                Width = 60,
                Height = 40,
                Margin = new Padding(5)
            };
            addButton.Click += AddButton_Click;

            Button editButton = new Button
            {
                Text = "✎",
                Font = new Font("Arial", 10, FontStyle.Bold),
                BackColor = Color.FromArgb(42, 45, 85),
                ForeColor = Color.Transparent,
                Tag = package.Id,
                Width = 60,
                Height = 40,
                Margin = new Padding(5)
            };
            editButton.Click += EditButton_Click;

            Button exportButton = new Button
            {
                Text = "⇪",
                Font = new Font("Arial", 15, FontStyle.Bold),
                BackColor = Color.FromArgb(42, 45, 85),
                ForeColor = Color.Transparent,
                Tag = package.Id,
                Width = 60,
                Height = 40,
                Margin = new Padding(5)
            };

            // Add the buttons to the FlowLayoutPanel

           
            buttonPanel.Controls.Add(addButton);
            buttonPanel.Controls.Add(editButton);
            buttonPanel.Controls.Add(exportButton);
            buttonPanel.Controls.Add(deleteButton);
            buttonPanel.Controls.Add(expandButton);

            // Add the label and button panel to the item panel
            itemPanel.Controls.Add(itemLabel);
            itemPanel.Controls.Add(buttonPanel);

            return itemPanel;
        }

        private Panel CreateSubItemsPanel(Product products, int quantity)
        {
            Panel subItemsPanel = new Panel
            {
                Height = 100,
                AutoScroll = true,
                BorderStyle = BorderStyle.None,
                Padding = new Padding(5),
                Dock = DockStyle.Top
            };

            // Add example subitems with full-width panels
            if (products != null)
            {
              
                    Panel subItemPanel = new Panel
                    {
                        Height = 30,
                        Dock = DockStyle.Top,
                        BorderStyle = BorderStyle.None,
                        Padding = new Padding(5)
                    };

                    Label subItemLabel = new Label
                    {
                        Text = $"Name:{products.ProductName} Count:{quantity}",
                        AutoSize = true,
                        Location = new System.Drawing.Point(10, 5)
                    };

                    subItemPanel.Controls.Add(subItemLabel);
                    subItemsPanel.Controls.Add(subItemPanel);
                }
            

            return subItemsPanel;
        }

        private void ToggleSubItems(Panel itemPanel)
        {
            // Find the associated subItemsPanel below itemPanel in the mainTableLayoutPanel
            int itemIndex = mainTableLayoutPanel.Controls.GetChildIndex(itemPanel);
            if (itemIndex >= 0 && itemIndex < mainTableLayoutPanel.Controls.Count - 1)
            {
                Panel subItemsPanel = mainTableLayoutPanel.Controls[itemIndex + 1] as Panel;
                if (subItemsPanel != null)
                {
                    subItemsPanel.Visible = !subItemsPanel.Visible;
                }
            }
        }

        public void EditButton_Click(object sender, EventArgs e)
        {

            EditProductForm editProductForm = new EditProductForm();
            editProductForm.Show();

            Button button = sender as Button;
            editProductForm.GetProducts(JsonManager.GetPackageById(button.Tag.ToString()));
        }

        public void DeleteButton_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;
            JsonManager.DeletePackageById(button.Tag.ToString());
            RestartPage();
            InitializeItems();
        }
        public async void AddButton_Click(object sender, EventArgs e)
        {
            AddProductForm addProductForm = new AddProductForm();
            addProductForm.ShowDialog();
            Button button = sender as Button;
           //if(Convert.ToInt32(button.Tag))

            if (addProductForm.DataSaved && addProductForm.NewProduct != null)
            {
                //JsonManager.AddProductToPackage(addProductForm.NewProduct, button.Tag.ToString());
                //await packageService.AddProductToPackageAsync(addProductForm.NewProduct.Product, Convert.ToInt32(button.Tag), addProductForm.NewProduct.Quantity);
                await packageService.AddProductToPackageAsync(addProductForm.NewProduct.Product, Convert.ToInt32(button.Tag), addProductForm.NewProduct.Quantity, addProductForm.NewProduct.Product.CategoryId);

                RestartPage();
                InitializeItems();
            }
        }
        //private void AddAccordionSection(string headerText, Control[] contentControls, Package package)
        //{
        //    // Create a panel to hold both header and content
        //    Panel accordionPanel = new Panel
        //    {
        //        Size = new Size(1325, 60), // Adjust the size accordingly
        //        BackColor = Color.White,
        //        BorderStyle = BorderStyle.FixedSingle,
        //        Anchor = AnchorStyles.Top,
        //        Margin = new Padding(10)
        //    };

        //    // Create the header button (to toggle content visibility)
        //    Button headerButton = new Button
        //    {
        //        Text = headerText,
        //        Height = 30,
        //        Width = 40,
        //        Location = new Point(1275, 10), // Positioned at the top right corner
        //        Anchor = AnchorStyles.Top | AnchorStyles.Right,
        //        BackColor = Color.LightGray,
        //        ForeColor = Color.Black,
        //        Font = new Font(this.Font.FontFamily, 10, FontStyle.Bold)
        //    };

        //    // Create year label
        //    Label labelYear;
        //    labelYear = new Label();
        //    labelYear.Name = String.Format("LblPackageYear{0}", package.PackageId);
        //    labelYear.Text = package.CreatedDate.ToString();
        //    labelYear.Location = new Point(10, 35);
        //    labelYear.ForeColor = Color.Gray;
        //    labelYear.Font = new Font(this.Font.FontFamily, 9.5f, FontStyle.Regular);
        //    labelYear.Tag = package.PackageId;

        //    // Create the content panel (starts collapsed)
        //    Panel contentPanel = new Panel
        //    {
        //        Size = new Size(1325, 10), // Start collapsed
        //        AutoScroll = true, // Enable scrolling if content is larger
        //        Visible = false, // Initially collapsed
        //        BackColor = Color.LightGray,
        //        Location = new Point(0, labelYear.Bottom) // Place it directly below the header button
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

        //    productAddButton.Click += (sender, e) =>
        //    {
        //        AddProductForm addProductForm = new AddProductForm();
        //        addProductForm.ShowDialog(); // Open the form
        //    };


        //    // Calculate the height for the content panel based on its controls
        //    int contentHeight = 0;
        //    foreach (var control in contentControls)
        //    {
        //        control.Dock = DockStyle.Top;
        //        contentPanel.Controls.Add(control);
        //        contentHeight += control.Height + 5; // Adjust height calculation based on the content
        //    }

        //    // Toggle content visibility when the header button is clicked
        //    headerButton.Click += (sender, e) =>
        //    {
        //        // Toggle visibility and adjust height of the content panel
        //        contentPanel.Visible = !contentPanel.Visible;
        //        if (contentPanel.Visible)
        //        {
        //            contentPanel.Height = contentHeight; // Expand the content panel to fit the controls
        //            accordionPanel.Height = 60 + contentPanel.Height; // Adjust the accordion panel size
        //        }
        //        else
        //        {
        //            contentPanel.Height = 0; // Collapse the content panel
        //            accordionPanel.Height = 60; // Reset the accordion height to its initial size
        //        }
        //    };

        //    accordionPanel.Controls.Add(editButton);
        //    accordionPanel.Controls.Add(productAddButton);
        //    accordionPanel.Controls.Add(deleteButton);

        //    // Add controls to panel
        //    accordionPanel.Controls.Add(labelId);
        //    accordionPanel.Controls.Add(labelYear);

        //    // Add the header button and content panel to the accordion panel
        //    accordionPanel.Controls.Add(headerButton);
        //    accordionPanel.Controls.Add(contentPanel);

        //    // Add the accordion panel to the FlowLayoutPanel
        //    flowLayoutPanel1.Controls.Add(accordionPanel);
        //}

        // Section controls

        // Helper method to create a Panel with a Label, Button, and Image
        //private Panel CreatePanelWithContent(string labelText, string buttonText, string imagePath)
        //{
        //    // Create a Panel
        //    Panel panel = new Panel
        //    {
        //        Height = 80,  // Adjust based on the content size
        //        Width = 400,
        //        BorderStyle = BorderStyle.FixedSingle
        //    };

        //    // Create a Label
        //    Label label = new Label
        //    {
        //        Text = labelText,
        //        Location = new Point(10, 10),
        //        AutoSize = true
        //    };

        //    // Create a Button
        //    Button button = new Button
        //    {
        //        Text = buttonText,
        //        Location = new Point(250, 10),
        //        Size = new Size(100, 30)
        //    };

        //    // Create a PictureBox for the image
        //    PictureBox pictureBox = new PictureBox
        //    {
        //        Location = new Point(150, 10),
        //        Size = new Size(80, 50), // Adjust image size
        //        SizeMode = PictureBoxSizeMode.StretchImage // Stretch image to fit
        //    };
        //    // Load the image from file path
        //    if (System.IO.File.Exists(imagePath))
        //    {
        //        pictureBox.Image = Image.FromFile(imagePath);
        //    }
        //    else
        //    {

        //    }
        //    {
        //        pictureBox.BackColor = Color.Gray; // Default to gray if no image
        //    }
        //    // Add the controls to the Panel
        //    panel.Controls.Add(label);
        //    panel.Controls.Add(button);
        //    panel.Controls.Add(pictureBox);

        //    return panel;
        //}
        //private void AddAccordionSection(string headerText, Control[] contentControls, Package package)
        //{
        //    // Create a panel to hold both header and content
        //    Panel accordionPanel = new Panel
        //    {
        //        Width = flowLayoutPanel1.ClientSize.Width - 20, // Adjust the size to be full width with some margin
        //        Anchor = AnchorStyles.Left | AnchorStyles.Right, // Ensure it resizes with the parent
        //        BackColor = Color.White,
        //        BorderStyle = BorderStyle.FixedSingle,
        //        Height = 50,
        //        AutoSize = true,
        //        AutoSizeMode = AutoSizeMode.GrowAndShrink,
        //        Margin = new Padding(10),
        //        Padding = new Padding(10)
        //    };

        //    // Create the header button (to toggle content visibility)
        //    Button headerButton = new Button
        //    {
        //        Text = headerText,
        //        Dock = DockStyle.Right,
        //        BackColor = Color.LightGray,
        //        ForeColor = Color.Black,
        //        Font = new Font(this.Font.FontFamily, 10, FontStyle.Bold),
        //       // Width = accordionPanel.Width - 20, // Set width to match the panel
        //        //Anchor = AnchorStyles.Left | AnchorStyles.Right
        //    };

        //    // Create the content panel
        //    Panel contentPanel = new Panel
        //    {
        //        Dock = DockStyle.Fill,
        //        AutoScroll = true, // Enable scrolling if content is larger
        //        Visible = false, // Initially collapsed
        //        AutoSize = true,
        //        AutoSizeMode = AutoSizeMode.GrowAndShrink,
        //        BackColor = Color.LightGray,
        //        Padding = new Padding(10)
        //    };

        //    // Create a FlowLayoutPanel to hold the buttons
        //    FlowLayoutPanel buttonPanel = new FlowLayoutPanel
        //    {
        //        Dock = DockStyle.Top,
        //        AutoSize = true,
        //        FlowDirection = FlowDirection.RightToLeft, // Align buttons to the right
        //        Padding = new Padding(0),
        //        Margin = new Padding(0),
        //        Anchor = AnchorStyles.Right // Ensure the buttons stay aligned to the right
        //    };

        //    // Create Id label
        //    Label labelId = new Label
        //    {
        //        Name = String.Format("LblPackageId{0}", package.PackageId),
        //        Text = package.PackageId.ToString(),
        //        Location = new Point(10, 5),
        //        ForeColor = Color.Black,
        //        Font = new Font(this.Font.FontFamily, 9.5f, FontStyle.Regular),
        //        AutoSize = true,
        //        Tag = package.PackageId
        //    };

        //    // Create export button
        //    if (!package.Exported)
        //    {
        //        Button exportButton = new Button
        //        {
        //            Name = "ExpBtnAction1",
        //            Text = "Export",
        //            Size = new Size(100, 30),
        //            Tag = package.PackageId
        //        };
        //        buttonPanel.Controls.Add(exportButton);
        //    }

        //    // Create edit button
        //    Button editButton = new Button
        //    {
        //        Name = "EditBtnAction1",
        //        Text = "Edit",
        //        Size = new Size(100, 30),
        //        Tag = package.PackageId
        //    };

        //    // Create delete button
        //    Button deleteButton = new Button
        //    {
        //        Name = "DeleteBtnAction1",
        //        Text = "Delete",
        //        Size = new Size(100, 30),
        //        Tag = package.PackageId
        //    };

        //    // Create product add button
        //    Button productAddButton = new Button
        //    {
        //        Name = "AddBtnAction1",
        //        Text = "Add",
        //        Size = new Size(100, 30),
        //        Tag = package.PackageId
        //    };

        //    // Create year label
        //    Label labelYear = new Label
        //    {
        //        Name = String.Format("LblPackageYear{0}", package.PackageId),
        //        Text = package.CreatedDate.ToString(),
        //        Location = new Point(10, 20),
        //        ForeColor = Color.Gray,
        //        Font = new Font(this.Font.FontFamily, 9.5f, FontStyle.Regular),
        //        AutoSize = true,
        //        Tag = package.PackageId
        //    };

        //    // Add buttons to the button panel
        //    buttonPanel.Controls.Add(editButton);
        //    buttonPanel.Controls.Add(deleteButton);
        //    buttonPanel.Controls.Add(productAddButton);

        //    // Add controls to the accordion panel
        //    accordionPanel.Controls.Add(buttonPanel);
        //    accordionPanel.Controls.Add(labelId);
        //    accordionPanel.Controls.Add(labelYear);

        //    // Add header button and content panel to the accordion panel
        //    accordionPanel.Controls.Add(contentPanel);
        //    accordionPanel.Controls.Add(headerButton);

        //    // Add the accordion panel to the main flowLayoutPanel
        //    flowLayoutPanel1.Controls.Add(accordionPanel);

        //    // Ensure that when the form resizes, the panel resizes too
        //    flowLayoutPanel1.Resize += (s, e) =>
        //    {
        //        accordionPanel.Width = flowLayoutPanel1.ClientSize.Width - 20;
        //    };

        //    // Toggle content visibility when the header button is clicked
        //    headerButton.Click += (sender, e) =>
        //    {
        //        contentPanel.Visible = !contentPanel.Visible;
        //        accordionPanel.Height = contentPanel.Visible ? contentPanel.Height + 50 : 50;
        //    };
        //}

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void metroSetBadge1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }


    }
}
