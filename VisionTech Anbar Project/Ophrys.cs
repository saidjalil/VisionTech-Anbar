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
        private readonly PackageService _packageService;
        private readonly ProductService _productService;
        private readonly CategoryService _categoryService;
        private readonly WarehouseService _warehouseService;
        private readonly VendorService _vendorService;
        private readonly ImageService _imageService;

        TableLayoutPanel mainTableLayoutPanel;

        private List<Package> selectedProducts = new List<Package>();
        public Ophrys(PackageService packageService, ProductService productService, CategoryService categoryService, WarehouseService warehouseService, VendorService vendorService, ImageService imageService)
        {
            this._packageService = packageService;
            this._productService = productService;
            this._categoryService = categoryService;
            _warehouseService = warehouseService;
            _vendorService = vendorService;
            _imageService = imageService;


            InitializeComponent();
            SetupMainTableLayoutPanel();
            InitializeItems();

            button3.BringToFront();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            ImageManager imageManager = new(_imageService);
            AddColumnForm addColumnForm = new AddColumnForm(_packageService, _categoryService, _productService, _warehouseService, _vendorService,_imageService);
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
                BackColor = Color.Transparent,
                Padding = new Padding(0, 30, 0, 0), // Add padding from the top
                AutoScroll = true,
                ColumnCount = 1, // Single column to arrange items vertically
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
                mainTableLayoutPanel.Controls.Add(itemPanel, 0, mainTableLayoutPanel.RowCount - 1); // Add item to new row

                var products = item.PackageProducts.Select(pp => new PackageProduct
                {
                    Product = pp.Product,
                    Quantity = pp.Quantity
                });
                foreach (var product in products)
                {
                    //Log.Information("hOW many TIMESSSSSSSSSSSSSSSS");
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
                Location = new System.Drawing.Point(5, 15),
                BackColor = Color.Transparent
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

            exportButton.Click += (s, e) =>
            {
                if (selectedProducts.Any())
                {
                    ExportSelectedProducts(selectedProducts);
                }
                else
                {
                    MessageBox.Show("No products selected for export.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            };
            CheckBox subItemCheckBox = new CheckBox
            {
                Location = new System.Drawing.Point(5, 5), // Position the checkbox
                Tag = package.Id,// Store product in the Tag for reference

            };

            subItemCheckBox.CheckedChanged += (s, e) =>
            {
                if (subItemCheckBox.Checked)
                {
                    selectedProducts.Add(package); // Add product to selected list
                }
                else
                {
                    selectedProducts.Remove(package); // Remove product from selected list
                }
            };


            // Add the buttons to the FlowLayoutPanel


            buttonPanel.Controls.Add(addButton);
            buttonPanel.Controls.Add(editButton);
            buttonPanel.Controls.Add(exportButton);
            buttonPanel.Controls.Add(deleteButton);
            buttonPanel.Controls.Add(expandButton);

            // Add the label and button panel to the item panel
            itemPanel.Controls.Add(itemLabel);
            itemPanel.Controls.Add(subItemCheckBox);
            itemPanel.Controls.Add(buttonPanel);

            return itemPanel;
        }
        private void ExportSelectedProducts(List<Package> products)
        {
            // export 
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
                    Location = new System.Drawing.Point(50, 5)
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
        public async void EditButton_Click(object sender, EventArgs e)
        {

            EditProductForm editProductForm = new EditProductForm(_categoryService, _productService, _packageService);
            editProductForm.Show();

            Button button = sender as Button;
            ;
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
            AddProductForm addProductForm = new AddProductForm(_categoryService, _productService);
            addProductForm.ShowDialog();
            Button button = sender as Button;
            //if(Convert.ToInt32(button.Tag))
            //   if(addProductForm.DataSaved && await packageService.IsExsistProductInPackage(Convert.ToInt32(button.Tag), addProductForm.EditedProduct.ProductId))
            //  {
            //      await productService.UpdateProductAsync(addProductForm.EditedProduct.Product);
            //   }
            if (addProductForm.DataSaved && addProductForm.EditedProduct != null && await _packageService.IsExsistProductInPackage(Convert.ToInt32(button.Tag), addProductForm.EditedProduct.Product.Id))
            {
                await _packageService.AddProductToPackageAsync(Convert.ToInt32(button.Tag), addProductForm.EditedProduct.Product.Id, addProductForm.EditedProduct.Quantity, addProductForm.EditedProduct.Product.CategoryId);
                // TURAL METHOD YAZMALIDIKI, HAZIRKI BARCODELAR VAR OLAN PRODUCTDA ELAVE EDILSEN 
                await _productService.UpdateProductAsync(addProductForm.EditedProduct.Product);

            }

            if (addProductForm.DataSaved && addProductForm.NewProduct != null)
            {
                //JsonManager.AddProductToPackage(addProductForm.NewProduct, button.Tag.ToString());
                //await packageService.AddProductToPackageAsync(addProductForm.NewProduct.Product, Convert.ToInt32(button.Tag), addProductForm.NewProduct.Quantity);
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
}
