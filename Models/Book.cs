using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Castle.MicroKernel.SubSystems.Conversion;
using System.ComponentModel.DataAnnotations.Schema;

namespace CompanyBookstore.Models
{
    public class Book
    {
        [DisplayName("Indeks")]
        [Required(ErrorMessage = "To pole jest wymagane")]
        [RegularExpression(@"\d+",
        ErrorMessage = "Indeks może się składać wyłącznie z samych cyfr!")]
        public int BookID { get; set; }

        [DisplayName("Kategoria")]
        public string Category { get; set; }

        [DisplayName("Tytuł")]
        [Required(ErrorMessage = "Tytuł jest wymagany")]
        [StringLength(300, ErrorMessage = "Tytuł może zawierać maksymalnie 300 znaków")]
        [RegularExpression(@"[A-ZĘÓĄŚŁŻŹĆŃ]{1}[a-zęóąśłżźćńA-ZĘÓĄŚŁŻŹĆŃ0-9!@#$&()\-`.+,/ ]*",
         ErrorMessage = "Tytuł musi zaczynać się od wielkiej litery, może zawierać znaki specjalne")]
        public string BookTitle { get; set; }

        [DisplayName("Autor")]
        [Required(ErrorMessage = "Autor jest wymagany")]
        [RegularExpression(@"[A-ZĘÓĄŚŁŻŹĆŃ]{1}[a-zęóąśłżźćńA-ZĘÓĄŚŁŻŹĆŃ\-, ]*",
        ErrorMessage = "Imię autora musi rozpoczynać się wielką literą, nie może zawierać znaków specjalnych (oprócz myślnika, przecinka oraz spacji)")]
        public string BookAuthor { get; set; }


        [DisplayName("Cena")]
        [Required(ErrorMessage = "Cena jest wymagana")]
        /*[RegularExpression(@"^\d+(\d{3})*(\[.,]*\d{1,2})?$",
         ErrorMessage = "Cena może zawierać tylko cyfry")]*/
        public decimal BookPrice { get; set; }

        [DisplayName("Opis")]
        [StringLength(2000, ErrorMessage = "Opis książki może maksymalnie 2000 znaków")]
        [Required(ErrorMessage = "Opis jest wymagany")]
        public string BookDescription { get; set; }

        [DisplayName("Bestseller")]
        public bool Bestseller { get; set; }

        [DisplayName("Wydawca")]
        [StringLength(100, ErrorMessage = "Nazwa wydawcy może składać się maksymalnie ze 100 znaków")]
        [Required(ErrorMessage = "Wydawca jest wymagany")]
        [RegularExpression(@"[a-zęóąśłżźćńA-ZĘÓĄŚŁŻŹĆŃ0-9!@#$&()\-`.+,/\] ]*",
        ErrorMessage = "Wydawca może zawierać tylko litery, cyfry oraz znaki specjalne")]
        public string Publisher { get; set; }

        [DisplayName("Data premiery")]
        [Required(ErrorMessage = "Data premiery jest wymagana")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0: dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime ReleaseDate { get; set; }

        [DisplayName("Liczba stron")]
        [Required(ErrorMessage = "Liczba stron jest wymagana")]
        [RegularExpression(@"[0-9]+", ErrorMessage = "Liczba stron może składać się tylko z cyfr")]
        public int Pages { get; set; }

    }
}
