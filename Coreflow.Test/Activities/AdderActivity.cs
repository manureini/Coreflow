using Coreflow.Interfaces;
using System;

namespace Coreflow.Test
{
    public class AdderActivity : ICodeActivity
    {
        public void Execute(
            int pFirstNumber,
            int pSecondNumber,
            out int rResult
           )
        {
            rResult = pFirstNumber + pSecondNumber;
            Console.WriteLine($"AdderActivity: {pFirstNumber} + {pSecondNumber} = {rResult}");
        }
    }
}
