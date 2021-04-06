using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Principal;
using BulletinBoardAPI.Models.Implementations;
using IdentityModel;

namespace BulletinBoardAPI.Models.Realizations
{
    public class Ad 
    {
        public Guid Id { get; set; }
        public int Number { get; set; }
        public string UserName { get; set; }
        public string Text { get; set; }
        public string ImageUrl { get; set; }
        public int Rating { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ExpirationDite { get; set; }
    }
}
