using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace AutomationStandards.Models.DatabaseModels
{
    public partial class AutomationStandardsContext : DbContext
    {
        public AutomationStandardsContext()
        {
        }

        public AutomationStandardsContext(DbContextOptions<AutomationStandardsContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Standard> Standard { get; set; }
        public virtual DbSet<StandardConfig> StandardConfig { get; set; }
        public virtual DbSet<StandardDataType> StandardDataType { get; set; }
        public virtual DbSet<StandardGroup> StandardGroup { get; set; }

        //        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //        {
        //            if (!optionsBuilder.IsConfigured)
        //            {
        //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
        //                optionsBuilder.UseSqlServer("Server=SQL03.observicing.net;Database=AutomationStandards;Trusted_Connection=true;");
        //            }
        //        }


        public static string ConnectionString { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(ConnectionString);
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Standard>(entity =>
            {
                entity.Property(e => e.DBTableName)
                    .HasMaxLength(60)
                    .IsUnicode(false);

                entity.Property(e => e.ManageRoles)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.OwnerEmail)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.StandardDefinition)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.StandardName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Tags)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.ViewerRoles)
                    .IsRequired()
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('GLOBAL_USER')");

                entity.HasOne(d => d.StandardGroup)
                    .WithMany(p => p.Standard)
                    .HasForeignKey(d => d.StandardGroupID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Standard_StandardGroup");
            });

            modelBuilder.Entity<StandardConfig>(entity =>
            {
                entity.Property(e => e.DisplayName)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.FieldName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.StandardFilterSQL)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.StandardLUValue)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ToolTip)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.HasOne(d => d.DataType)
                    .WithMany(p => p.StandardConfig)
                    .HasForeignKey(d => d.DataTypeID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_StandardConfig_StandardDataType");

                entity.HasOne(d => d.Standard)
                    .WithMany(p => p.StandardConfig)
                    .HasForeignKey(d => d.StandardID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_StandardConfig_Standard");
            });

            modelBuilder.Entity<StandardDataType>(entity =>
            {
                entity.HasKey(e => e.DataTypeID);

                entity.Property(e => e.DataTypeName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.SQLDataType)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<StandardGroup>(entity =>
            {
                entity.Property(e => e.StandardGroupName)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
