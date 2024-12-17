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

namespace VisionTech_Anbar_Project
{

    public partial class EditProductForm : MetroSetForm
    {
        private readonly ProductService _productService;
        private readonly CategoryService _categoryService;
        private readonly PackageService _packageService;
        private readonly BarcodeService _barcodeService;
        private readonly BrandService _brandService;


        public bool changed = false;
        TableLayoutPanel mainTableLayoutPanel;
        //List<Product> products = new List<Product>();
        List<PackageProduct> packProducts = new List<PackageProduct>();


        int currentPackageId;
        Package currentPackage;

        public EditProductForm(CategoryService categoryService, ProductService productService, PackageService packageService, BarcodeService barcodeService, BrandService brandService)
        {
            _categoryService = categoryService;
            _productService = productService;
            _packageService = packageService;
            _barcodeService = barcodeService;
            _brandService = brandService;

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

            AddProductForm addProductForm = new AddProductForm(_categoryService, _productService, _barcodeService, _brandService);
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

            if (addProductForm.DataSaved && addProductForm.EditedProduct != null && await _packageService.IsExsistProductInPackage(currentPackageId, addProductForm.EditedProduct.Product.Id))
            {
                await _packageService.AddProductToPackageAsync(currentPackageId, addProductForm.EditedProduct.Product.Id, addProductForm.EditedProduct.Quantity, addProductForm.EditedProduct.Product.CategoryId);
                // TURAL METHOD YAZMALIDIKI, HAZIRKI BARCODELAR VAR OLAN PRODUCTDA ELAVE EDILSEN 
                await _productService.UpdateProductAsync(addProductForm.EditedProduct.Product);

            }
            if (addProductForm.DataSaved && addProductForm.NewProduct != null)
            {
                //JsonManager.AddProductToPackage(addProductForm.NewProduct, button.Tag.ToString());
                //await packageService.AddProductToPackageAsync(addProductForm.NewProduct.Product, Convert.ToInt32(button.Tag), addProductForm.NewProduct.Quantity);
                await _packageService.AddProductToPackageAsync(addProductForm.NewProduct.Product, currentPackageId, addProductForm.NewProduct.Quantity, addProductForm.NewProduct.Product.CategoryId);
                var newPackageProduct = new PackageProduct
                {
                    Product = addProductForm.NewProduct.Product,
                    Quantity = addProductForm.NewProduct.Quantity
                };
                packProducts.Add(newPackageProduct);

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
            // Create the main panel for the item
            Panel itemPanel = new Panel
            {
                Height = 60, // Explicit height for the main item
                BorderStyle = BorderStyle.None,
                Dock = DockStyle.Top, // Align at the top
                Tag = product.Id,
                Margin = new Padding(0), // No extra margin outside the panel
                Padding = new Padding(0) // No extra padding inside the panel
            };

            // Label to display item text
            Label itemLabel = new Label
            {
                Text = product.ProductName.ToString(),
                AutoSize = true,
                Location = new Point(10, 15), // Adjusted to ensure consistent positioning
                Font = new Font("Arial", 10, FontStyle.Regular),
                ForeColor = Color.Black
            };

            // Create a FlowLayoutPanel to hold all buttons in a single line
            FlowLayoutPanel buttonPanel = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.LeftToRight, // Align buttons horizontally
                Dock = DockStyle.Right,
                AutoSize = true,
                WrapContents = false, // Prevent buttons from wrapping to the next line
                Margin = new Padding(0), // No extra margin
                Padding = new Padding(0) // No extra padding
            };

            // Button: Delete
            Button deleteButton = new Button
            {
                Text = "🗑",
                Tag = product.Id,
                ForeColor = Color.Red,
                Width = 50,
                Height = 40,
                Margin = new Padding(5)
            };
            deleteButton.Click += DeleteButton_Click;

            // Button: Edit
            Button editButton = new Button
            {
                Text = "✎",
                Font = new Font("Arial", 10, FontStyle.Bold),
                BackColor = Color.FromArgb(42, 45, 85),
                ForeColor = Color.White, // Changed to improve readability
                Tag = product,
                Width = 60,
                Height = 40,
                Margin = new Padding(5)
            };
            editButton.Click += EditButton_Click;

            // Add the buttons to the FlowLayoutPanel
            buttonPanel.Controls.Add(editButton);
            buttonPanel.Controls.Add(deleteButton);

            // Add the label and button panel to the item panel
            itemPanel.Controls.Add(itemLabel);
            itemPanel.Controls.Add(buttonPanel);

            return itemPanel;
        }
        private void RefreshProductList()
        {
            // Clear the panel first
            mainTableLayoutPanel.Controls.Clear();

            // Repopulate the panel with updated products
            foreach (PackageProduct packProduct in packProducts)
            {
                Panel itemPanel = CreateItemPanel(packProduct.Product);
                mainTableLayoutPanel.Controls.Add(itemPanel);
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
            AddProductForm addProductForm = new AddProductForm(_categoryService, _productService, _barcodeService, _brandService);
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
                AddProductForm addProductForm = new AddProductForm(_categoryService, _productService, _barcodeService, _brandService)
                {
                    currentProduct = selectedProduct
                };

                addProductForm.setCurrentProduct();
                addProductForm.ShowDialog();
                // If changes were made, update the product and refresh the UI
                if (addProductForm.DataSaved)
                {
                    // Update the product in the data source
                    await _productService.UpdateProductWithBarcodes(addProductForm.EditedProduct.Product);

                    // Update the product in the packProducts list
                    var existingProduct = packProducts.FirstOrDefault(p => p.Product.Id == selectedProduct.Id);
                    if (existingProduct != null)
                    {
                        existingProduct.Product = addProductForm.EditedProduct.Product;
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
