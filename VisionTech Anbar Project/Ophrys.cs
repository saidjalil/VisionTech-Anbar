using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VisionTech_Anbar_Project.ViewModel;

namespace VisionTech_Anbar_Project
{
    public partial class Ophrys : Form
    {
        private List<Package> Packages = new List<Package>();
        public Ophrys()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AddColumnForm addColumnForm = new AddColumnForm();
            addColumnForm.ShowDialog();

            // Add new Package to list and UI
            if (addColumnForm.DataSaved)
            {
                Packages.Add(addColumnForm.NewPackage);
                AddPackageToUI(addColumnForm.NewPackage);
            }

        }

        private void AddPackageToUI(Package package)
        {
            //Create Panel
            Panel panel;
            panel = new Panel();
            panel.Name = String.Format("PnlPackage{0}", package.PackageId);
            panel.BackColor = Color.White;
            panel.Size = new Size(125, 205);
            panel.Margin = new Padding(10);
            panel.Tag = package.PackageId;

            //Create Id label
            Label labelId;
            labelId = new Label();
            labelId.Name = String.Format("LblPackageId{0}", package.PackageId);
            labelId.Text = package.PackageId.ToString();
            labelId.Location = new Point(12, 165);
            labelId.ForeColor = Color.Black;
            labelId.Font = new Font(this.Font.FontFamily, 9.5f, FontStyle.Regular);
            labelId.AutoSize = true;
            labelId.Tag = package.PackageId;

            // Create year label
            Label labelYear;
            labelYear = new Label();
            labelYear.Name = String.Format("LblPackageYear{0}", package.PackageId);
            labelYear.Text = package.CreatedDate.ToString();
            labelYear.Location = new Point(12, 185);
            labelYear.ForeColor = Color.Gray;
            labelYear.Font = new Font(this.Font.FontFamily, 9.5f, FontStyle.Regular);
            labelYear.Tag = package.PackageId;

            // Add controls to panel
            panel.Controls.Add(labelId);
            panel.Controls.Add(labelYear);

            // Add panel to flowlayoutpanel
            flowLayoutPanel1.Controls.Add(panel);
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void metroSetBadge1_Click(object sender, EventArgs e)
        {

        }
    }
}
