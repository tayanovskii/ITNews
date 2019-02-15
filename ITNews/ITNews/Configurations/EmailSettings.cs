using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;




namespace ITNews.Configurations
{
    public class EmailSettings
    {
        public string Host { get; set; }
        public int Port  { get; set; }
        public bool EnableSSL { get; set; }
        public string UserEmail { get; set; }
        public string Password { get; set; }
    }
}
