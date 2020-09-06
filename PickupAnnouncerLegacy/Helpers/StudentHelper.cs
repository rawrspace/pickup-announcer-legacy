using AutoMapper;
using PickupAnnouncerLegacy.Interfaces;
using PickupAnnouncerLegacy.Models.DTO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PickupAnnouncerLegacy.Helpers
{
    public class StudentHelper : IStudentHelper
    {
        private readonly IDbHelper _dbHelper;
        private readonly IMapper _mapper;

        public StudentHelper(IDbHelper dbHelper, IMapper mapper)
        {
            _dbHelper = dbHelper;
            _mapper = mapper;
        }

        public async Task<IEnumerable<StudentDTO>> GetStudentsForCar(int carId)
        {
            var studentRecords = await _dbHelper.GetStudentsForRegistrationId(carId);
            var gradeLevels = studentRecords.Select(x => x.GradeLevel).Distinct();
            var gradeLevelConfigs = await _dbHelper.GetGradeLevelConfig(gradeLevels);
            return studentRecords.Select(x =>
            {
                var student = _mapper.Map<StudentDTO>(x);
                if (gradeLevelConfigs.TryGetValue(student.GradeLevel, out var gradeLevelConfig))
                {
                    student.BackgroundColor = gradeLevelConfig.BackgroundColor;
                    student.TextColor = gradeLevelConfig.TextColor;
                }
                return student;
            });
        }
    }
}
