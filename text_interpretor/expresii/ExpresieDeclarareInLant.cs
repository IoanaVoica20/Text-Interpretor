using Compiler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace proiect_limbaje
{
    sealed class ExpresieDeclarareInLant : Expresie
    {
        public ExpresieDeclarareInLant(List<Expresie> ex)
        {
            ListaExpresii = ex;
        }

        public void AdaugaExpresie(Expresie e)
        {
            ListaExpresii.Add(e);
        }

        public override TipAtomLexical Tip => TipAtomLexical.DeclarareInLantAtomLexical;
        public List<Expresie> ListaExpresii { get; }
        public override IEnumerable<NodSintactic> GetCopii()
        {
            for(int i = 0; i < ListaExpresii.Count; i++)
                yield return ListaExpresii[i];
        }

    }
}
