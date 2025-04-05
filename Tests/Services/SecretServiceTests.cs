using NUnit.Framework;
using Services;

namespace Tests.Services;

[TestFixture]
public class SecretServiceTests
{
    private ISecretService _secretService = null!;

    [OneTimeSetUp]
    public void Setup()
    {
        _secretService = new SecretService();
    }

    [Test]
    public void GetSecret_ReturnsSecret()
    {
        Assert.That(_secretService.GetSecret(), Is.EqualTo("Secret from SecretService"));
    }
}
