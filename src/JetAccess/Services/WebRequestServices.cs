using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Jet.Misc;
using JetAccess.Misc;

namespace JetAccess.Services
{
    internal class WebRequestServices: IWebRequestServices, ICreateCallInfo
    {
        #region BaseRequests
        public async Task< WebRequest > CreateGetRequestAsync( string serviceUrl, string body, Dictionary< string, string > rawHeaders )
        {
            return await CreateCustomRequestAsync( serviceUrl, body, rawHeaders, WebRequestMethods.Http.Get ).ConfigureAwait( false );
        }

        public async Task< WebRequest > CreatePostRequestAsync( string serviceUrl, string body, Dictionary< string, string > rawHeaders )
        {
            return await CreateCustomRequestAsync( serviceUrl, body, rawHeaders, WebRequestMethods.Http.Post ).ConfigureAwait( false );
        }

        protected async Task< WebRequest > CreateCustomRequestAsync( string serviceUrl, string body, Dictionary< string, string > rawHeaders, string method = WebRequestMethods.Http.Get )
        {
            try
            {
                if( rawHeaders == null )
                    rawHeaders = new Dictionary< string, string >();

                var encoding = new UTF8Encoding();
                var encodedBody = encoding.GetBytes( body );

                var serviceRequest = ( HttpWebRequest )WebRequest.Create( serviceUrl );
                serviceRequest.Method = method;
                serviceRequest.ContentType = "application/json";
                serviceRequest.ContentLength = encodedBody.Length;
                serviceRequest.KeepAlive = true;

                foreach( var rawHeadersKey in rawHeaders.Keys )
                {
                    serviceRequest.Headers.Add( rawHeadersKey, rawHeaders[ rawHeadersKey ] );
                }

                using( var newStream = await serviceRequest.GetRequestStreamAsync().ConfigureAwait( false ) )
                    newStream.Write( encodedBody, 0, encodedBody.Length );

                return serviceRequest;
            }
            catch( Exception exc )
            {
                var methodParameters = string.Format( "{{Url:\'{0}\', Body:\'{1}\', Headers:{2}}}", serviceUrl, body, rawHeaders.ToJson() );
                throw new Exception( string.Format( "Exception occured. {0}", this.CreateMethodCallInfo( methodParameters ) ), exc );
            }
        }

        public void PopulateRequestByBody( string body, HttpWebRequest webRequest )
        {
            try
            {
                if( !string.IsNullOrWhiteSpace( body ) )
                {
                    var encodedBody = new UTF8Encoding().GetBytes( body );

                    webRequest.ContentLength = encodedBody.Length;
                    webRequest.ContentType = "text/xml";
                    var getRequestStremTask = webRequest.GetRequestStreamAsync();
                    getRequestStremTask.Wait();
                    using( var newStream = getRequestStremTask.Result )
                        newStream.Write( encodedBody, 0, encodedBody.Length );
                }
            }
            catch( Exception exc )
            {
                var webrequestUrl = "null";

                if( webRequest != null )
                {
                    if( webRequest.RequestUri != null )
                    {
                        if( webRequest.RequestUri.AbsoluteUri != null )
                            webrequestUrl = webRequest.RequestUri.AbsoluteUri;
                    }
                }

                throw new Exception( string.Format( "Exception occured on PopulateRequestByBody(body:{0}, webRequest:{1})", body ?? "null", webrequestUrl ), exc );
            }
        }
        #endregion

        #region ResponseHanding
        public Stream GetResponseStream( WebRequest webRequest )
        {
            try
            {
                using( var response = ( HttpWebResponse )webRequest.GetResponse() )
                using( var dataStream = response.GetResponseStream() )
                {
                    var memoryStream = new MemoryStream();
                    if( dataStream != null )
                        dataStream.CopyTo( memoryStream, 0x100 );
                    memoryStream.Position = 0;
                    return memoryStream;
                }
            }
            catch( Exception ex )
            {
                var webrequestUrl = "null";

                if( webRequest != null )
                {
                    if( webRequest.RequestUri != null )
                    {
                        if( webRequest.RequestUri.AbsoluteUri != null )
                            webrequestUrl = webRequest.RequestUri.AbsoluteUri;
                    }
                }

                throw new Exception( string.Format( "Exception occured on GetResponseStream( webRequest:{0})", webrequestUrl ), ex );
            }
        }

        public async Task< Stream > GetResponseStreamAsync( WebRequest webRequest )
        {
            try
            {
                using( var response = ( HttpWebResponse )await webRequest.GetResponseAsync().ConfigureAwait( false ) )
                using( var dataStream = await new TaskFactory< Stream >().StartNew( () => response != null ? response.GetResponseStream() : null ).ConfigureAwait( false ) )
                {
                    var memoryStream = new MemoryStream();
                    await dataStream.CopyToAsync( memoryStream, 0x100 ).ConfigureAwait( false );
                    memoryStream.Position = 0;
                    return memoryStream;
                }
            }
            catch( Exception ex )
            {
                var webrequestUrl = PredefinedValues.NotAvailable;

                if( webRequest != null )
                {
                    if( webRequest.RequestUri != null )
                    {
                        if( webRequest.RequestUri.AbsoluteUri != null )
                            webrequestUrl = webRequest.RequestUri.AbsoluteUri;
                    }
                }

                throw new Exception( string.Format( "Exception occured on GetResponseStreamAsync( webRequest:{0})", webrequestUrl ), ex );
            }
        }
        #endregion
    }
}