using Coreflow.Helper;
using Coreflow.Interfaces;
using Coreflow.Objects;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.FindSymbols;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Coreflow.CodeCreators
{
    public class CodeActivityCreator<T> : ICodeActivityCreator, IVariableCreator, IUiDesignable, IParametrized where T : ICodeActivity
    {
        public string FactoryIdentifier { get; set; }

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

        public string Category
        {
            get
            {
                var attr = typeof(T).GetCustomAttribute<DisplayMetaAttribute>();

                if (attr != null && !string.IsNullOrWhiteSpace(attr.Category))
                    return attr.Category;

                return null;
            }
        }


        public void Initialize(FlowBuilderContext pBuilderContext, FlowCodeWriter pCodeWriter)
        {
            pCodeWriter.WriteIdentifierTagTop(this);

            string typeName = typeof(T).FullName;
            string variableName = pBuilderContext.CreateLocalVariableName(this);

            pCodeWriter.AppendLineTop($"{typeName} {variableName} = new {typeName}();");

            foreach (FieldInfo fi in typeof(T).GetFields())
            {
                pCodeWriter.AppendLineTop($"{variableName}.{fi.Name} = {fi.Name};");
            }
        }

        public void ToCode(FlowBuilderContext pBuilderContext, FlowCodeWriter pCodeWriter, ICodeCreatorContainerCreator pContainer)
        {
            MethodInfo mi = typeof(T).GetMethod("Execute");


            //validation part

            foreach (ParameterInfo pi in mi.GetParameters().OrderBy(p => p.Position))
            {
                IArgument argument = Arguments[pi.Position];

                if (argument.Name != pi.Name)
                    throw new Exception($"Inconsistent parameter and arguments. Argument '{argument.Name}' does not match Parameter '{pi.Name}'. Did the source method changed?");

                //TODO implement "CodeGenerator CodeCreation Errors"
            }


            string topString = pCodeWriter.ToStringTop();

            foreach (ParameterInfo pi in mi.GetParameters().OrderBy(p => p.Position))
            {
                IArgument argument = Arguments[pi.Position];

                if (argument is InputExpressionCreator iec)
                {
                    string variableName = argument.Identifier.ToString().ToVariableName();
                    pCodeWriter.AppendLineTop($"var {variableName} = {iec.Code};");
                }
            }

            pBuilderContext.UpdateCurrentSymbols();

            foreach (ParameterInfo pi in mi.GetParameters().OrderBy(p => p.Position))
            {
                IArgument argument = Arguments[pi.Position];

                if (argument is InputExpressionCreator iec)
                {
                    string variableName = argument.Identifier.ToString().ToVariableName();
                    ILocalSymbol symbol = (ILocalSymbol)pBuilderContext.CurrentSymbols.FirstOrDefault(s => s.Name == variableName);

                    if (symbol == null)
                    {
                        Console.WriteLine("WARNING: Variable could not be found in current context. Flow is inconsistent!");
                    }

                    if (symbol != null && symbol.Type.TypeKind != TypeKind.Error)
                    {
                        iec.ActualType = symbol.Type;
                    }
                }
            }

            pCodeWriter.SetTopString(topString);


            pBuilderContext.UpdateCurrentSymbols();

            pCodeWriter.WriteIdentifierTagTop(this);


            if (mi.ReturnType != typeof(void))
            {
                IArgument resultarg = Arguments.Last();
                resultarg.ToCode(pBuilderContext, pCodeWriter, pContainer);
                pCodeWriter.AppendLineTop(" = ");
            }

            pCodeWriter.AppendLineTop($"{pBuilderContext.GetLocalVariableName(this)}.Execute(");


            pCodeWriter.AppendLineTop(); //prevent issue with RemoveLastChar if no parameters are there

            foreach (ParameterInfo pi in mi.GetParameters().OrderBy(p => p.Position))
            {
                IArgument argument = Arguments[pi.Position];

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
                Direction = pParameterInfo.IsOut ? VariableDirection.Out : VariableDirection.In,
                DisplayName = pParameterInfo.Name
            };

            var attr = pParameterInfo.GetCustomAttribute<DisplayMetaAttribute>();

            if (attr != null)
                ret.DisplayName = attr.DisplayName;

            return ret;
        }

        public CodeCreatorParameter[] GetParameters()
        {
            MethodInfo mi = CodeActivityType.GetMethod("Execute");

            List<CodeCreatorParameter> additional = new List<CodeCreatorParameter>();

            if (mi.ReturnType != typeof(void))
            {
                additional.Add(new CodeCreatorParameter()
                {
                    Category = "Default",
                    Name = "Result",
                    Direction = VariableDirection.Out,
                    Type = typeof(LeftSideCSharpCode)
                });
            }

            return mi.GetParameters().Select(ConvertToParameter).Concat(additional).ToArray();
        }
    }
}
