﻿using GirModel;

namespace Generator.Renderer.Public.ReturnTypeExpression.ToNative;

internal class PrimitiveValueType : ReturnTypeConverter
{
    public bool Supports(AnyType type)
        => type.Is<GirModel.PrimitiveValueType>();

    public string GetString(GirModel.ReturnType returnType, string fromVariableName)
        => fromVariableName;
}
