using System.Threading.Tasks;
using JetAccess.Models.Services.JetRestService.GetMerchantSkusInventory;
using JetAccess.Models.Services.JetRestService.GetOrderIds;
using JetAccess.Models.Services.JetRestService.GetOrderWithOutShipmentDetail;
using JetAccess.Models.Services.JetRestService.GetOrderWithShipmentDetail;
using JetAccess.Models.Services.JetRestService.GetProductUrls;
using JetAccess.Models.Services.JetRestService.GetToken;
using JetAccess.Models.Services.JetRestService.PutMerchantSkusInventory;

namespace JetAccess.Services
{
	internal interface IJetRestService
	{
		Task< TokenInfo > GetTokenOrReturnChachedAsync();
		Task< GetTokenResponse > GetTokenAsync();
		GetTokenResponse GetToken();
		Task< GetOrderUrlsResponse > GetOrderUrlsAsync();
		Task< GetOrderWithoutShipmentDetailResponse > GetOrderWithoutShipmentDetailAsync( string orderUrl );
		Task< GetProductUrlsResponse > GetProductUrlsAsync();
		Task< GetMerchantSkusInventoryResponse > GetMerchantSkusInventoryAsync( string productUrl );
		Task< PatchMerchantSkusInventoryResponse > PatchMerchantSkusInventoryAsync( PatchMerchantSkusInventoryRequest patchMerchantSkusInventoryRequest );
		Task< GetOrderWithShipmentDetailResponse > GetOrderWithShipmentDetailAsync( string s );
	}
}