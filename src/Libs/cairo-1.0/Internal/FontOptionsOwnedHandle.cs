﻿namespace Cairo.Internal
{
    public partial class FontOptionsOwnedHandle : FontOptionsHandle
    {
        protected override partial bool ReleaseHandle()
        {
            FontOptions.Destroy(handle);
            return true;
        }
    }
}

