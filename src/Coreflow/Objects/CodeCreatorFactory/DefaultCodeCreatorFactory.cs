using Coreflow.CodeCreators;
using Coreflow.Interfaces;
using System;
using System.Linq;

namespace Coreflow.Objects
{
    public class DefaultCodeCreatorFactory : ICodeCreatorFactory
    {
        public string Identifier => typeof(DefaultCodeCreatorFactory).FullName + "_" + Type.FullName;

        public Type Type { get; }

        public Type[] OverrideableCustomTypes
        {
            get
            {
                if (Type.IsGenericType)
                {
                    if (Type.IsGenericTypeDefinition)
                    {
                        return Type.GetGenericArguments();
                    }
                    else
                    {
                        var cctype = Type.GetGenericArguments()[0];

                        if (cctype.IsGenericTypeDefinition)
                        {
                            return cctype.GetGenericArguments();
                        }
                    }
                }

                return null;
            }
        }

        public DefaultCodeCreatorFactory(Type pType)
        {
            Type = pType;
        }

        public ICodeCreator Create(Type[] pCustomTypes = null)
        {
            var type = Type;

            if (type.IsGenericType)
            {
                if (type.IsGenericTypeDefinition)
                {
                    type = type.GetGenericTypeDefinition().MakeGenericType(GetGenericTypes(pCustomTypes, type));
                }
                else
                {
                    var cctype = type.GetGenericArguments()[0];

                    if (cctype.IsGenericTypeDefinition)
                    {
                        Type[] genericTypes = GetGenericTypes(pCustomTypes, cctype);

                        var innerType = cctype.MakeGenericType(genericTypes);

                        type = type.GetGenericTypeDefinition().MakeGenericType(innerType);
                    }
                }
            }

            return Activator.CreateInstance(type) as ICodeCreator;
        }

        private static Type[] GetGenericTypes(Type[] pCustomTypes, Type cctype)
        {
            Type[] genericTypes = pCustomTypes;

            if (genericTypes == null)
            {
                var attr = cctype.GetCustomAttributes(typeof(DefaultGenericTypeAttribute), false).FirstOrDefault() as DefaultGenericTypeAttribute;

                if (attr != null)
                {
                    genericTypes = attr.DefaultGenericType;
                }
            }

            if (genericTypes == null)
            {
                genericTypes = new[] { typeof(object) };
            }

            return genericTypes;
        }
    }
}
