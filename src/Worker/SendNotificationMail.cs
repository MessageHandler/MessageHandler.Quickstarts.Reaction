using MessageHandler.Quickstart.Contract;
using MessageHandler.Runtime.AtomicProcessing;

namespace Worker
{
    public class SendNotificationMail : IHandle<PurchaseOrderBooked>
    {
        private readonly ILogger<SendNotificationMail> logger;
        private readonly ISendEmails emailSender;

        public SendNotificationMail(ISendEmails emailSender, ILogger<SendNotificationMail> logger = null!)
        {
            this.logger = logger;
            this.emailSender = emailSender;
        }

        public async Task Handle(PurchaseOrderBooked message, IHandlerContext context)
        {
            logger?.LogInformation("Received PurchaseOrderBooked, notifying the seller...");

            await emailSender.SendAsync("sender@seller.com", "seller@seller.com", "New purchase order", "A new purchase order is available for confirmation");

            logger?.LogInformation("Notification email sent");
        }
    }
}
