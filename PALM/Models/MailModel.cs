using DataAnnotationsExtensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PALM.Models
{
    public class MailModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Te rugăm să introduci numele tău"), Display(Name = "Numele tău")]
        public string Name { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Te rugăm să introduci adresa ta de email"), Display(Name = "Adresa ta de email"), EmailAddress(ErrorMessage ="Te rugăm să introduci o adresă de email validă")]
        public string Email { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Te rugăm să introduci numărul tău de telefon"), Display(Name = "Telefonul tău"), Phone(ErrorMessage = "Te rugăm să introduci un număr de telefon valid")]
        [StringLength(10, ErrorMessage = "Te rugăm să introduci un număr de telefon valid")]
        public string Phone { get; set; }

        [Required(AllowEmptyStrings=false,ErrorMessage = "Te rugăm să introduci un mesaj"), Display(Name = "Mesajul tău")]
        [StringLength(100_000, ErrorMessage = "Numărul maxim de caractere este de 100,000")]
        public string Message { get; set; }

        //Interesed In
        [DisplayName("Photobooks")]
        public bool PhotobooksCheck { get; set; }

        [DisplayName("Tablouri Canvas")]
        public bool CanvasCheck { get; set; }

        [DisplayName("Transfer Audio-Video")]
        public bool AudioVideoCheck { get; set; }

        [DisplayName("Alte Servicii")]
        public bool OtherSvcCheck { get; set; }
    }
}
