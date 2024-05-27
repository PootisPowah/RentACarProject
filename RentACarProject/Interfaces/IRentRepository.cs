using RentACarProject.Dto;
using RentACarProject.Models;

namespace RentACarProject.Interfaces
{
    public interface IRentRepository
    {
            ICollection<Rent> GetRents();
            Rent GetRent(int customerId, int carId);
            Rent GetByRentId(int rentId);
            bool RentExists(int rentId);

            bool CreateRent(Rent rent, Customer customer, Car car);
            bool UpdateRent(Rent rent);

            bool DeleteRent(Rent rent);
            public Car GetCarById(int? carId);
            public Customer GetCustomerById(int? customerId);
            bool Save();
    }
}
