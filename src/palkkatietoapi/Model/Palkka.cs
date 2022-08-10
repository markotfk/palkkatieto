namespace palkkatietoapi.Model;

public class Palkka {

    public Palkka() {
        Created = DateTime.UtcNow;
    }

    public long Id { get; set; }

    public User User { get; set; }

    public decimal Amount { get; set;}

    public string Company { get; set; }

    public string JobRole { get; set; }

    public string City { get; set; }

    public string CountryCode {get; set;} = "fi";

    public DateTime DateReported { get; set;}

    public DateTime Created  {get; set; }

    public DateTime? Modified { get; set; }

}

