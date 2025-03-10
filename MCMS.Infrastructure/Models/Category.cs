using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MCMS.Infrastructure.Models;

[Index("Slug", Name = "UQ__Categori__BC7B5FB64880D942", IsUnique = true)]
public partial class Category
{
    [Key]
    public int CategoryId { get; set; }

    [StringLength(150)]
    public string CategoryName { get; set; } = null!;

    [StringLength(255)]
    public string Slug { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? UpdatedAt { get; set; }

    [InverseProperty("Category")]
    public virtual ICollection<StaticPage> StaticPages { get; set; } = new List<StaticPage>();
}
