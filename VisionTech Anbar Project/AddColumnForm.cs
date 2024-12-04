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

        private OpenFileDialog openFileDialog = new OpenFileDialog();



        private readonly WarehouseService _warehouseService;
        private readonly VendorService _vendorService;


        TableLayoutPanel mainTableLayoutPanel;
        List<PackageProduct> products = new List<PackageProduct>();
        List<Warehouse> warehouseList = new List<Warehouse>();
        List<Vendor> vendorList = new List<Vendor>();
        List<ViewModel.User> users = new List<ViewModel.User>();



        public AddColumnForm(PackageService packageService, CategoryService categoryService, ProductService productService, WarehouseService warehouseService, VendorService vendorService)
        {
            //  _packageService = packageService;
            _categoryService = categoryService;
            _productService = productService;
            _packageService = packageService;
            _warehouseService = warehouseService;
            _vendorService = vendorService;
            InitializeComponent();

            mainTableLayoutPanel = tableLayoutPanel1;
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
        private void button1_Click(object sender, EventArgs e)
        {
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

            if (!string.IsNullOrEmpty(openFileDialog.FileName) && pictureBox2.Image != null)
            {
                ImageManager.SaveImage(openFileDialog); // Call the provided SaveImage method
                //MessageBox.Show("Image saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Şəkil seçilməyib.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
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
            DateTime createdDate;
            List<PackageProduct> packageProducts = new List<PackageProduct>();

            // Retrieve or create the selected warehouse
            Warehouse selectedWarehouse;
            if (comboBox1.Text != ((Warehouse)comboBox1.SelectedItem)?.WarehouseName)
            {
                // Create a new warehouse with the manually entered name
                selectedWarehouse = new Warehouse
                {
                    WarehouseName = comboBox1.Text
                    // Add other default properties if needed
                };
                // Optionally, save this new warehouse to the database
                // await _warehouseService.CreateWarehouseAsync(selectedWarehouse);
            }
            else
            {
                selectedWarehouse = (Warehouse)comboBox1.SelectedItem;
            }

            // Retrieve or create the selected vendor
            Vendor selectedVendor;
            if (comboBox2.Text != ((Vendor)comboBox2.SelectedItem)?.VendorName)
            {
                // Create a new vendor with the manually entered name
                selectedVendor = new Vendor
                {
                    VendorName = comboBox2.Text
                    // Add other default properties if needed
                };
                // Optionally, save this new vendor to the database
                // await _vendorService.CreateVendorAsync(selectedVendor);
            }
            else
            {
                selectedVendor = (Vendor)comboBox2.SelectedItem;
            }

            // Add products to the package
            if (products != null)
            {
                foreach (PackageProduct product in products)
                {
                    packageProducts.Add(product);
                }
            }

            // Parse the created date from the DateTimePicker
            // createdDate = DateTime.Parse(dateTimePicker1.Text);
            createdDate = DateTime.Now;


            // Create or edit a package
            if (IsEdit)
            {
                EditedPackage = new Package(
                    createdDate,
                    selectedVendor,
                    selectedWarehouse,
                    packageProducts
                );
            }
            else
            {
                NewPackage = new Package(
                    createdDate,
                    selectedVendor,
                    selectedWarehouse,
                    packageProducts
                );
            }
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
                RestartPage();
                InitializeItems();
            }
        }
        public void RestartPage()
        {
            mainTableLayoutPanel.Controls.Clear();
        }
        private async void InitializeItems()
        {
            foreach (var item in products)
            {
                foreach (var product in products)
                {
                    Panel itemPanel = CreateItemPanel(item);
                    mainTableLayoutPanel.RowCount++;
                    mainTableLayoutPanel.Controls.Add(itemPanel, 0, mainTableLayoutPanel.RowCount - 1); // Add item to new row
                }

                //Panel subItemsPanel = CreateSubItemsPanel(product.Product, product.Quantity);
                //mainTableLayoutPanel.RowCount++;
                //mainTableLayoutPanel.Controls.Add(subItemsPanel, 0, mainTableLayoutPanel.RowCount - 1); // Add subitems below item
                //subItemsPanel.Visible = false; // Initially hidden
            }
        }

        private Panel CreateItemPanel(PackageProduct product)
        {
            // Create the main panel for the item
            Panel itemPanel = new Panel
            {
                Dock = DockStyle.Top,
                //Height = 50, // Set explicit height for the main item
                BorderStyle = BorderStyle.None,
                BackColor = Color.Black,
                Margin = new Padding(5) // Add some margin between items
            };

            // Label to display item text
            Label itemLabel = new Label
            {
                Text = product.Product.ProductName,
                AutoSize = true,
                Location = new System.Drawing.Point(5, 15)
            };

            // Add the buttons to the FlowLayoutPanel

            // Add the label and button panel to the item panel
            itemPanel.Controls.Add(itemLabel);

            return itemPanel;
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

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }
    }
}
