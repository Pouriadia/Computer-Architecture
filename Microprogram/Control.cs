using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace Microprogram
{
    public class Control
    {
        public bool[] SBR = new bool[7];
        public bool[] CAR = new bool[7];
        public bool[][] controlMemory = new bool[128][];
        public string instruction = "";

        // write a constructor for the Control class
        public Control()
        {
            // initialize the control memory
            for (int i = 0; i < 128; i++)
            {
                controlMemory[i] = new bool[20];
            }
        }

        
    }
}
