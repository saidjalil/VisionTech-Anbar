using MetroSet_UI.Forms;
using Microsoft.EntityFrameworkCore.Infrastructure;
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
using Microsoft.EntityFrameworkCore;
using VisionTech_Anbar_Project.DAL;
using VisionTech_Anbar_Project.Entities;
using VisionTech_Anbar_Project.Entities.Categories;
using VisionTech_Anbar_Project.Repositories;
using VisionTech_Anbar_Project.Services;
using static System.Net.Mime.MediaTypeNames;

namespace VisionTech_Anbar_Project
{
    public partial class AddProductForm : MetroSetForm
    {
        private bool IsEdit;
        private PackageProduct OriginalProduct;
        public PackageProduct EditedProduct;
        public PackageProduct NewProduct;
        public bool DataSaved;
        private readonly IDbContextFactory<AppDbContext> _contextFactory;

        private int _nextControlY = 10; // Tracks Y position for next ComboBox and Button
        private readonly List<ComboBox> _comboBoxes = new(); // Keep track of all ComboBoxes

        public Product currentProduct;

        bool currentlyHaveBarcode;

        private readonly ProductService productService;
        private readonly CategoryService categoryService;
        private readonly BarcodeService barcodeService;
        private readonly BrandService brandService;

        public List<PackageProduct> EditedProductList { get; set; }
        public List<PackageProduct> NewProductList { get; set; }

        private List<Panel> rowPanels = new List<Panel>();


        public TextBox barcodeTextBox;

        private bool _isUpdatingComboBox = false;


        private Button lockButton;
        private bool isLocked = false;


        private int selectedId = 1;
        private int currentParentId = 0;

        private int currentProductId = 0;


        private List<Category> categories = new List<Category>();

        private int comboBoxCount = 0; // Counter for dynamically created ComboBoxes
        private Dictionary<int, ComboBox> comboBoxDictionary = new Dictionary<int, ComboBox>();

        private List<ComboBox> comboBoxes = new List<ComboBox>();
        //private List<Tuple<ComboBox, ComboBox, int?>> comboBoxRelationships = new List<Tuple<ComboBox, ComboBox, int?>>();

        private List<TextBox> barcodeTextBoxes = new List<TextBox>();
        private List<TextBox> quantityTextBoxes = new List<TextBox>();
        private List<Button> deleteButtons = new List<Button>();
        private List<Control[]> rows = new List<Control[]>();

        private int textBoxCount = 0; // To keep track of TextBox IDs

        ComboBox mainComboBox;
        public AddProductForm(CategoryService categoryService, ProductService productService, BarcodeService barcodeService, BrandService brandService, IDbContextFactory<AppDbContext> contextFactory)
        {
            this.categoryService = categoryService;
            this.productService = productService;
            this.barcodeService = barcodeService;
            this.brandService = brandService;
            _contextFactory = contextFactory;

            barcodeTextBox = textBox1;
            InitializeComponent();
            InitializeDynamicPanel();
            LoadTopCategories();
            LoadBrands();
            AddDefaultRow();
            //InitializeCategories();
            //if(textBoxCount == 0)
            //CreateTextBox();
            //InitializeMainComboBox();
        }
        public AddProductForm(PackageProduct product, IDbContextFactory<AppDbContext> contextFactory)
        {
            InitializeComponent();
            IsEdit = true;
            OriginalProduct = product;
            _contextFactory = contextFactory;
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
            //textBox3.Text = OriginalProduct.Quantity.ToString();

            categories = (await categoryService.GetAllCategoriesAsync()).ToList();

            mainComboBox.Items.AddRange(categories.Where(x => x.ParentId == null).ToArray());
            //comboBox1.Text = OriginalProduct.Product.Category.Name.ToString();
        }
        private void ClearInput()
        {
            textBox1.Clear();
            comboBox1.ResetText();
            // textBox3.Clear();

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
            string name;
            int quantity;
            bool isRegular;
            Brand newBrand;
            EditedProductList = new List<PackageProduct>();
            NewProductList = new List<PackageProduct>();

            // Safely retrieve inputs
            isRegular = checkBox1.Checked;

            // Get name input from comboBox1
            name = comboBox1.Text?.Trim();
            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("Məhsulun adı qeyd olunmayıb.", "Daxiletmə xətası", MessageBoxButtons.OK);
                return false;
            }

            // Safely get brand input (typed or selected)
            var brandInput = comboBox2?.Text?.Trim();
            if (string.IsNullOrWhiteSpace(brandInput))
            {
                MessageBox.Show("Brend adı qeyd olunmayıb.", "Daxiletmə xətası", MessageBoxButtons.OK);
                return false;
            }
            newBrand = CreateNewBrand(brandInput);

