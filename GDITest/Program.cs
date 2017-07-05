using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDITest
{
    class Program
    {
        private static GameForm dave;

        static void Main(string[] args)
        {
            dave = new GameForm();
            dave.ShowDialog();

        }
    }
}
