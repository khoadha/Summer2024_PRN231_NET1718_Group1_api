using BusinessObjects.ConfigurationModels;

using BusinessObjects.Entities;
using Microsoft.EntityFrameworkCore;
using Repositories.ReportRepository;
using System.Composition;


namespace Hosteland.Services.ReportService
{
    public class ReportService : IReportService
    {


        private readonly IReportRepository _repository;
        public ReportService(IReportRepository reportRepository)
        {
            _repository = reportRepository;
        }
        public async Task<ServiceResponse<List<Report>>> GetReports()
        {
            var result = new ServiceResponse<List<Report>>();
            try
            {
                var response = await _repository.GetReports();
                result.Data = response;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.Message;
            }
            return result;
        }

        public async Task<ServiceResponse<List<Report>>> GetLatestReports()
        {
            var result = new ServiceResponse<List<Report>>();
            try
            {
                var response = await _repository.GetLatestReports();
                result.Data = response;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.Message;
            }
            return result;
        }


        public async Task<ServiceResponse<List<Report>>> GetReportsByUserId(string userId)
        {
            var result = new ServiceResponse<List<Report>>();
            try
            {
                var response = await _repository.GetReportsByUserId(userId);
                result.Data = response;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.Message;
            }
            return result;
        }


        public async Task<ServiceResponse<Report>> AddReport(Report report)
        {
            var result = new ServiceResponse<Report>();
            try
            {
                var response = await _repository.AddReport(report);
                result.Data = response;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.Message;
            }
            return result;
        }


        public async Task<ServiceResponse<Report>> GetReportById(int id)
        {
            var result = new ServiceResponse<Report>();
            try
            {
                var response = await _repository.GetReportById(id);
                result.Data = response;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.Message;
            }
            return result;
        }


        public async Task<ServiceResponse<Report>> UpdateReport(int id, Report report)
        {
            var result = new ServiceResponse<Report>();
            try
            {
                var response = await _repository.UpdateReport(id, report);
                result.Data = response;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.Message;
            }
            return result;
        }




        public async Task<ServiceResponse<bool>> SaveAsync()
        {
            var result = new ServiceResponse<bool>();
            try
            {
                var response = await _repository.SaveAsync();
                result.Data = response;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.Message;
            }
            return result;
        }
    }
}
