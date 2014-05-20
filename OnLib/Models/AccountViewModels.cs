using System;
using System.ComponentModel.DataAnnotations;

namespace OnLib.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Benutzername")]
        public string UserName { get; set; }
    }

    public class ManageUserViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Aktuelles Kennwort")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "\"{0}\" muss mindestens {2} Zeichen lang sein.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Neues Kennwort")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Neues Kennwort bestätigen")]
        [Compare("NewPassword", ErrorMessage = "Das neue Kennwort stimmt nicht mit dem Bestätigungskennwort überein.")]
        public string ConfirmPassword { get; set; }
    }

    public class ManageUserDataViewModel
    {
        public string Vorname { get; set; }
        public string Nachname { get; set; }
        
        [Required]
        [Display(Name="Email-Adresse")]
        public string Email { get; set; }

        public DateTime Geburtstag { get; set; }

        [Display(Name="Straße")]
        public string Strasse { get; set; }
        public int HausNr { get; set; }
        public int PLZ { get; set; }
        public string Ort { get; set; }
        public string Land { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Benutzername")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Kennwort")]
        public string Password { get; set; }

        [Display(Name = "Speichern?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "Benutzername")]
        public string UserName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "\"{0}\" muss mindestens {2} Zeichen lang sein.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Kennwort")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Kennwort bestätigen")]
        [Compare("Password", ErrorMessage = "Das Kennwort stimmt nicht mit dem Bestätigungskennwort überein.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [DataType(DataType.EmailAddress, ErrorMessage="Keine gültige E-Mail")]
        [Display(Name="E-Mail")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Text, ErrorMessage="Vorname ist erforderlich. ")]
        [Display(Name="Vorname")]
        public string Vorname { get; set; }

        [Required]
        [DataType(DataType.Text, ErrorMessage = "Nachname ist erforderlich. ")]
        [Display(Name = "Nachname")]
        public string Nachname { get; set; }

        [Required]
        [DataType(DataType.DateTime, ErrorMessage="Kein gültiges Datum.")]
        [Display(Name="Geburtstag")]
        public DateTime Geburtstag { get; set; }

        [DataType(DataType.Text)]
        [Display(Name="Straße")]
        public string Strasse { get; set; }

        [Display(Name="Hausnummer")]
        public int HausNr { get; set; }

        [DataType(DataType.PostalCode)]
        [Display(Name="Postleitzahl")]
        public int PLZ { get; set; }

        [Display(Name="Ort")]
        public string Ort { get; set; }

        [Display(Name="Land")]
        public string Land { get; set; }

    }
}
