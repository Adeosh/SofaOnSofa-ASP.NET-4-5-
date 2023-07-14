using System.ComponentModel.DataAnnotations;

namespace SofaOnSofa.Domain.Entities
{
    public class ShippingDetails
    {
        [Required(ErrorMessage = "Введите Имя")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Введите Улицу")]
        public string Street { get; set; }
        [Required(ErrorMessage = "Введите Дом")]
        public string House { get; set; }
        [Required(ErrorMessage = "Введите Подъезд")]
        public string Entrance { get; set; }

        [Required(ErrorMessage = "Введите Страну")]
        public string Country { get; set; }

        [Required(ErrorMessage = "Введите Регион(Область)")]
        public string Region { get; set; }

        [Required(ErrorMessage = "Введите Город")]
        public string City { get; set; }

        public bool PaidDelivery { get; set; }
    }
}
