namespace Ensek.Domain.Entities
{
  using System;
  using System.Collections.Generic;

  public class Employee
  {
    public ICollection<EmployeeTerritory> EmployeeTerritories { get; private set; }
    // public ICollection<Employee> DirectReports { get; private set; }
    // public Employee Manager { get; set; }
    
    public Employee()
    {
      EmployeeTerritories = new HashSet<EmployeeTerritory>();
      // DirectReports = new HashSet<Employee>();
    }

    public int EmployeeId { get; set; }
    public string LastName { get; set; }
    public string FirstName { get; set; }
  }
}