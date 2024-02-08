using Ecom_Case_Study.Exceptions;
using Ecom_Case_Study.Model;
using Ecom_Case_Study.Service;
using Ecom_Case_Study.Utility;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom_Case_Study.Repository
{
    internal class OrderProcessorRepositoryImpl: IOrderProcessorRepository
    {
        SqlConnection sqlconnection = null;
        SqlCommand cmd = null;
        public OrderProcessorRepositoryImpl()
        {
            sqlconnection = new SqlConnection(DbConnUtil.GetConnectionString());
            cmd = new SqlCommand();
        }

        

        public Customer Login(string email)
        {
            try
            {
                cmd.Parameters.Clear();   
                cmd.CommandText = "select * from Customers where email=@email";
                cmd.Parameters.AddWithValue("email", email);
                cmd.Connection = sqlconnection;
                
                sqlconnection.Open();
                
                SqlDataReader reader = cmd.ExecuteReader();
                
                Customer customerObj = new Customer();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {

                        
                        customerObj.CustomerId = (int)reader["customer_id"];
                        customerObj.Name = (string)reader["name"];
                        customerObj.Email = (string)reader["email"];
                        customerObj.Password = (string)reader["password"];
                        customerObj.CustomerType = (string)reader["customer_type"];


                    }
                    

                    return customerObj;
                }
                else
                {
                    
                    throw new CustomerNotFoundException("No User Exists with the given Email address");
                }
            }
            
            catch(SqlException e) { Console.WriteLine(e.Message); return null; }
            finally { sqlconnection.Close(); }
        }

        public bool CreateProduct(Product product)
        {
            try
            {
                cmd.Parameters.Clear();
                cmd.CommandText = "insert into products(name,price,description,stockQuantity) values(@name,@price,@description,@stockQuantity);select SCOPE_IDENTITY();";
                cmd.Parameters.AddWithValue("name", product.Name);
                cmd.Parameters.AddWithValue("price", product.Price);
                cmd.Parameters.AddWithValue("description", product.Description);
                cmd.Parameters.AddWithValue("stockQuantity", product.StockQuantity);
                cmd.Connection = sqlconnection;
                sqlconnection.Open();
                object scope_identity = cmd.ExecuteScalar();

                sqlconnection.Close();


                if (scope_identity != null)
                {
                    product.ProductId = Convert.ToInt32(scope_identity);
                    return true;
                }
                return false;
            }
            catch (Exception e) { Console.WriteLine(e.Message);return false; }
        }

        public List<Product> GetAllProducts()
        {
            try
            {
                
                List<Product> products = new List<Product>();
                cmd.CommandText = "Select * from products";
                cmd.Connection = sqlconnection;
                sqlconnection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Product product = new Product();
                    product.ProductId = (int)reader["product_id"];
                    product.Name = (string)reader["name"];
                    product.Price = (decimal)reader["price"];
                    product.Description = (string)reader["description"];
                    product.StockQuantity = (int)reader["stockQuantity"];
                    products.Add(product);
                    
                }
                sqlconnection.Close();
                return products;
            }
            catch(Exception e) { Console.WriteLine(e.Message); return null; }
        }

        public bool DeleteProduct(int productId)
        {
            try
            {
                cmd.Parameters.Clear();
                cmd.CommandText = "delete from products where product_id=@productId";
                cmd.Parameters.AddWithValue("productId", productId);
                cmd.Connection = sqlconnection;
                sqlconnection.Open();
                int deletedProductStatus = cmd.ExecuteNonQuery();
                sqlconnection.Close();

                if (deletedProductStatus == 0) { throw new ProductNotFoundException("No Such Product to delete"); }
                else
                {
                    return deletedProductStatus > 0;
                }
            }
            catch(SqlException e) { Console.WriteLine(e.Message);return false; }
            
        }

        public List<Customer> GetAllCustomers()
        {
            try
            {
                List<Customer> customers = new List<Customer>();
                cmd.CommandText = "Select * from Customers";
                cmd.Connection = sqlconnection;
                sqlconnection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Customer customer = new Customer();
                    customer.CustomerId = (int)reader["customer_id"];
                    customer.Name = (string)reader["name"];
                    customer.Email = (string)reader["email"];
                    customer.Password = (string)reader["password"];
                    customer.CustomerType = (string)reader["customer_type"];
                    customers.Add(customer);
                }
                sqlconnection.Close();
                return customers;
            } catch(Exception e) { Console.WriteLine(e.Message);return null; }
        }

        public bool DeleteCustomer(int customerId)
        {
            try
            {
                cmd.Parameters.Clear();
                cmd.CommandText = "delete from Customers where customer_id=@customerId";
                cmd.Parameters.AddWithValue("customerId", customerId);
                cmd.Connection = sqlconnection;
                sqlconnection.Open();
                int deletedCustomerStatus = cmd.ExecuteNonQuery();
                sqlconnection.Close();

                if (deletedCustomerStatus == 0) { throw new CustomerNotFoundException("No Such Customer Exists to Delete"); }
                return deletedCustomerStatus > 0;
            
            }
            //catch(Exception e) { Console.WriteLine(e.Message);return false; }
            finally { cmd.Parameters.Clear(); }
        }

        public Customer CreateCustomer(Customer customer)
        {
            try
            {
                cmd.Parameters.Clear();
                cmd.CommandText = "insert into Customers(name,email,password,customer_type) values(@name,@email,@password,@type);select SCOPE_IDENTITY();";
                cmd.Parameters.AddWithValue("name", customer.Name);
                cmd.Parameters.AddWithValue("email", customer.Email);
                cmd.Parameters.AddWithValue("password", customer.Password);
                cmd.Parameters.AddWithValue("type", customer.CustomerType);
                cmd.Connection = sqlconnection;

                sqlconnection.Open();
                object scopeIdentity = cmd.ExecuteScalar();
                sqlconnection.Close();

                if (scopeIdentity != null)
                {
                    customer.CustomerId = Convert.ToInt32(scopeIdentity);
                    return customer;
                }

               
                return null;
            }
            catch (SqlException ex)
            {
                // Handle SQL-related exceptions
                Console.WriteLine($"SQL Exception: {ex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                // Handle other exceptions
                Console.WriteLine($"An error occurred: {ex.Message}");
                return null;
            }
        }


        public bool AddToCart(Customer loggedInUser, Product product_obj, int quantity)
        {
            try
            {

                cmd.Parameters.Clear();
                //cmd.CommandText = "update cart set quantity = case when not exists (select 1 from cart where customer_id=@customerId and product_id=@productId) then 1 else quantity+@quantity end where customer_id =@customerId and product_id = @productId";
                cmd.CommandText = "update cart set quantity = quantity+@quantity where customer_id = @customerId and product_id = @productId";
                cmd.Connection = sqlconnection;
                cmd.Parameters.AddWithValue("customerId", loggedInUser.CustomerId);
                cmd.Parameters.AddWithValue("productId", product_obj.ProductId);
                cmd.Parameters.AddWithValue("quantity", quantity);
                sqlconnection.Open();
                int cartstatus = cmd.ExecuteNonQuery();
                sqlconnection.Close();
                if (cartstatus > 0)
                {
                    return true;
                }
                else
                {
                    cmd.CommandText = "Insert into Cart(customer_id,product_id,quantity) values(@customerId,@productId,@quantity)";
                    cmd.Parameters.Clear();
                    cmd.Connection = sqlconnection;
                    cmd.Parameters.AddWithValue("customerId", loggedInUser.CustomerId);
                    cmd.Parameters.AddWithValue("productId", product_obj.ProductId);
                    cmd.Parameters.AddWithValue("quantity", quantity);
                    sqlconnection.Open();
                    cartstatus = cmd.ExecuteNonQuery();
                    sqlconnection.Close();
                    if (cartstatus > 0)
                    {
                        return true;
                    }
                    else { return false; }

                }
            }catch(SqlException e) { Console.WriteLine(e.Message); return false; }
            catch (Exception e) { Console.WriteLine(e.Message); return false; }
        }

        public Dictionary<Product, int> GetAllFromCart(Customer loggedInUser)
        {
            try
            {
                List<Product> products = GetAllProducts();

                Dictionary<Product, int> cartItems = new Dictionary<Product, int>();

                cmd.CommandText = "select * from cart where customer_id = @customerId";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("customerId", loggedInUser.CustomerId);
                cmd.Connection = sqlconnection;
                sqlconnection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Product product_obj = products.Find(x => x.ProductId == (int)reader["product_id"]);

                    int quantity = (int)reader["quantity"];
                    cartItems.Add(product_obj, quantity);

                }
                sqlconnection.Close();
                return cartItems;
            }catch(Exception e) { Console.WriteLine(e.Message);return null; }

        }

        public bool RemoveFromCart(Customer loggedInUser, Product product_obj_to_remove)
        {
            try
            {
                cmd.CommandText = "delete from cart where customer_id = @customerId and product_id = @productId";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@customerId", loggedInUser.CustomerId);
                cmd.Parameters.AddWithValue("@productId", product_obj_to_remove.ProductId);
                cmd.Connection = sqlconnection;
                sqlconnection.Open();
                int deleteCartItemStatus = cmd.ExecuteNonQuery();
                sqlconnection.Close();
                return deleteCartItemStatus > 0;
            }
            catch(Exception e) { Console.WriteLine(e.Message); return false; }
        }

        public List<Order> GetOrdersByCustomer(Customer loggedInUser)
        {
            try
            {

                List<Order> orders = new List<Order>();
                cmd.Parameters.Clear();
                cmd.CommandText = "select * from orders where customer_id = @customerId";
                cmd.Connection = sqlconnection;
                cmd.Parameters.AddWithValue("customerId", loggedInUser.CustomerId);
                sqlconnection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Order order = new Order();
                    order.OrderId = (int)reader["order_id"];
                    order.CustomerId = (int)reader["customer_id"];
                    order.OrderDate = (DateTime)reader["order_date"];
                    order.TotalPrice = (decimal)reader["total_price"];
                    order.ShippingAddress = (string)reader["shipping_address"];
                    orders.Add(order);
                }

                sqlconnection.Close();
                return orders;
            }
            catch (Exception e) { Console.WriteLine(e.Message);return null; }
        }

        public bool PlaceOrder(Customer loggedInUser, Dictionary<Product, int> cartitems, string? shippingAddress)
        {
            try
            {
                cmd.Parameters.Clear();
                decimal totalPrice = 0;
                DateTime orderDate = DateTime.Now.Date;
                foreach (var item in cartitems)
                {
                    totalPrice += item.Key.Price * item.Value;
                }
                cmd.CommandText = "insert into orders(customer_id,order_date,total_price,shipping_address) values(@customerId,@orderDate,@totalPrice,@shippingAddress);select SCOPE_IDENTITY();";
                cmd.Parameters.AddWithValue("customerId", loggedInUser.CustomerId);
                cmd.Parameters.AddWithValue("orderDate", orderDate);
                cmd.Parameters.AddWithValue("totalPrice", totalPrice);
                cmd.Parameters.AddWithValue("shippingAddress", shippingAddress);
                cmd.Connection = sqlconnection;
                sqlconnection.Open();
                object scope_identity = cmd.ExecuteScalar();

                sqlconnection.Close();


                if (scope_identity != null)
                {
                    try
                    {
                        int orderIdPlaced = Convert.ToInt32(scope_identity);

                        foreach (var item in cartitems)
                        {
                            cmd.Parameters.Clear();
                            cmd.CommandText = "insert into order_items(order_id,product_id,quantity) values(@orderId,@productId,@quantity)";
                            cmd.Parameters.AddWithValue("orderId", orderIdPlaced);
                            cmd.Parameters.AddWithValue("productId", item.Key.ProductId);
                            cmd.Parameters.AddWithValue("quantity", item.Value);
                            cmd.Connection = sqlconnection;
                            sqlconnection.Open();
                            int order_item_insert_status = cmd.ExecuteNonQuery();
                            sqlconnection.Close();


                        }
                        foreach (var item in cartitems)
                        {
                            cmd.Parameters.Clear();
                            cmd.CommandText = "update products set stockQuantity-=@quantity where product_id=@productId";
                            cmd.Parameters.AddWithValue("quantity", item.Value);
                            cmd.Parameters.AddWithValue("productId", item.Key.ProductId);
                            cmd.Connection = sqlconnection;
                            sqlconnection.Open();
                            int updateProductsCountStatus = cmd.ExecuteNonQuery();
                            sqlconnection.Close();

                        }
                        cmd.Parameters.Clear();
                        cmd.CommandText = "delete from Cart where customer_id=@customerId;";
                        cmd.Parameters.AddWithValue("customerId", loggedInUser.CustomerId);
                        cmd.Connection = sqlconnection;
                        sqlconnection.Open();
                        int deleteFromCartStatus = cmd.ExecuteNonQuery();
                        sqlconnection.Close();


                        return orderIdPlaced > 0;
                    }
                    catch (OrderNotFoundException ex)
                    {
                        sqlconnection.Close();
                        Console.WriteLine(ex.Message);
                    }


                }

                return false;


            }catch (Exception ex) { Console.WriteLine(ex.Message); return false;}
            }



        //get all products but returning a dictionary
        public Dictionary<int,Product> GetAllProductsWithDictionary()
        {
            try
            {
                Dictionary<int,Product> product_dict = new Dictionary<int,Product>();
                
                cmd.CommandText = "Select * from products";
                cmd.Connection = sqlconnection;
                sqlconnection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Product product = new Product();
                    product.ProductId = (int)reader["product_id"];
                    product.Name = (string)reader["name"];
                    product.Price = (decimal)reader["price"];
                    product.Description = (string)reader["description"];
                    product.StockQuantity = (int)reader["stockQuantity"];
                    product_dict.Add(product.ProductId, product);

                }
                sqlconnection.Close();
                return product_dict;
            }
            catch (Exception e) { Console.WriteLine(e.Message); return null; }
        }



        public Dictionary<int,int> GetOrderItemsOfSpecificOrder(int orderId)
        {
            Dictionary<int,int> items_of_orderid = new Dictionary<int,int>();
            cmd.Parameters.Clear();
            cmd.CommandText = "select * from order_items where order_id = @orderId";
            cmd.Parameters.AddWithValue("orderId", orderId);
            cmd.Connection = sqlconnection;
            sqlconnection.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                int product_id = (int)reader["product_id"];
                int quantity = (int)reader["quantity"];
                items_of_orderid.Add(product_id, quantity);

            }
            sqlconnection.Close();
            return items_of_orderid;
        }

       
    }
}
