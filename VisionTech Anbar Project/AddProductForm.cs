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
using VisionTech_Anbar_Project.Repositories;
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

        private int _nextControlY = 10; // Tracks Y position for next ComboBox and Button
        private readonly List<ComboBox> _comboBoxes = new(); // Keep track of all ComboBoxes

        public Product currentProduct;

        bool currentlyHaveBarcode;

        private readonly ProductService productService;
        private readonly CategoryService categoryService;
        private readonly BarcodeService barcodeService;

        public TextBox barcodeTextBox;


        private int selectedId = 1;
        private int currentParentId = 0;



        private List<Category> categories = new List<Category>();

        private int comboBoxCount = 0; // Counter for dynamically created ComboBoxes
        private Dictionary<int, ComboBox> comboBoxDictionary = new Dictionary<int, ComboBox>();

        private List<ComboBox> comboBoxes = new List<ComboBox>();
        //private List<Tuple<ComboBox, ComboBox, int?>> comboBoxRelationships = new List<Tuple<ComboBox, ComboBox, int?>>();

        private List<TextBox> textBoxList = new List<TextBox>();
        private int textBoxCount = 0; // To keep track of TextBox IDs

        ComboBox mainComboBox;
        public AddProductForm(CategoryService categoryService, ProductService productService, BarcodeService barcodeService)
        {
            this.categoryService = categoryService;
            this.productService = productService;
            this.barcodeService = barcodeService;


            barcodeTextBox = textBox1;
            InitializeComponent();
            InitializeDynamicPanel();
            LoadTopCategories();
            //InitializeCategories();
            //if(textBoxCount == 0)
            //CreateTextBox();
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
            comboBox1.Text = OriginalProduct.Product.ProductName.ToString();
            textBox3.Text = OriginalProduct.Quantity.ToString();

            categories = (await categoryService.GetAllCategoriesAsync()).ToList();

            mainComboBox.Items.AddRange(categories.Where(x => x.ParentId == null).ToArray());
            //comboBox1.Text = OriginalProduct.Product.Category.Name.ToString();
        }
        private void ClearInput()
        {
            textBox1.Clear();
            comboBox1.ResetText();
            textBox3.Clear();

        }
        private async void button1_Click(object sender, EventArgs e)
        {
            List<String> errors;
            errors = ValidateInput();

            if (errors.Count > 0)
            {
                ShowErrors(errors, 5);
                return;
            }


            DataSaved = true;
            if (await StoreInput())
            {
                this.Close();
            }
        }
        private List<string> ValidateInput()
        {
            List<String> errors = new List<string>();

            if (string.IsNullOrWhiteSpace(comboBox1.Text))
                errors.Add("Product Adı vacibdir");

            return errors;
        }
        private async Task<bool> StoreInput()
        {
            string Name;
            // string Description;
            int Quantity;
            int productId = 0;
            List<Barcode> barcodes = new List<Barcode>();

            bool isRegular;

            // string id;
            //string categoryName;

            isRegular = checkBox1.Checked;

            Name = comboBox1.Text;
            //var selectedCategory = mainComboBox.SelectedItem as Category;


            //Log.Information(selectedCategory.Name);
            //Description = comboBox1.Text;

            //if(!int.TryParse(textBox1.Text, out int currentbarcodeValue) && !string.IsNullOrWhiteSpace(textBox1.Text))
            //{
            //    barcodes.Add(createNewBarcode(currentbarcodeValue));
            //}
            if (string.IsNullOrWhiteSpace(textBox3.Text) || !int.TryParse(textBox3.Text, out int quantity))
            {
                MessageBox.Show(
                    "Məhsulun miqdarı qeyd olunmayıb və ya düzgün formatda deyil.",
                    "Daxiletmə xətası",
                    MessageBoxButtons.OK);
                return false;
            }
            Quantity = quantity;

            foreach (TextBox txtbox in textBoxList)
            {
                if (string.IsNullOrWhiteSpace(txtbox.Text) && !currentlyHaveBarcode)
                {
                    MessageBox.Show(
                        "Bütün barkod dəyərləri düzgün formatda olmalıdır.",
                        "Daxiletmə xətası",
                        MessageBoxButtons.OK);
                    return false;
                }

                //var barcode = new Barcode
                //{
                //    BarCode = barcodeValue,
                //};
                //barcodes.Add(barcode);
                barcodes.Add(createNewBarcode(txtbox.Text));
            }

            if (!string.IsNullOrWhiteSpace(textBox1.Text))
            {
                foreach (Barcode barcode in barcodes)
                {
                    if (barcode.BarCode == textBox1.Text)
                    {
                        MessageBox.Show(
                            "Daxil olunan barkod verilən barkod ilə eyni ola bilməz!",
                            "Daxiletmə xətası",
                            MessageBoxButtons.OK);
                        return false;
                    }
                    //productId = barcode.ProductId;
                }
            }
            else if (!string.IsNullOrWhiteSpace(textBox1.Text))
            {
                MessageBox.Show(
                    "Əsas barkod düzgün formatda deyil.",
                    "Daxiletmə xətası",
                    MessageBoxButtons.OK);
                return false;
            }

            List<Barcode> currentList = new List<Barcode>();
            currentList = (await barcodeService.CheckBarcodes(barcodes)).ToList();
            if (currentList.Count > 0)
            {
                string mes = string.Empty;

                foreach (Barcode barcode in currentList)
                {
                    mes += $"{barcode.BarCode},";
                }

                MessageBox.Show(
                           $"Bu barkodlar artıq əlavə edilib:{mes} ",
                           "Daxiletmə xətası",
                           MessageBoxButtons.OK);
                return false;
            }

            if (currentProduct != null)
            {
                EditedProduct = new PackageProduct(currentProduct.Id, Name,
                                         Quantity, selectedId, barcodes, isRegular);
            }
            else
            {
                //id = Guid.NewGuid().ToString();
                NewProduct = new PackageProduct(productId, Name,
                                         Quantity, selectedId, barcodes, isRegular);
            }
            currentlyHaveBarcode = false;
            return true;
            //  barcodes.Clear();
        }
        private Barcode createNewBarcode(string barcodeValue)
        {
            var barcode = new Barcode
            {
                BarCode = barcodeValue,
            };
            return barcode;
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

        //private async void button5_Click(object sender, EventArgs e)
        //{
        //   // await categoryService.CreateCategoryAsync(new Category { CreatedTime = DateTime.Now, Name = "Kulek", UpdatedTime = DateTime.Now });

        //    //await categoryService.CreateCategoryAsync(new Category { CreatedTime = DateTime.Now, Name = "Ermenistan", UpdatedTime = DateTime.Now, ParentId =14});
        //}
        public async void setCurrentProduct()
        {
            // write the products current data
            if (currentProduct != null)
            {
                SetCurrentCategories();
                comboBox1.Text = currentProduct.ProductName;
                setCurrentBarcodes();
                //comboBox1.Enabled = false;
                textBox1.Enabled = false;
                button2.Enabled = false;
            }
        }
        private async void SetCurrentCategories()
        {
            if (currentProduct == null) return;

            // Get the category hierarchy for the current product
            var categoryHierarchy = await GetCategoryHierarchyAsync(currentProduct.CategoryId);

            // Clear existing ComboBoxes
            foreach (var comboBox in _comboBoxes)
            {
                panelDynamic.Controls.Remove(comboBox);
            }
            _comboBoxes.Clear();
            _nextControlY = 10; // Reset the Y position for new ComboBoxes

            Category parentCategory = null;

            // Populate the first ComboBox with top-level categories (ParentId = null)
            var firstComboBoxCategories = categoryService.GetTopLevelCategories().ToList(); // Root categories
            if (firstComboBoxCategories.Any())
            {
                var firstComboBox = new ComboBox
                {
                    DataSource = firstComboBoxCategories,
                    DisplayMember = "Name",
                    ValueMember = "Id",
                    Location = new Point(10, _nextControlY),
                    Width = 200,
                    DropDownStyle = ComboBoxStyle.DropDown, // Allow user to type
                    Tag = null // Top-level categories don't have a parent
                };

                // Select the category that matches the hierarchy if applicable
                if (categoryHierarchy.Any())
                {
                    var rootCategory = categoryHierarchy.First();
                    firstComboBox.SelectedItem = firstComboBoxCategories.FirstOrDefault(c => c.Id == rootCategory.Id);
                }

                firstComboBox.SelectedIndexChanged += ComboBox_SelectedIndexChanged;
                firstComboBox.KeyDown += ComboBox_KeyDown;

                panelDynamic.Controls.Add(firstComboBox);
                _comboBoxes.Add(firstComboBox);

                _nextControlY += firstComboBox.Height + 5;

                parentCategory = firstComboBox.SelectedItem as Category;
            }

            // Add ComboBoxes for each subsequent category in the hierarchy
            foreach (var category in categoryHierarchy.Skip(1))
            {
                var subCategories = categoryService.GetSubCategories(parentCategory?.Id ?? 0).ToList();

                var comboBox = new ComboBox
                {
                    DataSource = subCategories,
                    DisplayMember = "Name",
                    ValueMember = "Id",
                    Location = new Point(10, _nextControlY),
                    Width = 200,
                    DropDownStyle = ComboBoxStyle.DropDown, // Allow user to type
                    Tag = parentCategory // Store the parent category
                };

                if (subCategories.Any(c => c.Id == category.Id))
                {
                    comboBox.SelectedItem = subCategories.First(c => c.Id == category.Id);
                }

                comboBox.SelectedIndexChanged += ComboBox_SelectedIndexChanged;
                comboBox.KeyDown += ComboBox_KeyDown;

                panelDynamic.Controls.Add(comboBox);
                _comboBoxes.Add(comboBox);

                _nextControlY += comboBox.Height + 5;

                parentCategory = category;
            }

            // Ensure the last ComboBox selects the correct subcategory
            var lastComboBox = _comboBoxes.LastOrDefault();
            if (lastComboBox != null && lastComboBox.DataSource is List<Category> lastCategories)
            {
                var lastCategory = categoryHierarchy.LastOrDefault();
                if (lastCategory != null)
                {
                    // Select the correct subcategory in the last ComboBox
                    lastComboBox.SelectedItem = lastCategories.FirstOrDefault(c => c.Id == lastCategory.Id);

                    // Trigger the event programmatically to simulate user interaction
                    ComboBox_SelectedIndexChanged(lastComboBox, EventArgs.Empty);
                }
            }
        }



        /// <summary>
        /// Retrieves the category hierarchy for a given category ID.
        /// </summary>
        private async Task<List<Category>> GetCategoryHierarchyAsync(int categoryId)
        {
            var hierarchy = new List<Category>();
            while (categoryId != 0)
            {
                // Await the result of the asynchronous call
                var category = await categoryService.GetCategoryByIdAsync(categoryId);
                if (category == null) break;

                hierarchy.Insert(0, category); // Add to the beginning to maintain the hierarchy order
                categoryId = category.ParentId ?? 0; // Move to the parent category
            }
            return hierarchy;
        }

        private void setCurrentBarcodes()
        {
            // Clear existing TextBoxes related to barcodes from the form and the list
            foreach (var textBox in textBoxList)
            {
                this.Controls.Remove(textBox);
            }
            textBoxList.Clear();

            // Get the current product barcodes
            List<Barcode> barcodes = currentProduct.Barcodes.ToList();

            if (barcodes == null || !barcodes.Any())
                return;

            // Reset the count and position logic
            textBoxCount = 0;

            for (int i = 0; i < barcodes.Count; i++)
            {
                // Increment TextBox count
                textBoxCount++;

                // Calculate position relative to the button (same logic as CreateTextBox)
                var buttonPosition = button3.Location;
                var buttonSize = button3.Size;

                // Calculate the X position (to the right of the button)
                int xOffset = buttonPosition.X + buttonSize.Width + 10; // 10 pixels padding to the right of the button

                // Calculate the Y position based on the count of existing TextBoxes
                int yOffset = buttonPosition.Y + (textBoxCount - 1) * (30 + 10); // 30 is TextBox height, 5 is vertical padding

                // Create a new TextBox
                TextBox newTextBox = new TextBox
                {
                    Name = "TextBox" + textBoxCount,
                    Width = 200,
                    Location = new System.Drawing.Point(xOffset, yOffset), // Adjusted position
                    Tag = textBoxCount,
                    Size = new System.Drawing.Size(177, 26),
                    PlaceholderText = "**********",
                    Text = barcodes[i].BarCode.ToString() // Populate with the barcode
                };

                newTextBox.KeyPress += NewTextBoxKeyPress;

                // Add the new TextBox to the list
                textBoxList.Add(newTextBox);

                // Add the new TextBox to the form
                this.Controls.Add(newTextBox);
            }
        }


        // Helper method to clear existing TextBoxes


        private async void button2_Click(object sender, EventArgs e)
        {

            if (textBox1.Text.Equals(""))
            {
                return;
            }
            currentProduct = await productService.GetProductByBarCode(textBox1.Text);
            if (currentProduct != null)
            {
                comboBox1.Text = currentProduct.ProductName;
                comboBox1.Enabled = false;
                currentlyHaveBarcode = true;
                //setCurrentBarcodes();
            }
            // combobox1 category add
        }



        private void button3_Click(object sender, EventArgs e)
        {
            CreateTextBox();
        }

        private void CreateTextBox()
        {
            // Increment TextBox count to use as an identifier
            textBoxCount++;

            // Get the position and size of the button
            var buttonPosition = button3.Location;
            var buttonSize = button3.Size;

            // Calculate the X position (to the right of the button)
            int xOffset = buttonPosition.X + buttonSize.Width + 10; // 10 pixels padding to the right of the button

            // Calculate the Y position based on the count of existing TextBoxes
            int yOffset = buttonPosition.Y + (textBoxCount - 1) * (30 + 10); // 30 is TextBox height, 5 is vertical padding

            // Create a new TextBox
            TextBox newTextBox = new TextBox
            {
                Name = "TextBox" + textBoxCount,
                Width = 200,
                Location = new System.Drawing.Point(
                    xOffset, // Position it to the right of the button
                    yOffset  // Stack vertically with padding
                ),
                Size = new System.Drawing.Size(177, 30), // Height is 30, adjust if needed
                PlaceholderText = "**********",
                Tag = textBoxCount // Store the ID as a tag
            };

            // Attach an event handler for KeyPress
            newTextBox.KeyPress += NewTextBoxKeyPress;

            // Add the new TextBox to the list
            textBoxList.Add(newTextBox);

            // Add the new TextBox to the form
            this.Controls.Add(newTextBox);
        }




        private void NewTextBoxKeyPress(object sender, KeyPressEventArgs e)
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

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
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

        private void button4_Click(object sender, EventArgs e)
        {
            if (textBoxCount > 0)
            {

                Controls.Remove(textBoxList[textBoxCount - 1]);
                textBoxList.RemoveAt(textBoxCount - 1);
                textBoxCount--;

            }

        }
        private void InitializeDynamicPanel()
        {
            panelDynamic.AutoScroll = true;
        }

        private async void LoadTopCategories()
        {
            var topCategories = categoryService.GetTopLevelCategories().ToList();
            var products = (await productService.GetProductsByCategoryAsync(topCategories[0].Id)).ToList();

            if (topCategories.Any())
            {
                selectedId = topCategories.First().Id;
            }

            AddComboBox(null, topCategories);
            SetProducts(products);
        }

        private void AddComboBox(Category parentCategory, List<Category> categories)
        {
            // Ensure the initial margin is added only once
            if (_comboBoxes.Count == 0)
            {
                _nextControlY = 10; // Start with a 10px margin from the top
            }
            else
            {
                // Add spacing between controls before adding a new ComboBox
                _nextControlY += 10;
            }

            // Create a new ComboBox
            var comboBox = new ComboBox
            {
                DataSource = categories,
                DisplayMember = "Name",
                ValueMember = "Id",
                Location = new Point(10, _nextControlY),
                Width = 200,
                DropDownStyle = ComboBoxStyle.DropDown, // Allow user to type
                Tag = parentCategory // Store the parent category
            };


            // Set selectedId to the first category's ID if the ComboBox has categories
            if (categories.Any())
            {
                // HAS AN Exception
                //  comboBox.SelectedIndex = 0; // Select the first item
                selectedId = categories.First().Id;
            }

            // ComboBox SelectionChanged Event
            comboBox.SelectedIndexChanged += ComboBox_SelectedIndexChanged;

            // ComboBox KeyDown Event for custom input
            comboBox.KeyDown += ComboBox_KeyDown;

            // Add ComboBox to the panel
            panelDynamic.Controls.Add(comboBox);

            // Update Y position for the next control
            _nextControlY += comboBox.Height;

            // Keep track of ComboBoxes
            _comboBoxes.Add(comboBox);
        }


        private async void ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var comboBox = sender as ComboBox;
            if (comboBox == null) return;

            var selectedCategory = comboBox.SelectedItem as Category;
            if (selectedCategory == null) return;

            // Selected currently category id
            selectedId = selectedCategory.Id;

            // Find the index of the current ComboBox
            var index = _comboBoxes.IndexOf(comboBox);

            // Remove all ComboBoxes that are below the current one
            for (int i = _comboBoxes.Count - 1; i > index; i--)
            {
                panelDynamic.Controls.Remove(_comboBoxes[i]);
                _comboBoxes.RemoveAt(i);
            }

            // Dynamically recalculate the Y position for the next control
            _nextControlY = (index + 1) * (comboBox.Height + 5);

            // Load subcategories of the selected category
            var subCategories = categoryService.GetSubCategories(selectedCategory.Id).ToList();
            var products = (await productService.GetProductsByCategoryAsync(selectedCategory.Id)).ToList();

            // Always add a new ComboBox, even if no subcategories exist
            AddComboBox(selectedCategory, subCategories);
            SetProducts(products);
        }

        private async void ComboBox_KeyDown(object sender, KeyEventArgs e)
        {
            var comboBox = sender as ComboBox;
            if (comboBox == null) return;

            // Check if the Enter key was pressed
            if (e.KeyCode == Keys.Enter)
            {
                var inputText = comboBox.Text.Trim();
                if (string.IsNullOrWhiteSpace(inputText)) return;

                // Get the parent category from the ComboBox tag
                var parentCategory = comboBox.Tag as Category;

                // Check if the input already exists in the ComboBox
                if (comboBox.Items.Cast<Category>().Any(c => c.Name.Equals(inputText, StringComparison.OrdinalIgnoreCase)))
                {
                    MessageBox.Show("This category already exists!", "Duplicate Entry", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // Add new category to the database
                var newCategory = new Category
                {
                    Name = inputText,
                    ParentId = parentCategory?.Id
                };
                await categoryService.CreateCategoryAsync(newCategory);

                // Refresh ComboBox data
                var categories = parentCategory == null
                    ? categoryService.GetTopLevelCategories()
                    : categoryService.GetSubCategories(parentCategory.Id);

                comboBox.DataSource = null;
                comboBox.DataSource = categories;
                comboBox.DisplayMember = "Name";
                comboBox.ValueMember = "Id";

                // Select the newly added item
                comboBox.SelectedItem = categories.First(c => c.Id == newCategory.Id);

                // Suppress the ding sound
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        private void SetProducts(List<Product> products)
        {
            if (products.Count != 0)
            {
                comboBox1.DataSource = products;
                comboBox1.DisplayMember = "ProductName"; // Display the 'Name' in the ComboBox
                comboBox1.ValueMember = "ProductName";
            }
            else
            {
                comboBox1.ResetText();
                comboBox1.DataSource = null;

            }
            //if(currentProduct != null)
            //{
            //    comboBox1.Text = currentProduct.ProductName;
            //}
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void panelDynamic_Paint(object sender, PaintEventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}

