using MetroSet_UI.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VisionTech_Anbar_Project.Utilts;
using VisionTech_Anbar_Project.Entities;
using VisionTech_Anbar_Project.Services;
using System.Security.Policy;
using Microsoft.EntityFrameworkCore;
using VisionTech_Anbar_Project.DAL;

namespace VisionTech_Anbar_Project
{

    public partial class EditProductForm : MetroSetForm
    {
        private readonly ProductService _productService;
        private readonly CategoryService _categoryService;
        private readonly PackageService _packageService;
        private readonly BarcodeService _barcodeService;
        private readonly BrandService _brandService;
        private readonly IDbContextFactory<AppDbContext> _contextFactory;


        public bool changed = false;
        TableLayoutPanel mainTableLayoutPanel;
        //List<Product> products = new List<Product>();
        List<PackageProduct> packProducts = new List<PackageProduct>();


        int currentPackageId;
        Package currentPackage;

        public EditProductForm(CategoryService categoryService, ProductService productService, PackageService packageService, BarcodeService barcodeService, BrandService brandService, IDbContextFactory<AppDbContext> contextFactory)
        {
            _categoryService = categoryService;
            _productService = productService;
            _packageService = packageService;
            _barcodeService = barcodeService;
            _brandService = brandService;
            _contextFactory = contextFactory;

            InitializeComponent();
            SetupMainTableLayoutPanel();
            InitializeItems();

            button3.BringToFront();
        }
        public List<PackageProduct> GetProducts(Package package)
        {
            currentPackageId = package.Id;
            currentPackage = package;

            packProducts.Clear();
            packProducts.AddRange(package.PackageProducts);

            RefreshProductList();
            return packProducts;
        }

        //public void FetchProducts()
        //{
        //    foreach (PackageProduct packProduct in packProducts)
        //    {
        //        packProducts.Add(packProduct);
        //        //products.Add(packProduct.Product);
        //        Panel itemPanel = CreateItemPanel(packProduct.Product);
        //        mainTableLayoutPanel.Controls.Add(itemPanel);
        //    }
        //}
        private async void button1_Click(object sender, EventArgs e)
        {

            AddProductForm addProductForm = new AddProductForm(_categoryService, _productService, _barcodeService, _brandService,_contextFactory);
            addProductForm.ShowDialog();
            // Example for Section 1
            //            Control[] section1Controls = {

            //            CreatePanelWithContent("Label for Section 1", "Click Me", "path_to_image.png"),
            //            CreatePanelWithContent("Label for Section 2", "Click Me", "path_to_image.png"),
            //            CreatePanelWithContent("Label for Section 3", "Click Me", "path_to_image.png"),
            //            CreatePanelWithContent("Label for Section 4", "Click Me", "path_to_image.png")

            //};

            // Add both sections to the accordion

            // Add new Package to list and UI

            //&& await _packageService.IsExsistProductInPackage(currentPackageId, addProductForm.EditedProduct.Product.Id

            if (addProductForm.DataSaved && addProductForm.EditedProductList.Count > 0 )
            {
                foreach (var editedProduct in addProductForm.EditedProductList)
                {
                    await _packageService.AddProductToPackageAsync(currentPackageId, editedProduct.Product.Id, editedProduct.Barcode, editedProduct.Quantity, editedProduct.Product.CategoryId);
                    // TURAL METHOD YAZMALIDIKI, HAZIRKI BARCODELAR VAR OLAN PRODUCTDA ELAVE EDILSEN 
                    await _productService.UpdateProductAsync(addProductForm.EditedProduct.Product);
                }

            }
            if (addProductForm.DataSaved && addProductForm.NewProductList.Count > 0)
            {
                //JsonManager.AddProductToPackage(addProductForm.NewProduct, button.Tag.ToString());
                //await packageService.AddProductToPackageAsync(addProductForm.NewProduct.Product, Convert.ToInt32(button.Tag), addProductForm.NewProduct.Quantity);
                foreach (var newProduct in addProductForm.NewProductList)
                {
                    await _packageService.AddProductToPackageAsync(newProduct.Product, currentPackageId, newProduct.Barcode, newProduct.Quantity, newProduct.Product.CategoryId);
                    var newPackageProduct = new PackageProduct
                    {
                        Product = newProduct.Product,
                        Quantity = newProduct.Quantity
                    };
                    packProducts.Add(newPackageProduct);

                }
                // Refresh the UI
                RefreshProductList();
            }
        }
        private void SetupMainTableLayoutPanel()
        {
            // Use the existing tableLayoutPanel1
            mainTableLayoutPanel = tableLayoutPanel1;

            // Set properties for the existing TableLayoutPanel
            mainTableLayoutPanel.BackColor = Color.FromArgb(243, 246, 249); // Soft light gray background
            mainTableLayoutPanel.AutoScroll = true; // Enable scrolling if needed
            mainTableLayoutPanel.ColumnCount = 1; // Single-column layout
            mainTableLayoutPanel.CellBorderStyle = TableLayoutPanelCellBorderStyle.None; // No borders for cells

            // Remove extra padding and margin
            mainTableLayoutPanel.Padding = new Padding(0);
            mainTableLayoutPanel.Margin = new Padding(0);

            // Set all rows to auto-adjust height
            mainTableLayoutPanel.RowStyles.Clear();
            mainTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        }

