namespace ReportHub.DAL.Models;

public class Report
{
    public string Path { get; set; }
    public DateTime GeneratedAt { get; set; }

    public Report(string path)
    {
        Path = path;
        GeneratedAt = DateTime.Now;
    }
}
