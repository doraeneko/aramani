using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Aramani.Domains;
using Mono.Cecil;

namespace Aramani.AbstractMachine
{

    abstract class AbstractMachine
        <STACKDOMAIN, STACKELEMENTDOMAIN, 
         LOCALVARDOMAIN, LOCALVARVALUEDOMAIN,
         FIELDDOMAIN, FIELDVALUEDOMAIN, FIELDINSTANCEDOMAIN, 
         PARAMETERDOMAIN, PARAMETERVALUEDOMAIN,
         HEAPDOMAIN, ADDRDOMAIN, HEAPVALUEDOMAIN
        >
        : 
        ReducedProduct<ReducedProduct<STACKDOMAIN, HEAPDOMAIN>, ReducedProduct<PARAMETERDOMAIN, LOCALVARDOMAIN, FIELDDOMAIN>>,
        Aramani.Domains.IMethodFrameManipulator
        where 
        STACKDOMAIN     : class, IEvalStack<STACKELEMENTDOMAIN>, IDomainElement<STACKDOMAIN>, new()
        where
        LOCALVARDOMAIN  : class, ILocalVariables<LOCALVARVALUEDOMAIN>, IDomainElement<LOCALVARDOMAIN>, new()
        where
        FIELDDOMAIN     : class, IFields<FIELDVALUEDOMAIN, FIELDINSTANCEDOMAIN>, IDomainElement<FIELDDOMAIN>, new()
        where
        PARAMETERDOMAIN : class, IParameters<PARAMETERDOMAIN>, IDomainElement<PARAMETERDOMAIN>, new()
        where
        HEAPDOMAIN      : class, IHeap<ADDRDOMAIN, HEAPVALUEDOMAIN>, IDomainElement<HEAPDOMAIN>, new()

