namespace TestTask.DTOs.Meetings;

public class MeetingResponse
{
    public int Id { get; set; }
    public List<int> ParticipantIds { get; set; } = new();
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
}