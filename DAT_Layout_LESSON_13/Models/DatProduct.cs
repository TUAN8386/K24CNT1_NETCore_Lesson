namespace DAT_Layout_LESSON_13.Models
{
    public class DatProduct
    {
        public int DatId { get; set; }
        public string DatName { get; set; } = "";
        public decimal DatPrice { get; set; }
        public string DatImage { get; set; } = "";
        public bool DatIsHot { get; set; }
    }
}
