using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVMCore
{
    public interface ITranslator
    {
        string Translate(String id, string text);
        string Translate(int id);

        IList<String> GetTextSuggestions();

        string LanguageIso
        {
            set; get;
        }
    }
}
