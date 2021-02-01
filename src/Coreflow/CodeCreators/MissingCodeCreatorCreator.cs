using System;
using Coreflow.Interfaces;
using Coreflow.Objects;

namespace Coreflow.CodeCreators
{
    public class MissingCodeCreatorCreator : ICodeCreator
    {
        public Guid Identifier { get; set; } = Guid.NewGuid();

        public string Type { get; set; }
        public string FactoryIdentifier { get; set; }

        public MissingCodeCreatorCreator()
        {
        }

        public MissingCodeCreatorCreator(string pType, string pFactoryIdentifier)
        {
            Type = pType;
            FactoryIdentifier = pFactoryIdentifier;
        }


        public void ToCode(FlowBuilderContext pBuilderContext, FlowCodeWriter pCodeWriter)
        {
            pCodeWriter.WriteIdentifierTagTop(this);
            pCodeWriter.AppendLineTop("//Code Creator is missing!!");
            pCodeWriter.AppendLineTop("//Type: " + Type);
            pCodeWriter.AppendLineTop("//FactoryIdentifier: " + FactoryIdentifier);
            pCodeWriter.AppendLineTop("throw new global::System.NotImplementedException($\"Code Creator " + Type + " " + FactoryIdentifier + " is missing!\");");
        }
    }
}
