using MessageHandler.Quickstart.Contract;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Worker;
using Xunit;

namespace ComponentTests
{
    public class WhenReactingToPurchaseOrderBooked
    {
        [Fact]
        public async Task GivenBookingStarted_WhenNotifyingTheSeller_ShouldSendAnEmailToTheSeller()
        {
            // given
            var bookingId = "91d6950e-2ddf-4e98-a97c-fe5f434c13f0";
            var history = new OrderBookingHistoryBuilder()
                                .WellknownBooking(bookingId)
                                .Build();
            var bookingStarted = (PurchaseOrderBooked) history.First();

            // mock email
            var mockEmailSender = new Mock<ISendEmails>();
            mockEmailSender.Setup(_ => _.SendAsync("sender@seller.com", "seller@seller.com", "New purchase order", "A new purchase order is available for confirmation")).Verifiable();

            //when
            var reaction = new SendNotificationMail(mockEmailSender.Object);
            await reaction.Handle(bookingStarted, null!);

            // Then
            mockEmailSender.Verify();
        }

    }
}
