﻿using System;
using System.Collections.Generic;
using System.Linq;
using Generator.Model;

namespace Generator.Renderer.Public;

internal static class Constants
{
    public static string Render(IEnumerable<GirModel.Constant> constants)
    {
        return $@"
using System;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

#nullable enable

namespace {Namespace.GetPublicName(constants.First().Namespace)}
{{
    // AUTOGENERATED FILE - DO NOT MODIFY

    public partial class Constants
    {{
        {constants
            .Select(Render)
            .Join(Environment.NewLine)}
    }}
}}";
    }

    private static string Render(GirModel.Constant constant)
    {
        var renderableConstant = RenderableConstantFactory.Create(constant);

        return @$"
{PlatformSupportAttribute.Render(constant as GirModel.PlatformDependent)}
public static {renderableConstant.Type} {renderableConstant.Name} = {renderableConstant.Value};";
    }
}
