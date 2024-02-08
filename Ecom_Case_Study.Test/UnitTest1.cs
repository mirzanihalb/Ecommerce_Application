using Ecom_Case_Study.Repository;
using Ecom_Case_Study.Model;
using System.Xml.Linq;
using Ecom_Case_Study.Exceptions;
namespace Ecom_Case_Study.Test
{
    public class Tests
    {

        OrderProcessorRepositoryImpl repository = null;
        [SetUp]
        public void Setup()
        {
            repository = new OrderProcessorRepositoryImpl();
        }

        [Ignore("already tested")]
        [Test]
        public void When_Product_Created_Successfully()
        {
            Product newProduct = new Product
            {
                Name = "Test Product4",
                Price = 29.99m,
                Description = "This is a test product for unit testing.",
                StockQuantity = 50
            };

            // Act
            bool productId = repository.CreateProduct(newProduct);

            // Assert
            List<Product> product_list = repository.GetAllProducts();
            Product retrived_obj = product_list.Find(x => x.ProductId == newProduct.ProductId);

            Assert.IsNotNull(retrived_obj);

            
        }


        [Test]
        [Ignore("already tested")]
        public void When_Product_added_To_Cart_Successfully()
        {
            Product product_obj = new Product
            {
                ProductId = 26,
                Name = "Test Product44",
                Price = 29.99m,
                Description = "This is a test product for unit testing.",
                StockQuantity = 50
            };

            Customer customer = new Customer
            {
                CustomerId = 1,
                Name = "Nihal",
                Email = "nihal@gmail.com",
                Password = "password1",
                CustomerType = "Customer"

            };

            bool addedToCart = repository.AddToCart(customer, product_obj, 2);

            Assert.IsTrue(addedToCart);
        }
        [Test]
        [Ignore("already tested")]
        public void When_Product_Ordered_Successfully()
        {
            Customer customer = new Customer
            {
                CustomerId = 18,
                Name = "rahim",
                Email = "rahim@gmail.com",
                Password = "passwordrahim",
                CustomerType = "Customer"
            };
            Dictionary<Product, int> cartItems = new Dictionary<Product, int>()
            {
                { new Product { ProductId = 1, Price = 1200 }, 2 },
                
            };

            string shippingAddress = "66-66 street-7 vijayawada";

            List<Order> orders = repository.GetOrdersByCustomer(customer);
            int count_before_order = orders.Count();
            bool status = repository.PlaceOrder(customer,cartItems, shippingAddress);
            List<Order> ordersAfter = repository.GetOrdersByCustomer(customer);

            Assert.IsTrue(status);

            Assert.AreEqual(ordersAfter.Count(),orders.Count()+1);


        }


        [Test]
        [Ignore("already tested")]
        public void When_Customer_Not_Found_Throws_Exception()
        {
            Customer customer = new Customer { CustomerId = 59 };

            var ex = Assert.Throws<CustomerNotFoundException>(() => repository.DeleteCustomer(customer.CustomerId));

            Assert.AreEqual("No Such Customer Exists to Delete", ex.Message);
        }

        [Test]
        public void When_Product_Not_Found_Throws_Exception()
        {
            Product product = new Product { ProductId = 58 };

            var ex = Assert.Throws<ProductNotFoundException>(() => repository.DeleteProduct(product.ProductId));
            
            Assert.AreEqual("No Such Product to delete", ex.Message);
        }
    }
    
}