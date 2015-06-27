using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aramani.Domains
{
    class MethodFrame<S, L, P> 
        : ReducedProduct<AbstractEvalStack<S>, L, P>, 
          IMethodFrameManipulator<MethodFrame<S, L, P>>
        where S : class, IDomainElement<S>, new()
        where L : class, IDomainElement<L>, new()
        where P : class, IDomainElement<P>, new()
    {

        #region /* Components */

        protected AbstractEvalStack<S> EvalStack { get { return this.Component1; } }
        protected L localVariables { get { return this.Component2; }}
        protected P parameters { get { return this.Component3; } }

        #endregion

        public MethodFrame
            (Mono.Cecil.MethodDefinition method,
             L initialLocalVariablesValues,
             P initialParametersValues)
            : base(new AbstractEvalStack<S>(method.Body.MaxStackSize), 
                   initialLocalVariablesValues, 
                   initialParametersValues)
        {
        }


        public virtual void LoadArgument(int index, bool loadItsAddress = true)
        {
            throw new NotImplementedException();
        }

        public void LoadLocalVariable(int index, bool loadItsAddress = true)
        {
            throw new NotImplementedException();
        }

        public void LoadField(Mono.Cecil.FieldReference field, bool loadItsAddress = true)
        {
            throw new NotImplementedException();
        }

        public void LoadIndirect(TypeDiscriminant type = TypeDiscriminant.UNSPECIFIED)
        {
            throw new NotImplementedException();
        }

        public void LoadConstant(int value)
        {
            throw new NotImplementedException();
        }

        public void LoadConstant(long value)
        {
            throw new NotImplementedException();
        }

        public void LoadConstant(float value)
        {
            throw new NotImplementedException();
        }

        public void LoadConstant(double value)
        {
            throw new NotImplementedException();
        }

        public void Pop()
        {
            throw new NotImplementedException();
        }

        public void Dup()
        {
            throw new NotImplementedException();
        }

        public void CopyReference()
        {
            throw new NotImplementedException();
        }

        public void LoadValue()
        {
            throw new NotImplementedException();
        }

        public void LoadString(string s)
        {
            throw new NotImplementedException();
        }

        public void NewObject(Mono.Cecil.MethodReference constructorRef)
        {
            throw new NotImplementedException();
        }

        public void CastClass(Mono.Cecil.TypeReference typeRef)
        {
            throw new NotImplementedException();
        }

        public void IsInst(Mono.Cecil.TypeReference typeRef)
        {
            throw new NotImplementedException();
        }

        public void Unbox()
        {
            throw new NotImplementedException();
        }

        public void Unbox(Mono.Cecil.TypeReference typeRef)
        {
            throw new NotImplementedException();
        }

        public void Box()
        {
            throw new NotImplementedException();
        }

        public void Throw()
        {
            throw new NotImplementedException();
        }

        public void Rethrow()
        {
            throw new NotImplementedException();
        }

        public void NewArray(Mono.Cecil.TypeReference typeRef)
        {
            throw new NotImplementedException();
        }

        public void LoadArrayLength()
        {
            throw new NotImplementedException();
        }

        public void LoadArrayElement(TypeDiscriminant type = TypeDiscriminant.UNSPECIFIED)
        {
            throw new NotImplementedException();
        }

        public void LoadArrayElement(Mono.Cecil.TypeReference typeRef)
        {
            throw new NotImplementedException();
        }

        public void LoadArrayElementIndirect(Mono.Cecil.TypeReference typeRef)
        {
            throw new NotImplementedException();
        }

        public void StoreArrayElement(TypeDiscriminant type = TypeDiscriminant.UNSPECIFIED)
        {
            throw new NotImplementedException();
        }

        public void StoreArrayElement(Mono.Cecil.TypeReference typeRef)
        {
            throw new NotImplementedException();
        }

        public void StoreArrayElementIndirect(Mono.Cecil.TypeReference typeRef)
        {
            throw new NotImplementedException();
        }

        public void StoreArgument(int index)
        {
            throw new NotImplementedException();
        }

        public void StoreLocalVariable(int index)
        {
            throw new NotImplementedException();
        }

        public void StoreField(Mono.Cecil.FieldReference field)
        {
            throw new NotImplementedException();
        }

        public void StoreIndirect(TypeDiscriminant type = TypeDiscriminant.UNSPECIFIED)
        {
            throw new NotImplementedException();
        }

        public void StoreIndirect(Mono.Cecil.TypeReference typeRef)
        {
            throw new NotImplementedException();
        }

        public void ComputeBinaryOperation(BinaryOperation op = BinaryOperation.UNSPECIFIED, bool interpretAsUnsigned = false, bool withOverflowCheck = false)
        {
            throw new NotImplementedException();
        }

        public void ComputeUnaryOperation(UnaryOperation op = UnaryOperation.UNSPECIFIED)
        {
            throw new NotImplementedException();
        }

        public void ComputeComparison(Comparison cmp = Comparison.UNSPECIFIED, ComparisonOption option = ComparisonOption.NONE)
        {
            throw new NotImplementedException();
        }

        public void BranchOperation(Comparison cmp = Comparison.UNSPECIFIED, bool interpretAsUnsigned = false)
        {
            throw new NotImplementedException();
        }

        public void BranchIfNullOnTop()
        {
            throw new NotImplementedException();
        }

        public void BranchIfNotNullOnTop()
        {
            throw new NotImplementedException();
        }

        public void Convert(TypeDiscriminant targetType = TypeDiscriminant.UNSPECIFIED, bool interpretAsUnsigned = false, bool withOverflowCheck = false)
        {
            throw new NotImplementedException();
        }

        public void CheckForFiniteness()
        {
            throw new NotImplementedException();
        }

        public void LocalAlloc(uint size)
        {
            throw new NotImplementedException();
        }

        public void InstructionPrefix(InstructionPrefix prefix, Mono.Cecil.TypeReference parameter = null)
        {
            throw new NotImplementedException();
        }

        public void Call(bool isIndirect = false, bool isInstance = true)
        {
            throw new NotImplementedException();
        }

        public void LoadFunctionPointer(bool isVirtual = false)
        {
            throw new NotImplementedException();
        }

        public void LoadAddressFromTypedReference(Mono.Cecil.TypeReference typeRef)
        {
            throw new NotImplementedException();
        }

        public void LoadTypeFromTypedReference()
        {
            throw new NotImplementedException();
        }

        public void StoreTypedReference(Mono.Cecil.TypeReference typeRef)
        {
            throw new NotImplementedException();
        }

        public void LoadMetadataTokenToRuntimeHandle(object token)
        {
            throw new NotImplementedException();
        }

        public void UnknownCommand(object instr)
        {
            throw new NotImplementedException();
        }

        public void InitObject(Mono.Cecil.TypeReference typeRef)
        {
            throw new NotImplementedException();
        }

        public void CopyBlock()
        {
            throw new NotImplementedException();
        }

        public void InitBlock()
        {
            throw new NotImplementedException();
        }

        public void SizeOf(Mono.Cecil.TypeReference typeRef)
        {
            throw new NotImplementedException();
        }
    }
}
