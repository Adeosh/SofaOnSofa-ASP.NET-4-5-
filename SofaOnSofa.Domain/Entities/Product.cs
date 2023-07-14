using System.ComponentModel.DataAnnotations;

namespace SofaOnSofa.Domain.Entities
{
    public class Product
    {
        [ScaffoldColumn(false)]
        [Display(Name = "№")]
        public int ProductId { get; set; }
        [Display(Name = "Название")]
        public string Name { get; set; }
        [Display(Name = "Механизм")]
        public string Mechanism { get; set; }
        [Display(Name = "Стиль")]
        public string Style { get; set; }
        [Display(Name = "Тип ткани")]
        public string FabricType { get; set; }
        [Display(Name = "Особенности")]
        public string Features { get; set; }
        [Display(Name = "Цена")]
        public decimal Price { get; set; }
        [Display(Name = "Изображение")]
        public byte[] ImageData { get; set; }

        [ScaffoldColumn(false)]
        public string ImageMimeType { get; set; }
    }
}
