using System;
using StatNeth.Blaise.Data.DataStructure;

namespace BlaiseAutoCompleteCases.Helpers
{
    internal static class ParameterValidationHelper
    {
        public static void ThrowExceptionIfNullOrEmpty(this string parameter, string parameterName)
        {
            if (parameter == null)
            {
                throw new ArgumentNullException(parameterName);
            }

            if (string.IsNullOrWhiteSpace(parameter))
            {

                throw new ArgumentException($"A value for the argument '{parameterName}' must be supplied");
            }
        }

        public static void ThrowExceptionIfLessThanOrEqualToZero(this int parameter, string parameterName)
        {
            if (parameter <= 0)
            {
                throw new ArgumentException(parameterName);
            }
        }

        //public static void ThrowExceptionIfNull(this IDataRecord parameter, string parameterName)
        //{
        //    if (parameter == null)
        //    {
        //        throw new ArgumentNullException(parameterName);
        //    }
        //}
    }
}
