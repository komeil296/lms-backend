LMS Database Design

Entities

User
Represents system users(Student,Teacher,Admin)

Course
Represents educational courses created by teachers

Enrollment
Represents the relationship between students and courses

Exam
Represents exams created for a course

ExamrRsult
Represents the result of student in an exam
____________________________________________________________________________________

Relationships
  - one User (Teacher) -> Many Courses
  - Many Student <-> Many Courses (via Enrollment)
  - One Course -> Many Exams
  - One User -> Many ExamResults

