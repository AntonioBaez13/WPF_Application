using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace HelloWPFApp.Models
{
    public partial class LOTOContext : DbContext
    {
        public LOTOContext()
        {
        }

        public LOTOContext(DbContextOptions<LOTOContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Jugada> Jugada { get; set; }
        public virtual DbSet<Loteria> Loteria { get; set; }
        public virtual DbSet<Ticket> Ticket { get; set; }
        public virtual DbSet<TicketJugada> TicketJugada { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=DESKTOP-OMRFS3R\\SQLEXPRESS;Database=LOTO;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Jugada>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Fecha).HasColumnType("date");

                entity.Property(e => e.Numero)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.HasOne(d => d.Loteria)
                    .WithMany(p => p.Jugada)
                    .HasForeignKey(d => d.LoteriaId)
                    .HasConstraintName("FK_Jugada_Loteria");
            });

            modelBuilder.Entity<Loteria>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Ticket>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Creado).HasColumnType("smalldatetime");

                entity.Property(e => e.Pin).HasColumnName("PIN");
            });

            modelBuilder.Entity<TicketJugada>(entity =>
            {
                entity.HasKey(e => new { e.TicketId, e.JugadaId });

                entity.ToTable("Ticket_Jugada");

                entity.Property(e => e.TicketId).HasColumnName("TicketID");

                entity.Property(e => e.JugadaId).HasColumnName("JugadaID");

                entity.HasOne(d => d.Jugada)
                    .WithMany(p => p.TicketJugada)
                    .HasForeignKey(d => d.JugadaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Ticket_Jugada_Jugada");

                entity.HasOne(d => d.Ticket)
                    .WithMany(p => p.TicketJugada)
                    .HasForeignKey(d => d.TicketId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Ticket_Jugada_Ticket");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