            // Process barcodes and quantities from the lists
            for (int i = 0; i < (barcodeTextBoxes.Count > 0 ? barcodeTextBoxes.Count : 1); i++)
            {
                string barcodeInput;

                if (barcodeTextBoxes.Count > 0)
                {
                    // Retrieve barcode
                    barcodeInput = barcodeTextBoxes[i].Text?.Trim();
                    if (string.IsNullOrWhiteSpace(barcodeInput))
                    {
                        MessageBox.Show("Bütün barkod dəyərləri düzgün formatda olmalıdır.", "Daxiletmə xətası", MessageBoxButtons.OK);
                        return false;
                    }
                }
                else
                {
                    // Default barcode value if no textboxes are present
                    barcodeInput = "Barkodsuz"; // Replace with a suitable default value
                }

                if (quantityTextBoxes.Count > 0 && i < quantityTextBoxes.Count)
                {
                    // Retrieve quantity
                    if (string.IsNullOrWhiteSpace(quantityTextBoxes[i].Text) ||
                        !int.TryParse(quantityTextBoxes[i].Text, out quantity))
                    {
                        MessageBox.Show($"Məhsulun miqdarı düzgün qeyd olunmayıb. Sətir: {i + 1}", "Daxiletmə xətası", MessageBoxButtons.OK);
                        return false;
                    }
                }
                else
                {
                    // Default quantity if no quantity textboxes are present
                    quantity = 1; // Replace with a suitable default quantity
                }

                // Create a new PackageProduct for each barcode and quantity
                var packageProduct = new PackageProduct(currentProductId, name, quantity, selectedId, barcodeInput, isRegular, newBrand);

                // Add to appropriate list
                if (currentProduct != null)
                {
                    EditedProductList.Add(packageProduct);
                }
                else
                {
                    NewProductList.Add(packageProduct);
                }
            }


            // Validate primary barcode
            if (!string.IsNullOrWhiteSpace(textBox1.Text))
            {
                foreach (var barcode in barcodeTextBoxes.Select(txtbox => txtbox.Text?.Trim()))
                {
                    if (barcode == textBox1.Text)
                    {
                        MessageBox.Show("Daxil olunan barkod verilən barkod ilə eyni ola bilməz!", "Daxiletmə xətası", MessageBoxButtons.OK);
                        return false;
                    }
                }
            }

            currentlyHaveBarcode = false;
            return true;
        }


        private string CreateNewBarcode(string barcodeValue)
        {
            return barcodeValue?.Trim();
        }

        private Brand CreateNewBrand(string brandName)
        {
            return new Brand { BrandName = brandName?.Trim() };
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
        private void AddLockButtonToControl(Control parentControl)
        {
            lockButton = new Button
            {
                Text = "🔒", // Initially locked
                Font = new Font("Segoe UI Emoji", 14),
                Size = new Size(60, 60),
                BackColor = Color.Transparent,
                FlatStyle = FlatStyle.Flat,
                TabStop = false
            };

            // Invisible border
            lockButton.FlatAppearance.BorderSize = 0;

            // Position the button
            lockButton.Location = new Point(
                metroSetControlBox1.Location.X + metroSetControlBox1.Width - lockButton.Width - 5,
                metroSetControlBox1.Location.Y + metroSetControlBox1.Height + 5
            );

            // Handle resizing
            parentControl.Resize += (s, e) =>
            {
                lockButton.Location = new Point(
                    metroSetControlBox1.Location.X + metroSetControlBox1.Width - lockButton.Width - 5,
                    metroSetControlBox1.Location.Y + metroSetControlBox1.Height + 5
                );
            };

            // Add click logic
            lockButton.Click += (s, e) => ToggleLockState();

            // Add button to the parent
            parentControl.Controls.Add(lockButton);

            // Manually trigger the lock state after adding the button
            ToggleLockState();
        }

        private void ToggleLockState()
        {
            isLocked = lockButton.Text == "🔒";

            // Update controls
            textBox1.Enabled = !isLocked;
            comboBox1.Enabled = !isLocked;
            button2.Enabled = !isLocked;

            foreach (ComboBox comboBox in _comboBoxes)
            {
                comboBox.Enabled = !isLocked;
            }

            // Update button text
            lockButton.Text = isLocked ? "🔓" : "🔒";
        }

        public async void setCurrentProduct()
        {
            if (currentProduct != null)
            {
                SetCurrentCategories();
                AddLockButtonToControl(this);
                comboBox1.Text = currentProduct.ProductName;
                //comboBox2.SelectedIndex = currentProduct.BrandId;
                comboBox2.Text = currentProduct.Brand.BrandName;
                setCurrentBarcodes();
                currentProductId = currentProduct.Id;

                textBox1.Visible = false;
                button2.Visible = false;
                label2.Visible = false;
            }
        }

        private async Task SetCurrentCategories()
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
                    Tag = null, // Top-level categories don't have a parent
                    Enabled = false // Disable by default
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
                    Tag = parentCategory, // Store the parent category
                    Enabled = false // Disable by default
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
            foreach (var textBox in barcodeTextBoxes)
            {
                this.Controls.Remove(textBox);
            }
            barcodeTextBoxes.Clear();

            // Get the current product barcodes
            List<string> barcodes = currentProduct.PackageProducts.Where(x => x.ProductId == currentProduct.Id).Select(x => x.Barcode).ToList();

            if (barcodes == null || !barcodes.Any())
                return;

            // Reset the count and position logic
            textBoxCount = 0;

            for (int i = 0; i < barcodes.Count; i++)
            {
                // Increment TextBox count
                textBoxCount++;

                AddRow();
            }
        }
        // Helper method to clear existing TextBoxes

