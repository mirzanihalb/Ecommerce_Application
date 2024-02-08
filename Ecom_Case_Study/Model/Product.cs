using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom_Case_Study.Model
{
    public class Product
    {
        int _productId;
        string _name;
        decimal _price;
        string _description;
        int _stockQuantity;

        public Product()
        {

        }
        public Product(string name, decimal price, string description, int stock_quantity)
        {
            
            _name = name;
            _price = price;
            _description = description;
            _stockQuantity = stock_quantity;

        }

        public int ProductId
        {
            get { return _productId; }
            set { _productId = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public decimal Price
        {
            get { return _price; }
            set { _price = value; }
        }

        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }
        public int StockQuantity
        {
            get { return _stockQuantity; }
            set { _stockQuantity = value; }
        }

        public override string ToString()
        {
            return $"ProductId     : {ProductId}\nName          : {Name}\nPrice         : {Price}\nStockQuantity : {StockQuantity}\nDescription   : {Description}";
        }
    }
}
