using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom_Case_Study.Model
{
    internal class Customer
    {
        int _customerId;
        string _name;
        string _email;
        string _password;
        string _customerType;

        public int CustomerId
        {
            get { return _customerId; }
            set { _customerId = value; }
        }
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        public string Email
        {
            get { return _email; }
            set { _email = value; }
        }
        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }

        public string CustomerType
        {
            get { return _customerType; }
            set { _customerType = value; }
        }


        public Customer()
        {
            
        }
        public Customer(string customer_name,string customer_email,string customer_password)
        {
            
            _name = customer_name;
            _email = customer_email;
            _password = customer_password;
            
        }

        public Customer(string customer_name, string customer_email, string customer_password,string type)
        {

            _name = customer_name;
            _email = customer_email;
            _password = customer_password;
            _customerType = type;

        }

        public override string ToString()
        {
            return $"CustomerId: {CustomerId}\tName: {Name}\t Email: {Email}";
        }
    }
}
