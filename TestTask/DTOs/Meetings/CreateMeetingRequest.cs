namespace TestTask.DTOs.Meetings;

public class CreateMeetingRequest
{
    public List<int> ParticipantIds { get; set; } = new();
    public int DurationMinutes { get; set; }
    public DateTime EarliestStart { get; set; }
    public DateTime LatestEnd { get; set; }
}