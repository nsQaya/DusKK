using System.ComponentModel.DataAnnotations;

namespace TDV.Authorization.Users.Dto
{
    public class ChangeUserLanguageDto
    {
        [Required]
        public string LanguageName { get; set; }
    }
}
