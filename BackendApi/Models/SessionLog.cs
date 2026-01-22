using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackendApi.Models;

public class SessionLog
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int UserId { get; set; }

    [Required]
    [MaxLength(50)]
    public string Action { get; set; }

    [Required]
    public DateTime LoginTime { get; set; }

    public DateTime? LogoutTime { get; set; }

    [MaxLength(45)]
    public string IpAddress { get; set; }

    [ForeignKey("UserId")]
    public User User { get; set; }
}