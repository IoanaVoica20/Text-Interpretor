using Compiler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace proiect_limbaje
{
    sealed class ExpresieDeclarareNeinitializata : Expresie
    {
        public ExpresieDeclarareNeinitializata(AtomLexical tipDeDate, AtomLexical numeVariabila)
        {
            TipDeDate = tipDeDate;
            NumeVariabila = numeVariabila;
        }

        public override TipAtomLexical Tip => TipAtomLexical.ExpresieDeclarareNeinitializataAtom;

        public AtomLexical TipDeDate { get; }
        public AtomLexical NumeVariabila { get; }

        public override IEnumerable<NodSintactic> GetCopii()
        {
            yield return TipDeDate;
            yield return NumeVariabila;
        }
    }
}
