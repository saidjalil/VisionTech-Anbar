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
using System.Runtime.InteropServices;
using Microsoft.Extensions.Configuration;

namespace VisionTech_Anbar_Project
{
    public partial class Ophrys : MetroSetForm
    {
        private readonly PackageService _packageService;
        private readonly ProductService _productService;
        private readonly CategoryService _categoryService;
        private readonly WarehouseService _warehouseService;
        private readonly VendorService _vendorService;
        private readonly ImageService _imageService;
        private readonly BarcodeService _barcodeService;
        private readonly IConfiguration _configuration;

        private PictureBox loadingSpinner;
        // Buttons
        Button expandButton;


        TableLayoutPanel mainTableLayoutPanel;

        private List<Package> selectedProducts = new List<Package>();
        public Ophrys(IConfiguration configuration, PackageService packageService, ProductService productService, CategoryService categoryService, WarehouseService warehouseService, VendorService vendorService, ImageService imageService, BarcodeService barcodeService)
        {
            this._packageService = packageService;
            this._productService = productService;
            this._categoryService = categoryService;
            _warehouseService = warehouseService;
            _vendorService = vendorService;
            _imageService = imageService;
            _barcodeService = barcodeService;
            _configuration = configuration;


            InitializeComponent();
            SetupMainTableLayoutPanel();
            SetupLoadingSpinner();
            InitializeItems();

            button3.BringToFront();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            ImageManager imageManager = new(_imageService);
            AddColumnForm addColumnForm = new AddColumnForm(_packageService, _categoryService, _productService, _warehouseService, _vendorService, _imageService, _barcodeService);
            addColumnForm.SetAllData();
            addColumnForm.ShowDialog();

            await _packageService.CreatePackageAsync(addColumnForm.NewPackage);

            if (!string.IsNullOrEmpty(addColumnForm.openFileDialog.FileName))
            {
                var image = imageManager.SaveImage(addColumnForm.openFileDialog, addColumnForm.NewPackage.Id);
                await _imageService.CreateImageAsync(image);
            }
            else
            {
                MessageBox.Show("Şəkil seçilməyib.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            // Dynamically add the new package to the mainTableLayoutPanel
            Panel newItemPanel = CreateItemPanel(addColumnForm.NewPackage);
            mainTableLayoutPanel.RowCount++;
            mainTableLayoutPanel.Controls.Add(newItemPanel, 0, mainTableLayoutPanel.RowCount - 1);

            // Add sub-items for the new package
            var products = addColumnForm.NewPackage.PackageProducts.Select(pp => new PackageProduct
            {
                Product = pp.Product,
                Quantity = pp.Quantity
            });

            foreach (var product in products)
            {
                Panel subItemsPanel = CreateSubItemsPanel(product.Product, product.Quantity);
                mainTableLayoutPanel.RowCount++;
                mainTableLayoutPanel.Controls.Add(subItemsPanel, 0, mainTableLayoutPanel.RowCount - 1);
                subItemsPanel.Visible = false; // Keep sub-items hidden initially
            }
        }

        private void SetupLoadingSpinner()
        {
            loadingSpinner = new PictureBox
            {
                Image = Image.FromFile(FileManager.GetGIFPath()), // Add a GIF spinner in project resources
                SizeMode = PictureBoxSizeMode.AutoSize,
                //Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom,
                
                BackColor = Color.White,
                Visible = false, // Hidden initially

            };

            loadingSpinner.Location = new Point((this.Width - loadingSpinner.Width) / 2, (this.Height - loadingSpinner.Height) / 2);
            this.Controls.Add(loadingSpinner);
            loadingSpinner.BringToFront();
        }

        private void ShowLoadingSpinner()
        {
            loadingSpinner.Visible = true;
            loadingSpinner.BringToFront();
        }

        private void HideLoadingSpinner()
        {
            loadingSpinner.Visible = false;
        }
        private void SetupMainTableLayoutPanel()
        {
            mainTableLayoutPanel = tableLayoutPanel1;
            mainTableLayoutPanel.Visible = false;
            mainTableLayoutPanel.BackColor = Color.FromArgb(243, 246, 249);
            mainTableLayoutPanel.AutoScroll = true;
            mainTableLayoutPanel.ColumnCount = 1;
            mainTableLayoutPanel.CellBorderStyle = TableLayoutPanelCellBorderStyle.None;

            // Remove extra space
            mainTableLayoutPanel.Padding = new Padding(0);
            mainTableLayoutPanel.Margin = new Padding(0);

            // Ensure all rows auto-adjust
            mainTableLayoutPanel.RowStyles.Clear();
            mainTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        }

        private async void InitializeItems()
        {
            try
            {
                ShowLoadingSpinner(); // Show spinner when loading starts
                mainTableLayoutPanel.Controls.Clear();

                var data = await Task.Run(() => _packageService.GetAllPackageWithNavigation());
                foreach (var item in data)
                {
                    Panel itemPanel = CreateItemPanel(item);
                    mainTableLayoutPanel.RowCount++;
                    mainTableLayoutPanel.Controls.Add(itemPanel, 0, mainTableLayoutPanel.RowCount - 1);
                    var products = item.PackageProducts.Select(pp => new PackageProduct
                    {
                        Product = pp.Product,
                        Quantity = pp.Quantity
                    });

                    foreach (var product in products)
                    {
                        Panel subItemsPanel = CreateSubItemsPanel(product.Product, product.Quantity);
                        mainTableLayoutPanel.RowCount++;
                        mainTableLayoutPanel.Controls.Add(subItemsPanel, 0, mainTableLayoutPanel.RowCount - 1);
                        subItemsPanel.Visible = false;
                    }
                }
                await Task.Delay(2000);
                mainTableLayoutPanel.Visible = true;

            }
            finally
            {
                HideLoadingSpinner(); // Hide spinner once data is loaded
            }
        }


        public void RestartPage()
        {
            mainTableLayoutPanel.Controls.Clear();
        }

        private Panel CreateItemPanel(Package package)
        {
            Panel itemPanel = new Panel
            {
                Height = 70,
                BorderStyle = BorderStyle.None,
                BackColor = Color.White,
                Dock = DockStyle.Top,
                Margin = new Padding(0, 0, 0, 10),
                Padding = new Padding(10)
            };
            itemPanel.SetBorderRadius(10);

            // Checkbox
            CheckBox packageCheckBox = new CheckBox
            {
                AutoSize = true,
                Location = new Point(10, (itemPanel.Height - 20) / 2),
                Tag = package.Id // Store the package ID in the Tag
            };

            Label itemLabel = new Label
            {
                Text = package.Warehouse.WarehouseName.ToString(),
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.FromArgb(42, 45, 85),
                AutoSize = true,
                Location = new Point(50, 20)
            };

            FlowLayoutPanel buttonPanel = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.RightToLeft,
                Dock = DockStyle.Right,
                AutoSize = true,
                WrapContents = false,
                Margin = new Padding(0)
            };

            Button CreateStyledButton(string text, Color backColor, Color foreColor)
            {
                var button = new Button
                {
                    Text = text,
                    Font = new Font("Segoe UI", 10, FontStyle.Bold),
                    BackColor = backColor,
                    ForeColor = foreColor,
                    FlatStyle = FlatStyle.Flat,
                    Tag = package.Id,
                    Size = new Size(40, 40),
                    Margin = new Padding(5),
                    TextAlign = ContentAlignment.MiddleCenter
                };
                button.FlatAppearance.BorderSize = 0;
                return button;
            }

            Button expandButton = package.PackageProducts.Count == 0
                ? CreateStyledButton("▽", Color.FromArgb(230, 236, 240), Color.Red)
                : CreateStyledButton("▽", Color.FromArgb(230, 236, 240), Color.FromArgb(42, 45, 85));
            expandButton.Click += (s, e) => ToggleSubItems(itemPanel, (Button)s);

            Button deleteButton = CreateStyledButton("🗑", Color.FromArgb(255, 223, 223), Color.Red);
            deleteButton.Click += DeleteButton_Click;

            Button addButton = CreateStyledButton("➕", Color.FromArgb(220, 240, 255), Color.FromArgb(0, 120, 215));
            addButton.Click += AddButton_Click;

            Button editButton = CreateStyledButton("✎", Color.FromArgb(220, 240, 255), Color.FromArgb(0, 120, 215));
            editButton.Click += EditButton_Click;

            Button exportButton = CreateStyledButton("⇪", Color.FromArgb(220, 240, 255), Color.FromArgb(0, 120, 215));
            exportButton.Click += ExportButton_Click;

            buttonPanel.Controls.Add(expandButton);
            buttonPanel.Controls.Add(deleteButton);
            buttonPanel.Controls.Add(addButton);
            buttonPanel.Controls.Add(editButton);
            buttonPanel.Controls.Add(exportButton);


            itemPanel.Controls.Add(packageCheckBox);
            itemPanel.Controls.Add(itemLabel);
            itemPanel.Controls.Add(buttonPanel);

            return itemPanel;
        }

        private void ExportButton_Click(object sender, EventArgs e)
        {
            // Only Export current 
        }

        private void ExportAllButton_Click(object sender, EventArgs e)
        {
            // Collect selected package IDs
            List<int> selectedPackageIds = new List<int>();

            foreach (Control control in mainTableLayoutPanel.Controls)
            {
                if (control is Panel itemPanel)
                {
                    // Find the CheckBox within the panel
                    CheckBox packageCheckBox = itemPanel.Controls.OfType<CheckBox>().FirstOrDefault();

                    if (packageCheckBox != null && packageCheckBox.Checked)
                    {
                        selectedPackageIds.Add((int)packageCheckBox.Tag); // Get package ID from Tag
                    }
                }
            }

            if (selectedPackageIds.Count > 0)
            {
                // Call your export logic here with the selected IDs
                ExportSelectedPackages(selectedPackageIds);
            }
            else
            {
                MessageBox.Show("No packages selected for export.", "Export", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // Your export logic
        private void ExportSelectedPackages(List<int> packageIds)
        {
            Task.Run(() =>
            {
                FileExporter fileExporter = new FileExporter(_packageService, _imageService,_configuration,_categoryService);
                fileExporter.CreateAndWriteExportFile(packageIds);

                // Back on the UI thread to show the message
                this.Invoke(() =>
                {
                    MessageBox.Show("Packages exported successfully!", "Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
                });
            });
        }




        private Panel CreateSubItemsPanel(Product products, int quantity)
        {
            Panel subItemsPanel = new Panel
            {
                Height = 50,
                BackColor = Color.FromArgb(247, 249, 251),
                AutoScroll = true,
                BorderStyle = BorderStyle.None,
                Padding = new Padding(5, 5, 5, 5),
                Dock = DockStyle.Top
            };
            subItemsPanel.SetBorderRadius(8);

            if (products != null)
            {
                Panel subItemPanel = new Panel
                {
                    Height = 40,
                    Dock = DockStyle.Top,
                    BackColor = Color.White,
                    Margin = new Padding(0, 0, 0, 5)
                };
                subItemPanel.SetBorderRadius(6);

                Label subItemLabel = new Label
                {
                    Text = $"Name: {products.ProductName} | Count: {quantity}",
                    Font = new Font("Segoe UI", 10),
                    ForeColor = Color.FromArgb(70, 70, 70),
                    AutoSize = true,
                    Location = new Point(15, 10)
                };

                subItemPanel.Controls.Add(subItemLabel);
                subItemsPanel.Controls.Add(subItemPanel);
            }

            return subItemsPanel;
        }

        private void ToggleSubItems(Panel itemPanel, Button expandButton)
        {
            bool isNextControlSubItem = false;
            bool areSubItemsVisible = false;

            // Iterate over the controls starting from the itemPanel
            foreach (Control control in mainTableLayoutPanel.Controls)
            {
                if (control == itemPanel)
                {
                    isNextControlSubItem = true;
                    continue;
                }

                if (isNextControlSubItem)
                {
                    if (control is Panel subItemPanel && subItemPanel.BackColor == Color.FromArgb(247, 249, 251))
                    {
                        // Toggle visibility of the sub-item panels
                        subItemPanel.Visible = !subItemPanel.Visible;

                        // Update the state to determine if any sub-item is visible
                        if (subItemPanel.Visible)
                        {
                            areSubItemsVisible = true;
                        }
                    }
                    else
                    {
                        break; // Stop toggling when reaching a new item panel
                    }
                }
            }

            // Update the expand button text based on visibility
            if (areSubItemsVisible)
            {
                expandButton.Text = "△"; // Up arrow
            }
            else
            {
                expandButton.Text = "▽"; // Down arrow
            }
        }


        public async void EditButton_Click(object sender, EventArgs e)
        {
            EditProductForm editProductForm = new EditProductForm(_categoryService, _productService, _packageService, _barcodeService);
            editProductForm.Show();

            Button button = sender as Button;
            editProductForm.GetProducts(await _packageService.GetPackageWithNavigation(int.Parse(button.Tag.ToString())));
        }

        public async void DeleteButton_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;
            DialogResult result = MessageBox.Show("Are you sure you want to delete this package?",
                "Confirm Deletion",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            await _packageService.DeletePackageAsync(int.Parse(button.Tag.ToString()));

            if (result == DialogResult.Yes)
            {
                // Find the parent item panel for this button
                Panel itemPanel = button.Parent.Parent as Panel;
                if (itemPanel != null)
                {
                    itemPanel.Visible = false; // Hide the panel
                }

                await _packageService.DeletePackageAsync(int.Parse(button.Tag.ToString()));
            }
            // RestartPage();
            // InitializeItems();
        }

        public async void AddButton_Click(object sender, EventArgs e)
        {
            AddProductForm addProductForm = new AddProductForm(_categoryService, _productService, _barcodeService);
            addProductForm.ShowDialog();
            Button button = sender as Button;

            if (addProductForm.DataSaved && addProductForm.EditedProduct != null && await _packageService.IsExsistProductInPackage(Convert.ToInt32(button.Tag), addProductForm.EditedProduct.Product.Id))
            {
                await _packageService.AddProductToPackageAsync(Convert.ToInt32(button.Tag), addProductForm.EditedProduct.Product.Id, addProductForm.EditedProduct.Quantity, addProductForm.EditedProduct.Product.CategoryId);
                await _productService.UpdateProductAsync(addProductForm.EditedProduct.Product);
            }

            if (addProductForm.DataSaved && addProductForm.NewProduct != null)
            {
                await _packageService.AddProductToPackageAsync(addProductForm.NewProduct.Product, Convert.ToInt32(button.Tag), addProductForm.NewProduct.Quantity, addProductForm.NewProduct.Product.CategoryId);

                RestartPage();
                InitializeItems();
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void metroSetBadge1_Click(object sender, EventArgs e)
        {
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }

    // Extension method for rounded corners
    public static class ControlExtensions
    {
        public static void SetBorderRadius(this Control control, int radius)
        {
            control.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, control.Width, control.Height, radius, radius));
            control.SizeChanged += (s, e) =>
                control.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, control.Width, control.Height, radius, radius));
        }

        [DllImport("Gdi32.dll")]
        private static extern IntPtr CreateRoundRectRgn(int nLeftRect, int nTopRect, int nRightRect, int nBottomRect, int nWidthEllipse, int nHeightEllipse);
    }
}

