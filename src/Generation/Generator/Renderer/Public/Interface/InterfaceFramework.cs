﻿using Generator.Model;

namespace Generator.Renderer.Public;

internal static class InterfaceFramework
{
    public static string Render(GirModel.Interface iface)
    {
        return $@"
using System;
using GObject;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

#nullable enable

namespace {Namespace.GetPublicName(iface.Namespace)}
{{
    // AUTOGENERATED FILE - DO NOT MODIFY

    {PlatformSupportAttribute.Render(iface as GirModel.PlatformDependent)}
    public partial interface {iface.Name} : GLib.IHandle
    {{
        IntPtr GLib.IHandle.Handle
        {{
            get
            {{
                System.Diagnostics.Debug.Assert(
                    condition: GetType().IsAssignableTo(typeof(GObject.Object)),
                    message: $""Interface '{{nameof({iface.Name})}}' can only be implemented on GObject-based classes""
                );

                return ((GObject.Object)this).Handle;
            }}
        }}
    }}
}}";
    }
}
