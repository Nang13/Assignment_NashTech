using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PES.Domain.Entities.Model.AggregateChat
{
    public class User
    {
        public string Username { get; set; }
        public List<string> Contacts { get; set; } = new List<string>();
    }
}
