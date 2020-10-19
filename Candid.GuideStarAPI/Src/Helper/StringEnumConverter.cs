using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Candid.GuideStarAPI.Types;

namespace Candid.GuideStarAPI.Src.Helper
{
  class StringEnumConverter<T> : JsonConverter<T> where T : StringEnum
  {
    public override T Read(
      ref Utf8JsonReader reader,
      Type typeToConvert,
      JsonSerializerOptions options)
    {
      throw new NotImplementedException();
    }

    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
      writer.WriteStringValue(value.ToString());
    }
  }
}
