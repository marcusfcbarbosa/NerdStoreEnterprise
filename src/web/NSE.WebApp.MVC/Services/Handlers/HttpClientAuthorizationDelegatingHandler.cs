using NSE.WebApi.Core.Usuarios;
using NSE.WebApp.MVC.Extensions;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace NSE.WebApp.MVC.Services.Handlers
{
    public class HttpClientAuthorizationDelegatingHandler : DelegatingHandler
    {
        private readonly IAspNetUser _user;
        public HttpClientAuthorizationDelegatingHandler(IAspNetUser user)
        {
            _user = user;
        }

        //Antes de efetuar Request ele pega o token e salva no Header
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var authorizationHeader = _user.ObterHttpContext().Request.Headers["Authorization"];
            if (!string.IsNullOrEmpty(authorizationHeader))
            {
                request.Headers.Add("Authorization", new List<string>() { authorizationHeader });
            }
            var token = _user.ObterUserToken();

            if(!string.IsNullOrEmpty(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            else
            {
                if (!request.RequestUri.AbsolutePath.Equals("/catalogo/produtos"))
                {
                    throw new CustomHttpRequestException(HttpStatusCode.Forbidden);
                }
            }
            return base.SendAsync(request, cancellationToken);
        }
    }
}
