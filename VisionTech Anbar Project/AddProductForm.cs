using MetroSet_UI.Forms;
using Serilog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VisionTech_Anbar_Project.Entities;
using VisionTech_Anbar_Project.Entities.Categories;
using VisionTech_Anbar_Project.Services;

namespace VisionTech_Anbar_Project
{
    public partial class AddProductForm : MetroSetForm
    {
        private bool IsEdit;
        private PackageProduct OriginalProduct;
        public PackageProduct EditedProduct;
        public PackageProduct NewProduct;
        public bool DataSaved;

        private readonly CategoryService categoryService;

        private List<Category> categories = new List<Category>();



        private int comboBoxCount = 0; // Counter for dynamically created ComboBoxes
        private List<ComboBox> comboBoxes = new List<ComboBox>();
        ComboBox mainComboBox;
        public AddProductForm()
        {
            categoryService = new CategoryService(new());

            InitializeComponent();
            CreateNewComboBox(null);
        }
        public AddProductForm(PackageProduct product)
        {
            InitializeComponent();
            IsEdit = true;
            OriginalProduct = product;
        }
        private void AddEditMovie_Load(object sender, EventArgs e)
        {
            DataSaved = false;
            if (IsEdit)
            {
                PopulateOriginalProduct();
                this.Text = "Edit";
            }

            else
            {
                ClearInput();
                this.Text = "Add";
            }
        }
        private async void PopulateOriginalProduct()
        {
            // comboBox1.Text = Category;
            textBox2.Text = OriginalProduct.Product.ProductName.ToString();
            textBox3.Text = OriginalProduct.Quantity.ToString();

            categories = (await categoryService.GetAllCategoriesAsync()).ToList();

            mainComboBox.Items.AddRange(categories.Where(x => x.ParentId == null).ToArray());
            //comboBox1.Text = OriginalProduct.Product.Category.Name.ToString();
        }
        private void ClearInput()
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();

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

            if (string.IsNullOrWhiteSpace(textBox2.Text))
                errors.Add("Product Adı vacibdir");

            return errors;
        }
        private void StoreInput()
        {
            string Name;
            // string Description;
            int Quantity;

            string categoryName;
            string id;

            Name = textBox2.Text;
            var selectedCategory = mainComboBox.SelectedItem as Category;

            Log.Information(selectedCategory.Name);
            //Description = textBox2.Text;
            Quantity = int.Parse(textBox3.Text);

            if (IsEdit)
                EditedProduct = new PackageProduct(Name,
                                         Quantity, selectedCategory, selectedCategory.Id);
            else
            {
                id = Guid.NewGuid().ToString();
                NewProduct = new PackageProduct(Name,
                                         Quantity, selectedCategory, selectedCategory.Id);
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

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
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

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void AddProductForm_Load(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }


        private void CreateNewComboBox(ComboBox triggeringComboBox)
        {
            comboBoxCount++;

            ComboBox newComboBox = new ComboBox
            {
                Name = "ComboBox" + comboBoxCount,
                Width = 200,
                Location = triggeringComboBox == null
                    ? new System.Drawing.Point(20, 20) // Position of the first ComboBox
                    : new System.Drawing.Point(20, triggeringComboBox.Bottom + 10), // Below the triggering ComboBox
                DropDownStyle = ComboBoxStyle.DropDown
            };

            // Add some dummy data for demonstration purposes
            newComboBox.Items.AddRange(new object[] { "Option 1", "Option 2", "Option 3" });

            // Event handler for when an item is selected or input is entered
            newComboBox.SelectedIndexChanged += comboBox1_Changed;
            newComboBox.TextChanged += comboBox1_Changed;

            mainComboBox = newComboBox;
            // Add the new ComboBox to the Form and to the list
            this.Controls.Add(newComboBox);
            comboBoxes.Add(newComboBox);
        }

        private void comboBox1_Changed(object sender, EventArgs e)
        {
            ComboBox currentComboBox = sender as ComboBox;

            // Check if this ComboBox already triggered the creation of a new one
            if (comboBoxes.Last() == currentComboBox && !string.IsNullOrWhiteSpace(currentComboBox.Text))
            {
                // Create a new ComboBox below the current one
                CreateNewComboBox(currentComboBox);
            }
        }
    }
}

