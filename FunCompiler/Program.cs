
using FunCompiler.Lexer;

Scanner scanner = new Scanner();
scanner.Init();

scanner.Scan(System.IO.File.ReadAllText(""));
scanner.Log();