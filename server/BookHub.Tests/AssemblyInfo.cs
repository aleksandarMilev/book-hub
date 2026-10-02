using Xunit.Sdk;
using Xunit.v3;

// xunit.v3 4.x replaced CollectionBehavior(DisableTestParallelization = true) with this.
[assembly: Parallelization(Mode = ParallelMode.None)]
