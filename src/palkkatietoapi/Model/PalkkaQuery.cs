namespace palkkatietoapi.Model;
public class PalkkaQuery {
    public int? AmountMin { get; set; }
    public int? AmountMax { get; set; }

    public string? Company { get; set; }

    public string? JobRole { get; set; }

    public string? CountryCode { get; set; }
}