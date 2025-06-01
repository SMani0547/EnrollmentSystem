from database import db

class Grade(db.Model):
    __tablename__ = 'Grades'

    id = db.Column(db.Integer, primary_key=True)
    student_id = db.Column("StudentId", db.String(20), nullable=False)
    course_id = db.Column("CourseId", db.String(20), nullable=False)
    marks = db.Column("Marks", db.Integer, nullable=False)
    grade_letter = db.Column("GradeLetter", db.String(5), nullable=False)
