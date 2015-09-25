using System;
using Funq;
using ServiceStack;

namespace SelfHost1
{
    [Route("/request")]
    public class Request : IReturn<Response>{}
    public class Response{}

    public class MyServices : Service
    {
        public object Any(Request request)
        {
            return new Response();
        }
    }

    public interface IDependency
    {
        Guid GetInstanceId();
    }

    public class SomeDependency : IDependency//, IDisposable
    {
        private readonly Guid _id = Guid.NewGuid();

        public Guid GetInstanceId()
        {
            return _id;
        }

        public void Dispose()
        {
            Console.WriteLine("Disposing: " + _id);
        }
    }

    public class AppHost : AppHostHttpListenerPoolBase
    {
        /// <summary>
        /// Default constructor.
        /// Base constructor requires a name and assembly to locate web service classes. 
        /// </summary>
        public AppHost()
            : base("SelfHost1", typeof(MyServices).Assembly)
        {
        }

        /// <summary>
        /// Application specific configuration
        /// This method should initialize any IoC resources utilized by your web service classes.
        /// </summary>
        /// <param name="container"></param>
        public override void Configure(Container container)
        {
            container.RegisterAs<SomeDependency, IDependency>().ReusedWithin(ReuseScope.Request);

            //if (container.TryResolve<IDependency>() != default(IDependency))
            //{
            //}

            GlobalRequestFilters.Add((req, res, requestDto) =>
            {
                var thatDependency = container.Resolve<IDependency>();
                Console.WriteLine("Filter received: " + thatDependency.GetInstanceId());
            });
        }
    }
}
