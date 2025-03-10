using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MCMS.Infrastructure.Models;

public partial class MediaFileTag
{
    [Key]
    public int Id { get; set; }

    public int MediaId { get; set; }

    public int TagId { get; set; }

    [ForeignKey("MediaId")]
    [InverseProperty("MediaFileTags")]
    public virtual MediaFile Media { get; set; } = null!;

    [ForeignKey("TagId")]
    [InverseProperty("MediaFileTags")]
    public virtual MediaTag Tag { get; set; } = null!;
}
