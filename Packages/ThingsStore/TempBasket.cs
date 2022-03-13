namespace ThingsStore
{
    using System.Collections.Concurrent;

    using CSharpFunctionalExtensions;

    /// <inheritdoc />
    public class TempBasket : ConcurrentDictionary<long, OrderInfo>, ITempBasket
    {
        /// <inheritdoc />
        public void AddProductToOrder(long customerId, Product product)
        {
            AddOrUpdate(
                customerId,
                orderCustomerId => AddNewOrderForCustomer(orderCustomerId, product),
                (orderCustomerId, order) => UpdateValueFactory(orderCustomerId, order, product));
        }

        /// <inheritdoc />
        public Maybe<OrderInfo> GetOrderProducts(long customerId)
        {
            return TryGetValue(customerId, out var order) ? order : Maybe<OrderInfo>.None;
        }

        /// <inheritdoc />
        public void RemoveProductFromOrder(long customerId, long productId)
        {
            var order = GetOrderProducts(customerId);
            if (order.HasValue)
            {
                order.Value.OrderProducts.Remove(productId);
            }
        }

        private static OrderInfo AddNewOrderForCustomer(long customerId, Product product)
        {
            var orderProduct = new ProductInfo
            {
                Amount = 1,
                Product = product
            };
            var order = new OrderInfo
            {
                OrderProducts = new ConcurrentDictionary<long, ProductInfo>()
            };

            order.OrderProducts.Add(product.Id, orderProduct);
            
            return order;
        }

        private static OrderInfo UpdateValueFactory(long customerId, OrderInfo order, Product product)
        {
            if (order.OrderProducts.TryGetValue(product.Id, out var orderProduct))
            {
                var updatedOrderProduct = orderProduct with
                {
                    Amount = orderProduct.Amount + 1,
                };
                order.OrderProducts[product.Id] = updatedOrderProduct;
            }
            else
            {
                var newOrderProduct = new ProductInfo
                {
                    Amount = 1,
                    Product = product
                };
                order.OrderProducts.Add(product.Id, newOrderProduct);
            }

            return order;
        }
    }
}