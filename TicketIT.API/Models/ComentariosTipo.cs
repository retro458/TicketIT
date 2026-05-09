using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TicketIT.API.Models;

[Table("comentarios_tipo")]
[Index("Nombre", Name = "comentarios_tipo_nombre_key", IsUnique = true)]
public partial class ComentariosTipo
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("nombre")]
    [StringLength(50)]
    public string Nombre { get; set; } = null!;

    [InverseProperty("Tipo")]
    public virtual ICollection<Comentario> Comentarios { get; set; } = new List<Comentario>();
}
