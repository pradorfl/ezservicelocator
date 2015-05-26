# ezservicelocator
A lightweight and very simple service locator with dependency injection libary for C#.

# Introduction
You need a service locator with automated dependency injection support? 
No need for heavyweight IoC Containers with so many options and functionalities?
This libary provides exactly this!!!

#Sample mapping class with constructor dependency injection

public class TestAppMap : ServiceMap
{
    public override void Load()
    {
        For<INavigationRepository>().Use<NavigationCassandraRepository>();
        For<ITracker>().Use<Tracking>(new ConstructorDependency(typeof(INavigationRepository)));
    }
}
