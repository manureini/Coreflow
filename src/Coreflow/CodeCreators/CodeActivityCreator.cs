using Coreflow.Interfaces;
using Coreflow.Objects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Coreflow.CodeCreators
{
    public class CodeActivityCreator<T> : ICodeActivityCreator, IVariableCreator, IUiDesignable, IParametrized where T : ICodeActivity
    {
        public List<IArgument> Arguments { get; set; } = new List<IArgument>();

        public Type CodeActivityType => typeof(T);

        public Guid Identifier { get; set; } = Guid.NewGuid();

        public string VariableIdentifier => $"CodeActivityCreator<{typeof(T).FullName}>";

        public string Name
        {
            get
            {
                var attr = typeof(T).GetCustomAttribute<DisplayMetaAttribute>();

                if (attr != null && !string.IsNullOrWhiteSpace(attr.DisplayName))
                    return attr.DisplayName;

                return typeof(T).Name;
            }
        }

        public string Icon
        {
            get
            {
                var attr = typeof(T).GetCustomAttribute<DisplayMetaAttribute>();

                if (attr != null && !string.IsNullOrWhiteSpace(attr.Icon))
                    return attr.Icon;

                return DisplayMetaAttribute.DEFAULT_ICON;
            }
        }

        public void Initialize(FlowBuilderContext pBuilderContext, FlowCodeWriter pCodeWriter)
        {
            pCodeWriter.WriteIdentifierTagTop(this);

            string typeName = typeof(T).Name;
            string variableName = pBuilderContext.CreateLocalVariableName(this);

            pCodeWriter.AppendLineTop($"{typeName} {variableName} = new {typeName}();");

            foreach (FieldInfo fi in typeof(T).GetFields())
            {
                pCodeWriter.AppendLineTop($"{variableName}.{fi.Name} = {fi.Name};");
            }
        }

        public void ToCode(FlowBuilderContext pBuilderContext, FlowCodeWriter pCodeWriter, ICodeCreatorContainerCreator pContainer)
        {
            pBuilderContext.UpdateCurrentSymbols();

            pCodeWriter.WriteIdentifierTagTop(this);

            pCodeWriter.AppendLineTop($"{pBuilderContext.GetLocalVariableName(this)}.Execute(");

            MethodInfo mi = typeof(T).GetMethod("Execute");

            pCodeWriter.AppendLineTop(); //prevent issue with RemoveLastChar if no parameters are there

            foreach (ParameterInfo pi in mi.GetParameters().OrderBy(p => p.Position))
            {
                IArgument argument = Arguments[pi.Position];

                if (argument.Name != pi.Name)
                    throw new Exception($"Inconsistent parameter and arguments. Argument '{argument.Name}' does not match Parameter '{pi.Name}'. Did the source method changed?");

                //TODO implement "CodeGenerator Compile Errors"

                argument.ToCode(pBuilderContext, pCodeWriter, pContainer);
                pCodeWriter.AppendTop(",");
            }

            pCodeWriter.RemoveLastCharTop();

            pCodeWriter.AppendLineTop($");");
        }

        private CodeCreatorParameter ConvertToParameter(ParameterInfo pParameterInfo)
        {
            CodeCreatorParameter ret = new CodeCreatorParameter()
            {
                Category = "Default",
                Name = pParameterInfo.Name,
                Type = pParameterInfo.ParameterType,
                Direction = pParameterInfo.IsOut ? VariableDirection.Out : VariableDirection.In
            };

            var attr = pParameterInfo.GetCustomAttribute<DisplayMetaAttribute>();
            ret.DisplayName = attr.DisplayName;

            return ret;
        }

        public CodeCreatorParameter[] GetParameters()
        {
            MethodInfo mi = CodeActivityType.GetMethod("Execute");
            return mi.GetParameters().Select(ConvertToParameter).ToArray();
        }
    }
}
