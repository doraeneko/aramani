using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aramani.Base;


namespace Aramani.IR.Commands
{

    abstract class Command : IDescription
    {

        public string Description
        {
            get
            {
                return "<Command>";
            }
        }

    }

    /*

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
     
     Conditions for jumps
     */
}