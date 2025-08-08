using TestTask.Models;
using TestTask.Services.Users;
using System.Linq;

namespace TestTask.Services.Meetings;

public class MeetingService : IMeetingService
{
    private readonly IUserService _userService;

    private readonly List<Meeting> _meetings = new();

    private int _nextId = 1;

    private static readonly TimeSpan BusinessDayStart = new(9, 0, 0);
    private static readonly TimeSpan BusinessDayEnd = new(17, 0, 0);

    public MeetingService(IUserService userService)
    {
        _userService = userService;
    }

    public Meeting CreateNewMeeting(List<int> participantIds, TimeSpan durationMinutes, DateTime earliestStart,
        DateTime latestEnd)
    {

        foreach (var id in participantIds)
        {
            if (_userService.GetUserById(id) == null)
                throw new ArgumentException($"User with ID {id} does not exist.");
        }

        if (participantIds == null || participantIds.Count == 0)
        {
            throw new ArgumentException("At least one participant must be specified.");
        }

        if (durationMinutes <= TimeSpan.Zero)
        {
            throw new ArgumentException("Duration must be greater than zero.");
        }

        if (earliestStart.Kind != DateTimeKind.Utc)
        {
            earliestStart = earliestStart.ToUniversalTime();
        }
        if (latestEnd.Kind != DateTimeKind.Utc)
        {
            latestEnd = latestEnd.ToUniversalTime();
        }

        (earliestStart, latestEnd) = ClampToBusinessHours(earliestStart, latestEnd);

        DateTime? startTime = GetEarliestAvailableSlot(participantIds, durationMinutes, earliestStart, latestEnd);
        if (startTime == null)
        {
            throw new InvalidOperationException("No available time slot found.");
        }
        DateTime endTime = startTime.Value.Add(durationMinutes);

        var meeting = new Meeting
        {
            Id = _nextId++,
            Participants = participantIds,
            StartTime = startTime.Value,
            EndTime = endTime
        };
        _meetings.Add(meeting);
        return meeting;
    }

    private DateTime? GetEarliestAvailableSlot(List<int> participantIds, TimeSpan durationMinutes, DateTime earliestStart, DateTime latestEnd)
    {
        var relevantMeetings = _meetings.Where(m => m.Participants.Any(p => participantIds.Contains(p)) &&
                                                  m.EndTime > earliestStart && m.StartTime < latestEnd)
            .OrderBy(m=>m.StartTime).ToList();

        if (relevantMeetings.Count > 0)
        {
            var firstMeetingStart = relevantMeetings.First().StartTime;
            if (firstMeetingStart - earliestStart >= durationMinutes)
            {
                return earliestStart;
            }
        }
        else
        {
            if (latestEnd - earliestStart >= durationMinutes)
            {
                return earliestStart;
            }

            return null;
        }
        
        for (int i = 0; i < relevantMeetings.Count - 1; i++)
        {
            var endOfCurrent = relevantMeetings[i].EndTime;
            var startOfNext = relevantMeetings[i + 1].StartTime;

            if (startOfNext - endOfCurrent >= durationMinutes)
            {
                return endOfCurrent;
            }
        }

        var lastMeetingEnd = relevantMeetings.Last().EndTime;
        if (lastMeetingEnd + durationMinutes <= latestEnd)
        {
            return lastMeetingEnd;
        }

        return null;
    }

    public List<Meeting> GetAllMeetingForUser(int userId)
    {
        if (_userService.GetUserById(userId) == null)
            throw new ArgumentException($"User with ID {userId} does not exist.");
        return _meetings.Where(m => m.Participants.Contains(userId)).ToList();
    }

    public Meeting? GetMeetingById(int id)
    {
        return _meetings.FirstOrDefault(m => m.Id == id);
    }

    private (DateTime, DateTime) ClampToBusinessHours(DateTime earliestStart, DateTime latestEnd)
    {
        var businessDay = earliestStart.Date;

        var businessStart = businessDay.Add(BusinessDayStart);
        var businessEnd = businessDay.Add(BusinessDayEnd);

        if (earliestStart < businessStart) earliestStart = businessStart;
        if (latestEnd > businessEnd) latestEnd = businessEnd;

        if (latestEnd < earliestStart)
            latestEnd = earliestStart;

        return (earliestStart, latestEnd);
    }
}