using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Jet.Misc;
using JetAccess;
using JetAccess.Models;
using JetAccess.Models.GetOrders;
using JetAccess.Models.GetProducts;
using JetAccess.Models.Ping;
using JetAccess.Models.UpdateInventory;

namespace QuickBooksOnlineAccess
{
	public class JetService : IJetService
	{
		public JetService(JetUserCredentials quickBooksAuthenticatedUserCredentials)
		{
		}

		public Func<string> AdditionalLogInfo { get; set; }

		public async Task<PingInfo> Ping()
		{
			try
			{
				//todo: replace me
				throw new NotImplementedException();
			}
			catch (Exception exception)
			{
				var quickBooksException = new JetException(CreateMethodCallInfo(), exception);
				JetLogger.LogTraceException(quickBooksException);
				throw quickBooksException;
			}
		}

		public async Task<IEnumerable<Product>> GetProductsAsync()
		{
			string methodParameters = string.Format("{{{0}}}", PredefinedValues.NotAvailable);
			string mark = Guid.NewGuid().ToString();
			try
			{
				JetLogger.LogTraceStarted(CreateMethodCallInfo(methodParameters, mark));

				JetLogger.LogTraceEnded(CreateMethodCallInfo(methodParameters, mark, methodResult: "result.ToJson()"));


				//todo: replace me
				throw new NotImplementedException();
			}
			catch (Exception exception)
			{
				var quickBooksException = new JetException(CreateMethodCallInfo(), exception);
				JetLogger.LogTraceException(quickBooksException);
				throw quickBooksException;
			}
		}

		public async Task UpdateInventoryAsync(IEnumerable<Inventory> products)
		{
			string methodParameters = string.Format("{{products:{0}}}", "products.ToJson()");
			string mark = Guid.NewGuid().ToString();
			try
			{
				JetLogger.LogTraceStarted(CreateMethodCallInfo(methodParameters, mark));

				JetLogger.LogTraceEnded(CreateMethodCallInfo(methodParameters, mark, methodResult: PredefinedValues.NotAvailable));
			}
			catch (Exception exception)
			{
				var quickBooksException = new JetException(CreateMethodCallInfo(), exception);
				JetLogger.LogTraceException(quickBooksException);
				throw quickBooksException;
			}
		}

		#region Orders

		public async Task<IEnumerable<Order>> GetOrdersAsync(DateTime dateFrom, DateTime dateTo)
		{
			string methodParameters = string.Format("{{dateFrom:{0}, dateTo:{1}}}", dateFrom, dateTo);
			string mark = Guid.NewGuid().ToString();
			try
			{
				JetLogger.LogTraceStarted(CreateMethodCallInfo(methodParameters, mark));

				JetLogger.LogTraceEnded(CreateMethodCallInfo(methodParameters, mark, methodResult: "result.ToJson()"));


				//todo: replace me
				throw new NotImplementedException();
			}
			catch (Exception exception)
			{
				var quickBooksException = new JetException(CreateMethodCallInfo(), exception);
				JetLogger.LogTraceException(quickBooksException);
				throw quickBooksException;
			}
		}

		public async Task<IEnumerable<Order>> GetOrdersAsync(params string[] docNumbers)
		{
			string methodParameters = string.Format("{{docNumbers:{0}}}", "docNumbers.ToJson()");
			string mark = Guid.NewGuid().ToString();

			try
			{
				JetLogger.LogTraceStarted(CreateMethodCallInfo(methodParameters, mark));

				JetLogger.LogTraceEnded(CreateMethodCallInfo(methodParameters, mark, methodResult: "result.ToJson()"));


				//todo: replace me
				throw new NotImplementedException();
			}
			catch (Exception exception)
			{
				var quickBooksException = new JetException(CreateMethodCallInfo(), exception);
				JetLogger.LogTraceException(quickBooksException);
				throw quickBooksException;
			}
		}

		public async Task<IEnumerable<Order>> GetOrdersAsync()
		{
			try
			{
				//todo: replace me
				throw new NotImplementedException();
			}
			catch (Exception exception)
			{
				var quickBooksException = new JetException(CreateMethodCallInfo(), exception);
				JetLogger.LogTraceException(quickBooksException);
				throw quickBooksException;
			}
		}

		#endregion

		private string CreateMethodCallInfo(string methodParameters = "", string mark = "", string errors = "",
			string methodResult = "", string additionalInfo = "", [CallerMemberName] string memberName = "")
		{
			string restInfo = " this._quickBooksOnlineServiceSdk.ToJson()";
			string str = string.Format(
				"{{MethodName:{0}, ConnectionInfo:{1}, MethodParameters:{2}, Mark:{3}{4}{5}{6}}}",
				memberName,
				restInfo,
				methodParameters,
				mark,
				string.IsNullOrWhiteSpace(errors) ? string.Empty : ", Errors:" + errors,
				string.IsNullOrWhiteSpace(methodResult) ? string.Empty : ", Result:" + methodResult,
				string.IsNullOrWhiteSpace(additionalInfo) ? string.Empty : ", " + additionalInfo
				);
			return str;
		}

		private static void LogTraceException(string message, JetException ebayException)
		{
			JetLogger.Log().Trace(ebayException, message);
		}
	}
}