using System;
using System.Collections.Generic;
using System.Text;

namespace GeolocationApp
{
    public class TryResult<TData> where TData : class // изначально "where" и далее было необязательно
    {
        private TryResult(bool operationSucceed, TData data)
        {
            OperationSucceed = operationSucceed;
            Data = data;
        }
        public bool OperationSucceed { get; }

        public TData Data { get; } //изначально было необязательно

        public static TryResult<TData> Succeed(TData param) => new TryResult<TData>(true, param);

        public static TryResult<TData> Unsucceed() => new TryResult<TData>(false, null);
    }
}
