using Autofac;
using CoderBuddy.Actions.Available;

namespace CoderBuddy.Actions
{
    public sealed class ActionsModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(ThisAssembly)
                .Where(t => t.IsAssignableTo<ActionBase>() && t != typeof(HelpAction))
                .As<ActionBase>()
                .AsSelf()
                .InstancePerRequest();

            builder.RegisterType<HelpAction>().AsSelf().InstancePerRequest();
        }
    }
}