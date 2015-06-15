/*
*Analyzer.cs - Manages Code Analysis
 *
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeAnalysis
{
    class Analyzer
    {
        static public string[] getFiles(string path, List<string> patterns) {

            FileMgr fm = new FileMgr();
            foreach (string pattern in patterns)
              
            fm.addpattern(pattern);
            fm.findFiles(path);
            return fm.getFiles().ToArray();
        
        }


        static void doAnalysis(string[] files)
        {
            Console.Write("\n  Demonstrating Parser");
            Console.Write("\n ======================\n");

            //ShowCommandLine(args);

            //List<string> files = TestParser.ProcessCommandline(args);
            foreach (object file in files)
            {
                Console.Write("\n  Processing file {0}\n", file as string);

                CSsemi.CSemiExp semi = new CSsemi.CSemiExp();
                semi.displayNewLines = false;
                if (!semi.open(file as string))
                {
                    Console.Write("\n  Can't open {0}\n\n", file);
                    return;
                }

                Console.Write("\n  Type and Function Analysis");
                Console.Write("\n ----------------------------\n");

                BuildCodeAnalyzer builder = new BuildCodeAnalyzer(semi);
                Parser parser = builder.build();

                try
                {
                    while (semi.getSemi())
                        parser.parse(semi);
                    Console.Write("\n\n  locations table contains:");
                }
                catch (Exception ex)
                {
                    Console.Write("\n\n  {0}\n", ex.Message);
                }
                Repository rep = Repository.getInstance();
                List<Elem> table = rep.locations;
                foreach (Elem e in table)
                {
                    Console.Write("\n  {0,10}, {1,25}, {2,5}, {3,5}", e.type, e.name, e.begin, e.end);
                }
                Console.WriteLine();
                Console.Write("\n\n  That's all folks!\n\n");
                semi.close();

            }
        }
        static void Main(string[] args)
        {
            string path = "../../";
            List<string> patterns = new List<string>();
            patterns.Add("*.cs");
            string[] files = Analyzer.getFiles(path,patterns);
            doAnalysis(files);
        }
    }
}
