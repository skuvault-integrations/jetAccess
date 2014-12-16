using System.Collections.Generic;
using System.Linq;

namespace JetAccess.Services
{
    internal class GetProductsResponse
    {
        public IEnumerable< string > SkuUrls{ get; private set; }

        public GetProductsResponse( IEnumerable< string > orderUrls )
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