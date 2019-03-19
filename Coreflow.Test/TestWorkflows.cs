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

            wf.Arguments.Add(new WorkflowArguments("pInput", typeof(string), VariableDirection.In, "\"DefaultInputValue\""));
            wf.Arguments.Add(new WorkflowArguments("Result", typeof(int), VariableDirection.Out));

            SequenceCreator sequence = new SequenceCreator(null);

            var cacSequence2 = new CodeActivityCreator<ConsoleWriteLineActivity>();
            cacSequence2.Arguments.Add(new InputExpressionCreator("pText", "\"Sequence 2\""));

            var cacSequence22 = new CodeActivityCreator<ConsoleWriteLineActivity>();
            cacSequence22.Arguments.Add(new InputExpressionCreator("pText", "\"Writeline Activity:\" + calcResult.ToString()"));

            var cacSequence2calc = new CodeActivityCreator<AdderActivity>();
            cacSequence2calc.Arguments.Add(new InputExpressionCreator("pFirstNumber", "2"));
            cacSequence2calc.Arguments.Add(new InputExpressionCreator("pSecondNumber", "1"));
            cacSequence2calc.Arguments.Add(new OutputVariableNameCreator("rResult", "calcResult"));

            var cacSequence2calc2 = new CodeActivityCreator<AdderActivity>();
            cacSequence2calc2.Arguments.Add(new InputExpressionCreator("pFirstNumber", "2"));
            cacSequence2calc2.Arguments.Add(new InputExpressionCreator("pSecondNumber", "1"));
            cacSequence2calc2.Arguments.Add(new OutputVariableNameCreator("rResult", "calcResult"));

            var assign = new AssignCreator();
            assign.Arguments.Add(new OutputVariableCodeInlineCreator("Left", "Result"));
            assign.Arguments.Add(new InputExpressionCreator("Right", "calcResult"));

            ForLoopCreator sequence2 = new ForLoopCreator(sequence);
            sequence2.CodeCreators.Add(new List<ICodeCreator>()
            {
                cacSequence2,
                cacSequence2calc,
                cacSequence2calc2,
                cacSequence22,
                assign
            });
            sequence2.Arguments.Add(new InputExpressionCreator("Expression", "int i = 0; i < 5; i++"));

            var cacSequence11 = new CodeActivityCreator<ConsoleWriteLineActivity>();
            cacSequence11.Arguments.Add(new InputExpressionCreator("pText", "\"Sequence 11\""));
            var cacSequence12 = new CodeActivityCreator<ConsoleWriteLineActivity>();
            cacSequence12.Arguments.Add(new InputExpressionCreator("pText", "\"Sequence 12\""));

            var noCreator = new NoCodeCreator();

            sequence.CodeCreators.Add(new List<ICodeCreator>()
            {
                cacSequence11,
                sequence2,
                cacSequence12,
                noCreator,
            });

            wf.CodeCreator = sequence;

            return wf;
        }

    }
}
