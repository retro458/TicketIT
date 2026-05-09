using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TicketIT.API.Models;

[Table("comentarios")]
[Index("TicketId", Name = "idx_comentarios_ticket")]
public partial class Comentario
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("ticket_id")]
    public int TicketId { get; set; }

    [Column("usuario_id")]
    public int UsuarioId { get; set; }

    [Column("tipo_id")]
    public int TipoId { get; set; }

    [Column("contenido")]
    public string Contenido { get; set; } = null!;

    [Column("creado_en")]
    public DateTime? CreadoEn { get; set; }

    [ForeignKey("TicketId")]
    [InverseProperty("Comentarios")]
    public virtual Ticket Ticket { get; set; } = null!;

    [ForeignKey("TipoId")]
    [InverseProperty("Comentarios")]
    public virtual ComentariosTipo Tipo { get; set; } = null!;

    [ForeignKey("UsuarioId")]
    [InverseProperty("Comentarios")]
    public virtual Usuario Usuario { get; set; } = null!;
}
