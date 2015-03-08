using System;

namespace RxRequestResponse.Interfaces
{
    public interface IApiService
    {
        void SendRequest(Request req);
        IObservable<Response> Responses { get; }
    }
}