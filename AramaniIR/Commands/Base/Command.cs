using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aramani.Base;


namespace Aramani.IR.Commands
{

    public abstract class Command : IDescription
    {

        public virtual string Description
        {
            get
            {
                return "<Command>";
            }
        }

    }

    /*
     x := y / &y 
     x := op y
     x := y op z
     x := new 
     
     NewArray, NewObject
     CastClass,
     IsInstanceOf,
     ArrayLen
     SizeOf,
     Default,
     Cpobj (vll Reduktion?)
     Convert
     CkFinite
     Call
     InitBlk?
     Box
     Unbox
     NOP
     
     Conditions for jumps
     */
}