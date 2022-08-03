using System;
using FluentAssertions;
using Kurdle.Commands;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Kurdle.Tests
{
    public class ContainerTests
    {
        [Theory]
        [InlineData(typeof(BuildCommand))]
        [InlineData(typeof(CreateCommand))]
        public void CanResolveCommand(Type type)
        {
            // Arrange
            var services = new ServiceCollection();

            services.RegisterServices();

            // Act
            object command;
            using (var container = services.BuildServiceProvider())
            {
                command = container.GetRequiredService(type);
            }

            // Assert
            command.Should().NotBeNull();
        }
    }
}
