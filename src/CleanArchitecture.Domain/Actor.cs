using CleanArchitecture.Domain.Common;

namespace CleanArchitecture.Domain;
public class Actor : BaseDomainModel
{
    public string Name { get; set; }
    public string LastName { get; set; }

    public virtual ICollection<Video> Videos { get; set; } = new HashSet<Video>();
}
