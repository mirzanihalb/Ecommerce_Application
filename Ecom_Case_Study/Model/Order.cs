using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom_Case_Study.Model
{
    internal class Order
    {
        int _orderId;
        int _customerId;
        DateTime _orderDate;
        decimal _totalPrice;
        string _shippingAddress;

        public Order()
        {
            
        }
        public Order(int order_id,int customer_id,DateTime order_date,decimal total_price,string shipping_address)
        {
            _orderId = order_id;
            _customerId = customer_id;
            _orderDate = order_date;
            _totalPrice = total_price;
            _shippingAddress = shipping_address;
        }

        public int OrderId
        {
            get { return _orderId; }
            set { _orderId = value; }
        }

        public int CustomerId
        {
            get { return _customerId; }
            set { _customerId = value; }
        }
        public DateTime OrderDate
        {
            get { return _orderDate; }
            set { _orderDate = value; }
        }
        public decimal TotalPrice
        {
            get { return _totalPrice; }
            set { _totalPrice = value; }
        }
        public string ShippingAddress
        {
            get { return _shippingAddress; }
            set { _shippingAddress = value; }
        }

        public override string ToString()
        {
            return $"OrderId          : {OrderId}\nOrderDate        : {OrderDate.Date}\nTotalPrice       : {TotalPrice}\nShipping Address : {ShippingAddress}";
        }
    }
}
