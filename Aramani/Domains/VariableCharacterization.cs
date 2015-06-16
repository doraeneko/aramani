
using System;
using System.Collections.Generic;
using Mono.Cecil.Cil;
using CODE = Mono.Cecil.Cil.Code;
using Aramani.Domains;


namespace Aramani.Domains
{

    enum VariableCharaterization : byte
    {
        BOTTOM,
        TOP,
        //TRUE,
        //FALSE,
        //ZERO,
        //ONE,
        //NOTZERO,
        NULL
    }

}