using System;
using System.Collections.Generic;
using System.Linq;
using Compiler;
using proiect_limbaje;
using System.IO;

namespace Compiler
{
    class Program
    {
        static public List<VariabilaLocala> variabileLocale = new List<VariabilaLocala>();
        static public void Afiseaza(object text)
        {
            Console.Write(text);
            File.AppendAllText("out.txt", text.ToString());
        }
        static void Main(string[] args)
        {
            int i=-1;
            if (args.Length == 0)
            {
                Afiseaza($"Programul nu a primit ca argument un fisier de test.\n");
            }
            else try
            {
                    Console.WriteLine($"Introduceti 1 pentru citirea de la consola si 2-pentru citirea din fisier");
                    var resp = Console.ReadLine();
                    if (resp == "1")
                    {
                        while(true)
                        {
                            Console.Write("> ");
                            var text = Console.ReadLine();
                            if(string.IsNullOrWhiteSpace(text))
                                return;
                            var parser = new Parser(text);
                            var arboreSintactic = parser.Parseaza();
                            AfiseazaArbore(arboreSintactic.Radacina);
                            var e = new Evaluator(arboreSintactic.Radacina);
                            var rezultat = e.Evalueaza();
                            if (rezultat != null)
                            {
                                Afiseaza(rezultat);
                                Afiseaza("\n");
                            }
                        }
                    }
                    else
                    {

                        File.WriteAllText("out.txt", "");
                        string[] lines = File.ReadAllLines(args[0]);

                        for (i = 0; i < lines.Length; i++)
                        {
                            if (lines[i] == "")
                                goto next;
                            Afiseaza("\n");
                            Afiseaza($"> {lines[i]}\n");
                            var text = lines[i];
                            if (string.IsNullOrWhiteSpace(text))
                                return;

                            var parser = new Parser(text);
                            var arboreSintactic = parser.Parseaza();
                            AfiseazaArbore(arboreSintactic.Radacina);
                            var e = new Evaluator(arboreSintactic.Radacina);
                            var rezultat = e.Evalueaza();
                            if (rezultat != null)
                            {
                                Afiseaza(rezultat);
                                Afiseaza("\n");
                            }
                        next:;
                        }
                    }
                    
            }
            catch(Exception exception)
            {
                    Afiseaza($"Eroare detectata. \n");
                    var culoare = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"S-a detectat eroarea: {exception.Message}  LINIA:{i+1}");
                    Console.ForegroundColor = culoare;
            }
            Console.Write("Apasa orice tasta: ");
            Console.ReadKey();
        }

        static void AfiseazaArbore(NodSintactic nod, string indentare = "", bool ultimulNod = true)
        {
            var prefix = ultimulNod ? "└──" : "├──";
            Afiseaza(indentare);
            Afiseaza(prefix);

            Afiseaza(nod.Tip);

            if (nod is AtomLexical t && t.Valoare != null)
            {
                Afiseaza(" ");
                Afiseaza(t.Valoare);
            }

            Afiseaza("\n");

            indentare += ultimulNod ? "    " : "|   ";

            var ultimulCopil = nod.GetCopii().LastOrDefault();

            foreach (var c in nod.GetCopii())
            {
                AfiseazaArbore(c, indentare, c == ultimulCopil);
            }
        }
    }

    enum TipAtomLexical
    {
        //tipuri de date
        TipDeDateAtomLexical,
        NumarAtomLexical,
        StringAtomLexical,
        DoubleAtomLexical,
                
        //operatori
        PlusAtomLexical,
        MinusAtomLexical,
        StarAtomLexical,
        SlashAtomLexical,
        EgalAtomLexical,

        //expresii
        ExpresieNumerica,
        ExpresieString,
        ExpresieBinara,
        ExpresieParanteze,
        ExpresieDeclarareNeinitializataAtom,
        ExpresieDeclarareInitializataAtom,

        NumeVariabilaAtomLexical,
        DeclarareInLantAtomLexical,
        
        PunctSiVirgulaAtomLexical,
        ParantezaDeschisaAtomLexical,
        ParantezaInchisaAtomLexical,
        VirgulaAtomLexical,
        SpatiuAtomLexical,
        TerminatorAtomLexical,
        InvalidAtomLexical,
    }

    abstract class NodSintactic
    {
        public abstract TipAtomLexical Tip { get; }

        public abstract IEnumerable<NodSintactic> GetCopii();
    }
  
}
