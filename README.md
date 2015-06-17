##Introduction
You need a service locator with automated dependency injection support? 
No need for heavyweight IoC Containers with so many options and functionalities?
This libary provides exactly this!!!

##Nuget package
```
PM> Install-Package EZServiceLocator
```

##Sample mapping class
```
public class TestAppMap : ServiceMap
{
    public override void Load()
    {
        For<INavigationRepository>().Use<NavigationCassandraRepository>();
        For<ITracker>().Use<Tracking>(new ConstructorDependency(typeof(INavigationRepository)));
    }
}
```

##Specific mapping
```
ServiceLocator.Current.For<INavigationRepository>().Use<NavigationMsSqlRepository>();
```

##Named instance mapping
```
ServiceLocator.Current.For<INavigationRepository>("sql").Use<NavigationMsSqlRepository>();
```

##Self mapping
```
ServiceLocator.Current.Use<TestAppMap>();
```

##Service get
```
ServiceLocator.Current.LoadServiceMap<TestAppMap>();
```

##Named instance get
```
ServiceLocator.Current.GetService<INavigationRepository>("sql");
```

##Upcoming updates
* Support to named instance injection
* Support to property injection
* JSON mapping
