﻿using Generator.Model;

namespace Generator.Renderer.Internal;

internal static class BitfieldParameter
{
    public static RenderableParameter Create(GirModel.Parameter parameter)
    {
        return new RenderableParameter(
            Attribute: string.Empty,
            Direction: GetDirection(parameter),
            NullableTypeName: GetNullableTypeName(parameter),
            Name: Parameter.GetName(parameter)
        );
    }

    private static string GetNullableTypeName(GirModel.Parameter parameter) => parameter.IsPointer switch
    {
        true => Type.Pointer,
        //Internal does not define any bitfields. They are part of the Public API to avoid converting between them.
        false => ComplexType.GetFullyQualified((GirModel.Bitfield) parameter.AnyType.AsT0)
    };

    private static string GetDirection(GirModel.Parameter parameter) => parameter switch
    {
        { Direction: GirModel.Direction.InOut } => ParameterDirection.Ref(),
        { Direction: GirModel.Direction.Out, CallerAllocates: true } => ParameterDirection.Ref(),
        { Direction: GirModel.Direction.Out } => ParameterDirection.Out(),
        _ => ParameterDirection.In()
    };
}