        //private async void SetCurrentBrands()
        //{
        //    if (currentProduct == null) return;

        //    // Get the brand hierarchy for the current product
        //    //var brandHierarchy = await GetBrandHierarchyAsync(currentProduct.BrandId);

        //    // Reset the ComboBox position and clear previous selections
        //    if (comboBox2 == null) return; // Ensure ComboBox exists

        //    _isUpdatingComboBox = true; // Prevent triggering SelectedIndexChanged event

        //    // Load all brands at the top level
        //    var allBrands = (await brandService.GetAllBrandsAsync()).ToList();

        //    // Bind the brands to the existing ComboBox
        //    comboBox2.DataSource = null; // Clear old data
        //    comboBox2.DataSource = allBrands;
        //    comboBox2.DisplayMember = "BrandName"; // Property for displaying names
        //    comboBox2.ValueMember = "Id";

        //    // Select the correct brand based on hierarchy
        //    //if (brandHierarchy.Any())
        //    //{
        //    //    var selectedBrand = brandHierarchy.FirstOrDefault();
        //    //    comboBox2.SelectedItem = allBrands.FirstOrDefault(b => b.Id == selectedBrand.Id);
        //    //}

        //    _isUpdatingComboBox = false;

        //    // Load products for the selected brand
        //    await LoadProductsByBrand((int)comboBox2.SelectedValue);
        //}
        //private async Task<List<Brand>> GetBrandHierarchyAsync(int brandId)
        //{
        //    var hierarchy = new List<Brand>();

        //    var currentBrand = await brandService.GetBrandByIdAsync(brandId);
        //    while (currentBrand != null)
        //    {
        //        hierarchy.Insert(0, currentBrand); // Build hierarchy from top to bottom
        //        currentBrand = await brandService.GetParentBrandAsync(currentBrand.ParentId);
        //    }

        //    return hierarchy;
        //}
        private async void button2_Click(object sender, EventArgs e)
        {

            if (textBox1.Text.Equals(""))
            {
                return;
            }
            currentProduct = await productService.GetProductByBarCode(textBox1.Text);
            if (currentProduct == null)
            {
                MessageBox.Show($"Bu barkoda uyğun məhsul tapılmadı", "Daxiletmə xətası", MessageBoxButtons.OK);
            }
            else
            {
                comboBox1.Text = currentProduct.ProductName;
                comboBox1.Enabled = false;
                currentlyHaveBarcode = true;
                SetCurrentCategories();
                //setCurrentBarcodes();
            }
            // combobox1 category add
        }

        private void button3_Click(object sender, EventArgs e)
        {
            AddRow();
        }
        // Declare necessary lists to hold dynamically created controls

