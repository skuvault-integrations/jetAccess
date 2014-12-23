using System;
using System.Threading.Tasks;
using Jet.Misc;
using Netco.ActionPolicyServices;
using Netco.Utils;

namespace JetAccess.Misc
{
	internal static class ActionPolicies
	{
		private static readonly ActionPolicy _jetSumbitPolicy = ActionPolicy.Handle< Exception >().Retry( 10, ( ex, i ) =>
		{
			JetLogger.Log().Trace( ex, "Retrying Jet API submit call for the {0} time", i );
			SystemUtil.Sleep( TimeSpan.FromSeconds( 0.5 + i ) );
		} );

		private static readonly ActionPolicyAsync _jetSumbitAsyncPolicy = ActionPolicyAsync.Handle< Exception >()
			.RetryAsync( 10, async ( ex, i ) =>
			{
				JetLogger.Log().Trace( ex, "Retrying Jet API submit call for the {0} time", i );
				await Task.Delay( TimeSpan.FromSeconds( 0.5 + i ) ).ConfigureAwait( false );
			} );

		private static readonly ActionPolicy _jetGetPolicy = ActionPolicy.Handle< Exception >().Retry( 10, ( ex, i ) =>
		{
			JetLogger.Log().Trace( ex, "Retrying Jet API get call for the {0} time", i );
			SystemUtil.Sleep( TimeSpan.FromSeconds( 0.5 + i ) );
		} );

		private static readonly ActionPolicyAsync _jetGetAsyncPolicy = ActionPolicyAsync.Handle< Exception >()
			.RetryAsync( 10, async ( ex, i ) =>
			{
				JetLogger.Log().Trace( ex, "Retrying Jet API get call for the {0} time", i );
				await Task.Delay( TimeSpan.FromSeconds( 0.5 + i ) ).ConfigureAwait( false );
			} );

		public static ActionPolicy Submit
		{
			get { return _jetSumbitPolicy; }
		}

		public static ActionPolicyAsync SubmitAsync
		{
			get { return _jetSumbitAsyncPolicy; }
		}

		public static ActionPolicy Get
		{
			get { return _jetGetPolicy; }
		}

		public static ActionPolicyAsync GetAsync
		{
			get { return _jetGetAsyncPolicy; }
		}
	}
}