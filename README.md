# Activity Tracker
An activity tracker lets you keep track of your daily activities and see statistics.
## User stories
1. As a user, I want to fill in the data to track my active-passive peaks of the day(s) post
2. As a user, I want to compare activity with days(weeks/months) of progress 1 per 2 per 

3. As a user, I want to follow the daily goals for more than 2 weeks all 

## HTTP API
- PUT /api/user -> Returns 201 with userID
- GET /api/user/{userId} -> Returns 200 OK / 404 Not Found if no such user
- PUT /api/userActivity BODY=activity details + userId -> Returns 201 with userActivity ID / 404 Not Found if no such user
- GET /api/user/ 


## Models

### class User

- id
- name

### class Activity

- activity_id
- activity_type
- activity_dateTime
- activity_duration
- user_id