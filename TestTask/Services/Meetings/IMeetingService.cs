using TestTask.Models;

namespace TestTask.Services.Meetings;

public interface IMeetingService
{
    Meeting CreateNewMeeting(List<int> participantIds, TimeSpan durationMinutes, DateTime earliestStart, DateTime latestEnd);
    List<Meeting> GetAllMeetingForUser(int userId);
    Meeting? GetMeetingById(int id);
}