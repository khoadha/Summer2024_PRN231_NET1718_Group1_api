using BusinessObjects.Entities;

namespace Repositories.ReportRepository
{
    public interface IReportRepository
    {
        Task<List<Report>> GetReports();
        Task<List<Report>> GetReportsByUserId(string userId);
        Task<Report> GetReportById(int id);
        Task<Report> UpdateReport(int id, Report report);
        Task<Report> AddReport(Report Report);
        Task<bool> SaveAsync();
    }
}
