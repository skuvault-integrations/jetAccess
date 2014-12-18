using System.Collections.Generic;
using System.Linq;

namespace JetAccess.Models.Services.JetRestService.GetProductUrls
{
    internal class GetProductUrlsResponse
    {
        public IEnumerable< string > SkuUrls{ get; private set; }

        public GetProductUrlsResponse( IEnumerable< string > orderUrls )
        {
            if( orderUrls == null )
                orderUrls = new string[ 0 ];

            var enumerable = orderUrls as string[] ?? orderUrls.ToArray();

            var tempArray = new string[ enumerable.Count() ];
            enumerable.ToArray().CopyTo( tempArray, 0 );
            SkuUrls = tempArray;
        }
    }
}