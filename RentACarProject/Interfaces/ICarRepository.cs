using RentACarProject.Models;

namespace RentACarProject.Interfaces
{
    public interface ICarRepository
    {
        ICollection<Car> GetCars();
        Car GetCarById(int id);
        //ICollection<Car> GetCarsByRent(int carId);
        bool CarExists(int id);
        bool CreateCar(Car car);

        bool UpdateCar(Car car);
        bool DeleteCar(Car car);

        bool IsCarAvailableForRent(int carId);

        bool Save();

    }
}
