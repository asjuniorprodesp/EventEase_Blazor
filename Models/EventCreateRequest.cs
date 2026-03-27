using System.ComponentModel.DataAnnotations;

namespace EventEase_Blazor.Models;

public class EventCreateRequest
{
    [Required(ErrorMessage = "Informe o nome do evento.")]
    [StringLength(200)]
    public string EventName { get; set; } = string.Empty;

    [StringLength(4000)]
    public string? EventDescription { get; set; }

    [Required(ErrorMessage = "Informe a data do evento.")]
    public DateOnly? EventDate { get; set; }

    [Required(ErrorMessage = "Informe o local do evento.")]
    [StringLength(200)]
    public string EventLocation { get; set; } = string.Empty;
}
