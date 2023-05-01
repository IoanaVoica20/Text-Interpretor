using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Compiler;

namespace proiect_limbaje
{
    class VariabilaLocala
    {
        public string _tipDeDate;
        public string _numeVariabila;
        public dynamic _valuare;

        public VariabilaLocala(string tip, string nume, dynamic valuare)
        {
            _tipDeDate = tip;
            _numeVariabila = nume;
            _valuare = valuare;
        }

        public static VariabilaLocala VerificaExistentaVariabila(string variabila)
        {
            for (int i = 0; i < Program.variabileLocale.Count; i++)
                if (Program.variabileLocale[i]._numeVariabila == variabila)
                    return Program.variabileLocale[i];
            return null;
        }

        public static bool VerificaCorectitudineVariabila(string tip, dynamic val)
        {
            if ((tip == "int" || tip == "double") && 
                (val == TipAtomLexical.ExpresieNumerica || val == TipAtomLexical.NumarAtomLexical ||
                val == "int" || val == "double"))
                return true;
            if (tip == "string" && (val == TipAtomLexical.ExpresieString ||
                val == TipAtomLexical.StringAtomLexical || val == "string"))
                return true;
            return false;
        }

        public void SeteazaValoare(dynamic valoare)
        {

            _valuare = valoare;
            for(int i =0; i <Program.variabileLocale.Count; i++)
            {
                if (Program.variabileLocale[i]._numeVariabila == _numeVariabila)
                    Program.variabileLocale[i]._valuare = _valuare;
            }
        }

    }
}
