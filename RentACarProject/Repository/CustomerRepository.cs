using RentACarProject.Data;
using RentACarProject.Interfaces;
using RentACarProject.Models;

namespace RentACarProject.Repository
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly DataContext _context;
        public CustomerRepository(DataContext context)
        {
            _context = context;
        }

       

        public Customer GetCustomerById(int id)
        {
            return _context.Customers.Where(c=>c.Id == id).FirstOrDefault();
        }

        public Customer GetCustomerByName(string fname)
        {
            return _context.Customers.Where(c => c.Fname == fname).FirstOrDefault();
        }

        public bool CustomerExists(int custId)
        {
            return _context.Customers.Any(c=>c.Id== custId);
        }
        public ICollection<Customer> GetCustomers()
        {
            return _context.Customers.OrderBy(c=>c.Id).ToList();
        }
        public bool CustomerExistsByName(string fname)
        {
            return _context.Customers.Any(c=>c.Fname== fname);
        }

        public bool CreateCustomer(Customer customer)
        {
            _context.Add(customer);
            return Save();
        }
        public bool UpdateCustomer(Customer customer)
        {
            _context.Update(customer);
            return Save();
        }

        public bool DeleteCustomer(Customer customer)
        {
            var hasRents = _context.Rents.Any(r => r.CustomerId == customer.Id);
            if (hasRents)
            {
                return false; // Or throw a custom exception
            }
            _context.Remove(customer);
            return Save();
        }
        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

       
    }
}
