using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PickupAnnouncerLegacy.Interfaces;
using PickupAnnouncerLegacy.Models.DAO.Config;
using PickupAnnouncerLegacy.Models.Requests;
using PickupAnnouncerLegacy.Models.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PickupAnnouncerLegacy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GradeLevelController : ControllerBase
    {
        private readonly IDbHelper _dbHelper;
        private readonly IMapper _mapper;

        public GradeLevelController(IDbHelper dbHelper, IMapper mapper)
        {
            _dbHelper = dbHelper;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IEnumerable<GradeLevel>> Get()
        {
            return await _dbHelper.GetGradeLevelConfig();
        }

        [HttpPost]
        public async Task<StatusResponse> Post([FromBody] GradeLevelRequest request) => await ProcessGradeLevel(request);

        private async Task<StatusResponse> ProcessGradeLevel(GradeLevelRequest request)
        {
            StatusResponse response = new StatusResponse();
            var gradeLevel = _mapper.Map<GradeLevel>(request);
            if (request.Id.HasValue)
            {
                response.Success = await _dbHelper.UpdateGradeLevel(gradeLevel);
                response.Message = response.Success ? "Successfully updated grade level" : "Failed to update grade level";
            }
            else
            {
                response.Success = await _dbHelper.InsertGradeLevel(gradeLevel);
                response.Message = response.Success ? "Successfully inserted grade level" : "Failed to insert grade level";
            }
            return response;
        }

        [HttpDelete]
        public async Task<StatusResponse> Delete(int id)
        {
            StatusResponse response = new StatusResponse();
            response.Success = await _dbHelper.DeleteGradeLevelConfig(id);
            response.Message = response.Success ? "Successfully deleted grade level" : "Failed to delete grade level";
            return response;
        }
    }
}