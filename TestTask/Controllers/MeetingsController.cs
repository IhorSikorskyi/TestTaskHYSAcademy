using Microsoft.AspNetCore.Mvc;
using TestTask.Services.Meetings;
using TestTask.Services.Users;
using TestTask.Models;
using TestTask.DTOs.Meetings;

namespace TestTask.Controllers
{
    [ApiController]
    [Route("meetings")]
    public class MeetingsController : ControllerBase
    {
        private readonly IMeetingService _meetingService;

        public MeetingsController(IMeetingService meetingService)
        {
            _meetingService = meetingService;
        }

        [HttpPost]
        public ActionResult<MeetingResponse> CreateMeeting([FromBody] CreateMeetingRequest request)
        {
            var meeting = _meetingService.CreateNewMeeting(
                request.ParticipantIds,
                TimeSpan.FromMinutes(request.DurationMinutes),
                request.EarliestStart,
                request.LatestEnd
            );

            var response = new MeetingResponse
            {
                Id = meeting.Id,
                ParticipantIds = meeting.Participants,
                StartTime = meeting.StartTime,
                EndTime = meeting.EndTime
            };

            return CreatedAtAction(nameof(GetMeetingById), new { id = meeting.Id }, response);
        }

        [HttpGet("{id}")]
        public ActionResult<MeetingResponse> GetMeetingById(int id)
        {
            var meeting = _meetingService.GetMeetingById(id);
            if (meeting == null)
                return NotFound();

            return new MeetingResponse
            {
                Id = meeting.Id,
                ParticipantIds = meeting.Participants,
                StartTime = meeting.StartTime,
                EndTime = meeting.EndTime
            };
        }

        [HttpGet("{userId}/meetings")]
        public ActionResult<List<MeetingResponse>> GetMeetingsForUser(int userId)
        {
            var meetings = _meetingService.GetAllMeetingForUser(userId);

            var response = meetings.Select(m => new MeetingResponse
            {
                Id = m.Id,
                ParticipantIds = m.Participants,
                StartTime = m.StartTime,
                EndTime = m.EndTime
            }).ToList();

            return response;
        }

    }
}
