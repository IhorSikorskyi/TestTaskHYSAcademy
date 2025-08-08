# TestTaskHYSAcademy

Intern Test Task for HYS Academy for Back-End Course

# Setup instractions

  #Users
  To create a new user, you should provide the user's Name as a parameter.
  <img width="1529" height="807" alt="image" src="https://github.com/user-attachments/assets/66b27177-0d22-4374-aeca-d6ecbe0e3593" />

  After that, the service generates a unique Id for the new user and returns their information.
  <img width="1499" height="374" alt="image" src="https://github.com/user-attachments/assets/201314dd-1a2e-4b31-b053-dfa363e6ee64" />

  Also, you can search for a user by their Id.
  <img width="1476" height="279" alt="image" src="https://github.com/user-attachments/assets/de06e49f-15f1-43a0-baac-cdd56841520d" />

  #Meetings
  To create a new meeting, you should provide the meeting's participant IDs, the meeting duration in minutes, and the time window for scheduling.
  <img width="1456" height="439" alt="image" src="https://github.com/user-attachments/assets/3779a6bd-97be-423f-944c-fbf68d2370a9" />

  After that, the service search for avalible slots in thet lap. If lap is biger then Business Day, hten it will be corrected to Business Day's duration
  After that, the service searches for available slots within that time window. If the window is longer than the Business Day, it will be adjusted to fit within the Business Day’s duration.
  <img width="1429" height="400" alt="image" src="https://github.com/user-attachments/assets/b144cf82-72e9-48d0-bece-bc97be49e811" />

  Also, you can search for a meeting by its Id.
  <img width="1447" height="696" alt="image" src="https://github.com/user-attachments/assets/07ed15d3-d95d-4b80-b264-6c1420229710" />

  You can search for all meetings for a specific user by the user's Id.
  <img width="1473" height="736" alt="image" src="https://github.com/user-attachments/assets/0fd0a6e7-e53b-4639-8e8b-d7bc5afd96dc" />

  #Schemes
  You can find all schemas in the Schemas section.
  In total, we have four schemas for Requests and Responses for both Users and Meetings.
  <img width="1452" height="730" alt="image" src="https://github.com/user-attachments/assets/ae238781-06af-4a1d-99de-d555bd5deba1" />

#Limitations or edge

  #Business hours fixed per day:
  Business hours are hardcoded (09:00–17:00) and do not account for weekends, holidays, or varying schedules.

  #Overlapping
  No overlapping meetings are allowed for participants. However, meetings can overlap if they involve completely different participants.

  #No meeting updates or cancellations:
  There is no functionality to update or cancel meetings once created.

  #Limited error details:
  Exceptions provide basic messages but no detailed error codes or structured error responses.

#Passing xUnit tests
In total, we have a 15 xUnit tests. 8 for MeetingService loggic and 7 for UserService. All tests check for different expectations and pass successfully.
<img width="353" height="411" alt="image" src="https://github.com/user-attachments/assets/b639a491-7c45-4deb-bd82-5440293c5f89" />
