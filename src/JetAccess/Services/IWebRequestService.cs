using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace JetAccess.Services
{
    internal interface IWebRequestServices
    {
        Stream GetResponseStream( WebRequest webRequest );

        Task< Stream > GetResponseStreamAsync( WebRequest webRequest );

        Task< WebRequest > CreateGetRequestAsync( string serviceUrl, string body, Dictionary< string, string > rawHeaders );

        void PopulateRequestByBody( string body, HttpWebRequest webRequest );
        Task< WebRequest > CreatePostRequestAsync( string serviceUrl, string body, Dictionary< string, string > rawHeaders );
    }
}