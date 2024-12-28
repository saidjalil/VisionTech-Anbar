namespace VisionTech_Anbar_Project.ViewModel;

public class Product
{
    public int id { get; set; }
    public string name { get; set; }
    public object photo { get; set; }
    public int is_permanent { get; set; }
    public List<Barcode> barcodes { get; set; }
    public List<Category> categories { get; set; }
}