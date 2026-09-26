namespace DatNetCoreLAB6_EF.Models
{
    public class DatErrorViewModel
    {
        public string? RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
