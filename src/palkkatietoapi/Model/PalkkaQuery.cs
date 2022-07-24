namespace palkkatietoapi.Model;
public class PalkkaQuery {
    public long? UserId { get; set; }
    public int? AmountMin { get; set; }
    public int? AmountMax { get; set; }
    public string? Company { get; set; }
    public string? JobRole { get; set; }
    public string? City { get; set; }
    public string? CountryCode { get; set; }
}