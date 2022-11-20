using System.ComponentModel.DataAnnotations;
namespace palkkatietoapi.Model;

public class User {
    public User() {
        Created = DateTime.UtcNow;
    }
    
    [Timestamp]
    public byte[] Timestamp { get; set; }
    public long Id { get; set; }
    public string Name { get; set;}
    public string Login { get; set; }
    public DateTime Created { get; set; }
    public DateTime? LastLogin { get; set; }
    public DateTime? Modified { get; set; }
}