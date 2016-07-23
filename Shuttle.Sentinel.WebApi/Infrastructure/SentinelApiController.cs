using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Shuttle.Sentinel.WebApi
{
    public class SentinelApiController : ApiController
    {
        protected SessionTokenResult GetSessionToken()
        {
            try
            {
                IEnumerable<string> values;

                if (Request.Headers.TryGetValues("sentinel-sessiontoken", out values) && values.Count() == 1)
                {
                    var sessionTokenValue = values.ElementAt(0);
                    Guid sessionToken;

                    return !Guid.TryParse(sessionTokenValue, out sessionToken)
                        ? SessionTokenResult.Failure(Unauthorized())
                        : SessionTokenResult.Success(sessionToken);
                }

                return SessionTokenResult.Failure(Unauthorized());
            }
            catch
            {
                throw new Exception("Could not retrieve the session token");
            }
        }
    }
}