    {

        STACKDOMAIN stack          { get { return this.Component1.Component1; } }
        HEAPDOMAIN heap            { get { return this.Component1.Component2; } }
        PARAMETERDOMAIN parameters { get { return this.Component2.Component1; } }
        LOCALVARDOMAIN localVars   { get { return this.Component2.Component2; } }
        FIELDDOMAIN fields         { get { return this.Component2.Component3; } }


        public delegate T Converter<U, T>(U u);

        static Converter<STACKELEMENTDOMAIN, FIELDVALUEDOMAIN>    convertStackElementToFieldValue;
        static Converter<STACKELEMENTDOMAIN, LOCALVARVALUEDOMAIN> convertStackElementToLocalVarValue;
        static Converter<STACKELEMENTDOMAIN, FIELDINSTANCEDOMAIN> convertStackElementToFieldInstance;
        static Converter<STACKELEMENTDOMAIN, PARAMETERDOMAIN>     convertStackElementToParameterValue;
        static Converter<STACKELEMENTDOMAIN, ADDRDOMAIN>          convertStackElementToAddress;
        static Converter<STACKELEMENTDOMAIN, HEAPVALUEDOMAIN>     convertStackElementToHeapValue;

        static Converter<FIELDVALUEDOMAIN, STACKELEMENTDOMAIN>    convertFieldValueToStackElement;
        static Converter<LOCALVARVALUEDOMAIN, STACKELEMENTDOMAIN> convertLocalVarValueToStackElement;
        static Converter<PARAMETERDOMAIN, STACKELEMENTDOMAIN>     convertParameterValueToStackElement;


        public virtual void LoadArgument(int index, bool loadItsAddress = false)
        {
            if (loadItsAddress)
            {
                stack.PushUnknownElement();
            }
            else
            {
                stack.Push(convertParameterValueToStackElement(parameters.GetParameter(index)));
            }
        }

        public virtual void LoadLocalVariable(int index, bool loadItsAddress = false)
        {
            if (loadItsAddress)
            {
                stack.PushUnknownElement();
            }
            else
            {
                stack.Push(convertLocalVarValueToStackElement(localVars.GetLocalVariable(index)));
            }
        }

        public virtual void LoadField
            (Mono.Cecil.FieldReference field, bool loadItsAddress = false)
        {
            var isStatic = field.Resolve().IsStatic;
            if (loadItsAddress)
            {
                if (isStatic)
                {
                    stack.Pop();
                }
                stack.PushUnknownElement();
            }
            else
            {

                if (isStatic)
                {
                    stack.Push(convertFieldValueToStackElement(fields.GetStaticField(field)));
                }
                else
                {
                    var instance = convertStackElementToFieldInstance(stack.Pop());
                    stack.Push(convertFieldValueToStackElement(fields.GetInstanceField(field, instance)));
                }
            }
        }

        public virtual void LoadIndirect
            (Domains.TypeDiscriminant type = Aramani.Domains.TypeDiscriminant.UNSPECIFIED)
        {
            stack.Pop();
            stack.PushUnknownElement();
            heap.ToUnknown();
        }

        public virtual void LoadConstant(int value)
        {
            stack.PushUnknownElement();
        }

        public virtual void LoadConstant(long value)
        {
            stack.PushUnknownElement();
        }

        public virtual void LoadConstant(float value)
        {
            stack.PushUnknownElement();
        }

        public virtual void LoadConstant(double value)
        {
            stack.PushUnknownElement();
        }

        public virtual void Pop()
        {
            stack.Pop();
        }

        public virtual void Dup()
        {
            stack.Push(stack.Top());
        }

        public virtual void CopyReference()
        {
            stack.Pop();
            stack.Pop();
            // now: reference handling
            heap.ToUnknown();
        }

        public virtual void LoadValue()
        {
            stack.Pop(); // get address
            // now: do the copy stuff :)
            heap.ToUnknown();

        }

        public virtual void LoadString(string s)
        {
            stack.PushUnknownElement();
        }

        public virtual void NewObject(Mono.Cecil.MethodReference constructorRef)
        {
            if (constructorRef.HasParameters)
            {
                for (int i = 0; i < constructorRef.Parameters.Count; ++i)
                {
                    // clean stack from arguments
                    stack.Pop();
                }
            }
            stack.PushUnknownElement();
            // side effect for heap?

        }

        public virtual void CastClass(Mono.Cecil.TypeReference typeRef)
        {
            stack.Pop();
            stack.PushUnknownElement();
        }

        public virtual void IsInst(Mono.Cecil.TypeReference typeRef)
        {
            stack.Pop();
            stack.PushUnknownElement();
        }

        public virtual void Unbox()
        {
            stack.Pop();
            stack.PushUnknownElement();
        }

        public virtual void Unbox(Mono.Cecil.TypeReference typeRef)
        {
            stack.Pop();
            stack.PushUnknownElement();
        }

        public virtual void Box()
        {
            stack.Pop();
            stack.PushUnknownElement();
        }

        public virtual void Throw()
        {
            // nothing
        }

        public virtual void Rethrow()
        {
            // nothing
        }

        public virtual void NewArray(Mono.Cecil.TypeReference typeRef)
        {
            stack.Pop(); // # of elements
            stack.PushUnknownElement(); // # array reference
            
        }

        public virtual void LoadArrayLength()
        {
            stack.Pop(); // array reference
            stack.PushUnknownElement();  // array length
        }

        public virtual void LoadArrayElement(Domains.TypeDiscriminant type = TypeDiscriminant.UNSPECIFIED)
        {
            stack.Pop(); // object ref
            stack.Pop(); // index
            stack.PushUnknownElement();
        }

        public virtual void LoadArrayElement(Mono.Cecil.TypeReference typeRef)
        {
            stack.Pop(); // object ref
            stack.Pop(); // index
            stack.PushUnknownElement();
        }

        public virtual void LoadArrayElementIndirect(Mono.Cecil.TypeReference typeRef)
        {
            stack.Pop(); // object ref
            stack.Pop(); // index
            stack.PushUnknownElement(); // address on stack
        }

        public virtual void StoreArrayElement(Domains.TypeDiscriminant type = TypeDiscriminant.UNSPECIFIED)
        {
            stack.Pop(); // object ref
            stack.Pop(); // index
            stack.Pop(); // value
            heap.ToUnknown(); // heap is dirty now
        }

        public virtual void StoreArrayElement(Mono.Cecil.TypeReference typeRef)
        {
            stack.Pop(); // object ref
            stack.Pop(); // index
            stack.Pop(); // value
            heap.ToUnknown(); // heap is dirty now
        }


        public virtual void StoreArgument(int index)
        {
            parameters.SetParameter(index, convertStackElementToParameterValue(stack.Pop()));
        }

        public void StoreLocalVariable(int index)
        {
            localVars.SetLocalVariable(index, convertStackElementToLocalVarValue(stack.Pop()));
        }

        public void StoreField(Mono.Cecil.FieldReference field)
        {
            if (field.Resolve().IsStatic)
            {
                fields.SetStaticField(field, convertStackElementToFieldValue(stack.Pop()));
            }
            else
            {
                var instance = convertStackElementToFieldInstance(stack.Pop());
                var value = convertStackElementToFieldValue(stack.Pop());
                fields.SetInstanceField(field, instance, value);
            }
        }

        public virtual void StoreIndirect(Domains.TypeDiscriminant type = TypeDiscriminant.UNSPECIFIED)
        {
            var address = stack.Pop();
            var value = stack.Pop();
            heap.SetMemoryValue(convertStackElementToAddress(address), convertStackElementToHeapValue(value));
        }

        public virtual void StoreIndirect(Mono.Cecil.TypeReference typeRef)
        {
            var address = stack.Pop();
            var value = stack.Pop();
            heap.SetMemoryValue(convertStackElementToAddress(address), convertStackElementToHeapValue(value));
        }

        public virtual void ComputeBinaryOperation(Domains.BinaryOperation op = BinaryOperation.UNSPECIFIED, bool interpretAsUnsigned = false, bool withOverflowCheck = false)
        {
            stack.Pop();
            stack.Pop();
            stack.PushUnknownElement();
        }

        public virtual void ComputeUnaryOperation(Domains.UnaryOperation op = UnaryOperation.UNSPECIFIED)
        {
            stack.Pop();
            stack.PushUnknownElement();
        }

        public virtual void ComputeComparison(Domains.Comparison cmp = Comparison.UNSPECIFIED, Domains.ComparisonOption option = ComparisonOption.NONE)
        {
            stack.Pop();
            stack.Pop();
            stack.PushUnknownElement();
        }

        public void BranchOperation(Domains.Comparison cmp = Comparison.UNSPECIFIED, bool interpretAsUnsigned = false)
        {
            stack.Pop();
            stack.Pop();

        }

        public void BranchIfNullOnTop()
        {
            stack.Pop();
        }

        public void BranchIfNotNullOnTop()
        {
            stack.Pop();
        }

        public virtual void Convert(Domains.TypeDiscriminant targetType = TypeDiscriminant.UNSPECIFIED, bool interpretAsUnsigned = false, bool withOverflowCheck = false)
        {
            stack.Pop();
            stack.PushUnknownElement();
        }

        public virtual void CheckForFiniteness()
        {
            // ... hm, exception possible.
        }

        public void LocalAlloc(uint size)
        {
            stack.Pop();
            stack.PushUnknownElement();
            heap.ToUnknown();
        }

        public virtual void InstructionPrefix(Domains.InstructionPrefix prefix, Mono.Cecil.TypeReference parameter = null)
        {

        }

        public void Call(IMethodSignature callSiteOrMethodRef, bool isIndirect = false)
        {
            if (callSiteOrMethodRef.HasThis)
            {
                stack.Pop();
            }
            if (callSiteOrMethodRef.HasParameters)
            {
                foreach (var el in callSiteOrMethodRef.Parameters)
                {
                    stack.Pop();
                }
            }
            // "destroy" heap
            heap.ToUnknown();
            // TODO side effects for params/local vars?

        }

        public void LoadFunctionPointer(bool isVirtual = false)
        {
            stack.PushUnknownElement();
        }

        public void LoadAddressFromTypedReference(Mono.Cecil.TypeReference typeRef)
        {
            stack.Pop();
            stack.PushUnknownElement();
        }

        public void LoadTypeFromTypedReference()
        {
            stack.Pop();
            stack.PushUnknownElement();
        }

        public void StoreTypedReference(Mono.Cecil.TypeReference typeRef)
        {
            stack.Pop();
            stack.PushUnknownElement();
        }

        public void LoadMetadataTokenToRuntimeHandle(object token)
        {
            stack.PushUnknownElement();
        }

        public void UnknownCommand(object instr)
        {
            throw new NotImplementedException();
        }

        public void InitObject(Mono.Cecil.TypeReference typeRef)
        {
            stack.Pop();
            heap.ToUnknown();
            // stack also unknown?
        }

        public void CopyBlock()
        {
            stack.Pop();
            stack.Pop();
            stack.Pop();
            heap.ToUnknown();
        }

        public void InitBlock()
        {
            stack.Pop();
            stack.Pop();
            stack.Pop();
            heap.ToUnknown();
        }

        public void SizeOf(Mono.Cecil.TypeReference typeRef)
        {
            stack.PushUnknownElement();
        }
    }
}
