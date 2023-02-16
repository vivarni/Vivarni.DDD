using System.ComponentModel.DataAnnotations;

namespace Vivarni.Example.Shared.Shared.Models;

public record GuestMessageCreateDTO([Required] string GuestMessage, [Required] string CreatedByUser);
