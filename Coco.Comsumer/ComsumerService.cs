using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Coco.Comsumer
{
    public abstract class ComsumerService : Comsumer, IHostedService
    {
        public Task StartAsync(CancellationToken cancellationToken)
        {
            //return Task.Run(() => base.Start());
            throw new NotImplementedException();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public ComsumerService(string host, string topicName) : base(host, topicName)
        {
        }
    }
}
