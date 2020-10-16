using System.Collections.Generic;

namespace OnboardingSIGDB1.Domain._Base
{
    public class ResultadoDaConsultaBase
    {
        public int Total { get; set; }
        public IEnumerable<object> Lista { get; set; }
    }
}
