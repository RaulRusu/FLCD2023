
using FunCompiler.DataStructers;
using FunCompiler.DataStructers.Grammar;
using FunCompiler.DataStructers.Grammar.ContextFreeGrammar;
using FunCompiler.FiniteAutoamataUI;
using FunCompiler.Lexer;

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

    var nonTerminal = "program";
    Console.WriteLine($"prod for: {nonTerminal}");
    cfg.GetProductionsFor(nonTerminal)?.ForEach(prod => Console.WriteLine(prod));
}

TestGrammarFromFile();

/*TestProductionParser();
RunUI();


Scanner scanner = new Scanner();
scanner.Init();

scanner.Scan(System.IO.File.ReadAllText("p1.fun"));
scanner.Log();*/