using BusinessObjects.ConfigurationModels;

using BusinessObjects.Entities;


namespace Hosteland.Services.ReportService
{
    public interface IReportService
    {
        Task<ServiceResponse<List<Report>>> GetReports();
        Task<ServiceResponse<List<Report>>> GetReportsByUserId(string userId);
        Task<ServiceResponse<Report>> GetReportById(int id);
        Task<ServiceResponse<Report>> UpdateReport(int id, Report report);
        Task<ServiceResponse<Report>> AddReport(Report Report);
        Task<ServiceResponse<bool>> SaveAsync();
    }
}