        private void AddDefaultRow()
        {
            // Determine base positioning
            int startX = button3.Location.X + button3.Width + 10;
            int startY = button3.Location.Y + rowPanels.Count * (30 + 10); // Vertical padding

            // Create a panel to encapsulate the row
            Panel rowPanel = new Panel
            {
                Location = new Point(startX, startY),
                Size = new Size(400, 30), // Adjust size as needed
                Tag = rowPanels.Count, // Store row index in the Tag
                BackColor = Color.Transparent

            };

            // Create a TextBox for Barcode (disabled with default text)
            TextBox barcodeTextBox = new TextBox
            {
                Name = "DefaultBarcodeTextBox",
                Location = new Point(0, 0),
                Size = new Size(150, 30),
                Text = "Barkodsuz", // Default text
                Enabled = false // Disable editing
            };
            barcodeTextBoxes.Add(barcodeTextBox); // Add to the list
            rowPanel.Controls.Add(barcodeTextBox);

            // Create a TextBox for Quantity
            TextBox quantityTextBox = new TextBox
            {
                Name = "DefaultQuantityTextBox",
                Location = new Point(160, 0), // Positioned to the right of Barcode
                Size = new Size(100, 30),
                PlaceholderText = "Quantity"
            };
            quantityTextBoxes.Add(quantityTextBox); // Add to the list
            rowPanel.Controls.Add(quantityTextBox);

            // Add the panel to the form and list
            rowPanels.Add(rowPanel);
            this.Controls.Add(rowPanel);
        }

        private void AddRow()
        {
            // Determine base positioning
            int startX = button3.Location.X + button3.Width + 10;
            int startY = button3.Location.Y + rowPanels.Count * (30 + 10); // Vertical padding

            // Create a panel to encapsulate the row
            Panel rowPanel = new Panel
            {
                Location = new Point(startX, startY),
                Size = new Size(400, 30), // Adjust size as needed
                Tag = rowPanels.Count, // Store row index in the Tag
                BackColor = Color.Transparent
            };

            // Create a TextBox for Barcode
            TextBox barcodeTextBox = new TextBox
            {
                Name = "BarcodeTextBox" + (rowPanels.Count + 1),
                Location = new Point(0, 0),
                Size = new Size(150, 30),
                PlaceholderText = "Barcode"
            };
            barcodeTextBoxes.Add(barcodeTextBox); // Add to the list
            rowPanel.Controls.Add(barcodeTextBox);

            // Create a TextBox for Quantity
            TextBox quantityTextBox = new TextBox
            {
                Name = "QuantityTextBox" + (rowPanels.Count + 1),
                Location = new Point(160, 0), // Positioned to the right of Barcode
                Size = new Size(100, 30),
                PlaceholderText = "Quantity"
            };
            quantityTextBoxes.Add(quantityTextBox); // Add to the list
            rowPanel.Controls.Add(quantityTextBox);

            // Create a Button for Deletion
            Button deleteButton = new Button
            {
                Name = "DeleteButton" + (rowPanels.Count + 1),
                Location = new Point(270, 0), // Positioned to the right of Quantity
                Size = new Size(40, 30),
                Text = "🗑",
                BackColor = Color.Red,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Tag = rowPanels.Count // Store row index in the Tag
            };
            deleteButton.Click += DeleteRow;
            rowPanel.Controls.Add(deleteButton);

            // Add the panel to the form and list
            rowPanels.Add(rowPanel);
            this.Controls.Add(rowPanel);
        }

        private void DeleteRow(object sender, EventArgs e)
        {
            if (sender is Button deleteButton)
            {
                int rowIndex = (int)deleteButton.Tag; // Get the row index from the button's Tag

                // Remove controls from the form
                Panel rowPanel = rowPanels[rowIndex];
                this.Controls.Remove(rowPanel);
                rowPanels.RemoveAt(rowIndex);

                // Remove associated TextBoxes from their lists
                barcodeTextBoxes.RemoveAt(rowIndex);
                quantityTextBoxes.RemoveAt(rowIndex);

                // Update positions and tags of remaining rows
                UpdateRowPositions();
            }
        }

        private void UpdateRowPositions()
        {
            int startX = button3.Location.X + button3.Width + 10;
            int startY = button3.Location.Y;
            int verticalPadding = 10;

            for (int i = 0; i < rowPanels.Count; i++)
            {
                // Update panel positions
                rowPanels[i].Location = new Point(startX, startY + i * (30 + verticalPadding));
                rowPanels[i].Tag = i;

                // Update the Tag of the delete button inside the panel
                Button deleteButton = rowPanels[i].Controls.OfType<Button>().FirstOrDefault();
                if (deleteButton != null)
                {
                    deleteButton.Tag = i;
                }
            }
        }

