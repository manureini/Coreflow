using Coreflow.Interfaces;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Coreflow.Helper.Serialization
{
    public class TypedConverter<T> : JsonConverter<T>
    {
        public const string TYPE_PROPERTY_NAME = "$type";

        public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
                return default(T);

            reader.Read(); // Start Array


            reader.Read(); // Start Object
            var typeProperty = reader.GetString();

            if (typeProperty != TYPE_PROPERTY_NAME)
                throw new FormatException($"Json format invalid, no {TYPE_PROPERTY_NAME} found in {nameof(ICodeCreator)}");

            reader.Read();
            var typeName = reader.GetString();
            var type = TypeHelper.SearchType(typeName);

            reader.Read();

            reader.Read(); // End Object

            var ret = (T)JsonSerializer.Deserialize(ref reader, type, options);

            reader.Read(); // End Array

            return ret;
        }

        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        {
            if (value == null)
            {
                JsonSerializer.Serialize(writer, default(T), options);
                return;
            }

            var type = value.GetType();

            writer.WriteStartArray();

            writer.WriteStartObject();
            writer.WritePropertyName(TYPE_PROPERTY_NAME);
            writer.WriteStringValue(type.AssemblyQualifiedName);
            writer.WriteEndObject();

            JsonSerializer.Serialize(writer, value, type, options);

            writer.WriteEndArray();
        }
    }
}