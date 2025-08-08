using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using TestTask.Models;
using TestTask.Services.Meetings;
using TestTask.Services.Users;
using Xunit;

namespace TestTask.Tests
{
    public class MeetingServiceTests
    {
        private Mock<IUserService> _userServiceMock;
        private MeetingService _meetingService;

        public MeetingServiceTests()
        {
            _userServiceMock = new Mock<IUserService>();
            _userServiceMock.Setup(us => us.GetUserById(It.IsAny<int>()))
                .Returns<int>(id => new User { Id = id, Name = $"User{id}" });

            _meetingService = new MeetingService(_userServiceMock.Object);
        }

        [Fact]
        public void CreateNewMeeting_ReturnsEarliestSlot_WhenSlotAvailable()
        {
            var participants = new List<int> { 1, 2 };
            var earliestStart = new DateTime(2025, 6, 20, 9, 0, 0, DateTimeKind.Utc);
            var latestEnd = new DateTime(2025, 6, 20, 17, 0, 0, DateTimeKind.Utc);
            var duration = TimeSpan.FromMinutes(60);

            var meeting = _meetingService.CreateNewMeeting(participants, duration, earliestStart, latestEnd);

            Assert.NotNull(meeting);
            Assert.Equal(participants, meeting.Participants);
            Assert.True(meeting.StartTime >= earliestStart);
            Assert.True(meeting.EndTime <= latestEnd);
            Assert.Equal(duration, meeting.EndTime - meeting.StartTime);
        }

        [Fact]
        public void CreateNewMeeting_ThrowsArgumentException_WhenUserDoesNotExist()
        {
            _userServiceMock.Setup(us => us.GetUserById(It.IsAny<int>())).Returns<User>(null);

            var participants = new List<int> { 1 };
            var earliestStart = DateTime.UtcNow.Date.AddHours(9);
            var latestEnd = earliestStart.AddHours(8);
            var duration = TimeSpan.FromMinutes(30);

            var ex = Assert.Throws<ArgumentException>(() =>
                _meetingService.CreateNewMeeting(participants, duration, earliestStart, latestEnd));

            Assert.Contains("does not exist", ex.Message);
        }

        [Fact]
        public void CreateNewMeeting_ThrowsInvalidOperationException_WhenNoSlotAvailable()
        {
            var participants = new List<int> { 1 };
            var earliestStart = new DateTime(2025, 6, 20, 9, 0, 0, DateTimeKind.Utc);
            var latestEnd = new DateTime(2025, 6, 20, 10, 0, 0, DateTimeKind.Utc);
            var duration = TimeSpan.FromMinutes(60);

            // Створимо зустріч, що займає весь доступний час
            _meetingService.CreateNewMeeting(participants, duration, earliestStart, latestEnd);

            // Тепер створення другої зустрічі на цей же час повинно кидати
            var ex = Assert.Throws<InvalidOperationException>(() =>
                _meetingService.CreateNewMeeting(participants, duration, earliestStart, latestEnd));

            Assert.Equal("No available time slot found.", ex.Message);
        }

        [Fact]
        public void GetAllMeetingForUser_ReturnsMeetingsForExistingUser()
        {
            var userId = 1;
            var participants = new List<int> { userId };
            var earliestStart = new DateTime(2025, 6, 20, 9, 0, 0, DateTimeKind.Utc);
            var latestEnd = new DateTime(2025, 6, 20, 17, 0, 0, DateTimeKind.Utc);
            var duration = TimeSpan.FromMinutes(60);

            var meeting = _meetingService.CreateNewMeeting(participants, duration, earliestStart, latestEnd);

            var meetings = _meetingService.GetAllMeetingForUser(userId);

            Assert.Single(meetings);
            Assert.Contains(meeting, meetings);
        }

        [Fact]
        public void GetAllMeetingForUser_ThrowsArgumentException_WhenUserDoesNotExist()
        {
            _userServiceMock.Setup(us => us.GetUserById(It.IsAny<int>())).Returns<User>(null);

            var ex = Assert.Throws<ArgumentException>(() =>
                _meetingService.GetAllMeetingForUser(123));

            Assert.Contains("does not exist", ex.Message);
        }

        [Fact]
        public void GetMeetingById_ReturnsMeeting_WhenExists()
        {
            var participants = new List<int> { 1 };
            var earliestStart = new DateTime(2025, 6, 20, 9, 0, 0, DateTimeKind.Utc);
            var latestEnd = new DateTime(2025, 6, 20, 17, 0, 0, DateTimeKind.Utc);
            var duration = TimeSpan.FromMinutes(60);

            var meeting = _meetingService.CreateNewMeeting(participants, duration, earliestStart, latestEnd);

            var foundMeeting = _meetingService.GetMeetingById(meeting.Id);

            Assert.NotNull(foundMeeting);
            Assert.Equal(meeting.Id, foundMeeting.Id);
        }

        [Fact]
        public void GetMeetingById_ReturnsNull_WhenNotExists()
        {
            var meeting = _meetingService.GetMeetingById(999);

            Assert.Null(meeting);
        }

        [Fact]
        public void ClampToBusinessHours_AdjustsTimesCorrectly()
        {
            var earliestStart = new DateTime(2025, 6, 20, 7, 0, 0, DateTimeKind.Utc);
            var latestEnd = new DateTime(2025, 6, 20, 19, 0, 0, DateTimeKind.Utc);

            var method = typeof(MeetingService).GetMethod("ClampToBusinessHours", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var adjustedTimes = (ValueTuple<DateTime, DateTime>)method.Invoke(_meetingService, new object[] { earliestStart, latestEnd });

            Assert.Equal(new DateTime(2025, 6, 20, 9, 0, 0, DateTimeKind.Utc), adjustedTimes.Item1);
            Assert.Equal(new DateTime(2025, 6, 20, 17, 0, 0, DateTimeKind.Utc), adjustedTimes.Item2);
        }
    }
}
