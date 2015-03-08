using System;

namespace RxRequestResponse.Interfaces
{
    public interface IRequester : IObservable<Request> { }
}