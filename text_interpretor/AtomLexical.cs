using Compiler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace proiect_limbaje
{
    class AtomLexical : NodSintactic
    {
        public AtomLexical(TipAtomLexical tip, int index, string text, object valoare)
        {
            Tip = tip;
            Index = index;
            Text = text;
            Valoare = valoare;
        }

        public override TipAtomLexical Tip { get; }
        public int Index { get; }
        public string Text { get; }
        public object Valoare { get; }

        public override IEnumerable<NodSintactic> GetCopii()
        {
            return Enumerable.Empty<NodSintactic>();
        }
    }
}
