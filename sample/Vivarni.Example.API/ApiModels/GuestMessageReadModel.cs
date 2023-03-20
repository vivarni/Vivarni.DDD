namespace Vivarni.Example.API.ApiModels;

public record GuestMessageReadModel(Guid id, string Message, string CreatedBy, DateTime CreationTime);
