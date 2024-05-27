using RentACarProject.Models;

namespace RentACarProject.Interfaces
{
    public interface ICustomerRepository
    {
        ICollection<Customer> GetCustomers();
        Customer GetCustomerById(int id);
        Customer GetCustomerByName(string fname);
        bool CustomerExists(int custId);
        bool CustomerExistsByName(string fname);
        bool CreateCustomer(Customer customer);

        bool UpdateCustomer(Customer customer);

        bool DeleteCustomer(Customer customer);
        bool Save();
    }
}
