using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ManageCollege.Models.Domain
{
    public class Authentication
    {
        [Key]
        public int Id { get; set; } = 0;
        public int isAuth { get; set; } = 0;

        public string authAs { get; set; } = "Unauthenticated";

    }
}
