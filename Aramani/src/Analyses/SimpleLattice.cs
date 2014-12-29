
using System;
using System.Collections.Generic;
using Mono.Cecil.Cil;
using CODE = Mono.Cecil.Cil.Code;
using DotNetAnalyser.Domains;


namespace DotNetAnalyser.Analyses
{

    enum SimpleLattice : byte
    {
        BOTTOM,
        TOP,
        TRUE,
        FALSE,
        ZERO,
        ONE,
        NOTZERO,
        NULL
    }

}