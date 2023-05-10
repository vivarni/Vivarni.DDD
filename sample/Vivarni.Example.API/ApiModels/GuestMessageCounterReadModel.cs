using System.ComponentModel.DataAnnotations;

namespace Vivarni.Example.API.ApiModels;

public record GuestMessageCounterReadModel([Required] int Count, [Required] string CreatedByUser);
