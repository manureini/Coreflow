using Coreflow.CodeCreators;
using Coreflow.Helper;
using Coreflow.Interfaces;
using Coreflow.Objects;
using Coreflow.Test;
using System.Collections.Generic;

namespace Coreflow.Test
{
    public static class TestWorkflows
    {
        public static WorkflowDefinition GetTestWorkflow1(Coreflow pCoreflow)
        {
            WorkflowDefinition wf = pCoreflow.WorkflowDefinitionFactory.Create("test");

            wf.Arguments.Add(new WorkflowArguments("pInput", typeof(string), "\"DefaultInputValue\""));

            SequenceCreator sequence = new SequenceCreator(null);

            /*

            var cacSequence2 = new CodeActivityCreator<ConsoleWriteLineActivity>();
            cacSequence2.Arguments.Set("pText", new InputExpressionCreator("\"Sequence 2\""));

            var cacSequence22 = new CodeActivityCreator<ConsoleWriteLineActivity>();
            cacSequence22.Arguments.Set("pText", new InputExpressionCreator("\"Writeline Activity:\" + calcResult.ToString()"));

            var cacSequence2calc = new CodeActivityCreator<AdderActivity>();
            cacSequence2calc.Arguments.Set("pFirstNumber", new InputExpressionCreator("2"));
            cacSequence2calc.Arguments.Set("pSecondNumber", new InputExpressionCreator("1"));
            cacSequence2calc.Arguments.Set("rResult", new OutputVariableCreator("calcResult"));

            ForLoopCreator sequence2 = new ForLoopCreator(sequence);
            sequence2.CodeCreators = new List<ICodeCreator>()
            {
                cacSequence2,
                cacSequence2calc,
                cacSequence22
            };
            sequence2.Arguments.Set("Expression", new InputExpressionCreator("int i = 0; i < 5; i++"));

            var cacSequence11 = new CodeActivityCreator<ConsoleWriteLineActivity>();
            cacSequence11.Arguments.Set("pText", new InputExpressionCreator("\"Sequence 11\""));
            var cacSequence12 = new CodeActivityCreator<ConsoleWriteLineActivity>();
            cacSequence12.Arguments.Set("pText", new InputExpressionCreator("\"Sequence 12\""));

            var noCreator = new NoCodeCreator();

            sequence.CodeCreators = new List<ICodeCreator>()
            {
                cacSequence11,
                sequence2,
                cacSequence12,
                noCreator
            };
            */

            wf.CodeCreator = sequence;

            return wf;
        }

    }
}
