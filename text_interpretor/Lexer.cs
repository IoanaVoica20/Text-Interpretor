using Compiler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace proiect_limbaje
{
    class Lexer
    {
        private readonly string _text;
        private int _index;
        private List<string> _erori = new List<string>();

        public Lexer(string text)
        {
            _text = text;
        }
        public IEnumerable<string> Erori => _erori;

        private char Curent
        {
            get
            {
                if (_index >= _text.Length)
                    return '\0';
                return _text[_index];
            }
        }

        private void Urmatorul()
        {
            _index++;
        }

        public AtomLexical Atom()
        {
            if (_index >= _text.Length)
            {
                throw new Exception($"Lipseste caracterul ; ");
            }

            if (char.IsDigit(Curent))
            {
                var start = _index;

                while (char.IsDigit(Curent))
                    Urmatorul();

                var dimensiune = _index - start;
                var text = _text.Substring(start, dimensiune);
                var n_index = 0;
                if (Curent == '.')
                {
                    Urmatorul();
                    while (char.IsDigit(Curent))
                        Urmatorul();
                    dimensiune = _index - start;
                    text = _text.Substring(start, dimensiune);

                    n_index = _index;
                    if (_text[n_index] != ' ' && _text[n_index] != '+' && _text[n_index] != '-' && _text[n_index] != '*' && _text[n_index] != '/' && _text[n_index] != '=' && _text[n_index] != ',' && _text[n_index] != ';')
                        throw new Exception($"Caracter  neasteptat.");


                    if (!double.TryParse(text, out var valoare))
                        throw new Exception($"Numarul { text } nu poate fi reprezentat ca un Int32.\n");
                    return new AtomLexical(TipAtomLexical.DoubleAtomLexical, start, text, valoare);
                }
                if (!int.TryParse(text, out var valoare2))
                    throw new Exception($"Numarul { text } nu poate fi reprezentat ca un Int32.\n");
                n_index = _index;
                if (_text[n_index] != ' ' && _text[n_index] != '+' && _text[n_index] != '-' && _text[n_index] != '*' && _text[n_index] != '/' && _text[n_index] != '=' && _text[n_index] != ',' && _text[n_index] != ';')
                    throw new Exception($"Caracter  neasteptat.");
                return new AtomLexical(TipAtomLexical.NumarAtomLexical, start, text, valoare2);
            
            }

            if (char.IsLetter(Curent) || Curent == '_')
            {
                var start = _index;

                while (char.IsLetterOrDigit(Curent) || Curent == '_')
                    Urmatorul();

                var dimensiune = _index - start;
                var text = _text.Substring(start, dimensiune);
                var valoare = text;

                if (text == "int" || text == "double" || text == "string")
                    return new AtomLexical(TipAtomLexical.TipDeDateAtomLexical, start, text, null);
                else
                {
                    VariabilaLocala variabila = VariabilaLocala.VerificaExistentaVariabila(text);
                    //var n_index = _index + 1;
                    //if (_text[n_index] != ' ' && _text[n_index] != '+' && _text[n_index] != '-' && _text[n_index] != '*' && _text[n_index] != '/' && _text[n_index] != '=' && _text[n_index] != ',' && _text[n_index] != ';')
                    //    throw new Exception($"Caracter  neasteptat.");

                    if (variabila != null)
                        return new AtomLexical(TipAtomLexical.NumeVariabilaAtomLexical, start, text, variabila._valuare);
                    return new AtomLexical(TipAtomLexical.NumeVariabilaAtomLexical, start, text, null);
                }
                
            }

            if(Curent == '"')
            {
                var start = _index + 1;
                Urmatorul();
                while (Curent != '"')
                {
                    if(Curent == '\0' || Curent == ';')
                    {
                        throw new Exception("Lipseste caracterul ghilimele.\n");
                    }
                    Urmatorul();
                }
                Urmatorul();
                var dimensiune = _index - start - 1;
                var text = _text.Substring(start, dimensiune);

                var n_index = _index + 1;
                if (_text[n_index] != ' ' && _text[n_index] != '+' && _text[n_index] != '-' && _text[n_index] != '*' && _text[n_index] != '/' && _text[n_index] != '=' && _text[n_index] != ',' && _text[n_index] != ';')
                    throw new Exception($"Caracter  neasteptat.");

                return new AtomLexical(TipAtomLexical.StringAtomLexical, start, text, text);

                
            }

            if (Curent == ';')
            {
                Urmatorul();
                return new AtomLexical(TipAtomLexical.TerminatorAtomLexical, _index - 1, ";", null);

            }

            if (Curent == '=')
            {
                Urmatorul();
                return new AtomLexical(TipAtomLexical.EgalAtomLexical, _index - 1, "=", null);
            }

            if(Curent == ',')
            {
                Urmatorul();
                return new AtomLexical(TipAtomLexical.VirgulaAtomLexical, _index - 1, ",", null); 
            }

            if (char.IsWhiteSpace(Curent))
            {
                var start = _index;

                while (char.IsWhiteSpace(Curent))
                    Urmatorul();

                var dimensiune = _index - start;
                var text = _text.Substring(start, dimensiune);
                return new AtomLexical(TipAtomLexical.SpatiuAtomLexical, start, text, null);
            }

            if (Curent == '+')
                return new AtomLexical(TipAtomLexical.PlusAtomLexical, _index++, "+", null);
            else if (Curent == '-')
                return new AtomLexical(TipAtomLexical.MinusAtomLexical, _index++, "-", null);
            else if (Curent == '*')
                return new AtomLexical(TipAtomLexical.StarAtomLexical, _index++, "*", null);
            else if (Curent == '/')
                return new AtomLexical(TipAtomLexical.SlashAtomLexical, _index++, "/", null);
            else if (Curent == '(')
                return new AtomLexical(TipAtomLexical.ParantezaDeschisaAtomLexical, _index++, "(", null);
            else if (Curent == ')')
                return new AtomLexical(TipAtomLexical.ParantezaInchisaAtomLexical, _index++, ")", null);


            throw new Exception($"EROARE: caracter invalid: '{Curent}'");
           // return new AtomLexical(TipAtomLexical.InvalidAtomLexical, _index++, _text.Substring(_index - 1, 1), null);
        }
    }

}
