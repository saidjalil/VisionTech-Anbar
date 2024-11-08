using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VisionTech_Anbar_Project.Utilts;

namespace VisionTech_Anbar_Project
{
    public partial class Ophrys : Form
    {
        public Ophrys()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var packages = JsonManager.GetAllPackages();
            foreach (var package in packages)
            {
                string packageInfo = $"Exported: {package.Exported}";
                listBox1.Items.Add(packageInfo);

                foreach (var product in package.Products)
                {
                    string productInfo = $"  Id: {product.Id}, Name: {product.Name}, " +
                                         $"Description: {product.Description}, Quantity: {product.Quantity}";
                    listBox1.Items.Add(productInfo);
                }
                listBox1.Items.Add(""); // Separator line
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void metroSetBadge1_Click(object sender, EventArgs e)
        {

        }

        private void metroSetLabel1_Click(object sender, EventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }
    }
}
