﻿using System;
using GLib;

namespace GdkPixbuf
{
    public partial class PixbufLoader
    {
        public static Pixbuf FromBytes(byte[] data)
        {
            IntPtr handle = Native.Methods.New();

            try
            {
                using var bytes = Bytes.From(data);

                Native.write_bytes(handle, bytes.Handle, out IntPtr error);
                Error.ThrowOnError(error);

                Native.close(handle, out error);
                Error.ThrowOnError(error);

                return new Pixbuf(Native.Methods.GetPixbuf(handle), false);
            }
            finally
            {
                GObject.Object.Native.unref(handle);
            }
        }
    }
}
