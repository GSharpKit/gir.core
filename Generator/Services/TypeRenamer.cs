﻿using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Repository;
using Repository.Model;

namespace Generator.Services
{
    internal class TypeRenamer
    {
        public void SetMetadata(IEnumerable<Namespace> namespaces)
        {
            foreach (Namespace ns in namespaces)
            {
                SetRecordMetadata(ns.Records);
                SetRecordFieldsCallbackMetadata(ns.Records);
                SetUnionMetadata(ns.Unions);
                SetCallbacksMetadata(ns.Callbacks);
            }
            
            Log.Information("Metadata set.");
        }

        private void SetRecordFieldsCallbackMetadata(IEnumerable<Record> records)
        {
            foreach (var record in records)
            {
                foreach (var callback in record.Fields.Select(x => x.Callback))
                {
                    if(callback is {})
                        SetCallbackMetadata(callback);   
                }
            }
        }

        private void SetCallbacksMetadata(IEnumerable<Callback> callbacks)
        {
            foreach(var callback in callbacks)
                SetCallbackMetadata(callback);
        }

        private void SetCallbackMetadata(Callback callback)
        {
            callback.Metadata["ManagedName"] = callback.SymbolName;
            callback.SymbolName = new SymbolName(callback.SymbolName + "Callback");
        }

        private void SetUnionMetadata(IEnumerable<Union> unions)
        {
            foreach(var union in unions)
                SetUnionMetadata(union);
        }
        
        private void SetUnionMetadata(Union union)
        {
            union.Metadata["UnionName"] = union.SymbolName;
            union.Metadata["PureName"] = "Struct";

            union.SymbolName = new SymbolName($"{union.SymbolName}.Struct");
        }
        
        private void SetRecordMetadata(IEnumerable<Record> records)
        {
            foreach (var record in records)
            {
                if (record.GLibClassStructFor is { })
                    SetClassStructMetadata(record);
                else
                    SetRecordMetadata(record);
            }
        }

        private void SetClassStructMetadata(Record record)
        {
            Debug.Assert(record.GLibClassStructFor is not null);
            
            var className = record.GLibClassStructFor.GetSymbol().SymbolName;
            record.Metadata["ClassName"] = className;
            record.Metadata["PureName"] = "Class";
            record.Metadata["SafeHandleName"] = "ClassSafeHandle";
            record.Metadata["SafeHandleRefName"] = "ClassSafeHandle";
            
            record.SymbolName = new SymbolName($"{className}.Native.Class");
        }

        private void SetRecordMetadata(Record record)
        {
            record.Metadata["RecordName"] = record.SymbolName;
            record.Metadata["PureName"] = "Struct";
            record.Metadata["SafeHandleName"] = record.SymbolName + "SafeHandle";
            record.Metadata["SafeHandleRefName"] = $"{record.SymbolName}.Native.{record.SymbolName }SafeHandle";

            record.SymbolName = new SymbolName($"{record.SymbolName}.Native.Struct");
        }
    }
}
