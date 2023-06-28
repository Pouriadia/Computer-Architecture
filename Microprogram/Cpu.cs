using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microprogram
{
    public class Cpu
    {
        public Control control;
        public bool[] PC = new bool[11];
        public bool[] currentPC = new bool[11];
        public bool[] AR = new bool[11];
        public bool[] currentAR = new bool[11];
        public bool[] DR = new bool[16];
        public bool[] currentDR = new bool[16];
        public bool[] AC = new bool[16];
        public bool[] currentAC = new bool[16];
        public bool[][] memory = new bool[2048][];
        public bool[][] currentMemory = new bool[2048][];
        public string[][] lookupTable = new string[100][];
        public int pointer = 0;
        public bool finishFlag = false;

        // write a constructor for the CPU class
        public Cpu()
        {
            // initialize the control class
            control = new Control();

            // initialize the memory
            for (int i = 0; i < 2048; i++)
            {
                memory[i] = new bool[16];
                currentMemory[i] = new bool[16];
            }
            for (int i = 0; i < 100; i++)
            {
                lookupTable[i] = new string[2];
            }
        }

        public void updateCpu()
        {
            for (int i = 0; i < 11; i++)
            {
                PC[i] = currentPC[i];
                AR[i] = currentAR[i];
            }
            for (int i = 0; i < 16; i++)
            {
                DR[i] = currentDR[i];
                AC[i] = currentAC[i];
            }
            for (int i = 0; i < 2048; i++)
            {
                for (int j = 0; j < 16; j++)
                {
                    memory[i][j] = currentMemory[i][j];
                }
            }
        }
        public bool U()
        {
            return true;
        }

        public bool I()
        {
            return currentDR[0];
        }

        public bool S()
        {
            return currentAC[0];
        }

        public bool Z()
        {
            bool zero = true;
            for (int i = 0; i < 16; i++)
            {
                if (currentAC[i])
                {
                    zero = false;
                }
            }
            return zero;
        }
        public void CLRAC()
        {
            for (int i = 0; i < 16; i++)
            {
                currentAC[i] = false;
            }
        }

        public void INCAC()
        {
            bool carry = true;
            for (int i = 15; i >= 0; i--)
            {
                if (carry)
                {
                    if (AC[i])
                    {
                        currentAC[i] = false;
                    }
                    else
                    {
                        currentAC[i] = true;
                        carry = false;
                    }
                }
            }
        }

        public void DRTAC()
        {
            for (int i = 0; i < 16; i++)
            {
                currentAC[i] = DR[i];
            }
        }

        public void DRTAR()
        {
            for (int i = 15; i > 4; i--)
            {
                currentAR[i - 5] = DR[i];
            }
        }

        public void PCTAR()
        {
            for (int i = 0; i < 11; i++)
            {
                currentAR[i] = PC[i];
            }
        }

        public void WRITE()
        {
            int address = 0;
            for (int i = 0; i < 11; i++)
            {
                if (AR[i])
                {
                    address += (int)Math.Pow(2, 10 - i);
                }
            }
            for (int i = 0; i < 16; i++)
            {
                currentMemory[address][i] = DR[i];
            }
        }

        public void ADD()
        {
            bool carry = false;
            for (int i = 15; i >= 0; i--)
            {
                if (AC[i] && DR[i])
                {
                    if (carry)
                    {
                        currentAC[i] = true;
                    }
                    else
                    {
                        currentAC[i] = false;
                        carry = true;
                    }
                }
                else if (AC[i] || DR[i])
                {
                    if (carry)
                    {
                        currentAC[i] = false;
                    }
                    else
                    {
                        currentAC[i] = true;
                    }
                }
                else
                {
                    if (carry)
                    {
                        currentAC[i] = true;
                        carry = false;
                    }
                    else
                    {
                        currentAC[i] = false;
                    }
                }
            }
        }

        public void AND()
        {
            for (int i = 0; i < 16; i++)
            {
                if (AC[i] && DR[i])
                {
                    currentAC[i] = true;
                }
                else
                {
                    currentAC[i] = false;
                }
            }
        }

        public void OR()
        {
            for (int i = 0; i < 16; i++)
            {
                if (AC[i] || DR[i])
                {
                    currentAC[i] = true;
                }
                else
                {
                    currentAC[i] = false;
                }
            }
        }

        public void READ()
        {
            int address = 0;
            for (int i = 0; i < 11; i++)
            {
                if (AR[i])
                {
                    address += (int)Math.Pow(2, 10 - i);
                }
            }
            for (int i = 0; i < 16; i++)
            {
                currentDR[i] = memory[address][i];
            }
        }

        public void ACTDR()
        {
            for (int i = 0; i < 16; i++)
            {
                currentDR[i] = AC[i];
            }
        }

        public void INCDR()
        {
            bool carry = true;
            for (int i = 15; i >= 0; i--)
            {
                if (carry)
                {
                    if (DR[i])
                    {
                        currentDR[i] = false;
                    }
                    else
                    {
                        currentDR[i] = true;
                        carry = false;
                    }
                }
            }
        }

        public void PCTDR()
        {
            for (int i = 0; i < 11; i++)
            {
                currentDR[i + 5] = PC[i];
            }
        }

        public void XOR()
        {
            for (int i = 0; i < 16; i++)
            {
                if (AC[i] ^ DR[i])
                {
                    currentAC[i] = true;
                }
                else
                {
                    currentAC[i] = false;
                }
            }
        }

        public void COM()
        {
            for (int i = 0; i < 16; i++)
            {
                if (AC[i])
                {
                    currentAC[i] = false;
                }
                else
                {
                    currentAC[i] = true;
                }
            }
        }

        public void SHL()
        {
            bool temp = AC[0];
            for (int i = 0; i < 15; i++)
            {
                currentAC[i] = AC[i + 1];
            }
            currentAC[15] = temp;
        }

        public void SHR()
        {
            bool temp = AC[15];
            for (int i = 15; i > 0; i--)
            {
                currentAC[i] = AC[i - 1];
            }
            currentAC[0] = temp;
        }

        public void INCPC()
        {
            bool carry = true;
            for (int i = 10; i >= 0; i--)
            {
                if (carry)
                {
                    if (PC[i])
                    {
                        currentPC[i] = false;
                    }
                    else
                    {
                        currentPC[i] = true;
                        carry = false;
                    }
                }
            }
        }

        public void ARTPC()
        {
            for (int i = 0; i < 11; i++)
            {
                currentPC[i] = AR[i];
            }
        }

        public void SUB()
        {

            bool[] temp = new bool[16];
            for (int i = 0; i < 16; i++)
            {
                temp[i] = DR[i];
            }

            // 2nd complement
            for (int i = 0; i < 16; i++)
            {
                DR[i] = !DR[i];
            }
            INCDR();
            ADD();

            for (int i = 0; i < 16; i++)
            {
                DR[i] = temp[i];
            }
        }

        public void executeF1(int code)
        {
            switch (code)
            {
                case 1:
                    control.instruction = "ADD";
                    ADD();
                    break;
                case 2:
                    control.instruction = "CLRAC";
                    CLRAC();
                    break;
                case 3:
                    control.instruction = "INCAC";
                    INCAC();
                    break;
                case 4:
                    control.instruction = "DRTAC";
                    DRTAC();
                    break;
                case 5:
                    control.instruction = "DRTAR";
                    DRTAR();
                    break;
                case 6:
                    control.instruction = "PCTAR";
                    PCTAR();
                    break;
                case 7:
                    control.instruction = "WRITE";
                    WRITE();
                    break;
                default:
                    control.instruction = "NOP";
                    break;
            }

        }

        public void executeF2(int code)
        {
            switch (code)
            {
                case 1:
                    control.instruction = "SUB";
                    SUB();
                    break;
                case 2:
                    control.instruction = "OR";
                    OR();
                    break;
                case 3:
                    control.instruction = "AND";
                    AND();
                    break;
                case 4:
                    control.instruction = "READ";
                    READ();
                    break;
                case 5:
                    control.instruction = "ACTDR";
                    ACTDR();
                    break;
                case 6:
                    control.instruction = "INCDR";
                    INCDR();
                    break;
                case 7:
                    control.instruction = "PCTDR";
                    PCTDR();
                    break;
                default:
                    control.instruction = "NOP";
                    break;
            }

        }

        public void executeF3(int code)
        {
            switch (code)
            {
                case 1:
                    control.instruction = "XOR";
                    XOR();
                    break;
                case 2:
                    control.instruction = "COM";
                    COM();
                    break;
                case 3:
                    control.instruction = "SHL";
                    SHL();
                    break;
                case 4:
                    control.instruction = "SHR";
                    SHR();
                    break;
                case 5:
                    control.instruction = "INCPC";
                    INCPC();
                    break;
                case 6:
                    control.instruction = "ARTPC";
                    ARTPC();
                    break;
                default:
                    control.instruction = "NOP";
                    break;
            }

        }

        public static int convertBoolArrayToInt(bool[] array)
        {
            int result = 0;
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i])
                {
                    result += (int)Math.Pow(2, i);
                }
            }
            return result;
        }

        public static bool[] convertIntToBoolArray(int num, int size)
        {
            bool[] result = new bool[size];
            for (int i = 0; i < size; i++)
            {
                if (num % 2 == 1)
                {
                    result[i] = true;
                }
                else
                {
                    result[i] = false;
                }
                num /= 2;
            }
            return result;
        }   

        public void execute(int code1, int code2, int code3, int condition, int branch, int address)
        {
            executeF1(code1);
            executeF2(code2);
            executeF3(code3);
            bool cond = false;
            switch (condition)
            {
                case 1:
                    cond = I();
                    break;
                case 2:
                    cond = S();
                    break;
                case 3:
                    cond = Z();
                    break;
                default:
                    cond = U();
                    break;
            }
            if (cond)
            {
                switch (branch)
                {
                    case 1:
                        // JMP
                        control.CAR = convertIntToBoolArray(address, 7);
                        break;
                    case 2:
                        // CALL
                        control.SBR = convertIntToBoolArray(convertBoolArrayToInt(control.CAR) + 1, 7);
                        control.CAR = convertIntToBoolArray(address, 7);
                        break;
                    case 3:
                        // RET
                        control.CAR = control.SBR;
                        break;
                    case 4:
                        // MAP
                        for (int i = 1; i <= 4; i++)
                        {
                            control.CAR[i] = DR[i];
                        }
                        control.CAR[0] = false; control.CAR[5] = false; control.CAR[6] = false;
                        break;
                    default:
                        control.CAR = convertIntToBoolArray(convertBoolArrayToInt(control.CAR) + 1, 7);
                        break;
                }
            }
            else
            {
                control.CAR = convertIntToBoolArray(convertBoolArrayToInt(control.CAR) + 1, 7);
            }
        }

        public void firstRead(string line)
        {
            if (line.Contains(':'))
            {
                for (int i = 0; i < 100; i++)
                {
                    if (String.IsNullOrEmpty(control.labelTabel[i][0]))
                    {
                        control.labelTabel[i][0] = Convert.ToString(control.memoryPointer);
                        control.labelTabel[i][1] = line.Split(':')[0];
                        break;
                    }
                }
            }
        }
        public void secondRead(string line)
        {
            control.label = "";
            if (line.Contains("END"))
            {
                control.finished = true;
                return;
            }
            else if (line.Contains("ORG"))
            {
                var temp = line.Split(' ');
                control.memoryPointer = int.Parse(temp[1]);
            }
            else
            {
                string[] instruction = line.Split(' ');
                if (line.Contains(':'))
                {
                    var temperary = line.Split(':');
                    control.label = temperary[0];
                    instruction = temperary[1].Split(' ');
                }

                if (instruction[0].Contains(','))
                {
                    string[] microOperations = instruction[0].Split(',');
                    foreach (var item in microOperations)
                    {
                        switch (item)
                        {
                            case "ADD":
                                control.controlMemory[control.memoryPointer][0] = false;
                                control.controlMemory[control.memoryPointer][1] = false;
                                control.controlMemory[control.memoryPointer][2] = true;
                                break;
                            case "CLRAC":
                                control.controlMemory[control.memoryPointer][0] = false;
                                control.controlMemory[control.memoryPointer][1] = true;
                                control.controlMemory[control.memoryPointer][2] = false;
                                break;
                            case "INCAC":
                                control.controlMemory[control.memoryPointer][0] = false;
                                control.controlMemory[control.memoryPointer][1] = true;
                                control.controlMemory[control.memoryPointer][2] = true;
                                break;
                            case "DRTAC":
                                control.controlMemory[control.memoryPointer][0] = true;
                                control.controlMemory[control.memoryPointer][1] = false;
                                control.controlMemory[control.memoryPointer][2] = false;
                                break;
                            case "DRTAR":
                                control.controlMemory[control.memoryPointer][0] = true;
                                control.controlMemory[control.memoryPointer][1] = false;
                                control.controlMemory[control.memoryPointer][2] = true;
                                break;
                            case "PCTAR":
                                control.controlMemory[control.memoryPointer][0] = true;
                                control.controlMemory[control.memoryPointer][1] = true;
                                control.controlMemory[control.memoryPointer][2] = false;
                                break;
                            case "WRITE":
                                control.controlMemory[control.memoryPointer][0] = true;
                                control.controlMemory[control.memoryPointer][1] = true;
                                control.controlMemory[control.memoryPointer][2] = true;
                                break;
                            case "SUB":
                                control.controlMemory[control.memoryPointer][3] = false;
                                control.controlMemory[control.memoryPointer][4] = false;
                                control.controlMemory[control.memoryPointer][5] = true;
                                break;
                            case "OR":
                                control.controlMemory[control.memoryPointer][3] = false;
                                control.controlMemory[control.memoryPointer][4] = true;
                                control.controlMemory[control.memoryPointer][5] = false;
                                break;
                            case "AND":
                                control.controlMemory[control.memoryPointer][3] = false;
                                control.controlMemory[control.memoryPointer][4] = true;
                                control.controlMemory[control.memoryPointer][5] = true;
                                break;
                            case "READ":
                                control.controlMemory[control.memoryPointer][3] = true;
                                control.controlMemory[control.memoryPointer][4] = false;
                                control.controlMemory[control.memoryPointer][5] = false;
                                break;
                            case "ACTDR":
                                control.controlMemory[control.memoryPointer][3] = true;
                                control.controlMemory[control.memoryPointer][4] = false;
                                control.controlMemory[control.memoryPointer][5] = true;
                                break;
                            case "INCDR":
                                control.controlMemory[control.memoryPointer][3] = true;
                                control.controlMemory[control.memoryPointer][4] = true;
                                control.controlMemory[control.memoryPointer][5] = false;
                                break;
                            case "PCTDR":
                                control.controlMemory[control.memoryPointer][3] = true;
                                control.controlMemory[control.memoryPointer][4] = true;
                                control.controlMemory[control.memoryPointer][5] = true;
                                break;
                            case "XOR":
                                control.controlMemory[control.memoryPointer][6] = false;
                                control.controlMemory[control.memoryPointer][7] = false;
                                control.controlMemory[control.memoryPointer][8] = true;
                                break;
                            case "COM":
                                control.controlMemory[control.memoryPointer][6] = false;
                                control.controlMemory[control.memoryPointer][7] = true;
                                control.controlMemory[control.memoryPointer][8] = false;
                                break;
                            case "SHL":
                                control.controlMemory[control.memoryPointer][6] = false;
                                control.controlMemory[control.memoryPointer][7] = true;
                                control.controlMemory[control.memoryPointer][8] = true;
                                break;
                            case "SHR":
                                control.controlMemory[control.memoryPointer][6] = true;
                                control.controlMemory[control.memoryPointer][7] = false;
                                control.controlMemory[control.memoryPointer][8] = false;
                                break;
                            case "INCPC":
                                control.controlMemory[control.memoryPointer][6] = true;
                                control.controlMemory[control.memoryPointer][7] = false;
                                control.controlMemory[control.memoryPointer][8] = true;
                                break;
                            case "ARTPC":
                                control.controlMemory[control.memoryPointer][6] = true;
                                control.controlMemory[control.memoryPointer][7] = true;
                                control.controlMemory[control.memoryPointer][8] = false;
                                break;
                        }
                    }
                }
                else
                {
                    switch (instruction[0])
                    {
                        case "ADD":
                            control.controlMemory[control.memoryPointer][0] = false;
                            control.controlMemory[control.memoryPointer][1] = false;
                            control.controlMemory[control.memoryPointer][2] = true;
                            break;
                        case "CLRAC":
                            control.controlMemory[control.memoryPointer][0] = false;
                            control.controlMemory[control.memoryPointer][1] = true;
                            control.controlMemory[control.memoryPointer][2] = false;
                            break;
                        case "INCAC":
                            control.controlMemory[control.memoryPointer][0] = false;
                            control.controlMemory[control.memoryPointer][1] = true;
                            control.controlMemory[control.memoryPointer][2] = true;
                            break;
                        case "DRTAC":
                            control.controlMemory[control.memoryPointer][0] = true;
                            control.controlMemory[control.memoryPointer][1] = false;
                            control.controlMemory[control.memoryPointer][2] = false;
                            break;
                        case "DRTAR":
                            control.controlMemory[control.memoryPointer][0] = true;
                            control.controlMemory[control.memoryPointer][1] = false;
                            control.controlMemory[control.memoryPointer][2] = true;
                            break;
                        case "PCTAR":
                            control.controlMemory[control.memoryPointer][0] = true;
                            control.controlMemory[control.memoryPointer][1] = true;
                            control.controlMemory[control.memoryPointer][2] = false;
                            break;
                        case "WRITE":
                            control.controlMemory[control.memoryPointer][0] = true;
                            control.controlMemory[control.memoryPointer][1] = true;
                            control.controlMemory[control.memoryPointer][2] = true;
                            break;
                        case "SUB":
                            control.controlMemory[control.memoryPointer][3] = false;
                            control.controlMemory[control.memoryPointer][4] = false;
                            control.controlMemory[control.memoryPointer][5] = true;
                            break;
                        case "OR":
                            control.controlMemory[control.memoryPointer][3] = false;
                            control.controlMemory[control.memoryPointer][4] = true;
                            control.controlMemory[control.memoryPointer][5] = false;
                            break;
                        case "AND":
                            control.controlMemory[control.memoryPointer][3] = false;
                            control.controlMemory[control.memoryPointer][4] = true;
                            control.controlMemory[control.memoryPointer][5] = true;
                            break;
                        case "READ":
                            control.controlMemory[control.memoryPointer][3] = true;
                            control.controlMemory[control.memoryPointer][4] = false;
                            control.controlMemory[control.memoryPointer][5] = false;
                            break;
                        case "ACTDR":
                            control.controlMemory[control.memoryPointer][3] = true;
                            control.controlMemory[control.memoryPointer][4] = false;
                            control.controlMemory[control.memoryPointer][5] = true;
                            break;
                        case "INCDR":
                            control.controlMemory[control.memoryPointer][3] = true;
                            control.controlMemory[control.memoryPointer][4] = true;
                            control.controlMemory[control.memoryPointer][5] = false;
                            break;
                        case "PCTDR":
                            control.controlMemory[control.memoryPointer][3] = true;
                            control.controlMemory[control.memoryPointer][4] = true;
                            control.controlMemory[control.memoryPointer][5] = true;
                            break;
                        case "XOR":
                            control.controlMemory[control.memoryPointer][6] = false;
                            control.controlMemory[control.memoryPointer][7] = false;
                            control.controlMemory[control.memoryPointer][8] = true;
                            break;
                        case "COM":
                            control.controlMemory[control.memoryPointer][6] = false;
                            control.controlMemory[control.memoryPointer][7] = true;
                            control.controlMemory[control.memoryPointer][8] = false;
                            break;
                        case "SHL":
                            control.controlMemory[control.memoryPointer][6] = false;
                            control.controlMemory[control.memoryPointer][7] = true;
                            control.controlMemory[control.memoryPointer][8] = true;
                            break;
                        case "SHR":
                            control.controlMemory[control.memoryPointer][6] = true;
                            control.controlMemory[control.memoryPointer][7] = false;
                            control.controlMemory[control.memoryPointer][8] = false;
                            break;
                        case "INCPC":
                            control.controlMemory[control.memoryPointer][6] = true;
                            control.controlMemory[control.memoryPointer][7] = false;
                            control.controlMemory[control.memoryPointer][8] = true;
                            break;
                        case "ARTPC":
                            control.controlMemory[control.memoryPointer][6] = true;
                            control.controlMemory[control.memoryPointer][7] = true;
                            control.controlMemory[control.memoryPointer][8] = false;
                            break;
                    }
                }
                switch (instruction[1])
                {
                    case "U":
                        control.controlMemory[control.memoryPointer][9] = false;
                        control.controlMemory[control.memoryPointer][10] = false;
                        break;
                    case "I":
                        control.controlMemory[control.memoryPointer][9] = false;
                        control.controlMemory[control.memoryPointer][10] = true;
                        break;
                    case "S":
                        control.controlMemory[control.memoryPointer][9] = true;
                        control.controlMemory[control.memoryPointer][10] = false;
                        break;
                    case "Z":
                        control.controlMemory[control.memoryPointer][9] = true;
                        control.controlMemory[control.memoryPointer][10] = true;
                        break;
                }

                int temp = 0;

                switch (instruction[2])
                {

                    case "JMP":
                        control.controlMemory[control.memoryPointer][11] = false;
                        control.controlMemory[control.memoryPointer][12] = false;
                        if (instruction[3] == "NEXT")
                        {
                            temp = control.memoryPointer + 1;
                        }
                        else
                        {
                            for (int i = 0; i < control.labelTabel.GetLength(0); i++)
                            {
                                if (instruction[3] == control.labelTabel[i][0])
                                {
                                    temp = Convert.ToInt32(control.labelTabel[i][1]);
                                    break;
                                }
                            }    
                        }
                        break;
                    case "CALL":
                        control.controlMemory[control.memoryPointer][11] = false;
                        control.controlMemory[control.memoryPointer][12] = true;
                        if (instruction[3] == "NEXT")
                        {
                            temp = control.memoryPointer + 1;
                        }
                        else
                        {
                            for (int i = 0; i < control.labelTabel.GetLength(0); i++)
                            {
                                if (instruction[3] == control.labelTabel[i][0])
                                {
                                    temp = Convert.ToInt32(control.labelTabel[i][1]);
                                    break;
                                }
                            }
                        }
                        break;
                    case "RET":
                        control.controlMemory[control.memoryPointer][11] = true;
                        control.controlMemory[control.memoryPointer][12] = false;
                        break;
                    case "MAP":
                        control.controlMemory[control.memoryPointer][11] = true;
                        control.controlMemory[control.memoryPointer][12] = true;
                        break;
                }

                bool[] ADDRESS = new bool[7];
                ADDRESS = convertIntToBoolArray(temp, 7);
                for (int i = 0; i < 7; i++)
                {
                    control.controlMemory[control.memoryPointer][i + 13] = ADDRESS[i];
                }
                
            }
        }
        
        public void loadLookupTable(string line)
        {
            for (int i = 0; i < 100; i++)
            {
                if (String.IsNullOrEmpty(lookupTable[i][0]))
                {
                    lookupTable[i][0] = Convert.ToString(pointer);
                    lookupTable[i][1] = line.Split(',')[0];
                    break;
                }
            }
        }

        public void Read(string line)
        {
            int value = 0;
            int opcode = 0;
            int address = 0;
            if (line == "HALT")
            {
                finishFlag = true;
                return;
            }
            if (line.Contains("INDRCT"))
            {
                memory[pointer][0] = true;
            }
            if (line.Contains(','))
            {
                if (line.Split(',')[1].Trim().Split(' ')[0] == "HEX")
                {
                    value = Convert.ToInt32(line.Split(',')[1].Trim().Split(' ')[1], 16);
                    bool[] binaryValue = new bool[16];
                    binaryValue = convertIntToBoolArray(value, 16);
                    for (int i = 0; i < 100; i++)
                    {
                        if (line.Split(',')[0] == lookupTable[i][1])
                        {
                            for (int j = 0; j < 16; j++)
                            {
                                memory[pointer][j] = binaryValue[j];
                            }
                            break;
                        }
                    }
                }
                else if (line.Split(',')[1].Trim().Split(' ')[0] == "DEC")
                {
                    value = Convert.ToInt32(line.Split(',')[1].Trim().Split(' ')[1]);
                    bool[] binaryValue = new bool[16];
                    binaryValue = convertIntToBoolArray(value, 16);
                    for (int i = 0; i < 100; i++)
                    {
                        if (line.Split(',')[0] == lookupTable[i][1])
                        {
                            for (int j = 0; j < 16; j++)
                            {
                                memory[pointer][j] = binaryValue[j];
                            }
                            break;
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < 100; i++)
                {
                    if (line.Split(' ')[0] == lookupTable[i][1])
                    {
                        opcode = Convert.ToInt32(control.labelTabel[i][0]);
                        opcode /= 4;
                        bool[] binaryValue = new bool[4];
                        binaryValue = convertIntToBoolArray(address, 4);
                        for (int j = 0; j < 4; j++)
                        {
                            memory[pointer][j + 1] = binaryValue[j];
                        }
                        break;
                    }
                }
                for (int i = 0; i < 100; i++)
                {
                    if (line.Split(' ')[1] == lookupTable[i][1])
                    {
                        address = Convert.ToInt32(lookupTable[i][0]);
                        bool[] binaryValue = new bool[11];
                        binaryValue = convertIntToBoolArray(address, 11);
                        for (int j = 0; j < 11; j++)
                        {
                            memory[pointer][j + 5] = binaryValue[j];
                        }
                        break;
                    }
                }
            }
        }
    }
}
