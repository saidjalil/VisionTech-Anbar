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
        // Create a new ComboBox
        var comboBox = new ComboBox
        {
            DataSource = categories,
            DisplayMember = "Name",
            ValueMember = "Id",
            Location = new Point(10, _nextControlY),
            Width = 250,
            Tag = parentCategory // Store the parent category
        };

        // ComboBox SelectionChanged Event
        comboBox.SelectedIndexChanged += ComboBox_SelectedIndexChanged;

        // Create the '+' button
        var addButton = new Button
        {
            Text = "+",
            Location = new Point(270, _nextControlY),
            Width = 30,
            Tag = comboBox // Link the button to its ComboBox
        };

        // Button Click Event
        addButton.Click += AddButton_Click;

        // Add controls to the panel
        panelDynamic.Controls.Add(comboBox);
        panelDynamic.Controls.Add(addButton);

        // Track new Y position for subsequent controls
        _nextControlY += 40;

        // Keep track of ComboBoxes
        _comboBoxes.Add(comboBox);
    }

    private void ComboBox_SelectedIndexChanged(object sender, EventArgs e)
    {
        var comboBox = sender as ComboBox;
        if (comboBox == null) return;

        var selectedCategory = comboBox.SelectedItem as Category;
        if (selectedCategory == null) return;

        // Remove ComboBoxes below this one
        var index = _comboBoxes.IndexOf(comboBox);
        for (int i = _comboBoxes.Count - 1; i > index; i--)
        {
            panelDynamic.Controls.Remove(_comboBoxes[i]);
            panelDynamic.Controls.Remove(panelDynamic.Controls.OfType<Button>().First(b => b.Tag == _comboBoxes[i]));
            _comboBoxes.RemoveAt(i);
        }

        // Load subcategories of the selected category
        var subCategories = _categoryRepository.GetSubCategories(selectedCategory.Id);
        if (subCategories.Any())
        {
            AddComboBox(selectedCategory, subCategories);
        }
    }

    private async void AddButton_Click(object sender, EventArgs e)
    {
        var button = sender as Button;
        if (button == null) return;

        var parentComboBox = button.Tag as ComboBox;
        var parentCategory = parentComboBox?.SelectedItem as Category;

        // Show input dialog for new category name
        var categoryName = Prompt.ShowDialog("Enter Category Name:", "Add Category");

        if (string.IsNullOrWhiteSpace(categoryName)) return;

        // Validate category name
        if (_categoryRepository.IsDuplicateName(categoryName, parentCategory.Id))
        {
            MessageBox.Show("Category name already exists!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        // Add new category to the database
        var newCategory = new Category
        {
            Name = categoryName,
            ParentId = parentCategory?.Id
        };
        await _categoryRepository.AddAsync(newCategory);

        // Refresh ComboBox data
        var categories = parentCategory == null
            ? _categoryRepository.GetTopLevelCategories()
            : _categoryRepository.GetSubCategories(parentCategory.Id);

        parentComboBox.DataSource = categories;
        parentComboBox.SelectedItem = categories.First(c => c.Id == newCategory.Id);
    }
}
