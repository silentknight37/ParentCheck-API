using MediatR;
using ParentCheck.Envelope;
using System;

namespace ParentCheck.Query
{
    public class DeviceTokenSaveCommand : IRequest<RequestSaveEnvelop>
    {
        public DeviceTokenSaveCommand(string deviceToken, long userId)
        {
            this.DeviceToken = deviceToken;
            this.UserId = userId;
        }

        public string DeviceToken { get; set; }
        public long UserId { get; set; }
    }
}
