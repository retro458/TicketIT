using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TicketIT.API.Models;

[Table("estado")]
[Index("Nombre", Name = "estado_nombre_key", IsUnique = true)]
public partial class Estado
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("nombre")]
    [StringLength(100)]
    public string Nombre { get; set; } = null!;

    [InverseProperty("Estado")]
    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}
