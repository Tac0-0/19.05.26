using Doner.Controller;
using Doner.Data.Entities;
using Doner.Test.Helpers;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Doner.Test.Services;

public class JsonControllerTests
{
    [Test]
    public async Task ExportAllWritesEveryDatasetAndUserDiscriminators()
    {
        var factory = new ControllerTestFactory();
        await factory.SeedAsync();
        string path = Path.GetTempFileName();
        try
        {
            await new JsonController(factory.CreateContext).ExportAll(path);
            string json = await File.ReadAllTextAsync(path);
            using JsonDocument document = JsonDocument.Parse(json);
            JsonElement root = document.RootElement;

            Assert.That(root.GetProperty("Users").GetArrayLength(), Is.EqualTo(3));
            Assert.That(root.GetProperty("UserAddresses").GetArrayLength(), Is.EqualTo(1));
            Assert.That(root.GetProperty("Categories").GetArrayLength(), Is.EqualTo(1));
            Assert.That(root.GetProperty("Products").GetArrayLength(), Is.EqualTo(1));
            Assert.That(root.GetProperty("Orders").GetArrayLength(), Is.EqualTo(1));
            Assert.That(root.GetProperty("OrderDetails").GetArrayLength(), Is.EqualTo(1));
            Assert.That(root.GetProperty("Payments").GetArrayLength(), Is.EqualTo(1));
            Assert.That(root.GetProperty("Deliveries").GetArrayLength(), Is.EqualTo(1));
            Assert.That(root.GetProperty("Suppliers").GetArrayLength(), Is.EqualTo(1));
            Assert.That(root.GetProperty("Ingredients").GetArrayLength(), Is.EqualTo(1));
            Assert.That(root.GetProperty("ProductIngredients").GetArrayLength(), Is.EqualTo(1));
            string[] discriminators = root.GetProperty("Users").EnumerateArray().Select(x => x.GetProperty("Discriminator").GetString()!).ToArray();
            Assert.That(discriminators, Is.EquivalentTo(new[] { "Customers", "Employees", "Admins" }));
        }
        finally
        {
            File.Delete(path);
        }
    }

    [TestCase("{\"Discriminator\":\"admins\",\"Role\":1}", typeof(Admins))]
    [TestCase("{\"Discriminator\":\"employees\",\"Role\":2,\"EmployeePosition\":\"Cook\",\"Salary\":12.5,\"HireDate\":\"2026-01-01\"}", typeof(Employees))]
    [TestCase("{\"Discriminator\":\"customers\",\"Role\":0}", typeof(Customers))]
    [TestCase("{\"Role\":1}", typeof(Admins))]
    [TestCase("{\"Role\":\"Employee\"}", typeof(Employees))]
    [TestCase("{}", typeof(Customers))]
    public void UsersJsonConverterReadsDiscriminatorsAndRoleFallbacks(string json, Type expectedType)
    {
        Users user = JsonSerializer.Deserialize<Users>(json, OptionsWithUsersConverter())!;
        Assert.That(user, Is.TypeOf(expectedType));
    }


    [Test]
    public void ImportAllRejectsMissingFiles()
    {
        var factory = new ControllerTestFactory();
        string path = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}.json");

        Assert.That((Func<Task>)(async () => await new JsonController(factory.CreateContext).ImportAll(path)), Throws.TypeOf<FileNotFoundException>());
    }

    [TestCase("{\"Role\":[]}")]
    [TestCase("{\"Role\":\"not-a-role\"}")]
    public void UsersJsonConverterRejectsInvalidRoles(string json)
    {
        Assert.That((Action)(() => JsonSerializer.Deserialize<Users>(json, OptionsWithUsersConverter())), Throws.TypeOf<JsonException>());
    }

    [Test]
    public void ImportAllRejectsMalformedJson()
    {
        var factory = new ControllerTestFactory();
        string path = Path.GetTempFileName();
        try
        {
            File.WriteAllText(path, "{");
            Assert.That((Func<Task>)(async () => await new JsonController(factory.CreateContext).ImportAll(path)), Throws.TypeOf<JsonException>());
        }
        finally
        {
            File.Delete(path);
        }
    }

    [Test]
    public void ImportAllPropagatesProviderTransactionExceptions()
    {
        var factory = new ControllerTestFactory();
        string path = Path.GetTempFileName();
        try
        {
            File.WriteAllText(path, "{}");
            Assert.That((Func<Task>)(async () => await new JsonController(factory.CreateContext).ImportAll(path)), Throws.TypeOf<InvalidOperationException>());
        }
        finally
        {
            File.Delete(path);
        }
    }

    [Test]
    public void ImportAndExportPropagateFileAndContextExceptions()
    {
        var factory = new ControllerTestFactory();
        var controller = new JsonController(factory.CreateContext);
        string missingDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString(), "export.json");
        Assert.That((Func<Task>)(async () => await controller.ImportAll(null!)), Throws.TypeOf<ArgumentNullException>());
        Assert.That((Func<Task>)(async () => await controller.ExportAll(missingDirectory)), Throws.TypeOf<DirectoryNotFoundException>());

        string path = Path.GetTempFileName();
        try
        {
            File.WriteAllText(path, "{}");
            var failingController = new JsonController(ControllerExceptionAssertions.ThrowContextCreation);
            Assert.That((Func<Task>)(async () => await failingController.ImportAll(path)), Throws.TypeOf<InvalidOperationException>()
                .With.Message.EqualTo("context creation failed"));
            Assert.That((Func<Task>)(async () => await failingController.ExportAll(path)), Throws.TypeOf<InvalidOperationException>()
                .With.Message.EqualTo("context creation failed"));
        }
        finally
        {
            File.Delete(path);
        }
    }

    private static JsonSerializerOptions OptionsWithUsersConverter()
    {
        Type converterType = typeof(JsonController).GetNestedType("UsersJsonConverter", BindingFlags.NonPublic)!;
        var converter = (JsonConverter)Activator.CreateInstance(converterType, nonPublic: true)!;
        var options = new JsonSerializerOptions();
        options.Converters.Add(converter);
        return options;
    }
}
