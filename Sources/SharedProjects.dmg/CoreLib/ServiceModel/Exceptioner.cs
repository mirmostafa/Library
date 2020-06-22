using System;
using System.ServiceModel;

namespace Mohammad.ServiceModel
{
    public static class Exceptioner
    {
        public static void Throw(Exception ex)
        {
            if (ex is FaultException)
                throw ex;
            throw new FaultException(new FaultReason(ex.GetBaseException().Message));
        }

        public static void Throw<TDetail>(string reason) where TDetail : new() { Throw(() => new TDetail(), reason); }

        public static void Throw<TDetail>(string message, string reason)
        {
            Throw(() => (TDetail) typeof(TDetail).GetConstructor(new Type[] {}).Invoke(new object[] {message}), reason);
        }

        public static void Throw<TDetail>(Func<TDetail> creator, string reason) { throw new FaultException<TDetail>(creator(), reason); }
    }
}