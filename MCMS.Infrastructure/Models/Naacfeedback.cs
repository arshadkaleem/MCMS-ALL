using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MCMS.Infrastructure.Models;

[Table("NAACFeedback")]
public partial class Naacfeedback
{
    [Key]
    public int FeedbackId { get; set; }

    [StringLength(50)]
    public string FeedbackType { get; set; } = null!;

    [StringLength(450)]
    public string UserId { get; set; } = null!;

    public int Rating { get; set; }

    public string? Comments { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? SubmittedAt { get; set; }
}
