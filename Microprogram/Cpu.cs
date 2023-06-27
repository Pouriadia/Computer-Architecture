using System;
using System.Collections.Generic;
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
    }
}
