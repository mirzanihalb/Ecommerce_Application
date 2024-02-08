using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom_Case_Study.Model
{
    internal class Cart
    {
        int _cartId;
        int _customerId;
        int _productId;
        int _quantity;

        public Cart()
        {
            
        }
        public Cart(int cart_id,int customer_id,int product_id,int quantity)
        {
            _cartId = cart_id;
            _customerId = customer_id;
            _productId = product_id;
            _quantity = quantity;
        }

        public int CartId
        {
            get { return _cartId; }
            set { _cartId = value; }
        }

        public int CustomerId
        {
            get { return _customerId; }
            set { _customerId = value; }
        }

        public int ProductId
        {
            get { return _productId; }
            set { _productId = value; }
        }

        public int Quantity
        {
            get { return _quantity; }
            set { _quantity = value; }
        }
    }
}
