using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BulletinBoardAPI.Models.Implementations
{
    interface IResponse
    {
        public string Status { get; set; }
        public string Message { get; set; }
    }
}
