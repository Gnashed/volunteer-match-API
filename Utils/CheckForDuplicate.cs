using VolunteerMatch.Data.SampleData;

namespace VolunteerMatch.Utils;

public class CheckForDuplicate
{
  // Duplicate organization
  public bool IsDuplicate(string name)
  {
    var existingOrganizations = OrganizationData.GetOrganizations().ToList();

    if (existingOrganizations.Any(o => o.Name == name))
    {
      Console.WriteLine("Duplicate name: " + name);
      return true;
    }
    return false;
  }
}

