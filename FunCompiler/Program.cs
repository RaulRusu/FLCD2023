
using FunCompiler.DataStructers;
using FunCompiler.FiniteAutoamataUI;
using FunCompiler.Lexer;

void RunUI()
{
    var fa = new FiniteAutomata();
    fa.FromFile("intConstFA.in");
    fa.Init();
    Console.WriteLine(fa.Accepts("-10131231231241123131141422323421432341242"));

    var ui = new FiniteAutomataUI(fa);
    ui.Run();

}

RunUI();


/*Scanner scanner = new Scanner();
scanner.Init();

scanner.Scan(System.IO.File.ReadAllText("p1.fun"));
scanner.Log();*/