        private void InitializeItems()
        {
            //var data = JsonManager.GetAllPackages();
            foreach (PackageProduct item in packProducts)
            {

                Panel itemPanel = CreateItemPanel(item.Product);
                mainTableLayoutPanel.RowCount++;
                mainTableLayoutPanel.Controls.Add(itemPanel, 0, mainTableLayoutPanel.RowCount - 1); // Add item to new row
                //Panel subItemsPanel = CreateSubItemsPanel(item.Products);
                //mainTableLayoutPanel.RowCount++;
                //mainTableLayoutPanel.Controls.Add(subItemsPanel, 0, mainTableLayoutPanel.RowCount - 1); // Add subitems below item
                //subItemsPanel.Visible = false; // Initially hidden
            }
        }
        public void RestartPage()
        {
            mainTableLayoutPanel.Controls.Clear();
        }
        private Panel CreateItemPanel(Product product)
        {
            Panel itemPanel = new Panel
            {
                Height = 80,
                BorderStyle = BorderStyle.None,
                BackColor = Color.FromArgb(252, 253, 255),
                Dock = DockStyle.Top,
                Margin = new Padding(15, 8, 15, 12),
                Tag = product.Id,
                Padding = new Padding(16, 20, 16, 16),
            };

            // Add rounded corners
            itemPanel.Paint += (sender, e) =>
            {
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                using (var path = new System.Drawing.Drawing2D.GraphicsPath())
                {
                    var radius = 10f;
                    var rect = itemPanel.ClientRectangle;
                    path.AddArc(rect.X, rect.Y, radius * 2, radius * 2, 180, 90);
                    path.AddArc(rect.Right - radius * 2, rect.Y, radius * 2, radius * 2, 270, 90);
                    path.AddArc(rect.Right - radius * 2, rect.Bottom - radius * 2, radius * 2, radius * 2, 0, 90);
                    path.AddArc(rect.X, rect.Bottom - radius * 2, radius * 2, radius * 2, 90, 90);
                    path.CloseFigure();

                    using (var pen = new Pen(Color.FromArgb(230, 232, 240), 1))
                    {
                        e.Graphics.DrawPath(pen, path);
                    }
                }
            };

            // Add hover effects
            itemPanel.MouseEnter += (sender, e) =>
            {
                itemPanel.BackColor = Color.FromArgb(248, 249, 252);
            };

            itemPanel.MouseLeave += (sender, e) =>
            {
                itemPanel.BackColor = Color.FromArgb(252, 253, 255);
            };

            // Product name label with enhanced styling
            Label itemLabel = new Label
            {
                Text = product.ProductName.ToString(),
                Font = new Font("Segoe UI Semibold", 13, FontStyle.Regular),
                ForeColor = Color.FromArgb(42, 45, 85),
                AutoSize = true,
                Location = new Point(16, itemPanel.Height / 2 - 10),
                Padding = new Padding(0, 0, 10, 0)
            };

            // Button creation helper method
            Button CreateStyledButton(string iconPath, Color backColor, Color foreColor)
            {
                var button = new Button
                {
                    Font = new Font("Segoe UI", 10, FontStyle.Bold),
                    BackColor = backColor,
                    ForeColor = foreColor,
                    FlatStyle = FlatStyle.Flat,
                    Tag = product.Id,
                    Size = new Size(40, 40),
                    Margin = new Padding(5),
                    TextAlign = ContentAlignment.MiddleCenter,
                    Cursor = Cursors.Hand
                };

                try
                {
                    using (var fileStream = new FileStream(iconPath, FileMode.Open, FileAccess.Read))
                    using (var img = System.Drawing.Image.FromStream(fileStream))
                    {
                        button.Image = new Bitmap(img, new Size(24, 24));
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error loading image: {ex.Message}");
                }

                button.FlatAppearance.BorderSize = 0;
                button.FlatAppearance.MouseOverBackColor = Color.FromArgb(
                    (int)(backColor.R * 0.95),
                    (int)(backColor.G * 0.95),
                    (int)(backColor.B * 0.95));

                return button;
            }

            // Create button panel with right-aligned buttons
            FlowLayoutPanel buttonPanel = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.RightToLeft,
                Dock = DockStyle.Right,
                AutoSize = true,
                WrapContents = false,
                Margin = new Padding(0),
                Padding = new Padding(5, 0, 0, 0)
            };

            // Create styled buttons
            Button deleteButton = CreateStyledButton(
                Path.Combine(FileManager.GetResourceFolder(), "trash.png"),
                Color.FromArgb(255, 223, 223),
                Color.Red);
            deleteButton.Tag = product.Id;
            deleteButton.Click += DeleteButton_Click;

            Button editButton = CreateStyledButton(
                Path.Combine(FileManager.GetResourceFolder(), "edit.png"),
                Color.FromArgb(220, 240, 255),
                Color.FromArgb(0, 120, 215));
            editButton.Tag = product;
            editButton.Click += EditButton_Click;

            // Add buttons to panel
            buttonPanel.Controls.Add(deleteButton);
            buttonPanel.Controls.Add(editButton);

            // Add all controls to main panel
            itemPanel.Controls.Add(itemLabel);
            itemPanel.Controls.Add(buttonPanel);

            return itemPanel;
        }

