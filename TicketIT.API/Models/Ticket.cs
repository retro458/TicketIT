using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TicketIT.API.Models;

[Table("tickets")]
[Index("ClienteId", Name = "idx_tickets_cliente")]
[Index("EstadoId", Name = "idx_tickets_estado")]
[Index("PrioridadId", Name = "idx_tickets_prioridad")]
[Index("TecnicoId", Name = "idx_tickets_tecnico")]
public partial class Ticket
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("titulo")]
    [StringLength(200)]
    public string Titulo { get; set; } = null!;

    [Column("descripcion")]
    public string Descripcion { get; set; } = null!;

    [Column("estado_id")]
    public int EstadoId { get; set; }

    [Column("prioridad_id")]
    public int PrioridadId { get; set; }

    [Column("categoria_id")]
    public int? CategoriaId { get; set; }

    [Column("cliente_id")]
    public int ClienteId { get; set; }

    [Column("tecnico_id")]
    public int? TecnicoId { get; set; }

    [Column("creado_en")]
    public DateTime? CreadoEn { get; set; }

    [Column("actualizado_en")]
    public DateTime? ActualizadoEn { get; set; }

    [Column("cerrado_en")]
    public DateTime? CerradoEn { get; set; }

    [InverseProperty("Ticket")]
    public virtual ICollection<AuditLog> AuditLogs { get; set; } = new List<AuditLog>();

    [ForeignKey("CategoriaId")]
    [InverseProperty("Tickets")]
    public virtual Categoria? Categoria { get; set; }

    [ForeignKey("ClienteId")]
    [InverseProperty("TicketClientes")]
    public virtual Usuario Cliente { get; set; } = null!;

    [InverseProperty("Ticket")]
    public virtual ICollection<Comentario> Comentarios { get; set; } = new List<Comentario>();

    [ForeignKey("EstadoId")]
    [InverseProperty("Tickets")]
    public virtual Estado Estado { get; set; } = null!;

    [InverseProperty("Ticket")]
    public virtual ICollection<MensajesChat> MensajesChats { get; set; } = new List<MensajesChat>();

    [ForeignKey("PrioridadId")]
    [InverseProperty("Tickets")]
    public virtual Prioridade Prioridad { get; set; } = null!;

    [ForeignKey("TecnicoId")]
    [InverseProperty("TicketTecnicos")]
    public virtual Usuario? Tecnico { get; set; }
}
