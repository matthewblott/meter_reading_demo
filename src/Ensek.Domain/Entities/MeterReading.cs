namespace Ensek.Domain.Entities
{
  using System.Collections.Generic;

  public class MeterReading
  {
    public int Id { get; set; }
    public int AccountId { get; set; }
    public string RecordedAt { get; set; }
    public int Value { get; set; }

    public Account Account { get; set; }
  }
}