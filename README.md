# Local Developer Agent — C# + Ollama

Um agente de IA local desenvolvido em C# utilizando o **Ollama** e o modelo **Gemma 3 1B**. O projeto permite interagir com um modelo de linguagem executado localmente, sem precisar utilizar uma API de IA na nuvem.

O agente foi configurado para atuar como um desenvolvedor de software sênior especializado em C# e .NET, com foco em boas práticas, código limpo, SOLID e Clean Architecture.

## Tecnologias utilizadas

- **C#** — linguagem de programação.
- **.NET** — plataforma de desenvolvimento.
- **Ollama** — execução local de modelos de linguagem (LLMs).
- **Gemma 3 1B** — modelo de linguagem utilizado pelo agente.
- **OllamaSharp** — biblioteca .NET para integração com o Ollama.

## Pré-requisitos

Antes de executar o projeto, verifique se você possui:

- Windows, Linux ou macOS.
- [SDK do .NET](https://dotnet.microsoft.com/download) compatível com a versão do projeto.
- [Ollama](https://ollama.com/download) instalado.
- Memória RAM suficiente para executar o modelo escolhido.

O Gemma 3 1B é uma opção compacta para experimentar modelos de IA localmente. O consumo de memória varia conforme a quantização, o contexto e a configuração da execução.

## 1. Instalar o Ollama

Acesse o site oficial:

https://ollama.com/download

Baixe a versão correspondente ao seu sistema operacional e siga as instruções de instalação.

Após instalar, abra um terminal e execute:

```bash
ollama --version
```

Se o comando retornar a versão instalada, o Ollama está disponível no terminal.

## 2. Baixar o modelo Gemma 3 1B

Com o Ollama instalado, execute:

```bash
ollama pull gemma3:1b
```

Esse comando baixa o modelo para sua máquina. O download precisa ser realizado apenas uma vez, desde que você mantenha o modelo instalado.

Para verificar os modelos disponíveis localmente, execute:

```bash
ollama list
```

Você deverá encontrar o modelo `gemma3:1b` na lista.

### Executar o modelo manualmente

Para iniciar uma conversa diretamente pelo terminal, execute:

```bash
ollama run gemma3:1b
```

Para sair da conversa, utilize `/bye`.

### Iniciar e parar o Ollama

Normalmente, o Ollama inicia seu serviço automaticamente após a instalação. Se necessário, inicie o aplicativo ou serviço do Ollama.

No Windows, você também pode abrir o aplicativo Ollama pelo menu Iniciar.

Para interromper uma execução interativa, use `/bye`. Para parar o servidor, encerre o processo ou serviço do Ollama conforme a forma como ele foi iniciado. Se estiver executando o servidor diretamente no terminal com `ollama serve`, use `Ctrl + C`.

> **Importante:** `ollama stop gemma3:1b` descarrega o modelo da memória, mas não encerra o servidor Ollama. O comando `ollama stop` sem o nome do modelo não é a forma correta de encerrar o serviço.

## 3. Configurar o projeto C#

Clone o repositório e entre na pasta do projeto:

```bash
git clone https://github.com/kaykymatos/ollama-agent.git
cd ollama-agent
```

Instale a biblioteca OllamaSharp:

```bash
dotnet add package OllamaSharp
```

Essa biblioteca permite que a aplicação C# se comunique com o servidor local do Ollama.

## 4. Configurar o agente

No código, o modelo e o endereço do servidor são definidos pelas constantes:

```csharp
const string model = "gemma3:1b";
const string ollamaUrl = "http://localhost:11434";
```

- `model`: identifica o modelo que será utilizado.
- `ollamaUrl`: endereço padrão da API local do Ollama.

O agente utiliza um *system prompt* para definir seu comportamento:

```csharp
const string systemPrompt = """
    Você é um Desenvolvedor de Software Sênior especializado em C# e .NET.

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
```

Esse prompt orienta as respostas do modelo, mas não garante que todas as instruções serão seguidas perfeitamente.

## 5. Executar o projeto

Antes de executar a aplicação, certifique-se de que:

1. O Ollama está instalado.
2. O modelo `gemma3:1b` foi baixado.
3. O servidor Ollama está em execução.
4. O projeto está configurado com a dependência `OllamaSharp`.

Na pasta que contém o arquivo `.csproj`, execute:

```bash
dotnet restore
dotnet run
```

A aplicação verificará a disponibilidade do modelo e iniciará a interface interativa no terminal.

A saída será semelhante a:

```text
Conectando ao Ollama...

======================================
       LOCAL DEVELOPER AGENT
======================================
Modelo: gemma3:1b
Digite '/exit' para encerrar.
Digite '/new' para reiniciar a conversa.

Você:
```

Digite uma pergunta ou solicitação para começar a conversar com o agente.

## 6. Comandos disponíveis

| Comando | Descrição |
|---|---|
| `/exit` | Encerra a aplicação. |
| `/new` | Reinicia o histórico da conversa. |

Exemplos de perguntas:

```text
Você: Explique o princípio da responsabilidade única do SOLID.
```

```text
Você: Crie uma API REST em C# utilizando Minimal APIs.
```

```text
Você: Como posso aplicar Clean Architecture em um projeto .NET?
```

O agente transmite a resposta gradualmente para o terminal, exibindo os tokens à medida que são recebidos.

## 7. Como funciona a aplicação

O fluxo de execução é simples:

1. A aplicação cria um cliente HTTP para a API local do Ollama.
2. Verifica se o modelo configurado está instalado.
3. Inicializa o chat com o *system prompt*.
4. Recebe as mensagens digitadas pelo usuário.
5. Envia as mensagens ao modelo e transmite a resposta gradualmente.
6. Mantém o histórico durante a conversa atual.
7. Permite reiniciar o histórico com `/new` ou encerrar a aplicação com `/exit`.

O histórico é mantido em memória durante a execução. Ao reiniciar a aplicação, uma nova conversa é iniciada.

## 8. Solução de problemas

### O modelo não está instalado

Se a aplicação informar que o modelo não está instalado, execute:

```bash
ollama pull gemma3:1b
```

Depois, execute novamente o projeto.

### Não foi possível conectar ao Ollama

Verifique se o serviço está disponível em:

```text
http://localhost:11434
```

Você também pode testar a API pelo terminal:

```bash
curl http://localhost:11434/api/tags
```

Se estiver utilizando o PowerShell, o comando `curl` pode ser um alias. Nesse caso, utilize:

```powershell
Invoke-RestMethod http://localhost:11434/api/tags
```

Se a conexão falhar, abra o aplicativo Ollama ou inicie o serviço conforme a instalação realizada.

### Respostas lentas

A velocidade depende do hardware, da memória disponível, da quantização do modelo e do tamanho do contexto utilizado.

Algumas recomendações:

- Feche aplicações que consomem muita memória.
- Utilize um modelo compatível com os recursos do computador.
- Considere um modelo menor caso o desempenho seja insuficiente.

### O agente apresenta respostas incorretas

Modelos de linguagem podem gerar informações imprecisas ou código com erros. Revise as respostas, compile os exemplos e execute testes antes de utilizar o código em ambientes reais.

## 9. Privacidade e execução local

Como o modelo é executado pelo Ollama na própria máquina, as solicitações são processadas localmente pelo serviço, sem exigir uma API de IA hospedada na nuvem.

Isso não significa que qualquer aplicação construída sobre o agente seja automaticamente privada: integrações externas, ferramentas adicionais ou modificações no código podem transmitir informações para outros serviços.

## Referências

- [Ollama — Site oficial](https://ollama.com/)
- [Ollama — Documentação da API](https://docs.ollama.com/api)
- [Gemma 3 1B — Biblioteca de modelos do Ollama](https://ollama.com/library/gemma3:1b)
- [OllamaSharp — GitHub](https://github.com/awaescher/OllamaSharp)
- [.NET — Download](https://dotnet.microsoft.com/download)
