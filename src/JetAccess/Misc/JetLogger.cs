using System;
using Netco.Logging;

namespace Jet.Misc
{
    internal class JetLogger
    {
        public static ILogger Log()
        {
            return NetcoLogger.GetLogger( "JetLogger" );
        }

        public static void LogTraceException( Exception exception )
        {
            Log().Trace( exception, "[jet] An exception occured." );
        }

        public static void LogTraceStarted( string info )
        {
            Log().Trace( "[jet] Start call:{0}.", info );
        }

        public static void LogTraceEnded( string info )
        {
            Log().Trace( "[jet] End call:{0}.", info );
        }

        public static void LogTrace( string info )
        {
            Log().Trace( "[jet] Trace info:{0}.", info );
        }
    }
}