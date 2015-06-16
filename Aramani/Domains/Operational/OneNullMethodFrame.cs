using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aramani.Domains;

namespace Aramani.Domains
{

    class OneNullMethodFrame
        : ReducedProduct<AbstractEvalStack<OneNull>, AbstractTuple<OneNull>, AbstractTuple<OneNull>>, 
          IEffectComputer<OneNullMethodFrame>
    {

        int instanceVariableOffset = 0;

        public OneNullMethodFrame
            (Mono.Cecil.MethodDefinition method)
            : base(new AbstractEvalStack<OneNull>(method.Body.MaxStackSize),
                   new AbstractTuple<OneNull>(method.Body.Variables.Count),
                   new AbstractTuple<OneNull>(method.IsStatic?method.Parameters.Count:method.Parameters.Count+1))
        {
            instanceVariableOffset = method.IsStatic ? 0 : 1;
        }


        public OneNullMethodFrame
            (AbstractEvalStack<OneNull> c1, AbstractTuple<OneNull> c2, AbstractTuple<OneNull> c3 )
            : base(c1, c2, c3)
        {
        }

        AbstractTuple<OneNull> variables { get { return Component2; } }
        AbstractTuple<OneNull> parameters { get { return Component3; } }


        public void ComputeEffect(Aramani.IntermediateForm.BasicBlock bb)
        {
            foreach (var instr in bb.Instructions)
            {
                ComputeEffect(instr);
            }
        }

        public void ComputeEffect
            (Mono.Cecil.Cil.Instruction instruction,
             bool UseElseBranch = false)
        {

            switch (instruction.OpCode.Code)
            {
                case Mono.Cecil.Cil.Code.Nop:
                case Mono.Cecil.Cil.Code.Break:
                    break;
                case Mono.Cecil.Cil.Code.Ldarg_0:
                    {
                        Component1.Push(parameters[0].Clone() as OneNull);
                        break;
                    }
                case Mono.Cecil.Cil.Code.Ldarg_1:
                    {
                        Component1.Push(parameters[1].Clone() as OneNull);
                        break;
                    }
                case Mono.Cecil.Cil.Code.Ldarg_2:
                    {
                        Component1.Push(parameters[2].Clone() as OneNull);
                        break;
                    }
                case Mono.Cecil.Cil.Code.Ldarg_3:
                    {
                        Component1.Push(parameters[3].Clone() as OneNull);
                        break;
                    }
                case Mono.Cecil.Cil.Code.Ldarg: 
                    {
                        var index =
                            ((Mono.Cecil.ParameterDefinition)instruction.Operand).Index + instanceVariableOffset;
                        Component1.Push(parameters[index].Clone() as OneNull);
                        break;
                    }
                case Mono.Cecil.Cil.Code.Starg:
                    {
                        var index =
                            ((Mono.Cecil.ParameterDefinition)instruction.Operand).Index + instanceVariableOffset;
                        parameters[index] = Component1.Pop(); // TODO: clone?
                        break;
                    }

                case Mono.Cecil.Cil.Code.Stloc:
                    {
                        var index = ((Mono.Cecil.Cil.VariableDefinition)instruction.Operand).Index;
                        variables[index] = Component1.Pop() as OneNull; // Clone?
                        break;
                    }
                case Mono.Cecil.Cil.Code.Ldloc:
                    {
                        var index = ((Mono.Cecil.Cil.VariableDefinition)instruction.Operand).Index;
                        Component1.Push(variables[index].Clone() as OneNull);
                        break;
                    }
                case Mono.Cecil.Cil.Code.Ldloc_0:
                    {
                        Component1.Push(variables[0].Clone() as OneNull);
                        break;
                    }
                case Mono.Cecil.Cil.Code.Ldloc_1:
                    {
                        Component1.Push(variables[1].Clone() as OneNull);
                        break;
                    }
                case Mono.Cecil.Cil.Code.Ldloc_2:
                    {
                        Component1.Push(variables[2].Clone() as OneNull);
                        break;
                    }
                case Mono.Cecil.Cil.Code.Ldloc_3:
                    {
                        Component1.Push(variables[3].Clone() as OneNull);
                        break;
                    }
                case Mono.Cecil.Cil.Code.Stloc_0:
                    {
                        var topElement = Component1.Pop();
                        variables[0] = topElement.Clone() as OneNull;
                        break;
                    }
                case Mono.Cecil.Cil.Code.Stloc_1:
                    {
                        var topElement = Component1.Pop();
                        variables[1] = topElement.Clone() as OneNull;
                        break;
                    }
                case Mono.Cecil.Cil.Code.Stloc_2:
                    {
                        var topElement = Component1.Pop();
                        variables[2] = topElement.Clone() as OneNull;
                        break;
                    }
                case Mono.Cecil.Cil.Code.Stloc_3:
                    {
                        var topElement = Component1.Pop();
                        variables[3] = topElement.Clone() as OneNull;
                        break;
                    }
                case Mono.Cecil.Cil.Code.Ldarg_S:
                    {
                        Component1.Push(parameters[((Mono.Cecil.ParameterDefinition)instruction.Operand).Index + instanceVariableOffset]);
                        break;
                    }
                case Mono.Cecil.Cil.Code.Ldarga_S:
                    {
                        // Lasset alle Hoffnung fahren!
                        Component1.Push(new OneNull(OneNull.OneNullEnum.TOP));
                        break;
                    }
                case Mono.Cecil.Cil.Code.Starg_S:
                    {
                        parameters[((Mono.Cecil.ParameterDefinition)instruction.Operand).Index + instanceVariableOffset] = Component1.Pop().Clone() as OneNull;
                        break;
                    }
                case Mono.Cecil.Cil.Code.Ldloc_S:
                    {
                        Component1.Push(
                            variables[((Mono.Cecil.Cil.VariableDefinition)instruction.Operand).Index].Clone() as OneNull);
                        break;
                    }
                case Mono.Cecil.Cil.Code.Ldloca_S:
                    {
                        // Lasset alle Hoffnung fahren!
                        Component1.Push(new OneNull(OneNull.OneNullEnum.TOP));
                        break;
                    }
                case Mono.Cecil.Cil.Code.Stloc_S:
                    {
                        var topElement = Component1.Pop();
                        variables[((Mono.Cecil.Cil.VariableDefinition)instruction.Operand).Index] = topElement.Clone() as OneNull;
                        break;
                    }
                case Mono.Cecil.Cil.Code.Ldnull:
                    {
                        var nullElement = new OneNull(OneNull.OneNullEnum.NULL);
                        Component1.Push(nullElement);
                        break;
                    }

                case Mono.Cecil.Cil.Code.Ldc_I4_1:
                    {
                        var element = new OneNull(OneNull.OneNullEnum.INT_ONE);
                        Component1.Push(element);
                        break;
                    }
                case Mono.Cecil.Cil.Code.Ldc_I4_0:
                    {
                        var element = new OneNull(OneNull.OneNullEnum.NULL);
                        Component1.Push(element);
                        break;
                    }
                case Mono.Cecil.Cil.Code.Ldc_I4_M1:
                case Mono.Cecil.Cil.Code.Ldc_I4_2:
                case Mono.Cecil.Cil.Code.Ldc_I4_3:
                case Mono.Cecil.Cil.Code.Ldc_I4_4:
                case Mono.Cecil.Cil.Code.Ldc_I4_5:
                case Mono.Cecil.Cil.Code.Ldc_I4_6:
                case Mono.Cecil.Cil.Code.Ldc_I4_7:
                case Mono.Cecil.Cil.Code.Ldc_I4_8:
                    {
                        var element = new OneNull(OneNull.OneNullEnum.INT_NOTONE);
                        Component1.Push(element);
                        break;
                    }
                case Mono.Cecil.Cil.Code.Ldc_I4_S:
                    {
                        // correct?
                        var op = (byte)instruction.Operand;
                        var element = op == 1 ? new OneNull(OneNull.OneNullEnum.INT_ONE) : new OneNull(OneNull.OneNullEnum.INT_NOTONE);
                        Component1.Push(element);
                        break;
                    }
                case Mono.Cecil.Cil.Code.Ldc_I4:
                    {
                        // correct?
                        var op = (Int32)instruction.Operand;
                        var element = op == 1 ? new OneNull(OneNull.OneNullEnum.INT_ONE) : new OneNull(OneNull.OneNullEnum.INT_NOTONE);
                        Component1.Push(element);
                        break;
                    }
                case Mono.Cecil.Cil.Code.Ldc_I8:
                    {
                        var op = (Int64)instruction.Operand;
                        var element = op == 1 ? new OneNull(OneNull.OneNullEnum.INT_ONE) : new OneNull(OneNull.OneNullEnum.INT_NOTONE);
                        Component1.Push(element);
                        break;
                    }
                case Mono.Cecil.Cil.Code.Ldc_R4:
                    {
                        var op = (Int32)instruction.Operand;
                        var element = op == 1 ? new OneNull(OneNull.OneNullEnum.INT_ONE) : new OneNull(OneNull.OneNullEnum.INT_NOTONE);
                        Component1.Push(element);
                        break;
                    }
                case Mono.Cecil.Cil.Code.Ldc_R8:
                    {
                        var element = new OneNull(OneNull.OneNullEnum.TOP);
                        Component1.Push(element);
                        break;
                    }
                case Mono.Cecil.Cil.Code.Dup:
                    {
                        var top = Component1.Top();
                        Component1.Push(top.Clone() as OneNull);
                        break;
                    }
                case Mono.Cecil.Cil.Code.Pop:
                    {
                        Component1.Pop();
                        break;
                    }
                case Mono.Cecil.Cil.Code.Jmp:
                    // strange instruction, jumps to another method...
                    // seems to be very rarely used.
                    break;
                case Mono.Cecil.Cil.Code.Call:
                case Mono.Cecil.Cil.Code.Calli:
                    // 
                    break;
                case Mono.Cecil.Cil.Code.Ret:
                    break;
                case Mono.Cecil.Cil.Code.Br_S:
                case Mono.Cecil.Cil.Code.Br:
                    // unconditional branch
                    break;


                case Mono.Cecil.Cil.Code.Brtrue_S:
                case Mono.Cecil.Cil.Code.Brtrue:
                    {

                        // todo: to bottom
                        var first = Component1.Pop();
                        if (UseElseBranch)
                        {
                            if (first.TheElement == OneNull.OneNullEnum.INT_ONE)
                                ToBottomElement();
                            
                        }
                        else
                        {
                            if (first.TheElement == OneNull.OneNullEnum.NULL)
                                ToBottomElement();
                        }
                        break;
                    }
                case Mono.Cecil.Cil.Code.Brfalse_S:


                case Mono.Cecil.Cil.Code.Beq_S:
                case Mono.Cecil.Cil.Code.Bge_S:
                case Mono.Cecil.Cil.Code.Bgt_S:
                case Mono.Cecil.Cil.Code.Ble_S:
                case Mono.Cecil.Cil.Code.Blt_S:
                case Mono.Cecil.Cil.Code.Bne_Un_S:
                case Mono.Cecil.Cil.Code.Bge_Un_S:
                case Mono.Cecil.Cil.Code.Bgt_Un_S:
                case Mono.Cecil.Cil.Code.Ble_Un_S:
                case Mono.Cecil.Cil.Code.Blt_Un_S:
                case Mono.Cecil.Cil.Code.Brfalse:

                case Mono.Cecil.Cil.Code.Beq:
                case Mono.Cecil.Cil.Code.Bge:
                case Mono.Cecil.Cil.Code.Bgt:
                case Mono.Cecil.Cil.Code.Ble:
                case Mono.Cecil.Cil.Code.Blt:
                case Mono.Cecil.Cil.Code.Bne_Un:
                case Mono.Cecil.Cil.Code.Bge_Un:
                case Mono.Cecil.Cil.Code.Bgt_Un:
                case Mono.Cecil.Cil.Code.Ble_Un:
                case Mono.Cecil.Cil.Code.Blt_Un:
                    {
                        Component1.Pop();
                        break;
                    }

                
                case Mono.Cecil.Cil.Code.Switch:
                    break;
                case Mono.Cecil.Cil.Code.Ldind_I1:
                case Mono.Cecil.Cil.Code.Ldind_U1:
                case Mono.Cecil.Cil.Code.Ldind_I2:
                case Mono.Cecil.Cil.Code.Ldind_U2:
                case Mono.Cecil.Cil.Code.Ldind_I4:
                case Mono.Cecil.Cil.Code.Ldind_U4:
                case Mono.Cecil.Cil.Code.Ldind_I8:
                case Mono.Cecil.Cil.Code.Ldind_I:
                case Mono.Cecil.Cil.Code.Ldind_R4:
                case Mono.Cecil.Cil.Code.Ldind_R8:
                case Mono.Cecil.Cil.Code.Ldind_Ref:
                    {
                        // Lasset alle Hoffnung ...
                        Component1.Push(new OneNull(OneNull.OneNullEnum.TOP));
                        break;
                    }
                case Mono.Cecil.Cil.Code.Stind_Ref:
                case Mono.Cecil.Cil.Code.Stind_I1:
                case Mono.Cecil.Cil.Code.Stind_I2:
                case Mono.Cecil.Cil.Code.Stind_I4:
                case Mono.Cecil.Cil.Code.Stind_I8:
                case Mono.Cecil.Cil.Code.Stind_R4:
                case Mono.Cecil.Cil.Code.Stind_R8:
                case Mono.Cecil.Cil.Code.Stind_I:
                    {
                        // Well, could overwrite almost everything.
                        break;
                        // TODO: invalidate ALL
                    }
                    
                case Mono.Cecil.Cil.Code.Add:
                case Mono.Cecil.Cil.Code.Sub:
                case Mono.Cecil.Cil.Code.Mul:
                case Mono.Cecil.Cil.Code.Div:
                case Mono.Cecil.Cil.Code.Div_Un:
                case Mono.Cecil.Cil.Code.Rem:
                case Mono.Cecil.Cil.Code.Rem_Un:
                case Mono.Cecil.Cil.Code.And:
                case Mono.Cecil.Cil.Code.Or:
                case Mono.Cecil.Cil.Code.Xor:
                case Mono.Cecil.Cil.Code.Shl:
                case Mono.Cecil.Cil.Code.Shr:
                case Mono.Cecil.Cil.Code.Shr_Un:
                case Mono.Cecil.Cil.Code.Add_Ovf:
                case Mono.Cecil.Cil.Code.Add_Ovf_Un:
                case Mono.Cecil.Cil.Code.Mul_Ovf:
                case Mono.Cecil.Cil.Code.Mul_Ovf_Un:
                case Mono.Cecil.Cil.Code.Sub_Ovf:
                case Mono.Cecil.Cil.Code.Sub_Ovf_Un:
                    {
                        var all = new OneNull(OneNull.OneNullEnum.TOP);
                        Component1.Pop();
                        Component1.Pop();
                        Component1.Push(all);
                        break;
                    }
                case Mono.Cecil.Cil.Code.Neg:
                case Mono.Cecil.Cil.Code.Not:
                    {
                        var all = new OneNull(OneNull.OneNullEnum.TOP);
                        Component1.Pop();
                        Component1.Push(all);
                        break;
                    }

                case Mono.Cecil.Cil.Code.Conv_I1:
                case Mono.Cecil.Cil.Code.Conv_I2:
                case Mono.Cecil.Cil.Code.Conv_I4:
                case Mono.Cecil.Cil.Code.Conv_I8:
                case Mono.Cecil.Cil.Code.Conv_R4:
                case Mono.Cecil.Cil.Code.Conv_R8:
                case Mono.Cecil.Cil.Code.Conv_U4:
                case Mono.Cecil.Cil.Code.Conv_U8:
                    {
                        var all = new OneNull(OneNull.OneNullEnum.TOP);
                        Component1.Pop();
                        Component1.Push(all);
                        break;

                    }

                case Mono.Cecil.Cil.Code.Callvirt:
                    break;
                case Mono.Cecil.Cil.Code.Cpobj:
                    {
                        Component1.Pop();
                        Component1.Pop();
                        // effect?
                        break;
                    }
                case Mono.Cecil.Cil.Code.Ldobj:
                    {
                        var all = new OneNull(OneNull.OneNullEnum.TOP);
                        Component1.Pop();
                        Component1.Push(all);
                        break;

                    }
                case Mono.Cecil.Cil.Code.Ldstr:
                    {
                        var all = new OneNull(OneNull.OneNullEnum.TOP);
                        Component1.Push(all);
                        break;
                    }

                case Mono.Cecil.Cil.Code.Newobj:
                    {
                        var all = new OneNull(OneNull.OneNullEnum.NOT_NULL);
                        Component1.Push(all);
                        break;
                    }

                case Mono.Cecil.Cil.Code.Castclass:
                    {
                        var all = new OneNull(OneNull.OneNullEnum.TOP);
                        Component1.Pop();
                        Component1.Push(all);
                        break;
                    }

                case Mono.Cecil.Cil.Code.Isinst:
                    {

                        var first = Component1.Pop();
                        if (first.TheElement == OneNull.OneNullEnum.NULL)
                        {
                            Console.WriteLine("HERE");
                            Component1.Push(first);
                        }
                        else
                        {
                            var all = new OneNull(OneNull.OneNullEnum.TOP);
                            Component1.Push(all);
                        }
                        break;
                    }
                case Mono.Cecil.Cil.Code.Conv_R_Un:
                case Mono.Cecil.Cil.Code.Unbox:
                case Mono.Cecil.Cil.Code.Ldfld:
                case Mono.Cecil.Cil.Code.Ldflda:
                    {
                        var all = new OneNull(OneNull.OneNullEnum.TOP);
                        Component1.Pop();
                        Component1.Push(all);
                        break;
                    }
                case Mono.Cecil.Cil.Code.Throw:
                    // TODO
                    break;

                case Mono.Cecil.Cil.Code.Ldsfld:
                case Mono.Cecil.Cil.Code.Ldsflda:
                    {
                        var all = new OneNull(OneNull.OneNullEnum.TOP);
                        Component1.Push(all);
                        break;
                    }

                case Mono.Cecil.Cil.Code.Conv_Ovf_I1_Un:
                case Mono.Cecil.Cil.Code.Conv_Ovf_I2_Un:
                case Mono.Cecil.Cil.Code.Conv_Ovf_I4_Un:
                case Mono.Cecil.Cil.Code.Conv_Ovf_I8_Un:
                case Mono.Cecil.Cil.Code.Conv_Ovf_U1_Un:
                case Mono.Cecil.Cil.Code.Conv_Ovf_U2_Un:
                case Mono.Cecil.Cil.Code.Conv_Ovf_U4_Un:
                case Mono.Cecil.Cil.Code.Conv_Ovf_U8_Un:
                case Mono.Cecil.Cil.Code.Conv_Ovf_I_Un:
                case Mono.Cecil.Cil.Code.Conv_Ovf_U_Un:
                case Mono.Cecil.Cil.Code.Conv_U2:
                case Mono.Cecil.Cil.Code.Conv_U1:
                case Mono.Cecil.Cil.Code.Conv_I:
                case Mono.Cecil.Cil.Code.Conv_Ovf_I:
                case Mono.Cecil.Cil.Code.Conv_Ovf_U:
                case Mono.Cecil.Cil.Code.Conv_Ovf_I1:
                case Mono.Cecil.Cil.Code.Conv_Ovf_U1:
                case Mono.Cecil.Cil.Code.Conv_Ovf_I2:
                case Mono.Cecil.Cil.Code.Conv_Ovf_U2:
                case Mono.Cecil.Cil.Code.Conv_Ovf_I4:
                case Mono.Cecil.Cil.Code.Conv_Ovf_U4:
                case Mono.Cecil.Cil.Code.Conv_Ovf_I8:
                case Mono.Cecil.Cil.Code.Conv_Ovf_U8:
                    // conversion seems to keep everything the same.
                    break;
                case Mono.Cecil.Cil.Code.Cgt_Un:
                    {
                        var first = Component1.Pop();
                        var second = Component1.Pop();
                        if (first.TheElement == OneNull.OneNullEnum.INT_ONE
                            && second.TheElement == OneNull.OneNullEnum.NULL)
                        {
                            Component1.Push(new OneNull(OneNull.OneNullEnum.INT_ONE));
                        }
                        else if (first.TheElement == second.TheElement 
                            && (first.TheElement == OneNull.OneNullEnum.INT_ONE
                                || first.TheElement == OneNull.OneNullEnum.NULL))
                        {
                            Component1.Push(new OneNull(OneNull.OneNullEnum.NULL));
                        }
                        else
                        {
                            Component1.Push(new OneNull(OneNull.OneNullEnum.TOP));
                        }
                        break;
                    }
                case Mono.Cecil.Cil.Code.Ceq:
                    {
                        var first = Component1.Pop();
                        var second = Component1.Pop();
                        if (first.TheElement == OneNull.OneNullEnum.INT_ONE
                            && second.TheElement == first.TheElement)
                        {
                            Component1.Push(new OneNull(OneNull.OneNullEnum.INT_ONE));
                        }
                        else if (first.TheElement == OneNull.OneNullEnum.NULL
                            && second.TheElement == first.TheElement)
                        {
                            Component1.Push(new OneNull(OneNull.OneNullEnum.INT_ONE));
                        }
                        else
                        {
                            Component1.Push(new OneNull(OneNull.OneNullEnum.TOP));
                        }
                        break;
                    }
                case Mono.Cecil.Cil.Code.Cgt:
                case Mono.Cecil.Cil.Code.Clt:
                case Mono.Cecil.Cil.Code.Clt_Un:
                    {
                        Component1.Pop();
                        Component1.Pop();
                        Component1.Push(new OneNull(OneNull.OneNullEnum.TOP));
                        break;
                    }
                // continue here
                case Mono.Cecil.Cil.Code.Stfld:
                    // TODO: side effect
                    {
                        Component1.Pop();
                        Component1.Pop();
                        break;
                    }
                case Mono.Cecil.Cil.Code.Stsfld:
                    // TODO: static field store
                    // TODO: side effect
                    {
                        Component1.Pop();
                        Component1.Pop();
                        break;
                    }
                case Mono.Cecil.Cil.Code.Stobj:
                    // TODO
                    {
                        Component1.Pop();
                        Component1.Pop();
                        break;
                    }

                case Mono.Cecil.Cil.Code.Box:
                    // TODO
                    {
                        Component1.Pop();
                        Component1.Push(new OneNull(OneNull.OneNullEnum.TOP));
                        break;
                    }
                case Mono.Cecil.Cil.Code.Newarr:
                    // TODO
                    {
                        Component1.Pop();
                        Component1.Push(new OneNull(OneNull.OneNullEnum.TOP));
                        break;
                    }
                case Mono.Cecil.Cil.Code.Ldlen:
                    // TODO
                    {
                        Component1.Pop();
                        Component1.Push(new OneNull(OneNull.OneNullEnum.TOP));
                        break;
                    }

                case Mono.Cecil.Cil.Code.Ldelema:
                    // TODO
                    {
                        Component1.Pop();
                        Component1.Pop();
                        Component1.Push(new OneNull(OneNull.OneNullEnum.TOP));
                        break;
                    }
                case Mono.Cecil.Cil.Code.Ldelem_I1:
                case Mono.Cecil.Cil.Code.Ldelem_U1:
                case Mono.Cecil.Cil.Code.Ldelem_I2:
                case Mono.Cecil.Cil.Code.Ldelem_U2:
                case Mono.Cecil.Cil.Code.Ldelem_I4:
                case Mono.Cecil.Cil.Code.Ldelem_U4:
                case Mono.Cecil.Cil.Code.Ldelem_I8:
                case Mono.Cecil.Cil.Code.Ldelem_I:
                case Mono.Cecil.Cil.Code.Ldelem_R4:
                case Mono.Cecil.Cil.Code.Ldelem_R8:
                case Mono.Cecil.Cil.Code.Ldelem_Ref:
                case Mono.Cecil.Cil.Code.Ldelem_Any:
                    {
                        Component1.Pop();
                        Component1.Pop();
                        Component1.Push(new OneNull(OneNull.OneNullEnum.TOP));
                        break;
                    }

                case Mono.Cecil.Cil.Code.Stelem_I:
                case Mono.Cecil.Cil.Code.Stelem_I1:
                case Mono.Cecil.Cil.Code.Stelem_I2:
                case Mono.Cecil.Cil.Code.Stelem_I4:
                case Mono.Cecil.Cil.Code.Stelem_I8:
                case Mono.Cecil.Cil.Code.Stelem_R4:
                case Mono.Cecil.Cil.Code.Stelem_R8:
                case Mono.Cecil.Cil.Code.Stelem_Ref:
                case Mono.Cecil.Cil.Code.Stelem_Any:
                    {
                        Component1.Pop();
                        Component1.Pop();
                        Component1.Pop();
                        break;
                    }
                case Mono.Cecil.Cil.Code.Unbox_Any:
                    {
                        Component1.Pop();
                        Component1.Push(new OneNull(OneNull.OneNullEnum.TOP));
                        break;
                    }

                case Mono.Cecil.Cil.Code.Refanyval:
                    {
                        Component1.Pop();
                        Component1.Push(new OneNull(OneNull.OneNullEnum.TOP));
                        break;
                    }
                case Mono.Cecil.Cil.Code.Ckfinite:
                    // TODO: exception...
                    break;
                case Mono.Cecil.Cil.Code.Mkrefany:
                    {
                        Component1.Pop();
                        Component1.Push(new OneNull(OneNull.OneNullEnum.TOP));
                        break;
                    }
                case Mono.Cecil.Cil.Code.Ldtoken:
                    {
                        Component1.Push(new OneNull(OneNull.OneNullEnum.TOP));
                        break;
                    }

                case Mono.Cecil.Cil.Code.Endfinally:
                    break;
                case Mono.Cecil.Cil.Code.Leave:
                case Mono.Cecil.Cil.Code.Leave_S:
                    break;

                case Mono.Cecil.Cil.Code.Ldvirtftn:
                case Mono.Cecil.Cil.Code.Ldftn:
                    {
                        Component1.Push(new OneNull(OneNull.OneNullEnum.TOP));
                        break;
                    }

                    // CONTINUE

//                    break;
                case Mono.Cecil.Cil.Code.Conv_U:
                    {
                        Component1.Pop();
                        Component1.Push(new OneNull(OneNull.OneNullEnum.TOP));
                        break;
                    }
                case Mono.Cecil.Cil.Code.Arglist:
                    {
                        Component1.Pop();
                        Component1.Push(new OneNull(OneNull.OneNullEnum.TOP));
                        break;
                    }
                case Mono.Cecil.Cil.Code.Ldarga:
                    {
                        Component1.Push(new OneNull(OneNull.OneNullEnum.TOP));
                        break;
                    }
                case Mono.Cecil.Cil.Code.Ldloca:
                    {
                        Component1.Push(new OneNull(OneNull.OneNullEnum.TOP));
                        break;
                    }

                case Mono.Cecil.Cil.Code.Localloc:
                    {
                        Component1.Push(new OneNull(OneNull.OneNullEnum.TOP));
                        break;
                    }
                case Mono.Cecil.Cil.Code.Endfilter:
                    // TODO
                    break;
                case Mono.Cecil.Cil.Code.Unaligned:
                case Mono.Cecil.Cil.Code.Volatile:
                case Mono.Cecil.Cil.Code.Tail:
                case Mono.Cecil.Cil.Code.Constrained:
                    break;

                
                case Mono.Cecil.Cil.Code.Initobj:
                case Mono.Cecil.Cil.Code.Cpblk:
                case Mono.Cecil.Cil.Code.Initblk:
                    {
                        Component1.Pop();
                        Component1.Pop();
                        Component1.Pop();
                        break;
                    }

                case Mono.Cecil.Cil.Code.No:
                    //?
                    break;
                case Mono.Cecil.Cil.Code.Rethrow:
                    // TODO
                    break;
                case Mono.Cecil.Cil.Code.Sizeof:
                    {
                        Component1.Push(new OneNull(OneNull.OneNullEnum.TOP));
                        break;
                    }
                case Mono.Cecil.Cil.Code.Refanytype:
                    {
                        Component1.Pop();
                        Component1.Push(new OneNull(OneNull.OneNullEnum.TOP));
                        break;
                    }
                case Mono.Cecil.Cil.Code.Readonly:
                    break;
                default:
                    break;
            }

        }

        public override object Clone()
        {
            var result = new OneNullMethodFrame
                (Component1.Clone() as AbstractEvalStack<OneNull>,
                 Component2.Clone() as AbstractTuple<OneNull>,
                 Component3.Clone() as AbstractTuple<OneNull>);
            return result;
        }
    }
}
