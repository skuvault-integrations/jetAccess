using System.Collections.Generic;
using System.Linq;

namespace JetAccess.Models.Services.JetRestService.GetOrderIds
{
    internal class GetOrderUrlsResponse
    {
        public IEnumerable< string > OrderUrls{ get; private set; }

        public GetOrderUrlsResponse( IEnumerable< string > orderUrls )
        {
            if( orderUrls == null )
                orderUrls = new string[ 0 ];

            var enumerable = orderUrls as string[] ?? orderUrls.ToArray();

            var tempArray = new string[ enumerable.Count() ];
            enumerable.ToArray().CopyTo( tempArray, 0 );
            OrderUrls = tempArray;
        }
    }
}