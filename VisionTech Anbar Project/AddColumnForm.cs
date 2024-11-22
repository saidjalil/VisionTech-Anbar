using MetroSet_UI.Forms;
using Microsoft.VisualBasic.Devices;
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
    public partial class AddColumnForm : MetroSetForm
    {
        private bool IsEdit;
        private Package OriginalPackage;
        public Package EditedPackage;
        public Package NewPackage;
        public bool DataSaved;
        public AddColumnForm()
        {
            InitializeComponent();
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
            textBox1.Text = OriginalPackage.PackageId.ToString();
            dateTimePicker1.Text = OriginalPackage.CreatedDate.ToString();
        }
        private void ClearInput()
        {
            textBox1.Clear();
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

            if (string.IsNullOrWhiteSpace(textBox1.Text))
                errors.Add("Id required");

            return errors;
        }

        private void StoreInput()
        {
            string packageId;
            DateTime createdDate;
            bool exported = false;
            List<Product> products = new List<Product>();
            //int id;

            packageId = textBox1.Text;
            createdDate = DateTime.Parse(dateTimePicker1.Text.ToString());


            if (IsEdit)
                EditedPackage = new Package(packageId,
                                         createdDate, exported, products);
            else
            {
                //id = Convert.ToInt32(DateTime.Now.ToString("ddHHmmss"));
                NewPackage = new Package(packageId, createdDate, exported, products);
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

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {

        }
    }
}
