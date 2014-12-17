using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Jet.Misc;
using JetAccess.Misc;
using JetAccess.Models;
using JetAccess.Models.GetOrders;
using JetAccess.Models.GetProducts;
using JetAccess.Models.Ping;
using JetAccess.Models.UpdateInventory;
using JetAccess.Services;

namespace JetAccess
{
    public class JetService: IJetService, ICreateCallInfo
    {
        internal IJetRestService JetRestService{ get; set; }

        public JetService( JetUserCredentials quickBooksAuthenticatedUserCredentials )
        {
        }

        public Func< string > AdditionalLogInfo{ get; set; }

        public async Task< PingInfo > Ping()
        {
            try
            {
                //todo: replace me
                throw new NotImplementedException();
            }
            catch( Exception exception )
            {
                var quickBooksException = new JetException( this.CreateMethodCallInfo(), exception );
                JetLogger.LogTraceException( quickBooksException );
                throw quickBooksException;
            }
        }

        #region Inventory
        public async Task< IEnumerable< Product > > GetProductsAsync()
        {
            string methodParameters = string.Format( "{{{0}}}", PredefinedValues.NotAvailable );
            string mark = Guid.NewGuid().ToString();
            try
            {
                JetLogger.LogTraceStarted( this.CreateMethodCallInfo( methodParameters, mark ) );

                JetLogger.LogTraceEnded( this.CreateMethodCallInfo( methodParameters, mark, methodResult : "result.ToJson()" ) );

                //todo: replace me
                throw new NotImplementedException();
            }
            catch( Exception exception )
            {
                var quickBooksException = new JetException( this.CreateMethodCallInfo(), exception );
                JetLogger.LogTraceException( quickBooksException );
                throw quickBooksException;
            }
        }

        public async Task UpdateInventoryAsync( IEnumerable< Inventory > products )
        {
            string methodParameters = string.Format( "{{products:{0}}}", "products.ToJson()" );
            string mark = Guid.NewGuid().ToString();
            try
            {
                JetLogger.LogTraceStarted( this.CreateMethodCallInfo( methodParameters, mark ) );

                JetLogger.LogTraceEnded( this.CreateMethodCallInfo( methodParameters, mark, methodResult : PredefinedValues.NotAvailable ) );
            }
            catch( Exception exception )
            {
                var quickBooksException = new JetException( this.CreateMethodCallInfo(), exception );
                JetLogger.LogTraceException( quickBooksException );
                throw quickBooksException;
            }
        }
        #endregion

        #region Orders
        public async Task< IEnumerable< Order > > GetOrdersAsync( DateTime dateFrom, DateTime dateTo )
        {
            string methodParameters = string.Format( "{{dateFrom:{0}, dateTo:{1}}}", dateFrom, dateTo );
            string mark = Guid.NewGuid().ToString();
            try
            {
                JetLogger.LogTraceStarted( this.CreateMethodCallInfo( methodParameters, mark ) );

                JetLogger.LogTraceEnded( this.CreateMethodCallInfo( methodParameters, mark, methodResult : "result.ToJson()" ) );

                //todo: replace me
                throw new NotImplementedException();
            }
            catch( Exception exception )
            {
                var quickBooksException = new JetException( this.CreateMethodCallInfo(), exception );
                JetLogger.LogTraceException( quickBooksException );
                throw quickBooksException;
            }
        }

        public async Task< IEnumerable< Order > > GetOrdersAsync( params string[] docNumbers )
        {
            string methodParameters = string.Format( "{{docNumbers:{0}}}", "docNumbers.ToJson()" );
            string mark = Guid.NewGuid().ToString();

            try
            {
                JetLogger.LogTraceStarted( this.CreateMethodCallInfo( methodParameters, mark ) );

                JetLogger.LogTraceEnded( this.CreateMethodCallInfo( methodParameters, mark, methodResult : "result.ToJson()" ) );

                //todo: replace me
                throw new NotImplementedException();
            }
            catch( Exception exception )
            {
                var quickBooksException = new JetException( this.CreateMethodCallInfo(), exception );
                JetLogger.LogTraceException( quickBooksException );
                throw quickBooksException;
            }
        }

        public async Task< IEnumerable< Order > > GetOrdersAsync()
        {
            try
            {
                //todo: replace me
                throw new NotImplementedException();
            }
            catch( Exception exception )
            {
                var quickBooksException = new JetException( this.CreateMethodCallInfo(), exception );
                JetLogger.LogTraceException( quickBooksException );
                throw quickBooksException;
            }
        }
        #endregion

        private static void LogTraceException( string message, JetException ebayException )
        {
            JetLogger.Log().Trace( ebayException, message );
        }
    }
}