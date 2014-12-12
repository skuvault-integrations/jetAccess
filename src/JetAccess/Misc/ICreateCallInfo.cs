using System.Runtime.CompilerServices;
using Jet.Misc;

namespace JetAccess.Misc
{
    public interface ICreateCallInfo
    {
    }

    public static class ICreateCallInfoExtensions
    {
        public static string CreateMethodCallInfo< T >( this T obj, string methodParameters = "", string mark = "", string errors = "", string methodResult = "", string additionalInfo = "", [ CallerMemberName ] string memberName = "" )
        {
            var restInfo = PredefinedValues.EmptyJsonObject;
            var str = string.Format(
                "{{MethodName:{0}, ConnectionInfo:{1}, MethodParameters:{2}, Mark:{3}{4}{5}{6}}}",
                memberName,
                restInfo,
                methodParameters,
                mark,
                string.IsNullOrWhiteSpace( errors ) ? string.Empty : ", Errors:" + errors,
                string.IsNullOrWhiteSpace( methodResult ) ? string.Empty : ", Result:" + methodResult,
                string.IsNullOrWhiteSpace( additionalInfo ) ? string.Empty : ", " + additionalInfo
                );
            return str;
        }
    }
}