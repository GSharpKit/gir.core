﻿using GirModel;

namespace Generator.Renderer.Public.ReturnTypeExpression.ToManaged;

internal class PrimitiveValueTypeArray : ReturnTypeConverter
{
    public bool Supports(AnyType type)
        => type.IsArray<GirModel.PrimitiveValueType>();

    public string GetString(GirModel.ReturnType returnType, string fromVariableName)
        => fromVariableName;
}
