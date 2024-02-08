using Ecom_Case_Study.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom_Case_Study.Repository
{
    internal interface IOrderProcessorRepository
    {
        public Customer Login(string email);

        public bool CreateProduct(Product product);

        public List<Product> GetAllProducts();
        bool DeleteProduct(int productId);
        List<Customer> GetAllCustomers();
        bool DeleteCustomer(int customerId);
        Customer CreateCustomer(Customer customer);
        bool AddToCart(Customer loggedInUser, Product product_obj, int quantity);
        Dictionary<Product,int> GetAllFromCart(Customer loggedInUser);
        bool RemoveFromCart(Customer loggedInUser, Product product_obj_to_remove);
        List<Order> GetOrdersByCustomer(Customer loggedInUser);
        bool PlaceOrder(Customer loggedInUser, Dictionary<Product, int> cartitems, string? shippingAddress);

        public Dictionary<int, Product> GetAllProductsWithDictionary();

        public Dictionary<int, int> GetOrderItemsOfSpecificOrder(int orderId);

        
    }
}
