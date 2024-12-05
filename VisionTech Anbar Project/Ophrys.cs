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


        TableLayoutPanel mainTableLayoutPanel;

        private List<Package> selectedProducts = new List<Package>();
        public Ophrys(PackageService packageService, ProductService productService, CategoryService categoryService, WarehouseService warehouseService, VendorService vendorService, ImageService imageService, BarcodeService barcodeService)
        {
            this._packageService = packageService;
            this._productService = productService;
            this._categoryService = categoryService;
            _warehouseService = warehouseService;
            _vendorService = vendorService;
            _imageService = imageService;
            _barcodeService = barcodeService;


            InitializeComponent();
            SetupMainTableLayoutPanel();
            InitializeItems();

            button3.BringToFront();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            ImageManager imageManager = new(_imageService);
            AddColumnForm addColumnForm = new AddColumnForm(_packageService, _categoryService, _productService, _warehouseService, _vendorService,_imageService, _barcodeService);
            addColumnForm.SetAllData();
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
                await _packageService.CreatePackageAsync(addColumnForm.NewPackage);
                RestartPage();
                InitializeItems();

                if (!string.IsNullOrEmpty(addColumnForm.openFileDialog.FileName))
                {
                    await imageManager.SaveImage(addColumnForm.openFileDialog, addColumnForm.NewPackage.Id); // Call the provided SaveImage method
                                                                                 //MessageBox.Show("Image saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Şəkil seçilməyib.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
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
                BackColor = Color.FromArgb(243, 246, 249), // Soft light gray background
                Padding = new Padding(10, 20, 10, 10),
                AutoScroll = true,
                ColumnCount = 1,
                CellBorderStyle = TableLayoutPanelCellBorderStyle.None
            };
            this.Controls.Add(mainTableLayoutPanel);
        }

        private async void InitializeItems()
        {
            var data = await _packageService.GetAllPackageWithNavigation();
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

            Label itemLabel = new Label
            {
                Text = package.Warehouse.WarehouseName.ToString(),
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.FromArgb(42, 45, 85),
                AutoSize = true,
                Location = new Point(15, 20)
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

            Button expandButton = CreateStyledButton("▽", Color.FromArgb(230, 236, 240), Color.FromArgb(42, 45, 85));
            expandButton.Click += (s, e) => ToggleSubItems(itemPanel);

            Button deleteButton = CreateStyledButton("🗑", Color.FromArgb(255, 223, 223), Color.Red);
            deleteButton.Click += DeleteButton_Click;

            Button addButton = CreateStyledButton("➕", Color.FromArgb(220, 240, 255), Color.FromArgb(0, 120, 215));
            addButton.Click += AddButton_Click;

            Button editButton = CreateStyledButton("✎", Color.FromArgb(220, 240, 255), Color.FromArgb(0, 120, 215));
            editButton.Click += EditButton_Click;

            Button exportButton = CreateStyledButton("⇪", Color.FromArgb(220, 240, 255), Color.FromArgb(0, 120, 215));

            buttonPanel.Controls.Add(expandButton);
            buttonPanel.Controls.Add(deleteButton);
            buttonPanel.Controls.Add(addButton);
            buttonPanel.Controls.Add(editButton);
            buttonPanel.Controls.Add(exportButton);

            itemPanel.Controls.Add(itemLabel);
            itemPanel.Controls.Add(buttonPanel);

            return itemPanel;
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

        private void ToggleSubItems(Panel itemPanel)
        {
            bool isNextControlSubItem = false;

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
                        subItemPanel.Visible = !subItemPanel.Visible;
                    }
                    else
                    {
                        break; // Stop toggling when reaching a new item panel
                    }
                }
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
            await _packageService.DeletePackageAsync(int.Parse(button.Tag.ToString()));

            RestartPage();
            InitializeItems();
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

