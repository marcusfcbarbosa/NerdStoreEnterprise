using System.Collections.Generic;

namespace NSE.Identity.API.Extensions
{
    public class AppSettings
    {
        public string Secret { get; set; }
        public int ExpiracaoHoras { get; set; }
        public string Emissor { get; set; }
        public List<string> ValidoEm { get; set; }
    }
}
