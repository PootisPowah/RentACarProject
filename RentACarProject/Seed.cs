using RentACarProject.Data;
using RentACarProject.Models;

namespace RentACarProject
{
    public class Seed
    {
        private readonly DataContext dataContext;
        public Seed(DataContext context)
        {
            this.dataContext = context;
        }
        public void SeedDataContext()
        {
            if (!dataContext.Customers.Any())
            {
                var customers = new List<Customer>()
        {
            new Customer()
            {
                Fname = "Kristiyan",
                Lname = "Kraev",
                Gender="Male",
                Age=20,
                Salary=0,
                Birthday=new DateTime(2003, 6, 20)
            },
        };

                dataContext.Customers.AddRange(customers);
                dataContext.SaveChanges();
            }

            if (!dataContext.Cars.Any())
            {
                var cars = new List<Car>()
        {
            new Car()
            {
                Brand = "Toyota",
                Model = "Camry",
                Year = "2024",
                Horsepower=150,
                IsAvailable=true
            },
        };

                dataContext.Cars.AddRange(cars);
                dataContext.SaveChanges();
            }

            if (!dataContext.Rents.Any())
            {
                var rent = new Rent()
                {
                    CustomerId = 1, 
                    CarId = 1,
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now.AddDays(7),
                    RentPrice = 100,
                    PickupStreet = "Trendafil 2"
                };

                dataContext.Rents.Add(rent);
                dataContext.SaveChanges();
            }
          
        }



    }
}