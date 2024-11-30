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

        TableLayoutPanel mainTableLayoutPanel;
        List<PackageProduct> products = new List<PackageProduct>();


        public AddColumnForm(PackageService packageService)
        {
          //  _packageService = packageService;
            _packageService = packageService;
            InitializeComponent();
            mainTableLayoutPanel = tableLayoutPanel1;
        }
        private AddColumnForm(Package package)
        {
            InitializeComponent();
            IsEdit = true;
            OriginalPackage = package;
        }
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
            string warehouseName;
            DateTime createdDate;
            string vendorName;

            warehouseName = comboBox1.Text;
            vendorName = comboBox2.Text;


            //List<Product> products = new List<Product>();
            //int id;

            // packageId = textBox1.Text;
            createdDate = DateTime.Parse(dateTimePicker1.Text.ToString());


            if (IsEdit)
                EditedPackage = new Package(createdDate,
                                         vendorName, warehouseName);
            else
            {
                //id = Convert.ToInt32(DateTime.Now.ToString("ddHHmmss"));
                NewPackage = new Package(createdDate, vendorName, warehouseName);
            }

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
        //private Panel CreateItemPanel(PackageProduct product)
        //{
        //    // Create the main panel for the item
        //    Panel itemPanel = new Panel
        //    {
        //        Height = 60, // Set explicit height for the main item
        //        BorderStyle = BorderStyle.None,
        //        Dock = DockStyle.Top,
        //        Margin = new Padding(5) // Add some margin between items
        //    };

        //    // Label to display item text
        //    Label itemLabel = new Label
        //    {
        //        Text = product.Product.ProductName.ToString(),
        //        AutoSize = true,
        //        Location = new System.Drawing.Point(5, 15)
        //    };

        //    // Create a FlowLayoutPanel to hold all buttons in a single line
        //    FlowLayoutPanel buttonPanel = new FlowLayoutPanel
        //    {
        //        FlowDirection = FlowDirection.LeftToRight, // Align buttons horizontally
        //        Dock = DockStyle.Right,
        //        AutoSize = true,
        //        WrapContents = false, // Prevent buttons from wrapping to the next line
        //        Margin = new Padding(5)
        //    };

        //    // Button to expand/collapse subitems


        //    // Add the label and button panel to the item panel
        //    itemPanel.Controls.Add(itemLabel);
        //    itemPanel.Controls.Add(buttonPanel);

        //    return itemPanel;
        //}
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void AddColumnForm_Load(object sender, EventArgs e)
        {

        }

    }
}
