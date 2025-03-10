using System;
using System.Collections.Generic;
using MCMS.Infrastructure.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;


namespace MCMS.Infrastructure.Data
{
    public partial class College365DbContext : IdentityDbContext<IdentityUser> // DbContext
    {
        public College365DbContext()
        {
        }

        public College365DbContext(DbContextOptions<College365DbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Category> Categories { get; set; }

        public virtual DbSet<Classroom> Classrooms { get; set; }

        public virtual DbSet<Course> Courses { get; set; }

        public virtual DbSet<Department> Departments { get; set; }

        public virtual DbSet<Faculty> Faculties { get; set; }

        public virtual DbSet<FacultyAward> FacultyAwards { get; set; }

        public virtual DbSet<FacultyBook> FacultyBooks { get; set; }

        public virtual DbSet<FacultyEducation> FacultyEducations { get; set; }

        public virtual DbSet<FacultyExperience> FacultyExperiences { get; set; }

        public virtual DbSet<FacultyResearch> FacultyResearches { get; set; }

        public virtual DbSet<FacultySubject> FacultySubjects { get; set; }

        public virtual DbSet<FacultyWorkshop> FacultyWorkshops { get; set; }

        public virtual DbSet<MediaFile> MediaFiles { get; set; }

        public virtual DbSet<MediaFileTag> MediaFileTags { get; set; }

        public virtual DbSet<MediaRelation> MediaRelations { get; set; }

        public virtual DbSet<MediaTag> MediaTags { get; set; }

        public virtual DbSet<Naaccriterion> Naaccriteria { get; set; }

        public virtual DbSet<Naacdatum> Naacdata { get; set; }

        public virtual DbSet<Naacfeedback> Naacfeedbacks { get; set; }

        public virtual DbSet<Naacmetric> Naacmetrics { get; set; }

        public virtual DbSet<Naacreport> Naacreports { get; set; }

        public virtual DbSet<NavigationItem> NavigationItems { get; set; }

        public virtual DbSet<NavigationMenu> NavigationMenus { get; set; }

        public virtual DbSet<StaticPage> StaticPages { get; set; }

        public virtual DbSet<Subject> Subjects { get; set; }

        public virtual DbSet<Timetable> Timetables { get; set; }

        public virtual DbSet<TimetableSlot> TimetableSlots { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(e => e.CategoryId).HasName("PK__Categori__19093A0BC2437999");

                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<Classroom>(entity =>
            {
                entity.HasKey(e => e.ClassroomId).HasName("PK__Classroo__11618EAA6E05438F");

                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<Course>(entity =>
            {
                entity.HasKey(e => e.CourseId).HasName("PK__Courses__C92D71A754AABD4E");

                entity.ToTable(tb => tb.HasTrigger("tr_Courses_Audit"));

                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Department).WithMany(p => p.Courses).HasConstraintName("FK_Courses_Department");
            });

            modelBuilder.Entity<Department>(entity =>
            {
                entity.HasKey(e => e.DepartmentId).HasName("PK__Departme__B2079BED4E59AD94");

                entity.ToTable(tb => tb.HasTrigger("tr_Departments_Audit"));

                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<Faculty>(entity =>
            {
                entity.HasKey(e => e.FacultyId).HasName("PK__Faculty__306F630EF709A240");

                entity.ToTable("Faculty", tb => tb.HasTrigger("tr_Faculty_Audit"));

                entity.HasIndex(e => e.DepartmentId, "IX_Faculty_IsHOD").HasFilter("([IsHOD]=(1))");

                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
                entity.Property(e => e.IsHod).HasDefaultValue(false);
                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Department).WithMany(p => p.Faculties).HasConstraintName("FK_Faculty_Department");
            });

            modelBuilder.Entity<FacultyAward>(entity =>
            {
                entity.HasKey(e => e.AwardId).HasName("PK__FacultyA__B08935FE5E72B3E3");

                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Faculty).WithMany(p => p.FacultyAwards).HasConstraintName("FK_FacultyAwards_Faculty");
            });

            modelBuilder.Entity<FacultyBook>(entity =>
            {
                entity.HasKey(e => e.BookId).HasName("PK__FacultyB__3DE0C2078B35C828");

                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Faculty).WithMany(p => p.FacultyBooks).HasConstraintName("FK_FacultyBooks_Faculty");
            });

            modelBuilder.Entity<FacultyEducation>(entity =>
            {
                entity.HasKey(e => e.EducationId).HasName("PK__FacultyE__4BBE3805483962C2");

                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Faculty).WithMany(p => p.FacultyEducations).HasConstraintName("FK_FacultyEducation_Faculty");
            });

            modelBuilder.Entity<FacultyExperience>(entity =>
            {
                entity.HasKey(e => e.ExperienceId).HasName("PK__FacultyE__2F4E344946692AC6");

                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Faculty).WithMany(p => p.FacultyExperiences).HasConstraintName("FK_FacultyExperience_Faculty");
            });

