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

namespace VisionTech_Anbar_Project
{
   
    public partial class EditProductForm : MetroSetForm
    {
        private readonly ProductService _productService;
        private readonly CategoryService _categoryService;
        private readonly PackageService _packageService;

        TableLayoutPanel mainTableLayoutPanel;
        //List<Product> products = new List<Product>();
        List<PackageProduct> packProducts = new List<PackageProduct>();


        int currentPackageId;
        Package currentPackage;

        public EditProductForm(CategoryService categoryService, ProductService productService, PackageService packageService)
        {
            _categoryService = categoryService;
            _productService = productService;
            _packageService = packageService;

            InitializeComponent();
            SetupMainTableLayoutPanel();
            InitializeItems();
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

            AddProductForm addProductForm = new AddProductForm(_categoryService, _productService);
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
                Height = 60, // Set explicit height for the main item
                BorderStyle = BorderStyle.None,
                Dock = DockStyle.Top,
                Margin = new Padding(5) // Add some margin between items
            };

            // Label to display item text
            Label itemLabel = new Label
            {
                Text = product.ProductName.ToString(),
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
            //Button expandButton = new Button
            //{
            //    Text = "^",
            //    Width = 30,
            //    Height = 30,
            //    Margin = new Padding(5)
            //};
            //expandButton.Click += (s, e) => ToggleSubItems(itemPanel);

            // Add Delete, Add, Edit, and Export buttons
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
            //Button addButton = new Button
            //{
            //    Text = "Add",
            //    Tag = product.Id,
            //    Width = 60,
            //    Height = 30,
            //    Margin = new Padding(5)
            //};
            //addButton.Click += AddButton_Click;

            Button editButton = new Button
            {
                Text = "✎",
                Font = new Font("Arial", 10, FontStyle.Bold),
                BackColor = Color.FromArgb(42, 45, 85),
                ForeColor = Color.Transparent,
                Tag = product,
                Width = 60,
                Height = 40,
                Margin = new Padding(5)
            };
            editButton.Click += EditButton_Click;
            //Button exportButton = new Button
            //{
            //    Text = "Export",
            //    Tag = product.Id,
            //    Width = 60,
            //    Height = 30,
            //    Margin = new Padding(5)
            //};

            // Add the buttons to the FlowLayoutPanel

            //buttonPanel.Controls.Add(addButton);
            buttonPanel.Controls.Add(editButton);
            buttonPanel.Controls.Add(deleteButton);

            //buttonPanel.Controls.Add(exportButton);
            //buttonPanel.Controls.Add(expandButton);

            // Add the label and button panel to the item panel
            itemPanel.Controls.Add(itemLabel);
            itemPanel.Controls.Add(buttonPanel);

            return itemPanel;
        }
        //private Panel CreateSubItemsPanel(List<Product> products)
        //{
        //    Panel subItemsPanel = new Panel
        //    {
        //        Height = 100,
        //        AutoScroll = true,
        //        BorderStyle = BorderStyle.None,
        //        Padding = new Padding(5),
        //        Dock = DockStyle.Top
        //    };

        //    // Add example subitems with full-width panels
        //    if (products != null)
        //    {
        //        foreach (var item in products)
        //        {
        //            Panel subItemPanel = new Panel
        //            {
        //                Height = 30,
        //                Dock = DockStyle.Top,
        //                BorderStyle = BorderStyle.None,
        //                Padding = new Padding(5)
        //            };

        //            Label subItemLabel = new Label
        //            {
        //                Text = $"Name:{item.Name} Count:{item.Quantity}",
        //                AutoSize = true,
        //                Location = new System.Drawing.Point(10, 5)
        //            };

        //            subItemPanel.Controls.Add(subItemLabel);
        //            subItemsPanel.Controls.Add(subItemPanel);
        //        }
        //    }

        //    return subItemsPanel;
        //}
        //private void ToggleSubItems(Panel itemPanel)
        //{
        //    // Find the associated subItemsPanel below itemPanel in the mainTableLayoutPanel
        //    int itemIndex = mainTableLayoutPanel.Controls.GetChildIndex(itemPanel);
        //    if (itemIndex >= 0 && itemIndex < mainTableLayoutPanel.Controls.Count - 1)
        //    {
        //        Panel subItemsPanel = mainTableLayoutPanel.Controls[itemIndex + 1] as Panel;
        //        if (subItemsPanel != null)
        //        {
        //            subItemsPanel.Visible = !subItemsPanel.Visible;
        //        }
        //    }
        //}
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
            Button button = sender as Button;

            if (button?.Tag == null)
                return;

            int productId = int.Parse(button.Tag.ToString());

            // Delete the product from the backend
            await _productService.DeleteProductAsync(productId);

            // Remove the product from the local list
            var productToRemove = packProducts.FirstOrDefault(p => p.Product.Id == productId);
            if (productToRemove != null)
            {
                packProducts.Remove(productToRemove);
            }

            // Refresh the UI
            RefreshProductList();

        }
        public void AddButton_Click(object sender, EventArgs e)
        {
            AddProductForm addProductForm = new AddProductForm(_categoryService, _productService);
            addProductForm.ShowDialog();
            Button button = sender as Button;

            if (addProductForm.DataSaved)
            {
                Debug.WriteLine("I WORK");
                //    JsonManager.AddProductToPackage(addProductForm.NewProduct, button.Tag.ToString());
                RestartPage();
                InitializeItems();
            }
        }
        public void EditButton_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;
            Product selectedProduct = button.Tag as Product;
            // AddProductForm addProductForm = new AddProductForm(selectedProduct);
            //addProductForm.ShowDialog();


            //if (addProductForm.DataSaved)
            //{
            //    //Debug.WriteLine(selectedProduct.Name);
            //  //  JsonManager.EditProductOfPackage(addProductForm.EditedProduct, currentPackageId);
            //    products.Remove(selectedProduct);
            //  //  products.Add(addProductForm.EditedProduct);

            //    RestartPage();
            //    InitializeItems();
            //}
        }
    }
}
