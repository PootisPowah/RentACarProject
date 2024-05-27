using AutoMapper;
using RentACarProject.Dto;
using RentACarProject.Models;

namespace RentACarProject.Helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Customer, CustomerDto>();
            CreateMap<CustomerDto, Customer>();

            CreateMap<Car,CarDto>();
            CreateMap<CarDto, Car>();


            CreateMap<Rent, RentDto>(); 
            CreateMap<RentDto, Rent>(); 
        }
    }
}
