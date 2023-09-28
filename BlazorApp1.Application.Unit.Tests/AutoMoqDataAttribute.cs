using AutoFixture;
using AutoFixture.AutoMoq;

namespace BlazorApp1.Application.Unit.Tests;

public class AutoMoqDataAttribute : AutoDataAttribute
{
    public AutoMoqDataAttribute() : base(Factory())
    {
    }

    private static Func<IFixture> Factory()
    {
        return () =>
        {
            var fixture = new Fixture();

            fixture.Customize<DateOnly>(composer => composer.FromFactory<DateTime>(DateOnly.FromDateTime));

            return fixture.Customize(new AutoMoqCustomization());
        };
    }
}