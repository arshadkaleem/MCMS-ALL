using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MCMS.Infrastructure.Models
{
    // NavigationItem.cs - Updated version
    public class NavigationItem
    {
        public NavigationItem()
        {
            InverseParent = new HashSet<NavigationItem>();
        }

        [Key]
        public int ItemId { get; set; }

        public int MenuId { get; set; }

        public int? ParentId { get; set; }

        [Required]
        [StringLength(255)]
        public string Title { get; set; }

        [StringLength(255)]
        public string Slug { get; set; }

        [StringLength(500)]
        public string ExternalUrl { get; set; }

        public int? PageId { get; set; }

        [Required]
        public int OrderIndex { get; set; }

        [Required]
        [StringLength(50)]
        public string RoleAccess { get; set; }

        public DateTime? CreatedAt { get; set; }

        [ForeignKey(nameof(MenuId))]
        public virtual NavigationMenu Menu { get; set; }

        [ForeignKey(nameof(PageId))]
        public virtual StaticPage Page { get; set; }

        [ForeignKey(nameof(ParentId))]
        public virtual NavigationItem Parent { get; set; }

        public virtual ICollection<NavigationItem> InverseParent { get; set; }
    }
}
