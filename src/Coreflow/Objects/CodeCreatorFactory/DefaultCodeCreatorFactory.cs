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
                if (typeof(ICodeActivityCreator).IsAssignableFrom(Type) && Type.IsGenericType)
                {
                    var cctype = Type.GetGenericArguments()[0];

                    if (cctype.IsGenericType)
                    {
                        return cctype.GetGenericArguments();
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

            if (typeof(ICodeActivityCreator).IsAssignableFrom(Type) && Type.IsGenericType)
            {
                var cctype = Type.GetGenericArguments()[0];

                if (cctype.IsGenericType)
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

                    var innerType = cctype.MakeGenericType(genericTypes);

                    type = type.GetGenericTypeDefinition().MakeGenericType(innerType);
                }
            }

            return Activator.CreateInstance(type) as ICodeCreator;
        }
    }
}
