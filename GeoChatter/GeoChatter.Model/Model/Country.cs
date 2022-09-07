using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoChatter.Model
{
    public class Country
    {
        public Country()
        {

        }
        public Country(string code, string name)
        {
            Code = code;
            Name = name;
        }
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
    }
}
