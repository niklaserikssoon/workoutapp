💪 Backend Builders – Workout API
📌 Project Overview
RESTful API for a workout application
Focus on performance, security and clean architecture
Built using agile methodology and team collaboration
Supports workout creation, user management and exercise handling

⚙️ Technologies
.NET (ASP.NET Core Web API)
Entity Framework Core
SQL Database (based on ER diagram)
Swagger / Scalar (API documentation)
JWT Authentication
GitHub Actions (CI/CD)
Notion (project management & documentation)

🧠 Architecture
Clean structure using:
Controllers
Services
DTOs
Entities
Separation between internal models and public API
ER Diagram used → implemented as SQL database

📊 Controllers (Overview)
UserController
Handles users
Login, profile, admin actions

WorkoutController
Manages workouts
Create, update, fetch workouts

ExerciseController
Handles exercise data
Fetch exercises, integrate external data

🧩 Services (Overview)
ExerciseService
Handles Exercise data
Create, update, fetch workouts (SQL database)

ExternalExerciseService
Fetches exercise data from external JSON
Returns exercises to the system

SaveExerciseService
Saves external exercises into the system
Enables creating workouts from saved exercises

UserService
Handles user data
Register, manage users


✍️ Notion
📄 What it is used for
Central place for:
Planning
Documentation
Team collaboration

📜 Group Contract
Defined:
Roles & responsibilities
Coding standards
Git workflow
Ensured:
Clear structure in team
Accountability

📅 Daily Standup Documentation
Each member logs:
What was done
What will be done
Problems
Helps:
Track progress
Improve communication
🗂️ Epic & Task Breakdown
Work divided into:
Epics (big goals)
User stories
Tasks
Organized in:
Sprint boards
Status tracking:
Not started / In progress / Review / Done

🔄 Agile Workflow
Sprint-based development
Daily standups
Sprint planning & retrospectives
Continuous improvement

🔀 Git & Version Control
Branch Strategy
main → protected(no direct merge)
dev → development
feature/* → new features

Pull Requests & CI
PR required
CI must pass
1 code review required
Then merge is allowed

🧪 Testing & Quality
Basic Unit testing
Error handling
Validation using Data Annotations
Custom filters implemented

🔐 Security
JWT Authentication
CORS policy configured
Input validation

🚀 Performance
Pagination implemented
Caching (basic)
Rate limiting
Optimized data handling

🔗 External Integrations
HTTP Client used
External exercise data source
Clean integration via services

🗄️ Database
Designed using ER Diagram
Implemented with SQL
Relationships between:
Users
Workouts
Exercises

📘 API Documentation
Swagger / Scalar used
Clear endpoints
XML comments for documentation

👥 Team
Adchariya Changtam
Jordan Foose
Niklas Eriksson
Abdalle Abdulkadir
Robin Markström

🧠 Reflection
Learned:
Agile teamwork
API design
CI/CD workflow
Clean architecture
Improved:
Communication
Code structure
Problem solving