using VisionTech_Anbar_Project.Entities.Base;

namespace VisionTech_Anbar_Project.Entities;

public class PackageProduct : BaseItem
{
    public int PackageId { get; set; }
    public Package Package { get; set; }

    public int ProductId { get; set; }
    public Product Product { get; set; }

    public int Quantity { get; set; }
}