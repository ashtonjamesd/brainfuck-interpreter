/// <summary>
/// A simple interpreter for the Brainfuck language
/// </summary>
class BrainFuckInterpreter
{
    private const int TAPE_SIZE = 32767;

    static void Main()
    {
        string filePath = "C:/Users/adunderdale/bf.txt";
        string? code = ReadCode(filePath);

        if (code != null) BrainFuck(code);
        else Console.WriteLine("Failed to read Brainfuck code from the file.");
    }

    static string? ReadCode(string filePath)
    {
        try { return File.ReadAllText(filePath); }
        catch (Exception ex)
        {
            Console.WriteLine($"Error reading the file: {ex.Message}");
            return null;
        }
    }

    static void BrainFuck(string code)
    {
        int instructionPointer = 0; int dataPointer = 0;
        byte[] tape = new byte[TAPE_SIZE]; Stack<int> loopStack = new();

        while (instructionPointer < code.Length)
        {
            char command = code[instructionPointer];

            switch (command)
            {
                case '>': dataPointer++; instructionPointer++; break;
                case '<': dataPointer--; instructionPointer++; break;

                case '+': tape[dataPointer]++; instructionPointer++; break;
                case '-': tape[dataPointer]--; instructionPointer++; break;

                case '.': Console.Write(Convert.ToChar(tape[dataPointer]));  instructionPointer++; break;

                case ',':
                    int input = Console.Read();

                    if (input == 13)
                    {
                        instructionPointer++;
                        continue;
                    }

                    tape[dataPointer] = (byte)input;
                    break;

                case '[':
                    if (tape[dataPointer] == 0)
                    {
                        int depth = 1;
                        while (depth > 0)
                        {
                            instructionPointer++;
                            if (instructionPointer >= code.Length) break;

                            if (code[instructionPointer] == '[') depth++;
                            else if (code[instructionPointer] == ']') depth--;
                        }
                    }
                    else loopStack.Push(instructionPointer); instructionPointer++; break;

                case ']':
                    if (tape[dataPointer] != 0) instructionPointer = loopStack.Peek() - 1;
                    else loopStack.Pop(); instructionPointer++; break;

                default: break;
            }
        }
    }
}