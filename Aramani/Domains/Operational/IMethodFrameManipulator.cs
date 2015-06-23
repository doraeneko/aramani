using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Aramani.Domains
{

    public enum TypeDiscriminant
    {
        UNSPECIFIED,
        I1,
        I2,
        I4,
        I8,
        U1,
        U2,
        U4,
        U8,
        R4,
        R8,
        REF,
        NATIVE_INT,
        UNSIGNED_NATIVE_INT,
    }

    public enum BinaryOperation
    {
        UNSPECIFIED,
        ADD,
        SUB,
        MUL,
        DIV,
        MOD,
        OR,
        XOR,
        AND,
        SHL,
        SHR,
        SHR_UNSIGNED,
        REM,
    }

    public enum UnaryOperation
    {
        NEG,
        NOT,
        UNSPECIFIED
    }

    public enum Comparison
    {
        ISNULL,
        ISNOTNULL,
        LEQ,
        GEQ,
        LT,
        GT,
        EQ,
        NEQ,
        UNSPECIFIED
    }

    public enum ComparisonOption
    {
        NONE,
        UNSIGNED,
        CHECKED
    }

    public enum InstructionPrefix
    {
        VOLATILE,
        UNALIGNED,
        TAILCALL,
        CONSTRAINED,
        READONLY,
    }


    interface IMethodFrameManipulator<T> 
        where T : IDomainElement<T>
    {

        void LoadArgument(int index, bool loadItsAddress = true);
        void LoadLocalVariable(int index, bool loadItsAddress = true);
        void LoadField(FieldReference field, bool loadItsAddress = true);
        void LoadIndirect(TypeDiscriminant type = TypeDiscriminant.UNSPECIFIED);
        void LoadConstant(int value);
        void LoadConstant(long value);
        void LoadConstant(float value);
        void LoadConstant(double value);
        void Pop();
        void Dup();
        void CopyReference();
        void LoadValue();
        void LoadString(string s);
        void NewObject(MethodReference constructorRef);
        void CastClass(TypeReference typeRef);
        void IsInst(TypeReference typeRef);
        void Unbox();
        void Unbox(TypeReference typeRef);
        void Box();
        void Throw();
        void Rethrow();
        void NewArray(TypeReference typeRef);
        void LoadArrayLength();
        void LoadArrayElement
            (TypeDiscriminant type = TypeDiscriminant.UNSPECIFIED);
        void LoadArrayElement
            (TypeReference typeRef);
        void LoadArrayElementIndirect
            (TypeReference typeRef);
        void StoreArrayElement
            (TypeDiscriminant type = TypeDiscriminant.UNSPECIFIED);
        void StoreArrayElement
            (TypeReference typeRef);
        void StoreArrayElementIndirect
            (TypeReference typeRef);

        void StoreArgument(int index);
        void StoreLocalVariable(int index);
        void StoreField(FieldReference field);
        void StoreIndirect(TypeDiscriminant type = TypeDiscriminant.UNSPECIFIED);
        void StoreIndirect(TypeReference typeRef);
        void ComputeBinaryOperation
            (BinaryOperation op = BinaryOperation.UNSPECIFIED,
             bool interpretAsUnsigned = false,
             bool withOverflowCheck = false);
        void ComputeUnaryOperation(UnaryOperation op = UnaryOperation.UNSPECIFIED);
        void ComputeComparison
            (Comparison cmp = Comparison.UNSPECIFIED, 
             ComparisonOption option = ComparisonOption.NONE);
        void BranchOperation
            (Comparison cmp = Comparison.UNSPECIFIED,
             bool interpretAsUnsigned = false);
        void BranchIfNullOnTop();
        void BranchIfNotNullOnTop();

        void Convert
            (TypeDiscriminant targetType = TypeDiscriminant.UNSPECIFIED, 
             bool interpretAsUnsigned = false, 
             bool withOverflowCheck = false);

        void CheckForFiniteness();

        void LocalAlloc(uint size);

        void InstructionPrefix(InstructionPrefix prefix, TypeReference parameter = null);

        void Call
            (bool isIndirect = false, 
             bool isInstance = true);


        void LoadFunctionPointer(bool isVirtual = false);

        void LoadAddressFromTypedReference(TypeReference typeRef);
        void LoadTypeFromTypedReference();
        void StoreTypedReference(TypeReference typeRef);
        void LoadMetadataTokenToRuntimeHandle(object token);

        void UnknownCommand(object instr);

        void InitObject(TypeReference typeRef);

        void CopyBlock();
        void InitBlock();
        void SizeOf(TypeReference typeRef);
    }
}