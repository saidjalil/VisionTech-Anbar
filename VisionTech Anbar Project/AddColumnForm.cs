using MetroSet_UI.Forms;
using Microsoft.VisualBasic.Devices;
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
using VisionTech_Anbar_Project.Entities;
using VisionTech_Anbar_Project.Services;
using VisionTech_Anbar_Project.Utilts;

namespace VisionTech_Anbar_Project
{
    public partial class AddColumnForm : MetroSetForm
    {
        private bool IsEdit;
        private Package OriginalPackage;
        public Package EditedPackage;
        public Package NewPackage;
        public bool DataSaved;
        private readonly PackageService _packageService;
        private readonly ProductService _productService;
        private readonly CategoryService _categoryService;
        private readonly ImageService _imageService;


        public OpenFileDialog openFileDialog = new OpenFileDialog();



        private readonly WarehouseService _warehouseService;
        private readonly VendorService _vendorService;


        TableLayoutPanel mainTableLayoutPanel;
        List<PackageProduct> products = new List<PackageProduct>();
        List<Warehouse> warehouseList = new List<Warehouse>();
        List<Vendor> vendorList = new List<Vendor>();
        List<ViewModel.User> users = new List<ViewModel.User>();



        public AddColumnForm(PackageService packageService, CategoryService categoryService, ProductService productService, WarehouseService warehouseService, VendorService vendorService, ImageService imageService)
        {
            //  _packageService = packageService;
            _categoryService = categoryService;
            _productService = productService;
            _packageService = packageService;
            _warehouseService = warehouseService;
            _vendorService = vendorService;
            _imageService = imageService;

            InitializeComponent();

            mainTableLayoutPanel = tableLayoutPanel1;
            mainTableLayoutPanel.ColumnCount = 1;
            mainTableLayoutPanel.AutoScroll = true; // Enable scrolling if needed
            //mainTableLayoutPanel.Dock = DockStyle.Fill;
            mainTableLayoutPanel.RowStyles.Clear();
        }
        //private AddColumnForm(Package package)
        //{
        //    InitializeComponent();
        //    IsEdit = true;
        //    OriginalPackage = package;
        //}
        private void AddEditMovie_Load(object sender, EventArgs e)
        {
            DataSaved = false;
            if (IsEdit)
            {
                PopulateOriginalPackage();
                this.Text = "Edit";
            }
            else
            {
                ClearInput();
                this.Text = "Add";
            }
        }
        private void PopulateOriginalPackage()
        {
            //textBox1.Text = OriginalPackage.Id.ToString();
            //comboBox1.
            //dateTimePicker1.Text = DateTime.Now.ToString();
        }
        private void ClearInput()
        {
            textBox2.Clear();
            dateTimePicker1.Text = DateTime.Now.ToString();
        }
        private async void button1_Click(object sender, EventArgs e)
        {
            //ImageManager imageManager = new(_imageService);
            List<String> errors;
            errors = ValidateInput();

            if (errors.Count > 0)
            {
                ShowErrors(errors, 5);
                return;
            }
            StoreInput();
            DataSaved = true;
            this.Close();

            //try
            //{
            //    ImageManager.SaveImage(openFileDialog); // Use your existing SaveImage method
            //   // MessageBox.Show("Şəkil əlavə edildi!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show($"Failed to save image: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}

            //if (!string.IsNullOrEmpty(openFileDialog.FileName) && pictureBox2.Image != null)
            //{
            //    await imageManager.SaveImage(openFileDialog, NewPackage.Id); // Call the provided SaveImage method
            //    //MessageBox.Show("Image saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //}
            //else
            //{
            //    MessageBox.Show("Şəkil seçilməyib.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //}
        }
        private List<string> ValidateInput()
        {
            List<String> errors = new List<string>();

            if (string.IsNullOrWhiteSpace(comboBox1.Text))
                errors.Add("Anbar bos ola bilmez");

            return errors;
        }

        private void StoreInput()
        {
            // Retrieve user inputs
            string retriever = RetrieveComboBoxInput(comboBox3, "Retriever");
            string address = RetrieveTextBoxInput(textBox2, "Address");

            // Retrieve or create warehouse and vendor
            Warehouse selectedWarehouse = GetOrCreateWarehouse(comboBox1);
            Vendor selectedVendor = GetOrCreateVendor(comboBox2);

            // Add products to the package
            List<PackageProduct> packageProducts = products?.ToList() ?? new List<PackageProduct>();

            // Parse the created date
            DateTime createdDate = DateTime.Now;

            // Create or edit a package
            if (IsEdit)
            {
                EditedPackage = CreatePackage(createdDate, selectedVendor, selectedWarehouse, packageProducts, retriever, address);
            }
            else
            {
                NewPackage = CreatePackage(createdDate, selectedVendor, selectedWarehouse, packageProducts, retriever, address);
            }
        }

        private string RetrieveTextBoxInput(TextBox textBox, string fieldName)
        {
            if (string.IsNullOrWhiteSpace(textBox.Text))
            {
                throw new ArgumentException($"{fieldName} cannot be empty.");
            }
            return textBox.Text.Trim();
        }

        private string RetrieveComboBoxInput(ComboBox comboBox, string fieldName)
        {
            if (comboBox.SelectedItem == null)
            {
                throw new ArgumentException($"Please select a valid {fieldName}.");
            }
            return comboBox.Text;
        }