            modelBuilder.Entity<FacultyResearch>(entity =>
            {
                entity.HasKey(e => e.ResearchId).HasName("PK__FacultyR__617A954EE5143ECF");

                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Faculty).WithMany(p => p.FacultyResearches).HasConstraintName("FK_FacultyResearch_Faculty");
            });

            modelBuilder.Entity<FacultySubject>(entity =>
            {
                entity.HasKey(e => e.FacultySubjectId).HasName("PK__FacultyS__437418637759BAA3");

                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Faculty).WithMany(p => p.FacultySubjects).HasConstraintName("FK_FacultySubjects_Faculty");

                entity.HasOne(d => d.Subject).WithMany(p => p.FacultySubjects)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FacultySubjects_Subject");
            });

            modelBuilder.Entity<FacultyWorkshop>(entity =>
            {
                entity.HasKey(e => e.WorkshopId).HasName("PK__FacultyW__7A008C0AA425C835");

                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Faculty).WithMany(p => p.FacultyWorkshops).HasConstraintName("FK_FacultyWorkshops_Faculty");
            });

            modelBuilder.Entity<MediaFile>(entity =>
            {
                entity.HasKey(e => e.MediaId).HasName("PK__MediaFil__B2C2B5CF0FE85615");

                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<MediaFileTag>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__MediaFil__64957A5532533B08");

                entity.HasOne(d => d.Media).WithMany(p => p.MediaFileTags).HasConstraintName("FK_MediaFileTags_Media");

                entity.HasOne(d => d.Tag).WithMany(p => p.MediaFileTags).HasConstraintName("FK_MediaFileTags_Tags");
            });

            modelBuilder.Entity<MediaRelation>(entity =>
            {
                entity.HasKey(e => e.RelationId).HasName("PK__MediaRel__E2DA16B57510D760");

                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Media).WithMany(p => p.MediaRelations).HasConstraintName("FK_MediaRelations_Media");
            });

            modelBuilder.Entity<MediaTag>(entity =>
            {
                entity.HasKey(e => e.TagId).HasName("PK__MediaTag__657CF9AC6BDFC520");

                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<Naaccriterion>(entity =>
            {
                entity.HasKey(e => e.CriteriaId).HasName("PK__NAACCrit__FE6ADBCD71342783");

                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<Naacdatum>(entity =>
            {
                entity.HasKey(e => e.DataId).HasName("PK__NAACData__9D05303D628DF37E");

                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Department).WithMany(p => p.Naacdata)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_NAACData_Department");

                entity.HasOne(d => d.Metric).WithMany(p => p.Naacdata).HasConstraintName("FK_NAACData_Metric");
            });

            modelBuilder.Entity<Naacfeedback>(entity =>
            {
                entity.HasKey(e => e.FeedbackId).HasName("PK__NAACFeed__6A4BEDD605B63130");

                entity.Property(e => e.SubmittedAt).HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<Naacmetric>(entity =>
            {
                entity.HasKey(e => e.MetricId).HasName("PK__NAACMetr__561056A56F62C2F3");

                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Criteria).WithMany(p => p.Naacmetrics).HasConstraintName("FK_NAACMetrics_Criteria");
            });

            modelBuilder.Entity<Naacreport>(entity =>
            {
                entity.HasKey(e => e.ReportId).HasName("PK__NAACRepo__D5BD48057ADE9DBB");

                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
                entity.Property(e => e.GeneratedDate).HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<NavigationItem>(entity =>
            {
                entity.HasKey(e => e.ItemId);

                entity.Property(e => e.Title).HasMaxLength(255).IsRequired();
                entity.Property(e => e.Slug).HasMaxLength(255);
                entity.Property(e => e.ExternalUrl).HasMaxLength(500);
                entity.Property(e => e.OrderIndex).IsRequired();
                entity.Property(e => e.RoleAccess).HasMaxLength(50).IsRequired();
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

                // Enforce the RoleAccess constraint
                entity.HasCheckConstraint("CK_NavigationItems_RoleAccess",
                    "[RoleAccess] IN ('Admin', 'Faculty', 'Student', 'Public')");

                entity.HasOne(d => d.Menu).WithMany(p => p.NavigationItems)
                    .HasForeignKey(d => d.MenuId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_NavigationItems_Menu");

                entity.HasOne(d => d.Page).WithMany(p => p.NavigationItems)
                    .HasForeignKey(d => d.PageId)
                    .HasConstraintName("FK_NavigationItems_Page");

                entity.HasOne(d => d.Parent).WithMany(p => p.InverseParent)
                    .HasForeignKey(d => d.ParentId)
                    .HasConstraintName("FK_NavigationItems_Parent");
            });

            modelBuilder.Entity<NavigationMenu>(entity =>
            {
                entity.HasKey(e => e.MenuId);

                entity.Property(e => e.MenuName).HasMaxLength(255).IsRequired();
                entity.Property(e => e.IsActive).HasDefaultValue(true);
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            });


            //modelBuilder.Entity<NavigationItem>(entity =>
            //{
            //    entity.HasKey(e => e.ItemId).HasName("PK__Navigati__727E838B43D4E678");

            //    entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

            //    entity.HasOne(d => d.Menu).WithMany(p => p.NavigationItems)
            //        .OnDelete(DeleteBehavior.ClientSetNull)
            //        .HasConstraintName("FK_NavigationItems_Menu");

            //    entity.HasOne(d => d.Page).WithMany(p => p.NavigationItems).HasConstraintName("FK_NavigationItems_Page");

            //    entity.HasOne(d => d.Parent).WithMany(p => p.InverseParent).HasConstraintName("FK_NavigationItems_Parent");
            //});

            //modelBuilder.Entity<NavigationMenu>(entity =>
            //{
            //    entity.HasKey(e => e.MenuId).HasName("PK__Navigati__C99ED2304E0E0B7C");

            //    entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            //    entity.Property(e => e.IsActive).HasDefaultValue(true);
            //});

            modelBuilder.Entity<StaticPage>(entity =>
            {
                entity.HasKey(e => e.PageId).HasName("PK__StaticPa__C565B104845A61BF");

                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Category).WithMany(p => p.StaticPages)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_StaticPages_Categories");
            });

            modelBuilder.Entity<Subject>(entity =>
            {
                entity.HasKey(e => e.SubjectId).HasName("PK__Subjects__AC1BA3A883E289C4");

                entity.ToTable(tb => tb.HasTrigger("tr_Subjects_Audit"));

                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Course).WithMany(p => p.Subjects).HasConstraintName("FK_Subjects_Course");

                entity.HasOne(d => d.Department).WithMany(p => p.Subjects)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Subjects_Department");
            });

            modelBuilder.Entity<Timetable>(entity =>
            {
                entity.HasKey(e => e.TimetableId).HasName("PK__Timetabl__68413F60DD0424F4");

                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Classroom).WithMany(p => p.Timetables).HasConstraintName("FK_Timetable_Classroom");

                entity.HasOne(d => d.Course).WithMany(p => p.Timetables).HasConstraintName("FK_Timetable_Course");

                entity.HasOne(d => d.Faculty).WithMany(p => p.Timetables)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Timetable_Faculty");

                entity.HasOne(d => d.Slot).WithMany(p => p.Timetables).HasConstraintName("FK_Timetable_Slot");

                entity.HasOne(d => d.Subject).WithMany(p => p.Timetables)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Timetable_Subject");
            });

            modelBuilder.Entity<TimetableSlot>(entity =>
            {
                entity.HasKey(e => e.SlotId).HasName("PK__Timetabl__0A124AAF21E075C5");

                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            });





            // ASP.NET Identity schema
            // Fix for IdentityUserLogin primary key issue
            modelBuilder.Entity<IdentityUserLogin<string>>(entity =>
            {
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });
            });

            // It's a good idea to also configure the other Identity entities explicitly
            modelBuilder.Entity<IdentityUserRole<string>>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId });
            });

            modelBuilder.Entity<IdentityUserToken<string>>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });
            });

            // Configure soft delete filter for users if needed
            //modelBuilder.Entity<IdentityUser>().HasQueryFilter(u => !u.IsDeleted);








            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}