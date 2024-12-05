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

        private int selectedId = 1;
        private int currentParentId = 0;
        private readonly CategoryService categoryService;



        private List<Category> categories = new List<Category>();

        private int comboBoxCount = 0; // Counter for dynamically created ComboBoxes
        private Dictionary<int, ComboBox> comboBoxDictionary = new Dictionary<int, ComboBox>();

        private List<ComboBox> comboBoxes = new List<ComboBox>();
        //private List<Tuple<ComboBox, ComboBox, int?>> comboBoxRelationships = new List<Tuple<ComboBox, ComboBox, int?>>();

        private List<TextBox> textBoxList = new List<TextBox>();
        private int textBoxCount = 0; // To keep track of TextBox IDs

        ComboBox mainComboBox;
        public AddProductForm(CategoryService categoryService, ProductService productService)
        {
            this.categoryService = categoryService;
            this.productService = productService;

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
            int productId = 0;
            List<Barcode> barcodes = new List<Barcode>();

            bool isRegular;

            // string id;
            //string categoryName;

            isRegular = checkBox1.Checked;

            Name = textBox2.Text;
            //var selectedCategory = mainComboBox.SelectedItem as Category;


            //Log.Information(selectedCategory.Name);
            //Description = textBox2.Text;

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
                return;
            }
            Quantity = quantity;

            foreach (TextBox txtbox in textBoxList)
            {
                if (!int.TryParse(txtbox.Text, out int barcodeValue) && string.IsNullOrWhiteSpace(txtbox.Text) && !currentlyHaveBarcode)
                {
                    MessageBox.Show(
                        "Bütün barkod dəyərləri düzgün formatda olmalıdır.",
                        "Daxiletmə xətası",
                        MessageBoxButtons.OK);
                    return;
                }

                //var barcode = new Barcode
                //{
                //    BarCode = barcodeValue,
                //};
                //barcodes.Add(barcode);
                barcodes.Add(createNewBarcode(barcodeValue));
            }

            if (!string.IsNullOrWhiteSpace(textBox1.Text) && int.TryParse(textBox1.Text, out int mainBarcode))
            {
                foreach (Barcode barcode in barcodes)
                {
                    if (barcode.BarCode == mainBarcode)
                    {
                        MessageBox.Show(
                            "Daxil olunan barkod verilən barkod ilə eyni ola bilməz!",
                            "Daxiletmə xətası",
                            MessageBoxButtons.OK);
                        return;
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
                return;
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
            //  barcodes.Clear();
        }
        private Barcode createNewBarcode(int barcodeValue)
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
                textBox2.Text = currentProduct.ProductName;
                setCurrentBarcodes();
                //textBox2.Enabled = false;

            }
        }
        private void setCurrentBarcodes()
        {
            // Clear existing TextBoxes if any

            // Get the current product barcodes
            List<Barcode> barcodes = currentProduct.Barcodes.ToList();

            if (barcodes == null || !barcodes.Any())
                return;

            // Iterate through the barcodes and create textboxes
            for (int i = 0; i < barcodes.Count; i++)
            {
                // Increment TextBox count
                textBoxCount++;

                // Create a new TextBox
                TextBox newTextBox = new TextBox
                {
                    Name = "TextBox" + textBoxCount,
                    Width = 200,
                    Location = new System.Drawing.Point(286, 200 + (40 * textBoxCount)), // Adjust location dynamically
                    Tag = textBoxCount,
                    Size = new System.Drawing.Size(177, 26),
                    PlaceholderText = "**********",
                    Text = barcodes[i].BarCode.ToString() // Populate the TextBox with the current barcode
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
            currentProduct = await productService.GetProductByBarCode(int.Parse(textBox1.Text));
            if (currentProduct != null)
            {
                textBox2.Text = currentProduct.ProductName;
                textBox2.Enabled = false;
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

            // Create a new TextBox
            TextBox newTextBox = new TextBox
            {
                Name = "TextBox" + textBoxCount,
                Width = 200,
                Location = new System.Drawing.Point(286, 200 + (40 * textBoxCount)), // Adjust location dynamically
                Tag = textBoxCount, // Store the ID as a tag
                Size = new System.Drawing.Size(177, 26),
                PlaceholderText = "**********"
            };

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

        private void LoadTopCategories()
        {
            var topCategories = categoryService.GetTopLevelCategories().ToList();

            if (topCategories.Any())
            {
                selectedId = topCategories.First().Id;
            }

            AddComboBox(null, topCategories);
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
                Width = 150,
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


        private void ComboBox_SelectedIndexChanged(object sender, EventArgs e)
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

            // Always add a new ComboBox, even if no subcategories exist
            AddComboBox(selectedCategory, subCategories);
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

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}

