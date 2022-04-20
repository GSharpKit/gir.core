using GirModel;

namespace Generator3.Converter.ReturnType.ToManaged;

internal class PrimitiveValueType : ReturnTypeConverter2
{
    public bool Supports(AnyType type) 
        => type.Is<GirModel.PrimitiveValueType>();

    public string GetString(GirModel.ReturnType returnType, string fromVariableName)
        => fromVariableName; //Valid for IsPointer = true && IsPointer = false
}
