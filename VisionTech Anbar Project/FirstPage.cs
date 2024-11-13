using MetroSet_UI.Forms;
using System;
using System.Windows.Forms;
using VisionTech_Anbar_Project.Utilts;
using VisionTech_Anbar_Project.ViewModel;
using static System.Windows.Forms.DataFormats;

namespace VisionTech_Anbar_Project
{
    public partial class FirstPage : MetroSetForm
    {
        TableLayoutPanel mainTableLayoutPanel;

        public FirstPage()
        {
            InitializeComponent();
            SetupMainTableLayoutPanel();
            InitializeItems();
        }

        private void FirstPage_Load(object sender, EventArgs e)
        {

        }

        private void SetupMainTableLayoutPanel()
        {
            mainTableLayoutPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                ColumnCount = 1, // Single column to arrange items vertically
            };
            this.Controls.Add(mainTableLayoutPanel);
        }

        private void InitializeItems()
        {
            var data = JsonManager.GetAllPackages();
            foreach (var item in data)
            {
                Panel itemPanel = CreateItemPanel(item);
                mainTableLayoutPanel.RowCount++;
                mainTableLayoutPanel.Controls.Add(itemPanel, 0, mainTableLayoutPanel.RowCount - 1); // Add item to new row

                Panel subItemsPanel = CreateSubItemsPanel(item.Products);
                mainTableLayoutPanel.RowCount++;
                mainTableLayoutPanel.Controls.Add(subItemsPanel, 0, mainTableLayoutPanel.RowCount - 1); // Add subitems below item
                subItemsPanel.Visible = false; // Initially hidden
            }
                
            
        }

        public void RestardPage()
        {
            mainTableLayoutPanel.Controls.Clear();
        }

        private Panel CreateItemPanel(Package package)
        {
            // Create the main panel for the item
            Panel itemPanel = new Panel
            {
                Height = 50, // Set explicit height for the main item
                BorderStyle = BorderStyle.None,
                Dock = DockStyle.Top,
                Margin = new Padding(5) // Add some margin between items
            };

            // Label to display item text
            Label itemLabel = new Label
            {
                Text = package.PackageId.ToString(),
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
            Button expandButton = new Button
            {
                Text = "^",
                Width = 30,
                Height = 30,
                Margin = new Padding(5)
            };
            expandButton.Click += (s, e) => ToggleSubItems(itemPanel);

            // Add Delete, Add, Edit, and Export buttons
            Button deleteButton = new Button
            {
                Text = "Delete",
                Tag = package.PackageId,
                Width = 60,
                Height = 30,
                Margin = new Padding(5)
            };

            deleteButton.Click += DeleteButton_Click;
            Button addButton = new Button
            {
                Text = "Add",
                Width = 60,
                Height = 30,
                Margin = new Padding(5)
            };
            Button editButton = new Button
            {
                Text = "Edit",
                Width = 60,
                Height = 30,
                Margin = new Padding(5)
            };
            Button exportButton = new Button
            {
                Text = "Export",
                Width = 60,
                Height = 30,
                Margin = new Padding(5)
            };

            // Add the buttons to the FlowLayoutPanel
            
            buttonPanel.Controls.Add(deleteButton);
            buttonPanel.Controls.Add(addButton);
            buttonPanel.Controls.Add(editButton);
            buttonPanel.Controls.Add(exportButton);
            buttonPanel.Controls.Add(expandButton);

            // Add the label and button panel to the item panel
            itemPanel.Controls.Add(itemLabel);
            itemPanel.Controls.Add(buttonPanel);

            return itemPanel;
        }

        private Panel CreateSubItemsPanel(List<Product> products)
        {
            Panel subItemsPanel = new Panel
            {
                Height = 100,
                BorderStyle = BorderStyle.None,
                Padding = new Padding(5),
                Dock = DockStyle.Top
            };

            // Add example subitems with full-width panels
            foreach (var item in products)
            {
                Panel subItemPanel = new Panel
                {
                    Height = 30,
                    Dock = DockStyle.Top,
                    BorderStyle = BorderStyle.None,
                    Padding = new Padding(5)
                };

                Label subItemLabel = new Label
                {
                    Text = $"Name:{item.Name} Count:{item.Quantity}",
                    AutoSize = true,
                    Location = new System.Drawing.Point(10, 5)
                };

                subItemPanel.Controls.Add(subItemLabel);
                subItemsPanel.Controls.Add(subItemPanel);
            }
              

            return subItemsPanel;
        }

        private void ToggleSubItems(Panel itemPanel)
        {
            // Find the associated subItemsPanel below itemPanel in the mainTableLayoutPanel
            int itemIndex = mainTableLayoutPanel.Controls.GetChildIndex(itemPanel);
            if (itemIndex >= 0 && itemIndex < mainTableLayoutPanel.Controls.Count - 1)
            {
                Panel subItemsPanel = mainTableLayoutPanel.Controls[itemIndex + 1] as Panel;
                if (subItemsPanel != null)
                {
                    subItemsPanel.Visible = !subItemsPanel.Visible;
                }
            }
        }

        public void DeleteButton_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;
            JsonManager.DeletePackageById(button.Tag.ToString());
            RestardPage();
            InitializeItems();
        }
    }
}
