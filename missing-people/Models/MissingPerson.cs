
namespace missing_people.Models;

public class MissingPerson
{
    public MissingPerson()
    {
    }


    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }
    public DateTime DateLastSeen { get; set; }
    public string LocationLastSeen { get; set; }
    public string ContactName { get; set; }
    public string ContactEmail { get; set; }

    public string Gender { get; set; }
}