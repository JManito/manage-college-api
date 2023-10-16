using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ManageCollege.Models.Domain
{
    public class Authentication
    {
        [Key]
        public int Id { get; set; }
        public int isAuth { get; set; }

        public string authAs { get; set; }

    }
}
