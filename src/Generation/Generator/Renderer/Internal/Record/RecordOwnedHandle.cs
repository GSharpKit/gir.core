﻿using System.Collections.Generic;
using System.Linq;
using Generator.Model;

namespace Generator.Renderer.Internal;

internal static class RecordOwnedHandle
{
    public static string Render(GirModel.Record record)
    {
        var ownedHandleName = Record.GetInternalOwnedHandleName(record);
        var handleName = Record.GetInternalHandleName(record);

        return $@"
using System;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

#nullable enable

namespace {Namespace.GetInternalName(record.Namespace)}
{{
    // AUTOGENERATED FILE - DO NOT MODIFY

    {PlatformSupportAttribute.Render(record as GirModel.PlatformDependent)}
    public partial class {ownedHandleName} : {handleName}
    {{
        private {ownedHandleName}() : base(true) {{ }}

        public {ownedHandleName}(IntPtr handle) : base(true)
        {{
            SetHandle(handle);
        }}

        {RenderReleaseHandle(record)}

        {RenderFreeFunction(record)}
    }}
}}";
    }

    private static string RenderFreeFunction(GirModel.Record record)
    {
        var freeMethod = GetFreeOrUnrefMethod(record.Methods);

        return freeMethod is null
            ? ""
            : $@"[DllImport(ImportResolver.Library, EntryPoint = ""{freeMethod.CIdentifier}"")]
private static extern void {freeMethod.Name}(IntPtr value);";
    }

    private static string RenderReleaseHandle(GirModel.Record record)
    {
        return record.Foreign
            ? $@"
        // For foreign records, the release function must be manually implemented.
        protected override partial bool ReleaseHandle();"
            : $@"
        protected override bool ReleaseHandle()
        {{
            {RenderFreeCall(record)}
        }}";
    }

    private static string RenderFreeCall(GirModel.Record record)
    {
        var freeMethod = GetFreeOrUnrefMethod(record.Methods);

        return freeMethod is null
            ? $"throw new System.Exception(\"Can't free native handle of type \\\"{Namespace.GetInternalName(record.Namespace)}.{Record.GetInternalOwnedHandleName(record)}\\\".\");"
            : @$"{freeMethod.Name}(handle);
return true;";
    }

    private static GirModel.Method? GetFreeOrUnrefMethod(IEnumerable<GirModel.Method> methods)
        //Unref functions takes precedense over free function
        => methods.FirstOrDefault(function => function.IsUnref())
           ?? methods.FirstOrDefault(function => function.IsFree());
}
