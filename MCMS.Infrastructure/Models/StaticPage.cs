using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MCMS.Infrastructure.Models;

[Index("CategoryId", "Published", Name = "IX_StaticPages_CategoryPublished")]
[Index("Slug", Name = "UQ__StaticPa__BC7B5FB61D550C1F", IsUnique = true)]
public partial class StaticPage
{
    public StaticPage()
    {
        NavigationItems = new HashSet<NavigationItem>();
    }

    [Key]
    public int PageId { get; set; }

    [StringLength(255)]
    public string Title { get; set; } = null!;

    [StringLength(255)]
    public string Slug { get; set; } = null!;

    public string PageContent { get; set; } = null!;

    [StringLength(255)]
    public string? MetaTitle { get; set; }

    [StringLength(500)]
    public string? MetaDescription { get; set; }

    public bool Published { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? UpdatedAt { get; set; }

    public int CategoryId { get; set; }

    public bool IsDeleted { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DeletedAt { get; set; }

    [ForeignKey("CategoryId")]
    [InverseProperty("StaticPages")]
    public virtual Category Category { get; set; } = null!;

    [InverseProperty("Page")]
    public virtual ICollection<NavigationItem> NavigationItems { get; set; } = new List<NavigationItem>();
}
