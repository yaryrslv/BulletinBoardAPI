using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BulletinBoardAPI.DTO
{
    public class CreateUserDto
    {
        public string Name { get; set; }

        public string Password { get; set; }
    }
}
