namespace BookHub.Tests.Infrastructure.Extensions;

using BookHub.Data.Models.Base;
using BookHub.Infrastructure.Extensions;
using Xunit;

public class DbQueryExtensionsTests
{
    [Fact]
    public void ApplyIsDeletedFilter_ReturnsOnlyNotDeletedEntities()
    {
        var entities = new[]
        {
            new TestEntity { Id = 1, IsDeleted = false, Name = "Active 1" },
            new TestEntity { Id = 2, IsDeleted = true, Name = "Deleted" },
            new TestEntity { Id = 3, IsDeleted = false, Name = "Active 2" }
        }.AsQueryable();

        var result = entities.ApplyIsDeletedFilter().ToList();

        Assert.Equal(2, result.Count);
        Assert.All(result, e => Assert.False(e.IsDeleted));
        Assert.Contains(result, e => e.Id == 1);
        Assert.Contains(result, e => e.Id == 3);
    }

    [Fact]
    public void ApplyIsDeletedFilter_OnEmptySequence_ReturnsEmpty()
    {
        var entities = Enumerable.Empty<TestEntity>().AsQueryable();

        var result = entities.ApplyIsDeletedFilter().ToList();

        Assert.Empty(result);
    }

    private sealed class TestEntity : DeletableEntity<int>
    {
        public string Name { get; set; } = string.Empty;
    }
}
