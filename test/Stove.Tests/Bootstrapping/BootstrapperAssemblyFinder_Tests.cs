﻿using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Shouldly;

using Stove.Bootstrapping;
using Stove.Reflection;
using Stove.TestBase;

using Xunit;

namespace Stove.Tests.Bootstrapping
{
    public class BootstrapperAssemblyFinder_Tests : TestBaseWithLocalIocResolver
    {
        [Fact]
        public void Should_Get_Bootstrapper_And_Additional_Assemblies()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            Building(builder =>
                {
                    builder
                        .UseStoveWithNullables(typeof(MyStartupBootstrapper))
                        .RegisterServices(r => r.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly()));
                })
                .Ok();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            var sut = The<StoveAssemblyFinder>();

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            List<Assembly> assemblies = sut.GetAllAssemblies();

            assemblies.Count.ShouldBe(3);
            assemblies.Any(a => a == typeof(MyStartupBootstrapper).Assembly).ShouldBeTrue();
            assemblies.Any(a => a == typeof(StoveKernelBootstrapper).Assembly).ShouldBeTrue();
            assemblies.Any(a => a == typeof(FactAttribute).Assembly).ShouldBeTrue();
        }

        public class MyStartupBootstrapper : StoveBootstrapper
        {
            public override Assembly[] GetAdditionalAssemblies()
            {
                return new[] { typeof(FactAttribute).Assembly };
            }
        }
    }
}