        private void QuantityTextBoxKeyPress(object sender, KeyPressEventArgs e)
        {
            // Allow only numeric input
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
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

                Controls.Remove(barcodeTextBoxes[textBoxCount - 1]);
                barcodeTextBoxes.RemoveAt(textBoxCount - 1);
                textBoxCount--;

            }
        }
        private void InitializeDynamicPanel()
        {
            panelDynamic.AutoScroll = true;
        }
        private async void LoadTopCategories()
        {
            // Get top-level categories and check if any exist
            var topCategories = categoryService.GetTopLevelCategories().ToList();

            if (!topCategories.Any())
            {
                MessageBox.Show("No top-level categories found.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return; // Exit the method if no categories exist
            }

            // Add ComboBox with the top categories
            AddComboBox(null, topCategories);
        }
        private async Task LoadBrands()
        {
            try
            {
                _isUpdatingComboBox = true;
                var brands = (await brandService.GetAllBrandsAsync()).ToList();
            
                if (!brands.Any())
                {
                    MessageBox.Show("No brands found.", "Information", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                comboBox2.DataSource = null; // Clear existing data source
                comboBox2.DataSource = brands;
                comboBox2.DisplayMember = "BrandName";
                comboBox2.ValueMember = "Id";

                selectedId = brands.First().Id;
                await LoadProductsByBrand(selectedId);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error loading brands");
                MessageBox.Show("Error loading brands. Please try again.", 
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                _isUpdatingComboBox = false;
            }

        }

        private async Task LoadProductsByBrand(int brandId)
        {
            try
            {
                var products = await productService.GetProductByBrandId(brandId);
            
                if (!products.Any())
                {
                    MessageBox.Show("No products found for the selected brand.", 
                        "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                SetProducts(products.ToList());
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error loading products for brand {BrandId}", brandId);
                MessageBox.Show("Error loading products. Please try again.", 
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void ComboBoxBrands_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_isUpdatingComboBox) return; // Prevent redundant calls during programmatic updates

            var comboBox = sender as ComboBox;
            if (comboBox == null) return;

            var selectedBrand = comboBox.SelectedItem as Brand;
            if (selectedBrand == null) return;

            // Update the selected ID to the current brand
            selectedId = selectedBrand.Id;

            // Load products based on the selected brand
            await LoadProductsByBrand(selectedId);
        }


        private async void ComboBoxBrands_KeyDown(object sender, KeyEventArgs e)
        {
            var comboBox = sender as ComboBox;
            if (comboBox == null) return;

            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;

                var inputText = comboBox.Text?.Trim();
                if (string.IsNullOrWhiteSpace(inputText)) return;

                // Check for duplicates
                if (comboBox.Items.Cast<Brand>().Any(b => b.BrandName.Equals(inputText, StringComparison.OrdinalIgnoreCase)))
                {
                    MessageBox.Show("This brand already exists!", "Duplicate Entry", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // Add new brand to the database
                var newBrand = new Brand { BrandName = inputText };
                await brandService.CreateBrandAsync(newBrand);

                // Refresh ComboBox data
                _isUpdatingComboBox = true;

                var brands = (await brandService.GetAllBrandsAsync()).ToList();
                comboBox.DataSource = null;
                comboBox.DataSource = brands;
                comboBox.DisplayMember = "BrandName";
                comboBox.ValueMember = "Id";

                // Select the newly added brand
                comboBox.SelectedItem = brands.First(b => b.Id == newBrand.Id);

                _isUpdatingComboBox = false;

                // Load products for the new brand
                await LoadProductsByBrand(newBrand.Id);
            }
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
            if (isLocked == true)
                comboBox.Enabled = false;


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

            comboBox.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            comboBox.AutoCompleteSource = AutoCompleteSource.ListItems;
            // Update Y position for the next control
            _nextControlY += comboBox.Height;

            // Keep track of ComboBoxes
            _comboBoxes.Add(comboBox);
        }
        private async void ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_isUpdatingComboBox) return; // Skip if updating programmatically

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
            // var products = (await productService.GetProductsByCategoryAsync(selectedCategory.Id)).ToList();

            // Always add a new ComboBox, even if no subcategories exist
            AddComboBox(selectedCategory, subCategories);
            //SetProducts(products);
        }


        private async void ComboBox_KeyDown(object sender, KeyEventArgs e)
        {
            var comboBox = sender as ComboBox;
            if (comboBox == null) return;

            // Check if the Enter key was pressed
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true; // Suppress default behavior
                e.SuppressKeyPress = true; // Suppress "ding" sound

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

                // Prevent triggering SelectedIndexChanged during DataSource update
                _isUpdatingComboBox = true;

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

                // Re-enable SelectedIndexChanged event
                _isUpdatingComboBox = false;
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

        private void metroSetControlBox1_Click(object sender, EventArgs e)
        {

        }

    }
}

