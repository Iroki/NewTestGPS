using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GeolocationApp
{
    public interface IPlacePicker
    {
        Task<TryResult<Place>> PickPlace();        
    }
}
