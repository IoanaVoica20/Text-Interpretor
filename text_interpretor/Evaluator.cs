using Compiler;
using proiect_limbaje.expresii;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace proiect_limbaje
{
    class Evaluator
    {
     

        private readonly Expresie _radacina;

        public Evaluator(Expresie radacina)
        {
            this._radacina = radacina;
        }

        public dynamic Evalueaza()
        {
            return EvalueazaExpresie(_radacina);
        }

        private dynamic EvalueazaExpresie(Expresie nod)
        {
            if (nod is ExpresieDouble db)
                return (double)db.NumarAtomLexical.Valoare;

            if (nod is ExpresieNumerica n)
            {
                return (int)n.NumarAtomLexical.Valoare;
            }

            if( nod is ExpresieString s)
            {
                return (string)s.StringAtomLexical.Valoare;
            }

            if (nod is ExpresieBinara b)
            {
                var stanga = EvalueazaExpresie(b.ExpresieStanga);
                var dreapta = EvalueazaExpresie(b.ExpresieDreapta);

                dynamic rezultat;

                if (b.OperatorAtomLexical.Tip == TipAtomLexical.PlusAtomLexical)
                    rezultat = stanga + dreapta;
                else if (b.OperatorAtomLexical.Tip == TipAtomLexical.MinusAtomLexical)
                    rezultat = stanga - dreapta;
                else if (b.OperatorAtomLexical.Tip == TipAtomLexical.StarAtomLexical)
                    rezultat = stanga * dreapta;
                else if (b.OperatorAtomLexical.Tip == TipAtomLexical.SlashAtomLexical)
                    rezultat = stanga / dreapta;
                else
                    throw new Exception($"Operator binar neasteptat {b.OperatorAtomLexical.Tip}.");
                return rezultat;

            }

            if (nod is ExpresieParanteze p)
                return EvalueazaExpresie(p.Expresie);

            if (nod is ExpresieDeclarareInLant de)
            {
                for(int i = 0; i < de.ListaExpresii.Count; i++)
                {
                    if (de.ListaExpresii[i].Tip == TipAtomLexical.ExpresieDeclarareInitializataAtom)
                        EvalueazaExpresie(de.ListaExpresii[i]);
                }
                return null;
            }

            if (nod is ExpresieDeclarareInitializata d)
            {
                return d.Valoare;
            }

            throw new Exception($"Expresie neasteptata {nod.Tip}.");
        }
      
    }

    
}
