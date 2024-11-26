using MetroSet_UI.Forms;
using Serilog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
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



        private int selectedId = 0;
        private int currentParentId = 0;
        private readonly CategoryService categoryService;

        private List<Category> categories = new List<Category>();



        private int comboBoxCount = 0; // Counter for dynamically created ComboBoxes
        private Dictionary<int, ComboBox> comboBoxDictionary = new Dictionary<int, ComboBox>();

        private List<ComboBox> comboBoxes = new List<ComboBox>();
        //private List<Tuple<ComboBox, ComboBox, int?>> comboBoxRelationships = new List<Tuple<ComboBox, ComboBox, int?>>();

        ComboBox mainComboBox;
        public AddProductForm()
        {
            categoryService = new CategoryService(new());

            InitializeComponent();
            InitializeCategories();
            //InitializeMainComboBox();
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
            // string id;
            //string categoryName;




            Name = textBox2.Text;
            //var selectedCategory = mainComboBox.SelectedItem as Category;


            //Log.Information(selectedCategory.Name);
            //Description = textBox2.Text;
            Quantity = int.Parse(textBox3.Text);

            if (IsEdit)
                EditedProduct = new PackageProduct(Name,
                                         Quantity, selectedId);
            else
            {
                //id = Guid.NewGuid().ToString();
                NewProduct = new PackageProduct(Name,
                                         Quantity, selectedId);
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

        private async void InitializeCategories()
        {
            comboBoxCount++;

            comboBox1.Tag = comboBoxCount;
             categories = (await categoryService.GetAllCategoriesAsync())
                .Where(x => x.ParentId == null)
                .ToList();
            
            comboBox1.DataSource = categories;
            comboBox1.DisplayMember = "Name"; // Display the 'Name' in the ComboBox
            comboBox1.ValueMember = "Id";

            comboBox1.SelectedIndexChanged += comboBox1_Changed;
            //mainComboBox.TextChanged += MainComboBox_TextChanged;
            comboBox1.KeyDown += new KeyEventHandler(MainComboBox_TextChanged);

            comboBoxes.Add(comboBox1);
            comboBoxDictionary.Add(comboBoxCount, comboBox1);



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

        private async void button5_Click(object sender, EventArgs e)
        {
            await categoryService.CreateCategoryAsync(new Category { CreatedTime = DateTime.Now, Name = "Kulek", UpdatedTime = DateTime.Now });
            //await categoryService.CreateCategoryAsync(new Category { CreatedTime = DateTime.Now, Name = "Ermenistan", UpdatedTime = DateTime.Now, ParentId =14});
        }

        private void comboBox1_Changed(object sender, EventArgs e)
        {
            if (comboBox1.SelectedValue is int selectedId && selectedId > 0)
            {
                // Create a new ComboBox for subcategories based on the selected category

                DeleteChildComboBoxes(comboBox1);
                currentParentId = selectedId;
                CreateSubComboBox(comboBox1, selectedId);
            }
        }

        private async void CreateSubComboBox(ComboBox triggeringComboBox, int parentId)
        {
            comboBoxCount++;

            ComboBox newComboBox = new ComboBox
            {
                Name = "ComboBox" + comboBoxCount,
                Width = 200,
                Location = new System.Drawing.Point(20, triggeringComboBox.Bottom + 10), // Position below the triggering ComboBox
                DropDownStyle = ComboBoxStyle.DropDown,
                Tag = comboBoxCount
            };

            // Fetch subcategories dynamically based on ParentId
            var subCategories = (await categoryService.GetSubCategoriesAsync(parentId)).ToList();

            if (!subCategories.Any())
            {
                // Add a placeholder item to indicate no subcategories
                subCategories.Add(new Category
                {
                    Id = -1, // A placeholder ID
                    Name = "No Subcategories"
                });
            }

            newComboBox.DataSource = subCategories;
            newComboBox.DisplayMember = "Name"; // Display the 'Name' in the ComboBox
            newComboBox.ValueMember = "Id";     // Use 'Id' as the selected value

            // Add event handler for the new ComboBox
            newComboBox.SelectedIndexChanged += SubComboBox_Changed;
            newComboBox.KeyDown += new KeyEventHandler(SubComboBox_TextChanged);

            // Track this new ComboBox with its parent ComboBox and associated parentId

           // comboBoxRelationships.Add(new Tuple<ComboBox, ComboBox, int?>(newComboBox, triggeringComboBox, parentId));

            // Add the new ComboBox to the Form and to the list
            this.Controls.Add(newComboBox);
            comboBoxes.Add(newComboBox);
            if (comboBoxDictionary.ContainsKey(comboBoxCount))
            {
                return; // Update existing entry
            }
            else
            {
                comboBoxDictionary.Add(comboBoxCount, newComboBox); // Add new entry
            }

        }

        private async void SubComboBox_Changed(object sender, EventArgs e)
        {
            ComboBox currentComboBox = sender as ComboBox;
            Log.Information("How many times");
            if (currentComboBox.SelectedValue is int selectedId && selectedId > 0)
            {
                //UpdateChildComboBoxes(currentComboBox, selectedId);
                DeleteChildComboBoxes(currentComboBox);
                currentParentId = selectedId;
                // Create a new ComboBox for subcategories based on the selected category
                CreateSubComboBox(currentComboBox, selectedId);
            }
        }

        private async void MainComboBox_TextChanged(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ComboBox mainComboBox = sender as ComboBox;

                // Get the user's input
                var inputText = mainComboBox.Text;

                // Check if the input matches an existing category
                var matchingCategory = categories.FirstOrDefault(x => x.Name.Equals(inputText, StringComparison.OrdinalIgnoreCase));

                if (matchingCategory == null && !string.IsNullOrWhiteSpace(inputText))
                {
                    // Input does not match an existing category
                    var result = MessageBox.Show(
                        $"The category '{inputText}' does not exist. Would you like to create it?",
                        "Create New Category",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);

                    if (result == DialogResult.Yes)
                    {
                        // Create the new category
                        var newCategory = await CreateCategoryAsync(inputText, null);

                        // Add the new category to the list and update the ComboBox's data source
                        categories.Add(newCategory);
                        mainComboBox.DataSource = null; // Reset the data source to refresh it
                        mainComboBox.DataSource = categories.Where(x => x.ParentId == null).ToList();
                        mainComboBox.DisplayMember = "Name";
                        mainComboBox.ValueMember = "Id";

                        // Select the newly created category
                        mainComboBox.SelectedItem = newCategory;
                    }
                }
            }
        }
        private async Task<Category> CreateCategoryAsync(string name, int? parentId)
        {
            // Create the new category
            var newCategory = new Category
            {
                Name = name,
                ParentId = parentId,
                UpdatedTime = DateTime.Now,
                CreatedTime = DateTime.Now,
            };
            // Save the new category to the database/service
            newCategory = await categoryService.CreateCategoryAsync(newCategory);

            Log.Information($"New category created: {newCategory.Name} (ID: {newCategory.Id})");

            return newCategory;
        }
        private async void SubComboBox_TextChanged(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ComboBox currentComboBox = sender as ComboBox;

                // Get the user's input
                var inputText = currentComboBox.Text;

                Log.Information(selectedId.ToString() + "NESSSSSLIVI ");
                // Find the parent ID of the current ComboBox
                //int? parentId = currentComboBox.SelectedValue as int?;
                int? parentId = currentParentId;



                if (string.IsNullOrWhiteSpace(inputText))
                    return;

                // Check if the input matches an existing category
                var matchingCategory = categories.FirstOrDefault(x => x.Name.Equals(inputText, StringComparison.OrdinalIgnoreCase));

                if (matchingCategory == null)
                {
                    // Ask the user to confirm the creation of the new subcategory
                    var result = MessageBox.Show(
                        $"The category '{inputText}' does not exist. Would you like to create it?",
                        "Create New Subcategory",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);

                    if (result == DialogResult.Yes)
                    {
                        if (parentId == null || parentId <= 0)
                        {
                            MessageBox.Show("Parent ID is not valid. Please select a valid category first.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        // Create the new subcategory
                        var newCategory = await CreateCategoryAsync(inputText, parentId);

                        // Add the new category to the list and update the ComboBox's data source
                        categories.Add(newCategory);
                        currentComboBox.DataSource = null; // Reset the data source to refresh it
                        currentComboBox.DataSource = categories.Where(x => x.ParentId == currentParentId).ToList(); 
                        currentComboBox.DisplayMember = "Name";
                        currentComboBox.ValueMember = "Id";

                        // Select the newly created category
                        currentComboBox.SelectedItem = newCategory;
                    }
                }
            }
        }
        private void DeleteChildComboBoxes(ComboBox parentComboBox)
        {
            // Get the ComboBoxId from the Tag property
            int parentComboBoxId = (int)parentComboBox.Tag;

            // Find all child ComboBoxes (those with IDs greater than the parent ID)
            var childComboBoxes = comboBoxDictionary
                .Where(kvp => kvp.Key > parentComboBoxId)
                .Select(kvp => kvp.Value)
                .ToList(); // Create a list of ComboBox controls to remove

            // Check if there is more than one child
            if (childComboBoxes.Count > 1)
            {
                // Remove child ComboBoxes from the Form and the dictionary
                foreach (var childComboBox in childComboBoxes)
                {
                    this.Controls.Remove(childComboBox); // Remove from the UI
                    comboBoxDictionary.Remove((int)childComboBox.Tag); // Remove from the dictionary
                }
            }
        }


    }
}

