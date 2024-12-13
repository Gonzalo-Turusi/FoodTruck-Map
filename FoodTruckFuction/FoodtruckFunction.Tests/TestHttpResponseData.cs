using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;

namespace FoodtruckFunction.Tests
{
    public class TestHttpRequestData : HttpRequestData
    {
        public TestHttpRequestData(FunctionContext functionContext) : base(functionContext)
        {
            Headers = new HttpHeadersCollection();
        }

        public override HttpHeadersCollection Headers { get; }

        public override Stream Body { get; } = new MemoryStream();

        public override Uri Url { get; } = new Uri("https://example.com");

        public override IReadOnlyCollection<IHttpCookie> Cookies { get; } = new List<IHttpCookie>();

        public override HttpResponseData CreateResponse()
        {
            return new TestHttpResponseData(FunctionContext);
        }

        public override IEnumerable<ClaimsIdentity> Identities { get; } = new List<ClaimsIdentity>();

        public override string Method { get; } = "GET";
    }

    public class TestHttpResponseData : HttpResponseData
    {
        public TestHttpResponseData(FunctionContext functionContext) : base(functionContext)
        {
        }

        public override HttpStatusCode StatusCode { get; set; }

        public override HttpHeadersCollection Headers { get; set; } = new HttpHeadersCollection();

        public override Stream Body { get; set; } = new MemoryStream();

        public override HttpCookies Cookies { get; } = new TestHttpCookies();
    }

    public class TestHttpCookies : HttpCookies
    {
        private readonly List<IHttpCookie> _cookies = new List<IHttpCookie>();

        public override void Append(string name, string value)
        {
            _cookies.Add(new TestHttpCookie(name, value));
        }

        public override void Append(IHttpCookie cookie)
        {
            _cookies.Add(cookie);
        }

        public override IHttpCookie CreateNew()
        {
            return new TestHttpCookie();
        }
    }

    public class TestHttpCookie : IHttpCookie
    {
        public TestHttpCookie() { }

        public TestHttpCookie(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public string Name { get; set; }
        public string Value { get; set; }
        public string? Domain { get; set; }
        public DateTimeOffset? Expires { get; set; }
        public bool? HttpOnly { get; set; }
        public double? MaxAge { get; set; }
        public string? Path { get; set; }
        public SameSite SameSite { get; set; }
        public bool? Secure { get; set; }
    }
}
