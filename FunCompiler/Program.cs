
using FunCompiler.DataStructers;
using FunCompiler.DataStructers.Grammar;
using FunCompiler.DataStructers.Grammar.ContextFreeGrammar;
using FunCompiler.FiniteAutoamataUI;
using FunCompiler.Lexer;
using FunCompiler.Parser;

void RunUI()
{
    var fa = new FiniteAutomata();
    fa.FromFile("identifierFA.in");
    fa.Init();
    Console.WriteLine(fa.Accepts("number1"));

    var ui = new FiniteAutomataUI(fa);
    ui.Run();
}

void TestProductionParser()
{
    var pp = new ProductionParser();
    pp.ParseProductionString(@"abc -> abc | bcs");
}

void TestGrammarFromFile()
{
    var grammar = GeneralGrammar.FromFile("Parser\\Grammar\\fun-grammar.in");
    var cfg = Grammar.FromGeneralGrammar(grammar);
    Console.WriteLine(cfg);

    //var rdp = new RecursiveDescentParserAlgorithm(cfg);
    //rdp.Parse("aacbc");
    //var nonTerminal = "program";
    //Console.WriteLine($"prod for: {nonTerminal}");
    //cfg.GetProductionsFor(nonTerminal)?.ForEach(prod => Console.WriteLine(prod));
}

void TestParser()
{
    var parser = new Parser();
    parser.Parse("Parser\\Grammar\\grammar1.in", "Parser\\Grammar\\seq.in", "parser1.out");
}

void TestParserFun()
{
    var parser = new Parser();
    parser.ParseFromPif("Parser\\Grammar\\fun-grammar.in", "Parser\\Grammar\\pif1.in", "parser1.out");
}

TestGrammarFromFile();
TestParser();
TestParserFun();
/*TestProductionParser();
RunUI();

*/
Scanner scanner = new Scanner();
scanner.Init();

scanner.Scan(System.IO.File.ReadAllText("p1.fun"));
scanner.Log();