﻿
using Generator.Model;

namespace Generator.Renderer.Public;

internal static class CallbackForeverHandler
{
    public static string Render(GirModel.Callback callback)
    {
        var handlerName = Callback.GetForeverHandlerName(callback);

        return $@"
using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

#nullable enable

namespace {Namespace.GetPublicName(callback.Namespace)}
{{
    // AUTOGENERATED FILE - DO NOT MODIFY

    /// <summary>
    /// Forever Handler for {callback.Name}. An forever annotation indicates the closure will
    /// be valid until the process terminates. Therefor the object will never be freed.
    /// </summary>
    {PlatformSupportAttribute.Render(callback as GirModel.PlatformDependent)}
    public class {handlerName}
    {{
        private {callback.Name} managedCallback;
        private GCHandle gch;

        public {Namespace.GetInternalName(callback.Namespace)}.{callback.Name} NativeCallback;
    
        public {handlerName}({callback.Name} managed)
        {{
            managedCallback = managed;
            gch = GCHandle.Alloc(this);
            {CallbackCommonHandlerRenderUtils.RenderNativeCallback(callback, GirModel.Scope.Forever)}
        }}
    }}
}}";
    }
}
