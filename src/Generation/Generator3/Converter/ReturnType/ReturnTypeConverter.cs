using GirModel;

namespace Generator3.Converter;

public interface ReturnTypeConverter2
{
    bool Supports(AnyType type);
    string GetString(GirModel.ReturnType returnType, string fromVariableName);
}
