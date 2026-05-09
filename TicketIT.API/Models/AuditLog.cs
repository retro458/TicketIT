using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TicketIT.API.Models;

[Table("audit_log")]
[Index("TicketId", Name = "idx_audit_ticket")]
public partial class AuditLog
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("ticket_id")]
    public int TicketId { get; set; }

    [Column("changed_by")]
    public int ChangedBy { get; set; }

    [Column("field_changed")]
    [StringLength(100)]
    public string FieldChanged { get; set; } = null!;

    [Column("old_value")]
    public string? OldValue { get; set; }

    [Column("new_value")]
    public string? NewValue { get; set; }

    [Column("created_at")]
    public DateTime? CreatedAt { get; set; }

    [ForeignKey("ChangedBy")]
    [InverseProperty("AuditLogs")]
    public virtual Usuario ChangedByNavigation { get; set; } = null!;

    [ForeignKey("TicketId")]
    [InverseProperty("AuditLogs")]
    public virtual Ticket Ticket { get; set; } = null!;
}
