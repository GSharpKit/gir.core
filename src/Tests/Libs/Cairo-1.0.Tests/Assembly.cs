﻿using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Cairo.Tests;

[TestClass]
public static class Assembly
{
    [AssemblyInitialize]
    public static void Initialize(TestContext context)
    {
        Module.Initialize();
    }
}
