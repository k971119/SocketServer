using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityManager
{
    public class StringSecure
    {
        private readonly string key = "";
        public StringSecure(string hashKey)
        {
            key = hashKey;
        }

        public string StirngHash(string value, string salt)
        {
            string returnStr;
            return "";
        }  
    }
}
