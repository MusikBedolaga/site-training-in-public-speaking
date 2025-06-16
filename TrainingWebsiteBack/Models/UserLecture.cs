namespace TrainingWebsiteBack.Models;

public class UserLecture
{
    public int Id { get; set; }

    public int UserId { get; set; }
    public User User { get; set; }

    public int LectureId { get; set; }
    public Lecture Lecture { get; set; }
}
