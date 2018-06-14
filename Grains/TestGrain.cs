using System;
using System.Linq;
using System.Threading.Tasks;
using Interfaces;
using Orleans;
using Orleans.Streams;

namespace Grains
{
    public class TestGrain : Grain, ITestGrain
    {
        private readonly Random _random = new Random();
        private StreamSubscriptionHandle<string> _streamHandle;

        public async override Task OnActivateAsync()
        {
            var streamProvider = GetStreamProvider("SMSProvider");
            var stream = streamProvider.GetStream<string>(this.GetPrimaryKey(), "MyStreamNamespace");
            var subscriptionHandles = await stream.GetAllSubscriptionHandles();

            if( subscriptionHandles == null || subscriptionHandles.Count == 0 )
            {
                _streamHandle = await stream.SubscribeAsync( ObserveAsync );
            }
            else if( subscriptionHandles.Count == 1 )
            {
                _streamHandle = subscriptionHandles.First( );
                await _streamHandle.ResumeAsync( ObserveAsync );
            }
            else if( subscriptionHandles.Count > 1 )
            {
                foreach( var h in subscriptionHandles )
                    await h.UnsubscribeAsync( );

                _streamHandle = await stream.SubscribeAsync( ObserveAsync );
            }
        }

        public async override Task OnDeactivateAsync( )
        {
            if( _streamHandle != null )
                await _streamHandle.UnsubscribeAsync( );
        }

        public Task<int> GetARandomNumberAsync()
        {
            return Task.FromResult(_random.Next());
        }

        public Task<string> GetARandomStringAsync()
        {
            return Task.FromResult(_random.Next().ToString());
        }

        private Task ObserveAsync( object _, StreamSequenceToken sst )
        {
            return Task.CompletedTask;
        }
    }
}
