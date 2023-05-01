using Compiler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace proiect_limbaje
{
    sealed class ExpresieNumerica : Expresie
    {
        public ExpresieNumerica(AtomLexical numarAtomLexical)
        {
            NumarAtomLexical = numarAtomLexical;
        }

        public override TipAtomLexical Tip => TipAtomLexical.ExpresieNumerica;
        public AtomLexical NumarAtomLexical { get; }

        public override IEnumerable<NodSintactic> GetCopii()
        {
            yield return NumarAtomLexical;
        }
    }
}
