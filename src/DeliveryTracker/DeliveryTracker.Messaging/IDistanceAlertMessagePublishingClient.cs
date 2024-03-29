﻿using System.Threading;
using System.Threading.Tasks;

namespace DeliveryTracker.Messaging
{
    public interface IDistanceAlertMessagePublishingClient
    {
        Task<string> PublishAsync<T>(T message, CancellationToken cancellationToken) where T : class;
    }
}