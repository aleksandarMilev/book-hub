using Xunit.Sdk;
using Xunit.v3;

// Test classes run in parallel (each class is its own collection); the tests inside a class run
// one after another. MaxThreads is pinned so a local run behaves like CI's 4-core runner.
// Shared state between classes is limited to PostgresServer (see the rules in CLAUDE.md).
[assembly: Parallelization(Mode = ParallelMode.Collections, MaxThreads = 4)]
