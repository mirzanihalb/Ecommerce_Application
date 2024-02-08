using Ecom_Case_Study.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom_Case_Study.Service
{
    internal interface IOrderProcessorService
    {
        public Customer Login();
        public void CreateProduct();

        public void DeleteProduct();

        public Customer CreateCustomer();

        public void DeleteCustomer();

        public void AddToCart();

        public void RemoveFromCart();

        public int GetAllFromCart();

        public void PlaceOrder();

        public void GetOrdersByCustomer();

        public List<Product> ProductsDisplay();

        
    }

}
