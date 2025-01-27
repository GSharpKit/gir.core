﻿using Generator.Model;

namespace Generator.Renderer.Public;

internal static class RecordClass
{
    public static string Render(GirModel.Record record)
    {
        var name = Record.GetPublicClassName(record);
        var internalHandleName = Record.GetInternalHandleName(record);

        return $@"
using System;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

#nullable enable

namespace {Namespace.GetPublicName(record.Namespace)}
{{
    // AUTOGENERATED FILE - DO NOT MODIFY

    {PlatformSupportAttribute.Render(record as GirModel.PlatformDependent)}
    public partial class {name} : GLib.IHandle
    {{
        private readonly Internal.{internalHandleName} _handle;

        public Internal.{internalHandleName} Handle => !_handle.IsInvalid ? _handle : throw new Exception(""Invalid Handle"");
        IntPtr GLib.IHandle.Handle => _handle.DangerousGetHandle();

        // Override this to perform additional steps in the constructor
        partial void Initialize();
        
        public {name}(Internal.{internalHandleName} handle)
        {{
            _handle = handle;
            Initialize();
        }}

        //TODO: This is a workaround constructor as long as we are
        //not having https://github.com/gircore/gir.core/issues/397
        private {name}(IntPtr ptr, bool ownsHandle) : this(ownsHandle
            ? new Internal.{Record.GetInternalOwnedHandleName(record)}(ptr)
            : new Internal.{Record.GetInternalUnownedHandleName(record)}(ptr)){{ }}

        // TODO: Default Constructor (allocate in managed memory and free on Dispose?)
        // We need to be able to create instances of records with full access to
        // fields, e.g. Gdk.Rectangle, Gtk.TreeIter, etc. 
        
        // TODO: Implement IDispose and free safe handle
    }}
}}";
    }
}
