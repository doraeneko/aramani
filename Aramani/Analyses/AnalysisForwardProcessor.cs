
using System;
using System.Linq;
using System.Collections.Generic;
using Mono.Collections.Generic;
using Mono.Cecil;
using Mono.Cecil.Cil;

using CODE = Mono.Cecil.Cil.Code;

namespace Aramani.IntermediateForm
{

    /// <summary>
    /// Analysis processor
    /// </summary>
    class ForwardAnalysisProcessor<T>
        where T: class, Domains.IDomainElement<T>, Domains.IEffectComputer<T>
    {


        Dictionary<BasicBlock, T> outputValues;
        Dictionary<BasicBlock, T> outputBackupValues;
        T entryInitValue;

        public ForwardAnalysisProcessor(T entryInitValue)
        {
            this.entryInitValue = entryInitValue;
            outputValues = new Dictionary<BasicBlock, T>();
            outputBackupValues = new Dictionary<BasicBlock, T>();
        }


        public void InitValuesForMethod
             (MethodFlowGraph flowGraph)
        {

            foreach (var basicBlock 
                     in flowGraph.BasicBlocks)
            {
                outputValues[basicBlock] = null;
                outputBackupValues[basicBlock] = null;
            }
        }


        public void ProcessForward(BasicBlock block)
        {
            Console.WriteLine("PROCESS: " + block.GetHashCode());
            var analysisValues = outputValues[block];
            if (analysisValues != null)
                outputBackupValues[block] = analysisValues.Clone() as T;
            T input = null;
            if (block.IsStartBlock)
            {
                input = entryInitValue.Clone() as T;
            }
            else
            {
                // Compute union of input values
                foreach (var predecessor in block.Predecessors)
                {
                    //Console.WriteLine(predecessor.Source.GetHashCode() + "-->" + predecessor.Target.GetHashCode());
                    var singleInput = outputValues[predecessor.Source];
                    if (singleInput == null)
                        continue;
                    singleInput = singleInput.Clone() as T;
                    switch (predecessor.Kind)
                    {
                        case FlowEdgeKind.COND_IF:
                            singleInput.ComputeEffect(predecessor.BranchInstruction, false);
                            break;
                        case FlowEdgeKind.COND_ELSE:
                            singleInput.ComputeEffect(predecessor.BranchInstruction, true);
                            break;
                        case FlowEdgeKind.NORMAL:
                        default:
                            break;
                    }
                    if (input == null)
                    {
                        input = singleInput.Clone() as T;
                    }
                    else
                    {
                        //Console.WriteLine("UNION");
                        //Console.WriteLine(input.Description());
                        //Console.WriteLine(singleInput.Description());

                        input.UnionWith(singleInput);
                    }

                }
            }
            bool isBranchBlock = block is BranchingBlock;

            if (input == null)
            {
                outputBackupValues[block] = null;
            }
            else
            {
                Console.WriteLine("INPUT:" + input.Description());
                foreach (var instruction in block.Instructions)
                {
                    if (!isBranchBlock || instruction != block.LastInstruction)
                    {
                        input.ComputeEffect(instruction);
                    }
                }
                outputValues[block] = input;
                Console.WriteLine("NEW VAL:" + input.Description());
            }
            
        }

        public void Description(MethodFlowGraph flowGraph)
        {
            System.Console.WriteLine("RESULTS:");
            foreach (var basicBlock in flowGraph.BasicBlocks)
            {
                var value = outputValues[basicBlock];
                if (value == null)
                    System.Console.WriteLine("NULL");
                else
                    System.Console.WriteLine(basicBlock.GetHashCode() + "->" + value.Description());
            }
            System.Console.WriteLine("----------------------------");

        }

        public bool PerformForwardIteration
            (MethodFlowGraph flowGraph)
        {

            bool endReached = true;
            foreach (var basicBlock in flowGraph.BasicBlocks)
            {

                ProcessForward(basicBlock);
                var oldValue = outputBackupValues[basicBlock];
                var newValue = outputValues[basicBlock];

                endReached =
                    endReached
                    && oldValue != null 
                    && newValue != null 
                    && newValue.IsSubsetOrEqual(oldValue);

               
            }
            return endReached;
            
        }
    }
}