        private Warehouse GetOrCreateWarehouse(ComboBox comboBox)
        {
            if (comboBox.Text != ((Warehouse)comboBox.SelectedItem)?.WarehouseName)
            {
                return new Warehouse
                {
                    WarehouseName = comboBox.Text.Trim()
                };
            }
            return (Warehouse)comboBox.SelectedItem;
        }

        private Vendor GetOrCreateVendor(ComboBox comboBox)
        {
            if (comboBox.Text != ((Vendor)comboBox.SelectedItem)?.VendorName)
            {
                return new Vendor
                {
                    VendorName = comboBox.Text.Trim()
                };
            }
            return (Vendor)comboBox.SelectedItem;
        }

        private Package CreatePackage(DateTime createdDate, Vendor vendor, Warehouse warehouse, List<PackageProduct> products, string retriever, string address)
        {
            return new Package(
                createdDate,
                vendor,
                warehouse,
                products,
                retriever,
                address
            );
        }

        public async void SetAllData()
        {
            warehouseList = (await _warehouseService.GetAllWarehousesAsync())
              .ToList();
            vendorList = (await _vendorService.GetAllVendorsAsync()).ToList();
            users = JsonManager.GetUsers();
            comboBox1.DataSource = warehouseList;
            comboBox1.DisplayMember = "WarehouseName"; // Display the 'Name' in the ComboBox
            comboBox1.ValueMember = "Id";

            comboBox2.DataSource = vendorList;
            comboBox2.DisplayMember = "VendorName"; // Display the 'Name' in the ComboBox
            comboBox2.ValueMember = "Id";

            comboBox3.DataSource = users;
            comboBox3.DisplayMember = "UserName"; // Display the 'Name' in the ComboBox
            comboBox3.ValueMember = "UserName";

            // QEBUL EDEN SIDE YAZILMALIDI
        }

        private void ShowErrors(List<string> errors, int max)
        {
            MessageBoxIcon icon;
            MessageBoxButtons buttons;
            string text = null;

            icon = MessageBoxIcon.Error;
            buttons = MessageBoxButtons.OK;

            if (max > errors.Count)
                max = errors.Count;

            for (int i = 0; i < max; i++)
            {
                text += errors[i] + "\n";
            }

            MessageBox.Show(text, "", buttons, icon);
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Allow control keys like Backspace, Enter, and Tab
            if (char.IsControl(e.KeyChar))
            {
                return;
            }
            // Check if the key is a number
            if (!char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private async void button3_Click(object sender, EventArgs e)
        {
            AddProductForm addProductForm = new AddProductForm(_categoryService, _productService);
            addProductForm.ShowDialog();

            // Add both sections to the accordion
            // Add new Package to list and UI
            if (addProductForm.DataSaved)
            {
                products.Add(addProductForm.NewProduct);
                //await packageService.CreatePackageAsync(addProductForm.NewProduct);
                Debug.WriteLine(products);
                //RestartPage();
                InitializeItems();
            }
        }
        //public void RestartPage()
        //{
        //    mainTableLayoutPanel.Controls.Clear();
        //}
        private void InitializeItems()
        {
            // Clear any existing controls in the TableLayoutPanel
            mainTableLayoutPanel.Controls.Clear();
            mainTableLayoutPanel.RowCount = 0;

            foreach (var product in products)
            {
                // Create a new item panel for each product
                Panel itemPanel = CreateItemPanel(product);

                // Add the panel to a new row in the TableLayoutPanel
                mainTableLayoutPanel.RowCount++;
                mainTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
                mainTableLayoutPanel.Controls.Add(itemPanel, 0, mainTableLayoutPanel.RowCount - 1);
            }
        }

        private Panel CreateItemPanel(PackageProduct product)
        {
            // Create the panel that represents a product
            Panel itemPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 60, // Explicit height for uniformity
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.White,
                Margin = new Padding(5) // Margin for spacing between rows
            };

            // Create a label to show the product name
            Label itemLabel = new Label
            {
                Text = $"Product Name: {product.Product.ProductName} | Quantity: {product.Quantity}",
                AutoSize = true,
                Location = new Point(10, 15), // Position inside the panel
                Font = new Font("Segoe UI", 10, FontStyle.Regular),
                ForeColor = Color.Black
            };

            // Create the delete button
            Button deleteButton = new Button
            {
                Text = "🗑", // Trash icon
                Tag = product, // Attach the product object to the button for easy access
                ForeColor = Color.Red,
                Width = 50,
                Height = 40,
                Anchor = AnchorStyles.Right, // Align it to the right side
                Margin = new Padding(5)
            };
            deleteButton.Click += DeleteButton_Click;

            // Position the delete button inside the panel
            deleteButton.Location = new Point(itemPanel.Width - deleteButton.Width - 10, 10);
            deleteButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;

            // Add the label and delete button to the item panel
            itemPanel.Controls.Add(itemLabel);
            itemPanel.Controls.Add(deleteButton);

            return itemPanel;
        }


        private void DeleteButton_Click(object sender, EventArgs e)
        {
            // Get the product object from the button's Tag property
            var button = sender as Button;
            if (button?.Tag is PackageProduct productToRemove)
            {
                // Remove the product from the list
                products.Remove(productToRemove);

                // Refresh the UI
                InitializeItems();
            }
        }



        private void button4_Click(object sender, EventArgs e)
        {
            openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif";
            openFileDialog.Title = "Select an Image";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                pictureBox2.Image = System.Drawing.Image.FromFile(openFileDialog.FileName);
                pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
            }
        }

    }
}
