/// <summary>
/// A simple interpreter for the Brainfuck language
/// </summary>
class BrainFuckInterpreter
{
    private const int TAPE_SIZE = 32767;
    private const int CR_CHAR = 13;
    private const char INC_PTR = '>';
    private const char DEC_PTR = '<';
    private const char INC_VAL = '+';
    private const char DEC_VAL = '-';
    private const char OUT_CHAR = '.';
    private const char IN_CHAR = ',';
    private const char LOOP_START = '[';
    private const char LOOP_END = ']';

    static void Main()
    {
        string filePath = "C:/Users/adunderdale/bf.txt";
        
        if (ReadCode(filePath) is { } code) BrainFuck(code);
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
                case INC_PTR: dataPointer++; instructionPointer++; break;
                case DEC_PTR: dataPointer--; instructionPointer++; break;

                case INC_VAL: tape[dataPointer]++; instructionPointer++; break;
                case DEC_VAL: tape[dataPointer]--; instructionPointer++; break;

                case OUT_CHAR: Console.Write(Convert.ToChar(tape[dataPointer]));  instructionPointer++; break;

                case IN_CHAR:
                    int input = Console.Read();

                    if (input == CR_CHAR)
                    {
                        instructionPointer++;
                        continue;
                    }

                    tape[dataPointer] = (byte)input;
                    break;

                case LOOP_START:
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

                case LOOP_END:
                    if (tape[dataPointer] != 0) instructionPointer = loopStack.Peek() - 1;
                    else loopStack.Pop(); instructionPointer++; break;

                default: break;
            }
        }
    }
}