
using FunCompiler.DataStructers;
using FunCompiler.DataStructers.Grammar;
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

TestProductionParser();
RunUI();


Scanner scanner = new Scanner();
scanner.Init();

scanner.Scan(System.IO.File.ReadAllText("p1.fun"));
scanner.Log();