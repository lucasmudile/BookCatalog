using BookCatalog.Application.Mappings;

namespace BookCatalog.Tests.Unit.Helpers;
public abstract class TestBase
{
    static TestBase()
    {
        MapsterConfig.Configure();
    }
}