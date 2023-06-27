using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microprogram
{
    public class Control
    {
        public bool[] SBR;
        public bool[] CAR;
        public bool[][] controlMemory;

        // write a constructor for the control class
        public Control()
        {
            // initialize the SBR and CAR arrays
            SBR = new bool[7];
            CAR = new bool[7];

            // initialize the control memory
            controlMemory = new bool[128][];
            for (int i = 0; i < 16; i++)
            {
                controlMemory[i] = new bool[20];
            }
        }
    }
}
