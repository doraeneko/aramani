using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aramani.IR.Variables;

namespace Aramani.IR.Commands
{


    public abstract class UnaryOperation : Command
    {
        public enum UnaryOp
        {
            UNKNOWN,
            ID,
            NEG,
            NOT,
            ISNULL,
            ISNOTNULL,
            SIZEOF,
            LOADLEN,
            THROW,
            LOCALLOC,
            CKFINITE,
            INITOBJ,
            BOX, 
            UNBOX, 
            CASTCLASS,
            ISINST,
            NEWARR,
            UNBOX_ANY 
            // TODO
        }

        public UnaryOperation(StackVariable target, UnaryOp kind, StackVariable operand)
        {
            Kind = kind;
            Operand = operand;
            Target = target;
        }

        public UnaryOp Kind { get; set; }

        public StackVariable Operand { get; set; }

        public StackVariable Target { get; set; }

        public override string Description
        {
            get
            {
                return
                    Target.Description + " := "
                    + Kind + " " 
                    + Operand.Description + "";
            }
        }
    }

    public class UntypedUnaryOperation : UnaryOperation
    {

        public UntypedUnaryOperation(StackVariable target, UnaryOp kind, StackVariable operand)
            : base(target, kind, operand)
        {
        }
    }

    public class TypedUnaryOperation : UnaryOperation
    {
        Aramani.IR.Types.GroundType TypeInfo { get; set; }

        public TypedUnaryOperation(StackVariable target, UnaryOp kind, StackVariable operand, Aramani.IR.Types.GroundType typeInfo)
            : base(target, kind, operand)
        {
            TypeInfo = typeInfo;
        }
        public override string Description
        {
            get
            {
                var typeDescr = (TypeInfo == null) ? "<unknown type>" : TypeInfo.Description;
                return
                    base.Description + " [" + typeDescr + "]";
            }
        }
    }

}
