﻿using System;
using System.Collections.Generic;
using Generator.Model;

namespace Generator.Renderer.Public.ParameterExpressions.ToNative;

internal class Enumeration : ToNativeParameterConverter
{
    public bool Supports(GirModel.AnyType type)
        => type.Is<GirModel.Enumeration>();

    public void Initialize(ParameterToNativeData parameter, IEnumerable<ParameterToNativeData> _)
    {
        if (parameter.Parameter.Direction != GirModel.Direction.In)
            throw new NotImplementedException($"{parameter.Parameter.AnyType}: Enumeration with direction != in not yet supported");

        //We don't need any conversion for enumerations
        var parameterName = Parameter.GetName(parameter.Parameter);
        parameter.SetSignatureName(parameterName);
        parameter.SetCallName(parameterName);
    }
}
