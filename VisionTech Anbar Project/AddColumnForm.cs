using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VisionTech_Anbar_Project
{
    public partial class AddColumnForm : Form
    {
        Ophrys Main;
        public AddColumnForm(Ophrys main)
        {
            InitializeComponent();
            Main = main;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Main.dataGridView1.Rows.Add(textBox1.Text,textBox2.Text, comboBox1.Text);
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
