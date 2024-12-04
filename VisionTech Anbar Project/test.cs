using VisionTech_Anbar_Project.Entities.Categories;
using VisionTech_Anbar_Project.Repositories;
using VisionTech_Anbar_Project.Utilts;

namespace VisionTech_Anbar_Project;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

public partial class test : Form
{
    private readonly CategoryRepository _categoryRepository;
    private int _nextControlY = 10; // Tracks Y position for next ComboBox and Button
    private readonly List<ComboBox> _comboBoxes = new(); // Keep track of all ComboBoxes

    public test(CategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
        InitializeComponent();
        _categoryRepository = _categoryRepository;

        InitializeDynamicPanel();
        LoadTopCategories();
    }

    private void InitializeDynamicPanel()
    {
        panelDynamic.AutoScroll = true;
    }

    private void LoadTopCategories()
    {
        var topCategories = _categoryRepository.GetTopLevelCategories();
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
            Width = 250,
            DropDownStyle = ComboBoxStyle.DropDown, // Allow user to type
            Tag = parentCategory // Store the parent category
        };

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
        var subCategories = _categoryRepository.GetSubCategories(selectedCategory.Id);

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
            await _categoryRepository.AddAsync(newCategory);

            // Refresh ComboBox data
            var categories = parentCategory == null
                ? _categoryRepository.GetTopLevelCategories()
                : _categoryRepository.GetSubCategories(parentCategory.Id);

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


    //private async void AddButton_Click(object sender, EventArgs e)
    //{
    //    var button = sender as Button;
    //    if (button == null) return;

    //    var parentComboBox = button.Tag as ComboBox;
    //    var parentCategory = parentComboBox?.SelectedItem as Category;

    //    // Show input dialog for new category name
    //    var categoryName = Prompt.ShowDialog("Enter Category Name:", "Add Category");

    //    if (string.IsNullOrWhiteSpace(categoryName)) return;

    //    // Validate category name
    //    if (_categoryRepository.IsDuplicateName(categoryName, parentCategory.Id))
    //    {
    //        MessageBox.Show("Category name already exists!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
    //        return;
    //    }

    //    // Add new category to the database
    //    var newCategory = new Category
    //    {
    //        Name = categoryName,
    //        ParentId = parentCategory?.Id
    //    };
    //    await _categoryRepository.AddAsync(newCategory);

    //    // Refresh ComboBox data
    //    var categories = parentCategory == null
    //        ? _categoryRepository.GetTopLevelCategories()
    //        : _categoryRepository.GetSubCategories(parentCategory.Id);

    //    parentComboBox.DataSource = categories;
    //    parentComboBox.SelectedItem = categories.First(c => c.Id == newCategory.Id);
    //}
}
