namespace RentACarWebsite.Models
{
    public  class RentCustCarModel
    {
        public RentModel Rent { get; set; }
        public IEnumerable<RentModel> Rents { get; set; } // Added Rents collection
        public IEnumerable<CustomerModel> Customers { get; set; }
        public IEnumerable<CarModel> Cars { get; set; }
    }
}