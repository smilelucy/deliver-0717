using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace JSJ.Services
{
    public interface IQRCode
    {
        Task<string> ScanAsync();
    }
}
