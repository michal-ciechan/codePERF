using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using RxRequestResponse.Interfaces;

namespace RxRequestResponse
{
    public class RequestResponseController
    {
        private readonly Subject<int> _throttle = new Subject<int>();
        public RequestResponseController(IRequester requester, IResponseHandler responseHandler, 
            IApiService service, ISchedulers schedulers)
        {
            service.Responses
                .ObserveOn(schedulers.ThreadPool)
                .Subscribe(responseHandler);

            service.Responses
                .Subscribe(r => _throttle.OnNext(0));

            requester
                .ObserveOn(schedulers.ThreadPool)
                .Zip(_throttle,(request, i) => request)
                .Subscribe(request => service.SendRequest(request));
            
            for (int i = 0; i < 4; i++)
                _throttle.OnNext(i);
        }
    }
}