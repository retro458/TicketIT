using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TicketIT.API.Models;

[Table("mensajes_chat")]
[Index("TicketId", "CreadoEn", Name = "idx_chat_ticket")]
public partial class MensajesChat
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("ticket_id")]
    public int TicketId { get; set; }

    [Column("emisor_id")]
    public int EmisorId { get; set; }

    [Column("contenido")]
    public string Contenido { get; set; } = null!;

    [Column("leido")]
    public bool? Leido { get; set; }

    [Column("tipo_mensaje")]
    [StringLength(100)]
    public string? TipoMensaje { get; set; }

    [Column("es_privado")]
    public bool? EsPrivado { get; set; }

    [Column("creado_en")]
    public DateTime? CreadoEn { get; set; }

    [ForeignKey("EmisorId")]
    [InverseProperty("MensajesChats")]
    public virtual Usuario Emisor { get; set; } = null!;

    [ForeignKey("TicketId")]
    [InverseProperty("MensajesChats")]
    public virtual Ticket Ticket { get; set; } = null!;
}
