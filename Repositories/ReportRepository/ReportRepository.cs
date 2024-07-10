using BusinessObjects.Entities;

using BusinessObjects.Enums;
using Microsoft.EntityFrameworkCore;


namespace Repositories.ReportRepository
{
    public class ReportRepository : IReportRepository
    {


        private readonly AppDbContext _context;


        public ReportRepository(AppDbContext context)
        {
            _context = context;


        }


        public async Task<Report> AddReport(Report report)
        {
            try
            {
                report.Status = MaintainanceStatus.Pending;
                report.CreatedDate = DateTime.Now;
                await _context.Reports.AddAsync(report);
                await SaveAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
            return report;
        }


        public async Task<Report> GetReportById(int id)
        {
            return await _context.Reports
            .Include(a => a.Author)
            .Include(a => a.Room)
            .FirstOrDefaultAsync(a => a.Id == id);
        }


        public async Task<List<Report>> GetReports()
        {
            return await _context.Reports
            .Include(a => a.Author)
            .Include(a => a.Room)
            .ToListAsync();
        }


        public async Task<Report> UpdateReport(int id, Report report)
        {
            try
            {
                var reportFromDb = await _context.Reports.FirstOrDefaultAsync(a => a.Id == id);
                if (reportFromDb != null)
                {


                    if (!string.IsNullOrEmpty(report.Reply))
                    {
                        reportFromDb.Reply = report.Reply;
                    }


                    if (report.Status == MaintainanceStatus.Rejected)
                    {


                        reportFromDb.EndDate = DateTime.Now;
                        reportFromDb.Status = MaintainanceStatus.Rejected;


                    }
                    else if (report.Status == MaintainanceStatus.Processing)
                    {


                        reportFromDb.StartDate = DateTime.Now;
                        reportFromDb.Status = MaintainanceStatus.Processing;


                    }
                    else if (report.Status == MaintainanceStatus.Completed)
                    {


                        if (reportFromDb.Status != MaintainanceStatus.Processing)
                        {
                            return null;
                        }


                        reportFromDb.EndDate = DateTime.Now;
                        reportFromDb.Status = MaintainanceStatus.Completed;
                    }
                    _context.Reports.Update(reportFromDb);
                    await SaveAsync();
                    return reportFromDb;
                }
            }
            catch (Exception ex)
            {
                throw;
            }


            return report;
        }


        public async Task<List<Report>> GetReportsByUserId(string userId)
        {
            return await _context.Reports
            .Where(a => a.AuthorId == userId)
            .Include(a => a.Author)
            .Include(a => a.Room)
            .ToListAsync();


        }


        public async Task<bool> SaveAsync()
        {
            return await _context.SaveChangesAsync() > 0;


        }
    }
}
