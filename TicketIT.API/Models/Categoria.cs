using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TicketIT.API.Models;

[Table("categorias")]
[Index("Nombre", Name = "categorias_nombre_key", IsUnique = true)]
public partial class Categoria
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("nombre")]
    [StringLength(100)]
    public string Nombre { get; set; } = null!;

    [Column("prefijo")]
    [StringLength(5)]
    public string Prefijo { get; set; } = null!;

    [InverseProperty("Categoria")]
    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}
