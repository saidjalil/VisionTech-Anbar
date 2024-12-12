

using VisionTech_Anbar_Project.Entities;

namespace VisionTech_Anbar_Project.ViewModel;

public class Data
{
    public int id { get; set; }
    public string type { get; set; }
    public Warehouse warehouse { get; set; }
    public Vendor vendor { get; set; }
    public List<Department> departments { get; set; }
    public List<Product> products { get; set; }
    public string recipient { get; set; }
    public string place { get; set; }
    public object file { get; set; }
    public DateTime created_at { get; set; }
}