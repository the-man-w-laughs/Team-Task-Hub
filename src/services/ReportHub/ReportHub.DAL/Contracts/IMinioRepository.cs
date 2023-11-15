namespace ReportHub.DAL.Contracts
{
    public interface IMinioRepository
    {
        Task<string> UploadReportAsync(Stream reportContent);
        Task<Stream> GetFileFromMinioAsync(string objectName);
        Task<string> DeleteFileFromMinioAsync(string objectName);
        Task DeleteFilesFromMinioAsync(IList<string> objectNames);
    }
}
