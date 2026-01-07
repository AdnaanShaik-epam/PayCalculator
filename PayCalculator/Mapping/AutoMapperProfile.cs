using AutoMapper;
using PayCalculator.Models;
using PayCalculator.DTO;

namespace PayCalculator.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // Employee mappings
            CreateMap<Employee, EmployeeDto>().ReverseMap();

            // Attendance/time entry mappings
            CreateMap<TimeEntry, TimeEntryDto>().ReverseMap();
            CreateMap<BreakEntry, BreakEntryDto>().ReverseMap();

            // Salary related mappings
            CreateMap<SalaryInfo, SalaryCalculationDto>().ReverseMap();

            // Auth/User mappings
            CreateMap<User, LoginDto>().ReverseMap();
        }
    }
}
