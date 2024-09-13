public class Infos
{
    public string Mail;
    public string Program;
    public string Course;
    public string Lection;
    public int CurrentPosition;

    public Infos(string email, string program, string course, string lection, int position)
    {
        Mail = email;
        Program = program;
        Course = course;
        Lection = lection;
        CurrentPosition = position;
    }
}