        private void RefreshProductList()
        {
            // Use a HashSet to track unique product names
            var uniqueProductNames = new HashSet<int>();

            // Clear the panel first
            mainTableLayoutPanel.Controls.Clear();

            // Repopulate the panel with updated unique products
            foreach (PackageProduct packProduct in packProducts)
            {
                int productId = packProduct.Product.Id;

                // Add only if the product name is not already in the HashSet
                if (uniqueProductNames.Add(productId)) // Add returns false if the name already exists
                {
                    Panel itemPanel = CreateItemPanel(packProduct.Product);
                    mainTableLayoutPanel.Controls.Add(itemPanel);
                }
            }
        }

        public async void DeleteButton_Click(object sender, EventArgs e)
        {
            if (sender is not Button button || button.Tag == null)
                return;

            int productId = int.Parse(button.Tag.ToString());

            DialogResult confirmResult = MessageBox.Show(
                "Are you sure you want to delete this product?",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (confirmResult != DialogResult.Yes)
                return;

            // Delete the product from the backend
            await _productService.DeleteProductAsync(productId);

            // Remove the product from the local list
            var productToRemove = packProducts.FirstOrDefault(p => p.Product.Id == productId);
            if (productToRemove != null)
            {
                packProducts.Remove(productToRemove);

                // Find and remove the corresponding panel from the UI
                var panelToRemove = mainTableLayoutPanel.Controls
                    .OfType<Panel>()
                    .FirstOrDefault(panel => panel.Tag?.ToString() == productId.ToString());

                if (panelToRemove != null)
                {
                    // Remove the panel from the TableLayoutPanel
                    mainTableLayoutPanel.Controls.Remove(panelToRemove);

                    // Adjust the RowCount only if it is greater than 0
                    if (mainTableLayoutPanel.RowCount > 0)
                    {
                        mainTableLayoutPanel.RowCount--;
                    }
                }
            }

            changed = true;
        }


        public void AddButton_Click(object sender, EventArgs e)
        {
            AddProductForm addProductForm = new AddProductForm(_categoryService, _productService, _barcodeService, _brandService,_contextFactory);
            addProductForm.ShowDialog();

            if (!addProductForm.DataSaved || addProductForm.NewProduct == null)
                return;

            // Add the new product to the local list
            var newPackageProduct = new PackageProduct
            {
                Product = addProductForm.NewProduct.Product,
                Quantity = addProductForm.NewProduct.Quantity
            };
            packProducts.Add(newPackageProduct);

            // Create a new panel for the product and add it to the UI
            Panel newItemPanel = CreateItemPanel(newPackageProduct.Product);
            mainTableLayoutPanel.RowCount++;
            mainTableLayoutPanel.Controls.Add(newItemPanel, 0, mainTableLayoutPanel.RowCount - 1);

            changed = true;
        }

        public async void EditButton_Click(object sender, EventArgs e)
        {
            // Retrieve the button and the associated product
            Button button = sender as Button;
            if (button?.Tag is Product selectedProduct)
            {
                // Show the AddProductForm with the selected product for editing
                AddProductForm addProductForm = new AddProductForm(_categoryService, _productService, _barcodeService, _brandService,_contextFactory)
                {
                    currentProduct = selectedProduct
                };

                addProductForm.setCurrentProduct();
                addProductForm.ShowDialog();
                // If changes were made, update the product and refresh the UI
                if (addProductForm.DataSaved)
                {
                    // Update the product in the data source
                    await _productService.UpdateProductWithBarcodes(currentPackageId, addProductForm.EditedProductList.First().ProductId, addProductForm.EditedProductList);
                    foreach (var editedProduct in addProductForm.EditedProductList)
                    {
                        // Update the product in the packProducts list
                        var existingProduct = packProducts.FirstOrDefault(p => p.Product.Id == selectedProduct.Id);
                        if (existingProduct != null)
                        {
                            existingProduct.Product = editedProduct.Product;
                        }
                    }
                    // Refresh the UI
                    RefreshProductList();
                }
            }
            else
            {
                MessageBox.Show("Failed to identify the product to edit.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            changed = true;
        }

    }
}
