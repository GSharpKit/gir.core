﻿namespace GirLoader.Output
{
    public partial class SingleParameter : Symbol, Parameter
    {
        public TypeReference TypeReference { get; }
        public Direction Direction { get; }
        public Transfer Transfer { get; }
        public bool Nullable { get; }
        public bool CallerAllocates { get; }
        public int? ClosureIndex { get; }
        public int? DestroyIndex { get; }
        public Scope CallbackScope { get; }
        string Parameter.Name => OriginalName;

        public SingleParameter(string originalName, TypeReference typeReference, Direction direction, Transfer transfer, bool nullable, bool callerAllocates, int? closureIndex, int? destroyIndex, Scope scope) : base(originalName)
        {
            TypeReference = typeReference;
            Direction = direction;
            Transfer = transfer;
            Nullable = nullable;
            CallerAllocates = callerAllocates;
            ClosureIndex = closureIndex;
            DestroyIndex = destroyIndex;
            CallbackScope = scope;
        }
    }
}
