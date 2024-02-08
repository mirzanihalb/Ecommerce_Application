using Ecom_Case_Study.Model;
using Ecom_Case_Study.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Ecom_Case_Study.App
{
    internal class EcomApp
    {
        public void run()
        {
            IOrderProcessorService orderService = new OrderProcessorServiceImpl();
            
            Customer loggedInUser = null;
            
            try
            {
                while (loggedInUser == null)
                {
                    Console.Title = "ECommerce Application";
                    
                    Console.WriteLine("Welcome To ECommerce Application");
                    Console.WriteLine("1. Login");
                    Console.WriteLine("2. Register");
                    Console.WriteLine("0. Exit");
                    Console.Write("Enter : ");
                    int userInput = int.Parse(Console.ReadLine());
                    switch (userInput)
                    {
                        case 0: return;
                        case 1: loggedInUser = orderService.Login(); break;
                        case 2: loggedInUser = orderService.CreateCustomer(); break;
                        default: Console.WriteLine("choose correct input"); break;
                    }
                }


                while (loggedInUser != null)
                {
                    Console.WriteLine(new string('-', Console.WindowWidth));
                    Console.Write($"Hey ");
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine($"Hey {loggedInUser.Name}!");
                    Console.ResetColor();
                    
                    Console.WriteLine("1 > View Products");
                    Console.WriteLine("2 > Add To Cart");
                    Console.WriteLine("3 > Delete From Cart");
                    Console.WriteLine("4 > View Cart");
                    Console.WriteLine("5 > Place Order");
                    Console.WriteLine("6 > View Your Orders");



                    if (loggedInUser.CustomerType != "customer")
                    {
                        Console.WriteLine("7 > Create customer");
                        Console.WriteLine("8 > delete Customer");
                        Console.WriteLine("9 > Create Product");
                        Console.WriteLine("10 > delete Product");
                    }
                    Console.WriteLine("0 > Exit");

                    Console.Write("Enter : ");

                    int userInput = int.Parse(Console.ReadLine());
                    switch (userInput)
                    {
                        case 0:Console.ForegroundColor = ConsoleColor.Yellow; Console.WriteLine("Thanks For Visiting"); Console.ResetColor(); loggedInUser = null; return;
                        case 1: orderService.ProductsDisplay(); break;
                        case 2: orderService.AddToCart(); break;
                        case 3: orderService.RemoveFromCart(); break;
                        case 4: orderService.GetAllFromCart(); break;
                        case 5: orderService.PlaceOrder(); break;
                        case 6: orderService.GetOrdersByCustomer(); break;

                        case 7: if (loggedInUser.CustomerType == "admin") { Customer new_customer = orderService.CreateCustomer(); } else { Console.WriteLine("Enter Correct Option"); } break;
                        case 8: if (loggedInUser.CustomerType == "admin") { orderService.DeleteCustomer(); } else { Console.WriteLine("Enter Correct Option"); } break;
                        case 9: if (loggedInUser.CustomerType == "admin") { orderService.CreateProduct(); } else { Console.WriteLine("Enter Correct Option"); } break;
                        case 10: if (loggedInUser.CustomerType == "admin") { orderService.DeleteProduct(); } else { Console.WriteLine("Enter Correct Option"); } break;
                        
                        default: Console.WriteLine("Enter Correct Option"); break;
                    }
                }


            }
            catch (Exception e) { Console.WriteLine(e.Message); }
        }
    }
}
