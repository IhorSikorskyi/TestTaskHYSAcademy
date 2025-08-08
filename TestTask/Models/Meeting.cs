namespace TestTask.Models;

public class Meeting
{
    public int Id { get; set; }
    public List<int> Participants { get; set; } = new();
    public DateTime StartTime { get; set; } = DateTime.MinValue;
    public DateTime EndTime { get; set; } = DateTime.MinValue;
}