using Ecom_Case_Study.Exceptions;
using Ecom_Case_Study.Model;
using Ecom_Case_Study.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Ecom_Case_Study.Service
{
    internal class OrderProcessorServiceImpl : IOrderProcessorService
    {
        readonly IOrderProcessorRepository _orderProcessorRepository;

        Customer LoggedInUser;

        public OrderProcessorServiceImpl()
        {
            _orderProcessorRepository = new OrderProcessorRepositoryImpl();
            LoggedInUser = null;
        }



        public Customer Login()
        {
            try
            {
                Console.Write("Enter Email    : ");

                Console.ForegroundColor = ConsoleColor.Cyan;
                string email = Console.ReadLine();
                Console.ResetColor();
                Console.Write("Enter Password : ");
                Console.ForegroundColor = ConsoleColor.Cyan;
                string password = Console.ReadLine();
                Console.ResetColor();
                Customer customerObj = _orderProcessorRepository.Login(email);






                if (customerObj != null && customerObj.Password != password)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("The password you have entered is wrong please check");
                    Console.ResetColor();


                }
                else if (customerObj != null)
                {
                    LoggedInUser = customerObj;
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Successfully Logged In");
                    Console.ResetColor();
                    return LoggedInUser;
                }



            }
            catch (CustomerNotFoundException ex)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine(ex.Message);
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine(ex.Message);
                Console.ResetColor();
            }

            return null;

        }
        public void AddToCart()
        {
            //call the method to get all the List<produts> display them with their id
            try
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("Add Products to Your Cart"); Console.ResetColor();
                List<Product> products = ProductsDisplay();

                Console.WriteLine(new string('-', Console.WindowWidth));
                Console.Write("Enter the Product Id to Add to your cart : ");
                int productId = int.Parse(Console.ReadLine());
                //iterate through the list to get the product object
                Product product_obj = products.Find(x => x.ProductId == productId);
                if (product_obj == null) { throw new ProductNotFoundException("No Product is available"); }
                Console.Write("Enter the Quantity to add : ");
                int quantity = int.Parse(Console.ReadLine());


                //call the same method with the loggedinuser obj, product obj,quantity
                bool status = _orderProcessorRepository.AddToCart(LoggedInUser, product_obj, quantity);
                if (status)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("product added successfully to the cart");
                    Console.ResetColor();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("Could not add to cart please try again..");
                    Console.ResetColor();
                }

            }
            catch (ProductNotFoundException ex)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine(ex.Message);
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine(ex.Message);
                Console.ResetColor();
            }

        }

        public Customer CreateCustomer()
        {
            try
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("Create Customer"); Console.ResetColor();
                Console.Write("Name          : ");
                string customerName = Console.ReadLine();

                Console.Write("Email Address : ");
                string customerEmail = Console.ReadLine();

                Console.Write("Set Password  : ");
                string customerPassword = Console.ReadLine();
                string type = "customer";
                if (LoggedInUser != null && LoggedInUser.CustomerType == "admin")
                {
                    Console.Write("User Type[Customer/Admin]  : ");
                    type = Console.ReadLine().ToLower();
                }

                Customer customer = new Customer(customerName, customerEmail, customerPassword, type);

                Customer customerObj = _orderProcessorRepository.CreateCustomer(customer);

                if (customerObj != null)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("User created Successfully");
                    Console.ResetColor();
                    return customerObj;
                }

                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("user Cannot be created please try again..");
                Console.ResetColor();
                return null;

            }

            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"An error occurred: {ex.Message}");
                Console.ResetColor();
                return null;

            }

        }


        public void CreateProduct()
        {
            try
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("Create Product "); Console.ResetColor();
                Console.Write("Product Name           : ");
                string productName = Console.ReadLine();

                Console.Write("Product Price          : ");
                decimal productPrice = decimal.Parse(Console.ReadLine());

                Console.Write("Product Description    : ");
                string productDescrition = Console.ReadLine();

                Console.Write("Product Stock Quantity : ");
                int productStockQuantity = int.Parse(Console.ReadLine());

                Product product = new Product(productName, productPrice, productDescrition, productStockQuantity);

                _orderProcessorRepository.CreateProduct(product);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Product created Successfully");
                Console.ResetColor();
                //call the repo layer with same name with the product obj and return the boolean

            }
            catch (Exception ex) { Console.ForegroundColor = ConsoleColor.Red; Console.WriteLine(ex.Message); Console.ResetColor(); }

        }

        public void DeleteCustomer()
        {
            try
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("Delete Customer"); Console.ResetColor();
                List<Customer> customers = new List<Customer>();
                customers = _orderProcessorRepository.GetAllCustomers();
                foreach (Customer customer in customers)
                {
                    Console.WriteLine(customer.ToString());
                }
                // call the repository to get all the products , keep them in a list and display the take down input    

                Console.Write("Enter Customer Id To delete the Customer: ");
                int customerId = int.Parse(Console.ReadLine());

                bool status = _orderProcessorRepository.DeleteCustomer(customerId);

                if (status)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("User Deleted Successfully");
                    Console.ResetColor();
                }

            }
            catch (CustomerNotFoundException ex)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine(ex.Message);
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine(ex.Message);
                Console.ResetColor();
            }


            //call the repo layer of the same method with this argument
        }

        public void DeleteProduct()
        {
            try
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("Delete Product "); Console.ResetColor();
                List<Product> products = ProductsDisplay();


                Console.Write("Enter Product Id To delete the Product: ");
                int productId = int.Parse(Console.ReadLine());

                bool status = _orderProcessorRepository.DeleteProduct(productId);

                if (status)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Product Deleted Successfully");
                    Console.ResetColor();
                }

            }
            catch (ProductNotFoundException ex)
            {
                //Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine(ex.Message);
                //Console.ResetColor();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine(ex.Message);
                Console.ResetColor();
            }


            //call the repo layer of the same method with this argument
        }



        public int GetAllFromCart()
        {
            try
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("Items In Your Cart"); Console.ResetColor();
                //from the loggedin user take the id 
                //pass this id to the same method with and call the repo layer with the same method with paramete
                //here you should get the list of products and quantity of those in the cart and display it
                Dictionary<Product, int> cartitems = null;
                cartitems = _orderProcessorRepository.GetAllFromCart(LoggedInUser);
                int flag = 0;
                decimal cartTotal = 0;
                foreach (var item in cartitems)
                {
                    flag = 1;
                    Console.WriteLine(new string('-', Console.WindowWidth));
                    Console.WriteLine($"Product Name : {item.Key.Name}\nQuantity     : {item.Value}\nPrice : {item.Key.Price}");
                    cartTotal += item.Key.Price * item.Value;


                }
                if (flag == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("No Items in the Cart . Start Shopping and Add your Products.");
                    Console.ResetColor();
                    return 0;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.Write("Cart Total : "); Console.ResetColor();
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine(cartTotal); Console.ResetColor();

                }
                return 1;
            }
            catch (Exception ex) { Console.ForegroundColor = ConsoleColor.Red; Console.WriteLine(ex.Message); Console.ResetColor(); return 0; }
        }

        public void GetOrdersByCustomer()
        {
            try
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("Order History"); Console.ResetColor();
                //getting all the products in the Database
                Dictionary<int, Product> products = _orderProcessorRepository.GetAllProductsWithDictionary();


                //here you should get the list of orders from the repo layer 
                List<Order> customerOrdersList = new List<Order>();
                customerOrdersList = _orderProcessorRepository.GetOrdersByCustomer(LoggedInUser);

                int flag = 0;
                foreach (var item in customerOrdersList)
                {
                    flag = 1;
                    Console.WriteLine(new string('-', Console.WindowWidth));
                    Console.WriteLine(item);

                    //here we havce to display the products for that specific order call the repo layer to get the order_items

                    Dictionary<int, int> orderId_items = _orderProcessorRepository.GetOrderItemsOfSpecificOrder(item.OrderId);
                    Console.WriteLine("Products Ordered ");
                    foreach (var kv in orderId_items)
                    {
                        Console.WriteLine($"\tName : {products[kv.Key].Name} | Price : {products[kv.Key].Price}| Quantity : {kv.Value} | Cost : {products[kv.Key].Price * kv.Value}");
                    }



                }
                if (flag == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("You have Not Placed Any Orders Yet.");
                    Console.ResetColor();
                }
            }
            catch (Exception ex) { Console.ForegroundColor = ConsoleColor.Red; Console.WriteLine(ex.Message); Console.ResetColor(); }
        }

        public void PlaceOrder()
        {
            try
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("Placing Your order "); Console.ResetColor();
                Dictionary<Product, int> cartitems = null;
                cartitems = _orderProcessorRepository.GetAllFromCart(LoggedInUser);

                if (cartitems.Count != 0)
                {

                    Console.Write("Enter the Shipping Address : ");
                    string shippingAddress = Console.ReadLine();
                    int flag = 0;
                    foreach (var item in cartitems)
                    {

                        if (item.Key.StockQuantity < item.Value)
                        {
                            flag = 1;
                            Console.ForegroundColor = ConsoleColor.Red;

                            Console.WriteLine($"This Product: {item.Key.Name} has stockQuantity{item.Key.StockQuantity}");
                            Console.WriteLine("Please add number of items accordingly and then place order");
                            Console.ResetColor();
                        }
                    }
                    if (flag == 0)
                    {
                        bool placeOrderStatus = _orderProcessorRepository.PlaceOrder(LoggedInUser, cartitems, shippingAddress);

                        if (placeOrderStatus)
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("Order Placed Successfully"); Console.ResetColor();

                        }
                        else
                        {
                            Console.WriteLine("Try Again to Order");
                        }
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Cart is Empty! Please Add Items to cart to Place Order");
                    Console.ResetColor();
                }

            }
            catch (OrderNotFoundException ex)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine(ex.Message);
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine(ex.Message);
                Console.ResetColor();
            }

        }

        public void RemoveFromCart()
        {
            try
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("Remove Item From Cart"); Console.ResetColor();
                int items_status = GetAllFromCart(); // to display the users cart items
                                                     // from the cart of the user make a sql call to this by passing the loggedin user id
                Console.WriteLine(new string('-', Console.WindowWidth));
                if (items_status == 1)
                {
                    Console.Write("Enter the Product Name to remove from cart : ");
                    string productName = Console.ReadLine();

                    Console.Write("Enter Quantity to remove : ");
                    int quantity = int.Parse(Console.ReadLine());



                    //find the product obj from list and storeit
                    Dictionary<Product, int> cartitems = null;
                    cartitems = _orderProcessorRepository.GetAllFromCart(LoggedInUser);



                    Product product_obj_to_remove = null;
                    foreach (var item in cartitems)
                    {
                        if (item.Key.Name == productName)
                        {
                            product_obj_to_remove = item.Key;
                        }
                    }
                    if (product_obj_to_remove == null) { throw new ProductNotFoundException("No Product is available to remove"); }





                    bool status = _orderProcessorRepository.RemoveFromCart(LoggedInUser, product_obj_to_remove);
                    if (status)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Item Removed From Cart Successfully"); Console.ResetColor();
                    }

                    else { Console.ForegroundColor = ConsoleColor.Red; Console.WriteLine("Try Again To remove the Item"); Console.ResetColor(); }

                }


            }
            catch (ProductNotFoundException ex)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine(ex.Message);
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine(ex.Message);
                Console.ResetColor();
            }


        }

        public List<Product> ProductsDisplay()
        {
            try
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("All Products"); Console.ResetColor();
                List<Product> products = new List<Product>();
                // call the repository to get all the products , keep them in a list and display the take down input    
                products = _orderProcessorRepository.GetAllProducts();
                foreach (Product product in products)
                {
                    Console.WriteLine(new string('-', Console.WindowWidth));


                    Console.WriteLine(product);
                    //Console.WriteLine(new string('-', Console.WindowWidth));

                }
                return products;
            }
            catch (Exception ex) { Console.ForegroundColor = ConsoleColor.Red; Console.WriteLine(ex.Message); Console.ResetColor(); return null; }
        }


    }
}
