using System.ComponentModel.DataAnnotations;

namespace Vivarni.Example.API.ApiModels;

public record GuestMessageWriteModel([Required] string GuestMessage, [Required] string CreatedByUser);
