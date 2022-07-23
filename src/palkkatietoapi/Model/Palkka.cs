namespace palkkatietoapi.Model;

public class Palkka {
    public long Id { get; set; }

    public User User { get; set; }

    public decimal Amount { get; set;}

    public string Company { get; set; }

    public string JobRole { get; set; }

    public string CountryCode {get; set;} = "FI";

    public DateTime DateReported { get; set;}

    public DateTime? Created  {get; set; }

}

