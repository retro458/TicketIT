using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TicketIT.API.Models;

[Table("usuarios")]
[Index("Email", Name = "usuarios_email_key", IsUnique = true)]
[Index("ExternalId", Name = "usuarios_external_id_key", IsUnique = true)]
public partial class Usuario
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("nombre")]
    [StringLength(100)]
    public string Nombre { get; set; } = null!;

    [Column("email")]
    [StringLength(100)]
    public string Email { get; set; } = null!;

    [Column("password_hash")]
    public string? PasswordHash { get; set; }

    [Column("external_id")]
    [StringLength(255)]
    public string? ExternalId { get; set; }

    [Column("provider")]
    [StringLength(50)]
    public string? Provider { get; set; }

    [Column("rol_id")]
    public int RolId { get; set; }

    [Column("activo")]
    public bool? Activo { get; set; }

    [Column("creado_en")]
    public DateTime? CreadoEn { get; set; }

    [InverseProperty("ChangedByNavigation")]
    public virtual ICollection<AuditLog> AuditLogs { get; set; } = new List<AuditLog>();

    [InverseProperty("Usuario")]
    public virtual ICollection<Comentario> Comentarios { get; set; } = new List<Comentario>();

    [InverseProperty("Emisor")]
    public virtual ICollection<MensajesChat> MensajesChats { get; set; } = new List<MensajesChat>();

    [ForeignKey("RolId")]
    [InverseProperty("Usuarios")]
    public virtual Role Rol { get; set; } = null!;

    [InverseProperty("Cliente")]
    public virtual ICollection<Ticket> TicketClientes { get; set; } = new List<Ticket>();

    [InverseProperty("Tecnico")]
    public virtual ICollection<Ticket> TicketTecnicos { get; set; } = new List<Ticket>();
}
