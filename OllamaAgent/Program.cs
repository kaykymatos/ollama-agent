
using OllamaSharp;

const string model = "gemma3:1b";
const string ollamaUrl = "http://localhost:11434";

const string systemPrompt = """
    Você é um Desenvolvedor de Software Sênior especializado em C# e .NET.
    Quando perguntarem quem é você responda que você é um Desenvolvedor de Software Sênior especializado em C# e .NET.
    Suas responsabilidades:
    - Criar código limpo, seguro, legível e de fácil manutenção.
    - Aplicar SOLID, Clean Code e Clean Architecture quando apropriado.
    - Priorizar soluções simples e evitar complexidade desnecessária.
    - Explicar decisões técnicas e fornecer exemplos funcionais.
    - Identificar possíveis bugs, problemas de desempenho e riscos.
    - Não afirmar que executou ou testou código sem realmente fazê-lo.
    - Perguntar quando faltarem informações importantes.

    Responda em português brasileiro.
    Utilize nomes em inglês para classes, métodos e variáveis nos exemplos.
    """;

var ollama = new OllamaApiClient(
    new Uri(ollamaUrl),
    model);

Console.WriteLine("Conectando ao Ollama...");

try
{
    var models = await ollama.ListLocalModelsAsync();

    if (!models.Any(m => m.Name.StartsWith(model,StringComparison.OrdinalIgnoreCase)))
    {
        Console.WriteLine($"O modelo '{model}' não está instalado.");

        Console.WriteLine($"Execute: ollama pull {model}");

        return;
    }

    var chat = new Chat(ollama, systemPrompt);

    Console.WriteLine();
    Console.WriteLine("======================================");
    Console.WriteLine("       LOCAL DEVELOPER AGENT");
    Console.WriteLine("======================================");
    Console.WriteLine($"Modelo: {model}");
    Console.WriteLine("Digite '/exit' para encerrar.");
    Console.WriteLine("Digite '/new' para reiniciar a conversa.");
    Console.WriteLine();

    while (true)
    {
        Console.Write("Você: ");

        var input = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(input))
            continue;

        if (input.Equals("/exit",StringComparison.OrdinalIgnoreCase))
            break;

        if (input.Equals("/new",StringComparison.OrdinalIgnoreCase))
        {
            chat = new Chat(ollama, systemPrompt);

            Console.WriteLine("Histórico reiniciado.\n");
            continue;
        }

        Console.WriteLine();
        Console.Write("Desenvolvedor: ");

        try
        {
            Console.ForegroundColor = ConsoleColor.Green;
            await foreach (var token in chat.SendAsync(input))
                Console.Write(token);

            Console.ResetColor();
            Console.WriteLine();
            Console.WriteLine();
        }
        catch (HttpRequestException ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\nErro de comunicação com o Ollama: {ex.Message}\n");
            Console.ResetColor();
        }
        catch (OperationCanceledException)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\nA operação foi cancelada ou excedeu o tempo limite.\n");
            Console.ResetColor();
        }
    }
}
catch (HttpRequestException ex)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine("Não foi possível conectar ao Ollama.");
    Console.WriteLine("Verifique se o serviço está em execução em " +$"{ollamaUrl}.");
    Console.WriteLine(ex.Message);
    Console.ResetColor();
}
catch (Exception ex)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine($"Erro inesperado: {ex.Message}");
    Console.ResetColor();
}

Console.WriteLine("Agente encerrado.");
