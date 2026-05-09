using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TicketIT.API.Models;

[Table("prioridades")]
[Index("Nombre", Name = "prioridades_nombre_key", IsUnique = true)]
public partial class Prioridade
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("nombre")]
    [StringLength(100)]
    public string Nombre { get; set; } = null!;

    [InverseProperty("Prioridad")]
    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}
