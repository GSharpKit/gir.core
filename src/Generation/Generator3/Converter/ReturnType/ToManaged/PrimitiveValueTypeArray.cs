using GirModel;

namespace Generator3.Converter.ReturnType.ToManaged;

internal class PrimitiveValueTypeArray : ReturnTypeConverter2
{
    public bool Supports(AnyType type)
        => type.IsArray<GirModel.PrimitiveValueType>();

    public string GetString(GirModel.ReturnType returnType, string fromVariableName)
        => fromVariableName;
}
