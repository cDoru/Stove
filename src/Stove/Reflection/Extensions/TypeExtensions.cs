﻿using System;
using System.Collections.Generic;
using System.Reflection;

using FluentAssemblyScanner;

using Stove.IO;

namespace Stove.Reflection.Extensions
{
    public static class TypeExtensions
    {
        public static IList<Type> AssignedTypesInAssembly(this Type @this, Assembly assembly)
        {
            return AssemblyScanner.FromAssembly(assembly)
                                  .BasedOn(@this)
                                  .Filter()
                                  .Classes()
                                  .NonStatic()
                                  .Scan();
        }

        public static IList<Type> AssignedTypes(this Type @this)
        {
            return AssemblyScanner.FromAssemblyInDirectory(new AssemblyFilter(AppDomain.CurrentDomain.GetActualDomainPath()))
                                  .IncludeNonPublicTypes()
                                  .BasedOn(@this)
                                  .Filter()
                                  .Classes()
                                  .NonStatic()
                                  .Scan();
        }
    }
}
