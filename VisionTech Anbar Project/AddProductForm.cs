using MetroSet_UI.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VisionTech_Anbar_Project.ViewModel;

namespace VisionTech_Anbar_Project
{
    public partial class AddProductForm : MetroSetForm
    {
        private bool IsEdit;
        private Product OriginalProduct;
        public Product EditedProduct;
        public Product NewProduct;
        public bool DataSaved;
        public AddProductForm()
        {
            InitializeComponent();
        }
        public AddProductForm(Product product)
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
        private void PopulateOriginalProduct()
        {
            textBox1.Text = OriginalProduct.Name.ToString();
            textBox2.Text = OriginalProduct.Description.ToString();
            textBox3.Text = OriginalProduct.Quantity.ToString();
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

            if (string.IsNullOrWhiteSpace(textBox1.Text))
                errors.Add("Id required");

            return errors;
        }
        private void StoreInput()
        {
            string Name;
            string Description;
            string Quantity;

            string id;

            Name = textBox1.Text;
            Description = textBox2.Text;
            Quantity = textBox3.Text;

            if (IsEdit)
                EditedProduct = new Product(OriginalProduct.Id, Name,
                                         Description, Quantity);
            else
            {
                id = Guid.NewGuid().ToString();
                NewProduct = new Product(id, Name, Description, Quantity);
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


    }
}

