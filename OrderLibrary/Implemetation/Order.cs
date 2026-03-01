using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using OrderLibrary.Interfaces;
namespace OrderLibrary.Implemetation
{
    
    public delegate void PlacedEventHandler(PlacedEventArgs e);
   
    public delegate void ErroredEventHandler(ErrorEventArgs e);
   
    public class Order : IOrder
    {
        private readonly IOrderService _orderService;
        private readonly decimal _threshold;

        // 0 = not completed, 1 = completed (placed or errored)
        private int _isCompleted;

        public event PlacedEventHandler Placed;
        public event ErroredEventHandler Errored;

        public Order(IOrderService orderService, decimal threshold)
        {
            _orderService = orderService ?? throw new ArgumentNullException(nameof(orderService));
            _threshold = threshold;
        }

        public void RespondToTick(string code, decimal price)
        {
            if (string.IsNullOrWhiteSpace(code))
                return;

            // Fast exit if already completed
            if (Volatile.Read(ref _isCompleted) == 1)
                return;

            if (price >= _threshold)
                return;

            // Ensure only one thread can proceed
            if (Interlocked.CompareExchange(ref _isCompleted, 1, 0) != 0)
                return;

            try
            {
                _orderService.Buy(code, 1, price);

                Placed?.Invoke(new PlacedEventArgs(code, price));
            }
            catch (Exception ex)
            {
                Errored?.Invoke(new ErrorEventArgs(ex));
            }
        }
    }
}
