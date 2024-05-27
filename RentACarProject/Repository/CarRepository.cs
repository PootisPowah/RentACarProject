using Microsoft.EntityFrameworkCore;
using RentACarProject.Data;
using RentACarProject.Interfaces;
using RentACarProject.Models;

namespace RentACarProject.Repository
{
    public class CarRepository : ICarRepository
    {
        private readonly DataContext _context;

        public CarRepository(DataContext context)
        {
            _context = context;
        }
        
        public bool CarExists(int id)
        {
            return _context.Cars.Any(c=>c.Id == id);
        }

        

        public Car GetCarById(int id)
        {
            return _context.Cars.Where(c => c.Id == id).FirstOrDefault();
        }

    

        public ICollection<Car> GetCars()
        {
            return _context.Cars.OrderBy(c => c.Id).ToList();
        }
        //public ICollection<Car> GetCarsByRent(int carId)
        //{
        //    return _context.Rents.Where(c => c.CarId == carId).Select(c => c.Car).ToList();
        //}

        public bool CreateCar(Car car)
        {
            _context.Add(car);
            return Save();
        }


        public bool UpdateCar(Car car)
        {
            _context.Update(car);
            return Save();
        }

        public bool DeleteCar(Car car)
        {
            var hasRents = _context.Rents.Any(c => c.CarId == car.Id);
            if (hasRents)
            {
                return false;
            }
            _context.Remove(car);
            return Save();
        }

        public bool IsCarAvailableForRent(int carId)
        {
            var car = GetCarById(carId);
            return car != null && car.IsAvailable;
        }
        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

       




        //finish after rent is done

    }
}
