using Compiler;
using proiect_limbaje.expresii;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace proiect_limbaje
{
    class Parser
    {
        private readonly AtomLexical[] _atomiLexicali;
        private int _index;
        private List<string> _erori = new List<string>();
       
       
        public Parser(string text)
        {
            var atomiLexicali = new List<AtomLexical>();
            var lexer = new Lexer(text);
            AtomLexical atomLexical;
            do
            {
                atomLexical = lexer.Atom();

                if (atomLexical.Tip != TipAtomLexical.SpatiuAtomLexical &&
                    atomLexical.Tip != TipAtomLexical.InvalidAtomLexical)
                {
                    atomiLexicali.Add(atomLexical);
                }

            }
            while (atomLexical.Tip != TipAtomLexical.TerminatorAtomLexical);

            _atomiLexicali = atomiLexicali.ToArray();
            _erori.AddRange(lexer.Erori);
        }

        public IEnumerable<string> Erori => _erori;
        private AtomLexical Varf(int avans)
        {
            var index = _index + avans;
            if (index >= _atomiLexicali.Length)
                return _atomiLexicali[_atomiLexicali.Length - 1];

            return _atomiLexicali[index];
        }

        private AtomLexical Curent => Varf(0);

        private AtomLexical UrmatorulAtomLexical()
        {
            var curent = Curent;
            _index++;
            return curent;
        }

        private AtomLexical Verifica(TipAtomLexical tip)
        {
            if (Curent.Tip == tip)
                return UrmatorulAtomLexical();

            _erori.Add($"EROARE: Atom lexical neasteptat <{Curent.Tip}>; se asteapta <{tip}>");
            return new AtomLexical(tip, Curent.Index, null, null);
        }

        private Expresie ParseazaExpresie()
        {
            return ParseazaTermen();
        }

        public ArboreSintactic Parseaza()
        {
            var expresie = ParseazaTermen();
            var terminator = Verifica(TipAtomLexical.TerminatorAtomLexical);

            return new ArboreSintactic(_erori, expresie, terminator);
        }

        private Expresie ParseazaTermen()
        {
            var stanga = ParseazaFactor();

            while (Curent.Tip == TipAtomLexical.PlusAtomLexical ||
                Curent.Tip == TipAtomLexical.MinusAtomLexical)
            {
                var operatorAtomLexical = UrmatorulAtomLexical();
                var dreapta = ParseazaFactor();
                var tip = ComparaTipDeDate(stanga, dreapta);
                if (tip == null)
                    throw new Exception($"Nu se pot efectua operatii intre operanzi de tip diferit.");
                stanga = new ExpresieBinara(stanga, operatorAtomLexical, dreapta,tip);
            }

            return stanga;
        }

        private Expresie ParseazaFactor()
        {
            var stanga = ParseazaPrimaExpresie();

            while (Curent.Tip == TipAtomLexical.StarAtomLexical ||
                Curent.Tip == TipAtomLexical.SlashAtomLexical)
            {
                var operatorAtomLexical = UrmatorulAtomLexical();
                var dreapta = ParseazaPrimaExpresie();
                var tip = ComparaTipDeDate(stanga, dreapta);
                if (tip == null)
                    throw new Exception($"Nu se pot efectua operatii intre operanzi de tip diferit.");
                stanga = new ExpresieBinara(stanga, operatorAtomLexical, dreapta,tip);
            }

            return stanga;
        }
        private Expresie ParseazaPrimaExpresie()
        {

            if (Curent.Tip == TipAtomLexical.TipDeDateAtomLexical)
            {
                List<Expresie> l = new List<Expresie>();
                ExpresieDeclarareInLant expr = new ExpresieDeclarareInLant(l);
                var tipDeDate = UrmatorulAtomLexical();
                AtomLexical urmatorul;
                do
                {
                    var variabila = UrmatorulAtomLexical();
                    urmatorul = UrmatorulAtomLexical();
                    VariabilaLocala v;

                    if (urmatorul.Text == "=")
                    {
                        dynamic rezultat;
                        AtomLexical atom;
                      
                        var valoare = ParseazaExpresie();
       
                        var e = new Evaluator(valoare);
                        rezultat = e.Evalueaza();
                       
                        if (tipDeDate.Text == "int")
                        {
                            rezultat = (int)rezultat;
                            atom = new AtomLexical(TipAtomLexical.NumarAtomLexical, 0, "", rezultat);
                        }
                        else if(tipDeDate.Text == "double")
                            atom = new AtomLexical(TipAtomLexical.DoubleAtomLexical, 0, "", rezultat);
                        else
                            atom = new AtomLexical(TipAtomLexical.StringAtomLexical, 0, "", rezultat);
                       
                        v = new VariabilaLocala(tipDeDate.Text, variabila.Text, rezultat);
                
                        if (VariabilaLocala.VerificaExistentaVariabila(variabila.Text) == null)
                        {
                            Program.variabileLocale.Add(v);
                            expr.AdaugaExpresie(new ExpresieDeclarareInitializata(tipDeDate, variabila, atom));
                            
                        }
                        else
                            throw new Exception($"Redeclarare variabila.");
                        urmatorul = UrmatorulAtomLexical();
                    }
                    else
                    {
                        v = new VariabilaLocala(tipDeDate.Text, variabila.Text, null);
                        if (VariabilaLocala.VerificaExistentaVariabila(variabila.Text) == null)
                        {
                            Program.variabileLocale.Add(v);
                            expr.AdaugaExpresie(new ExpresieDeclarareNeinitializata(tipDeDate, variabila));
                        
                        }
                        else
                            throw new Exception($"Redeclarare variabila.");
                    }
                } while (urmatorul.Text != ";");
                return expr;

            }
            if (Curent.Tip == TipAtomLexical.ParantezaDeschisaAtomLexical)
            {
                var stanga = UrmatorulAtomLexical();
                var expresie = ParseazaExpresie();
                var dreapta = Verifica(TipAtomLexical.ParantezaInchisaAtomLexical);
                return new ExpresieParanteze(stanga, expresie, dreapta);
            }
            if (Curent.Tip == TipAtomLexical.NumeVariabilaAtomLexical)
            {
                var numeVariabila = UrmatorulAtomLexical();
                VariabilaLocala v = VariabilaLocala.VerificaExistentaVariabila(numeVariabila.Text);
                if (v != null)
                {
                    
                    if(Curent.Tip == TipAtomLexical.EgalAtomLexical)
                    {
                        var egal = UrmatorulAtomLexical();
                       
                        dynamic rezultat;
                        Expresie expresie;
                        if (Curent.Tip == TipAtomLexical.NumeVariabilaAtomLexical)
                        {
                            expresie = ParseazaPrimaExpresie();
                            var e = new Evaluator(expresie);
                            rezultat = e.Evalueaza();
                           
                        }
                        else
                        {
                            var valoare = UrmatorulAtomLexical();
                            rezultat = valoare.Valoare;
                        }
                        
                        v.SeteazaValoare(rezultat);
                        
                    }
                    if (v._valuare == null)
                    {
                        throw new Exception($"Variabila {v._numeVariabila} nu a fost initializata.");
                    }

                    if (v._tipDeDate == "int")
                    {
                        AtomLexical atom = new AtomLexical(TipAtomLexical.NumarAtomLexical, 0, v._numeVariabila, v._valuare);
                        return new ExpresieNumerica(atom);
                    }
                    if (v._tipDeDate == "double")
                    {
                        AtomLexical atom = new AtomLexical(TipAtomLexical.DoubleAtomLexical, 0, v._numeVariabila, v._valuare);
                        return new ExpresieDouble(atom);
                    }
                    if (v._tipDeDate == "string")
                    {
                        AtomLexical atom = new AtomLexical(TipAtomLexical.StringAtomLexical, 0, v._numeVariabila, v._valuare);
                        return new ExpresieString(atom);
                    }
                }
                else throw new Exception($"Variabila {numeVariabila.Text} nu a fost declarata.");
                
            }
            if (Curent.Tip == TipAtomLexical.StringAtomLexical)
            {
                return new ExpresieString(UrmatorulAtomLexical());
            }
            if (Curent.Tip == TipAtomLexical.DoubleAtomLexical)
            {
                return new ExpresieDouble(UrmatorulAtomLexical());
            }

            var numberToken = Verifica(TipAtomLexical.NumarAtomLexical);
                return new ExpresieNumerica(numberToken);
            
        }

        private dynamic ComparaTipDeDate(Expresie a, Expresie b)
        {
            TipAtomLexical tipA = a.Tip;
            TipAtomLexical tipB = b.Tip;

            if (a is ExpresieBinara)
                tipA = ((ExpresieBinara)a)._Tip;
            if (b is ExpresieBinara)
                tipB = ((ExpresieBinara)b)._Tip;
            if (tipA == tipB)
                return tipA;
            return null;
        }
    }


}
