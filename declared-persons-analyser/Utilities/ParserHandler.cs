using CommandLine;
using declared_persons_analyser.Models;
using System;
using System.Collections.Generic;
using System.Text;


namespace declared_persons_analyser.Utilities
{
    public class ParserHandler
    {
        private Options args;

        public ParserHandler()
        {
            this.args = new Options();
        }

        public Options ParseArgs(string[] args)
        {
            CommandLine.Parser.Default.ParseArguments<Options>(PrepareArgsForParsing(args))
                .WithParsed(x => this.args = x)
                .WithNotParsed(HandleParseError);

            return this.args;
        }

        //CommandLine Parser bibliotēka funkcionē pareizi, kad lietotāja parametriem ir priekša divas --
        private string[] PrepareArgsForParsing(string[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i].Contains("-")) args[i] = "-" + args[i];
            }

            return args;
        }

        private void HandleParseError(IEnumerable<Error> errs)
        {
            Console.WriteLine("Kļūda parametru lasīšanā, pārbaudiet to pareizību un mēģiniet vēlreiz \n");

            
        }
    }
}
