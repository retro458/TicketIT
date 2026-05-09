using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using TicketIT.API.Models;

namespace TicketIT.API.Data;

public partial class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AuditLog> AuditLogs { get; set; }

    public virtual DbSet<Categoria> Categorias { get; set; }

    public virtual DbSet<Comentario> Comentarios { get; set; }

    public virtual DbSet<ComentariosTipo> ComentariosTipos { get; set; }

    public virtual DbSet<Estado> Estados { get; set; }

    public virtual DbSet<MensajesChat> MensajesChats { get; set; }

    public virtual DbSet<Prioridade> Prioridades { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Ticket> Tickets { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AuditLog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("audit_log_pkey");

            entity.Property(e => e.Id).UseIdentityAlwaysColumn();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");

            entity.HasOne(d => d.ChangedByNavigation).WithMany(p => p.AuditLogs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("audit_log_changed_by_fkey");

            entity.HasOne(d => d.Ticket).WithMany(p => p.AuditLogs).HasConstraintName("audit_log_ticket_id_fkey");
        });

        modelBuilder.Entity<Categoria>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("categorias_pkey");

            entity.Property(e => e.Id).UseIdentityAlwaysColumn();
        });

        modelBuilder.Entity<Comentario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("comentarios_pkey");

            entity.Property(e => e.Id).UseIdentityAlwaysColumn();
            entity.Property(e => e.CreadoEn).HasDefaultValueSql("now()");

            entity.HasOne(d => d.Ticket).WithMany(p => p.Comentarios).HasConstraintName("comentarios_ticket_id_fkey");

            entity.HasOne(d => d.Tipo).WithMany(p => p.Comentarios)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("comentarios_tipo_id_fkey");

            entity.HasOne(d => d.Usuario).WithMany(p => p.Comentarios)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("comentarios_usuario_id_fkey");
        });

        modelBuilder.Entity<ComentariosTipo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("comentarios_tipo_pkey");

            entity.Property(e => e.Id).UseIdentityAlwaysColumn();
        });

        modelBuilder.Entity<Estado>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("estado_pkey");

            entity.Property(e => e.Id).UseIdentityAlwaysColumn();
        });

        modelBuilder.Entity<MensajesChat>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("mensajes_chat_pkey");

            entity.Property(e => e.Id).UseIdentityAlwaysColumn();
            entity.Property(e => e.CreadoEn).HasDefaultValueSql("now()");
            entity.Property(e => e.EsPrivado).HasDefaultValue(false);
            entity.Property(e => e.Leido).HasDefaultValue(false);
            entity.Property(e => e.TipoMensaje).HasDefaultValueSql("'text'::character varying");

            entity.HasOne(d => d.Emisor).WithMany(p => p.MensajesChats)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("mensajes_chat_emisor_id_fkey");

            entity.HasOne(d => d.Ticket).WithMany(p => p.MensajesChats).HasConstraintName("mensajes_chat_ticket_id_fkey");
        });

        modelBuilder.Entity<Prioridade>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("prioridades_pkey");

            entity.Property(e => e.Id).UseIdentityAlwaysColumn();
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("roles_pkey");

            entity.Property(e => e.Id).UseIdentityAlwaysColumn();
        });

        modelBuilder.Entity<Ticket>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("tickets_pkey");

            entity.Property(e => e.Id).UseIdentityAlwaysColumn();
            entity.Property(e => e.ActualizadoEn).HasDefaultValueSql("now()");
            entity.Property(e => e.CreadoEn).HasDefaultValueSql("now()");

            entity.HasOne(d => d.Categoria).WithMany(p => p.Tickets).HasConstraintName("tickets_categoria_id_fkey");

            entity.HasOne(d => d.Cliente).WithMany(p => p.TicketClientes)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("tickets_cliente_id_fkey");

            entity.HasOne(d => d.Estado).WithMany(p => p.Tickets)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("tickets_estado_id_fkey");

            entity.HasOne(d => d.Prioridad).WithMany(p => p.Tickets)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("tickets_prioridad_id_fkey");

            entity.HasOne(d => d.Tecnico).WithMany(p => p.TicketTecnicos).HasConstraintName("tickets_tecnico_id_fkey");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("usuarios_pkey");

            entity.Property(e => e.Id).UseIdentityAlwaysColumn();
            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.CreadoEn).HasDefaultValueSql("now()");

            entity.HasOne(d => d.Rol).WithMany(p => p.Usuarios)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("usuarios_rol_id_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
