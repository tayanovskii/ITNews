using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


//    "EmailSender": {
//"Host": "smtp.yandex.ru",
//"Port": 25,
//"EnableSSL": false,
//"UserEmail": "test.itnews@yandex.ru",
//"Password": "Test_itnews"
//}


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
