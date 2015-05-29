using System;
using System.Diagnostics;
using System.Reflection;
using Autofac;
using Kurdle.Generation;
using Kurdle.Misc;
using Kurdle.Server;
using Yaclops;


namespace Kurdle
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var container = CreateContainer();

                var parser = container.Resolve<CommandLineParser>();

                var command = parser.Parse(args);

                command.Execute();
            }
            catch (CommandLineParserException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (ProjectException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unhandled exception in main.");
                Console.WriteLine(ex);
            }


            if (Debugger.IsAttached)
            {
                Console.Write("<press ENTER to continue>");
                Console.ReadLine();
            }
        }



        private static IContainer CreateContainer()
        {
            ContainerBuilder builder = new ContainerBuilder();

            // Services; consider switching to auto-register scheme if we get too many
            builder.RegisterType<ProjectInfo>().As<IProjectInfo>();
            builder.RegisterType<PageGeneratorFactory>().As<IPageGeneratorFactory>();
            builder.RegisterType<SiteGenerator>().As<ISiteGenerator>();
            builder.RegisterType<SimpleServer>().As<ISimpleServer>();
            builder.RegisterType<ChangeMonitor>().As<IChangeMonitor>();

            // Command-line specific stuff
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .Where(t => typeof(ISubCommand).IsAssignableFrom(t) && t.IsPublic)
                .SingleInstance()
                .As<ISubCommand>();

            builder.RegisterType<CommandLineParser>();

            return builder.Build();
        }
    }
}
