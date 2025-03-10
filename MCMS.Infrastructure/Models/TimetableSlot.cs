using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MCMS.Infrastructure.Models;

public partial class TimetableSlot
{
    [Key]
    public int SlotId { get; set; }

    public TimeOnly StartTime { get; set; }

    public TimeOnly EndTime { get; set; }

    [StringLength(20)]
    public string DayOfWeek { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [InverseProperty("Slot")]
    public virtual ICollection<Timetable> Timetables { get; set; } = new List<Timetable>();
}
