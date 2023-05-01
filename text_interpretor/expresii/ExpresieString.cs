using Compiler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace proiect_limbaje.expresii
{
    sealed class ExpresieString : Expresie
    {
        public ExpresieString(AtomLexical stringAtomLexical)
        {
            StringAtomLexical = stringAtomLexical;
        }

        public override TipAtomLexical Tip => TipAtomLexical.ExpresieString;
        public AtomLexical StringAtomLexical { get; }

        public override IEnumerable<NodSintactic> GetCopii()
        {
            yield return StringAtomLexical;
        }
    }
}
