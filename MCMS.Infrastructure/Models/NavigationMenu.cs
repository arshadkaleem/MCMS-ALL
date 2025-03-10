using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MCMS.Infrastructure.Models
{
    public partial class NavigationMenu
    {
        [Key]
        public int MenuId { get; set; }

        [StringLength(255)]
        public string MenuName { get; set; } = null!;

        public bool? IsActive { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? CreatedAt { get; set; }

        [InverseProperty("Menu")]
        public virtual ICollection<NavigationItem> NavigationItems { get; set; } = new List<NavigationItem>();
    }
}