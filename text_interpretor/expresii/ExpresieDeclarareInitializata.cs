using Compiler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace proiect_limbaje
{
    sealed class ExpresieDeclarareInitializata : Expresie
    {
        public ExpresieDeclarareInitializata(AtomLexical tipDeDate, AtomLexical numeVariabila, AtomLexical valoare)
        {
            TipDeDate = tipDeDate;
            NumeVariabila = numeVariabila;
            Valoare = valoare;
        }

        public override TipAtomLexical Tip => TipAtomLexical.ExpresieDeclarareInitializataAtom;

        public AtomLexical TipDeDate { get; }
        public AtomLexical NumeVariabila { get; }
        public AtomLexical Valoare { get; }

        public override IEnumerable<NodSintactic> GetCopii()
        {
            yield return TipDeDate;
            yield return NumeVariabila;
            yield return Valoare;
        }
    }
}
