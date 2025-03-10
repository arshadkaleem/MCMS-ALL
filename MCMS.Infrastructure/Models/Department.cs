using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MCMS.Infrastructure.Models;

[Index("DepartmentName", Name = "IX_Departments_Name")]
[Index("Slug", Name = "UQ__Departme__BC7B5FB675B55E13", IsUnique = true)]
public partial class Department
{
    [Key]
    public int DepartmentId { get; set; }

    [StringLength(100)]
    public string DepartmentName { get; set; } = null!;

    [StringLength(150)]
    public string Slug { get; set; } = null!;

    [StringLength(20)]
    public string CollegeType { get; set; } = null!;

    [Column("HODId")]
    public int? Hodid { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? UpdatedAt { get; set; }

    public bool IsDeleted { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DeletedAt { get; set; }

    [InverseProperty("Department")]
    public virtual ICollection<Course> Courses { get; set; } = new List<Course>();

    [InverseProperty("Department")]
    public virtual ICollection<Faculty> Faculties { get; set; } = new List<Faculty>();

    [InverseProperty("Department")]
    public virtual ICollection<Naacdatum> Naacdata { get; set; } = new List<Naacdatum>();

    [InverseProperty("Department")]
    public virtual ICollection<Subject> Subjects { get; set; } = new List<Subject>();
}
