using Coreflow.CodeCreators;
using Coreflow.Interfaces;
using Coreflow.Objects;
using System.Collections.Generic;

namespace Coreflow.Test
{
    public static class TestFlows
    {
        public static FlowDefinition GetTestFlow1(Coreflow pCoreflow)
        {
            FlowDefinition wf = pCoreflow.FlowDefinitionFactory.Create("test");

            wf.Arguments.Add(new FlowArguments("pInput", typeof(string), VariableDirection.In, "\"DefaultInputValue\""));
            wf.Arguments.Add(new FlowArguments("Result", typeof(int), VariableDirection.Out));

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
