using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BoardingHouse.Models
{
    public class DataBooking
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Phone Number is required")]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "ID Card Number is required")]
        public string IDCardNumber { get; set; }
        [Required(ErrorMessage = "Work is required")]
        public string Work { get; set; }
        [Required(ErrorMessage = "Count is required")]
        public int Count { get; set; }
        public DataKost dataKost { get; set; }
    }
    public class BookingForm
    {
        public string Name { get; set; }
        [Required(ErrorMessage = "Phone Number is required")]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "ID Card Number is required")]
        public string IDCardNumber { get; set; }
        [Required(ErrorMessage = "Work is required")]
        public string Work { get; set; }
        [Required(ErrorMessage = "Count is required")]
        public int Count { get; set; }
        public int DataKost { get; set; }
    }
}

