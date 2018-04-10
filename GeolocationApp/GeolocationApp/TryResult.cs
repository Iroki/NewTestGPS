using System;
using System.Collections.Generic;
using System.Text;

namespace GeolocationApp
{
    public class TryResult<TData> where TData : class
    {
        private TryResult(bool operationSucceed, TData data)
        {
            OperationSucceed = operationSucceed;
        }
        public bool OperationSucceed { get; }

        public TData Data { get; }

        public static TryResult<TData> Succeed(TData param) => new TryResult<TData>(true, param);

        public static TryResult<TData> Unsucceed() => new TryResult<TData>(false, null);
    }
}
