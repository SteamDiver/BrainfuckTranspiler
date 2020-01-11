using System;
using System.IO;
using Antlr.Runtime;
using Antlr.Runtime.Tree;
using BrainfuckTranspiler;

namespace MathLang
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                var input = args.Length == 1 ? new ANTLRFileStream(args[0])
                                                     : (ICharStream)new ANTLRReaderStream(Console.In);
                var lexer = new MathLangLexer(input);
                var tokens = new CommonTokenStream(lexer);
                var parser = new MathLangParser(tokens);
                var program = (ITree)parser.execute().Tree;
                AstNodePrinter.Print(program);
                Console.WriteLine();
                var transpiler = new Transpiler(program);
                var output = transpiler.Transpile();
                Console.WriteLine(output);
                if(args.Length > 1)
                    SaveToFile(output, args[1]);
                Console.ReadKey();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: {0}", e);
            }
        }

        private static void SaveToFile(string content, string path)
        {
            using (var file = File.CreateText(path))
            {
                file.Write(content);
            }
        }
    }
}
