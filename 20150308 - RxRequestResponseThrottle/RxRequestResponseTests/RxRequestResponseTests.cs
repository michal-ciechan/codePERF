using System;
using System.Collections.Generic;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using RxRequestResponse;
using RxRequestResponse.Interfaces;
using RxRequestResponseTests.Helpers;

namespace RxRequestResponseTests.Tests
{
    [TestFixture]
    public class RxRequestResponseTests
    {
        private TestSchedulers _schedulers;
        private TestSchedulerEx _serviceScheduler;
        private IApiService _service;
        private Subject<Request> _requests;
        private RequestResponseController _controller;
        private Subject<Response> _responses;
        private IRequester _requester;
        private IResponseHandler _responseHandler;

        private List<Response> _responsesProcessed;
        private List<Request> _requestsReceived;


        [SetUp]
        public void A0_Setup()
        {
            _schedulers = new TestSchedulers();
            _serviceScheduler = new TestSchedulerEx("ServiceScheduler");

            _requests = new Subject<Request>();
            _responses = new Subject<Response>();
            _responsesProcessed = new List<Response>();
            _requestsReceived = new List<Request>();
            
            // Setup service mock
            _service = Substitute.For<IApiService>();
            _service.WhenForAnyArgs(service => service.SendRequest(new Request())).Do(c =>
            {
                var request = c.Arg<Request>();
                Console.WriteLine("Received Request: " + request);
                _requestsReceived.Add(request);
                _serviceScheduler.ScheduleNext(() =>
                {
                    _responses.OnNext(new Response(request.Value));
                });
            });
            _service.Responses.Returns(_responses);
            _service.Responses.Subscribe(x => Console.WriteLine("Sending Response: " + x));

            // Setup IRequester mock
            _requester = Substitute.For<IRequester>();
            _requester.WhenForAnyArgs(r => r.Subscribe(Arg.Any<IObserver<Request>>()))
                .Do(info => 
                    _requests.Subscribe(info.Arg<IObserver<Request>>()));

            // Setup IResponseHandler
            _responseHandler = Substitute.For<IResponseHandler>();
            _responseHandler.WhenForAnyArgs(x => x.OnNext(new Response()))
                .Do(info =>
                {
                    var response = info.Arg<Response>();
                    _responsesProcessed.Add(response);
                    Console.WriteLine("Processed Response: " + response);
                });

            _controller = new RequestResponseController(_requester, _responseHandler, _service, _schedulers);

            Console.WriteLine("// Act");
        }


        [Test]
        public void Should_get_respponse_when_request_received()
        {
            _requests.OnNext(new Request(1));

            _schedulers.ThreadPool.AdvanceBy(1); // Send Request
            _requestsReceived.Should().HaveCount(1);

            _serviceScheduler.AdvanceBy(1); // Process Request

            _schedulers.ThreadPool.AdvanceBy(1); // Process Response
            _responsesProcessed.Should().HaveCount(1);
        }


        [Test]
        public void Should_send_max_4_requests_at_one_time()
        {
            // Schedule 5 requests
            _requests.OnNext(new Request(1));
            _requests.OnNext(new Request(2));
            _requests.OnNext(new Request(3));
            _requests.OnNext(new Request(4));
            _requests.OnNext(new Request(5));

            _schedulers.ThreadPool.AdvanceBy(5); // Attempt to send all 5 requests
            _requestsReceived.Should().HaveCount(4); // Check if only 4 requests received by service
        }

        [Test]
        public void Should_send__5th_queued_request_after_receiving_1st_response()
        {
            // Schedule 5 requests
            _requests.OnNext(new Request(1));
            _requests.OnNext(new Request(2));
            _requests.OnNext(new Request(3));
            _requests.OnNext(new Request(4));
            _requests.OnNext(new Request(5));
            _requests.OnNext(new Request(6));

            _schedulers.ThreadPool.AdvanceBy(5); // Attempt to send all 5 requests

            _serviceScheduler.AdvanceBy(1);
            _requestsReceived.Should().HaveCount(5); // Check that 5th request received by service
        }
    }
}
