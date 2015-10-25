using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Aramani.IR;
using Aramani.IR.Commands;
using Aramani.IR.Variables;

namespace Aramani.IR.Tests
{
    public class Test1
    {
         
        public static void Main()
        {
            StackVariable s1 = new StackVariable();
            s1.ID = 0;
            StackVariable s2 = new StackVariable();
            s2.ID = 1;
            StackVariable s3 = new StackVariable();
            s3.ID = 2;

            //ArrayElementLocation ai1 = new ArrayElementLocation();
            //ai1.ArrayBase = s2;
            //ai1.Index = s3;

            //BasicBlocks.BasicBlock succ = new BasicBlocks.BasicBlock();
            //var c1 = new BinaryOperation(s1, s2, BinaryOperation.BinaryOp.ADD, s3);
            //var c2 = new Receive(s3, ai1);
            //var c3 = new Receive(s3, ai1);


            //succ.Code.Add(c1);
           // succ.Code.Add(c2);
            //succ.Code.Add(c3);
           // Console.WriteLine("" + succ.Description);
            Console.Read();

        }

    }
}
