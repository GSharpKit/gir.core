﻿using System;
using System.Text;
using Repository.Model;
using Type = Repository.Model.Type;

namespace Generator
{
    internal static class ArgumentExtension
    {
        public static string WriteNative(this Argument argument, Namespace currentNamespace)
            => argument.Write(Target.Native, currentNamespace);
        
        public static string WriteManaged(this Argument argument, Namespace currentNamespace)
            => argument.Write(Target.Managed, currentNamespace);

        private static string Write(this Argument argument, Target target,  Namespace currentNamespace)
        {
            var type = GetFullType(argument, target, currentNamespace);
            
            var builder = new StringBuilder();
            builder.Append(type);
            builder.Append(' ');
            builder.Append(target == Target.Native ? argument.NativeName : argument.ManagedName);

            return builder.ToString();
        }

        private static string GetFullType(this Argument argument, Target target, Namespace currentNamespace)
        {
            var attribute = GetAttribute(argument, target);
            var direction = GetDirection(argument);
            var type = GetType(argument, target, currentNamespace);

            return $"{attribute}{direction}{type}";
        }

        private static string GetAttribute(Argument argument, Target target)
        {
            if (target == Target.Managed)
                return "";
            
            var attribute = argument.Array.GetMarshallAttribute();
            
            if (attribute.Length > 0)
                attribute += " ";

            return attribute;
        }

        private static string GetDirection(Argument argument)
        {
            return argument.Direction switch
            {
                Direction.OutCalleeAllocates => "out ",
                Direction.OutCallerAllocates => "ref ",
                _ => ""
            };
        }

        private static string GetType(this Argument argument, Target target, Namespace currentNamespace) => target switch
        {
            Target.Managed => argument.WriteManagedType(currentNamespace) + GetNullable(argument),
            Target.Native => argument.WriteNativeType(currentNamespace),
            _ => throw new Exception($"Unknown {nameof(Target)}")
        };

        private static string GetNullable(Argument argument)
            => argument.Nullable ? "?" : string.Empty;
        
        internal static string WriteMarshalArgumentToManaged(this Argument arg, Namespace currentNamespace)
        {
            if (arg.ManagedName == "fields")
            {
                
            }
            
            // TODO: We need to support disguised structs (opaque types)
            var expression = (arg.SymbolReference.IsPointer, arg.SymbolReference.GetSymbol(), arg) switch
            {
                (true, Record r, { Array: null } a) => $"Marshal.PtrToStructure<{r.ManagedName}>({a.NativeName});",
                (true, Record r, { Array: {}} a) => $"{a.NativeName}.MarshalToStructure<{r.ManagedName}>();",
                (true, Class { IsFundamental: true} c, {Array: null} a) => $"{c.ManagedName}.From({a.NativeName});",
                (true, Class c, {Array: null} a) => $"Object.WrapHandle<{c.ManagedName}>({a.NativeName}, {a.Transfer.IsOwnedRef().ToString().ToLower()});",
                (true, Class c, {Array: {}}) => throw new NotImplementedException($"Cant create delegate for argument {arg.ManagedName}"),
                _ => $"({arg.WriteManagedType(currentNamespace)}){arg.NativeName};" // Other -> Try a brute-force cast
            };

            return $"{arg.WriteManagedType(currentNamespace)} {arg.ManagedName}Managed = " + expression;
        }
    }
}
