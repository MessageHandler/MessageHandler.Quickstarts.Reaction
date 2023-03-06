using MessageHandler.Quickstart.Contract;
using MessageHandler.Runtime;
using MessageHandler.Runtime.AtomicProcessing;
using Worker;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddLogging();

        services.AddPostmark();

        var serviceBusConnectionString = hostContext.Configuration.GetValue<string>("servicebusnamespace")
                                                ?? throw new Exception("No 'servicebusnamespace' was provided. Use User Secrets or specify via environment variable.");

        services.AddMessageHandler("orderbooking.worker", runtimeConfiguration =>
        {
            runtimeConfiguration.AtomicProcessingPipeline(pipeline =>
            {
                pipeline.PullMessagesFrom(p => p.Topic(name: "orderbooking.events", subscription: "orderbooking.worker", serviceBusConnectionString));
                pipeline.DetectTypesInAssembly(typeof(PurchaseOrderBooked).Assembly);
                pipeline.HandleMessagesWith<SendNotificationMail>();
            });

            runtimeConfiguration.ImmediateDispatchingPipeline(dispatching =>
            {
                dispatching.RouteMessagesOfType<PurchaseOrderBooked>(to => to.Topic("orderbooking.events", serviceBusConnectionString));
            });
        });

        services.AddHostedService<EmulatePublisher>();
     
    })
    .Build();

await host.RunAsync();
