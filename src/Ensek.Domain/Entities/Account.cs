namespace Ensek.Domain.Entities
{
  using System;
  using System.Collections.Generic;

  public class Account
  {
    public ICollection<MeterReading> Orders { get; private set; }
    public Account() => Orders = new HashSet<MeterReading>();

    public int Id { get; set; }
    public string LastName { get; set; }
    public string FirstName { get; set; }
  }
}