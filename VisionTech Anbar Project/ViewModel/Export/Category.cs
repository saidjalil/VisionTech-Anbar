namespace VisionTech_Anbar_Project.ViewModel;

public class Category
{
    public int id { get; set; }
    public string name { get; set; }
    public object description { get; set; }
    public object icon { get; set; }
    public int? parent_id { get; set; }
}