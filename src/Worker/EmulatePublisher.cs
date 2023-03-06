using MessageHandler.Quickstart.Contract;
using MessageHandler.Runtime.AtomicProcessing;

namespace Worker
{
    public class EmulatePublisher : IHostedService
    {
        private readonly IDispatchMessages dispatcher;

        public EmulatePublisher(IDispatchMessages dispatcher)
        {
            this.dispatcher = dispatcher;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var bookingId = "91d6950e-2ddf-4e98-a97c-fe5f434c13f0";
            var history = new OrderBookingHistoryBuilder()
                                .WellknownBooking(bookingId)
                                .Build();
            var bookingStarted = (PurchaseOrderBooked)history.First();

            await this.dispatcher.Dispatch(bookingStarted);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
