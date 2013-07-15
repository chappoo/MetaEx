namespace MetaEx.Tests
{
    using NUnit.Framework;

    using ServiceStack.Text;

    public class TestObjectWithMetaData : IMeta
    {
        public string Meta { get; set; }
    }

    public class ThingWithLegs
    {
        public int NumberOfLegs { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public class ThingWithArms
    {
        public int NumberOfArms { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    [TestFixture]
    public class MetaTests
    {
        [Test]
        public void GetMetaDictionary()
        {
            var testObj = new TestObjectWithMetaData();
            var dictionary = testObj.GetMetaDictionary();
            Assert.That(dictionary, Is.Not.Null);
            Assert.That(dictionary.Count, Is.EqualTo(0));
            Assert.That(testObj.Meta, Is.EqualTo("{}"));
        }

        [Test]
        public void GetMetaDataInstanceOnNewObjectIsNull()
        {
            var testObj = new TestObjectWithMetaData();

            var metaData = testObj.GetMeta<ThingWithLegs>();

            Assert.That(metaData, Is.Null);
        }

        [Test]
        public void SetMetaDataInstanceOnNewObject()
        {
            var testObj = new TestObjectWithMetaData();
            testObj.SetMeta<ThingWithLegs>(new ThingWithLegs());

            var metaData = testObj.GetMeta<ThingWithLegs>();

            Assert.That(metaData, Is.Not.Null);
            Assert.That(testObj.Meta, Is.EqualTo("{ThingWithLegs:\"{NumberOfLegs:0}\"}"));


            metaData.FirstName = "Frank";
            metaData.LastName = "Sinatra";
            metaData.NumberOfLegs = 2;

            testObj.SetMeta(metaData);

            Assert.That(testObj.Meta, Is.EqualTo("{ThingWithLegs:\"{NumberOfLegs:2,FirstName:Frank,LastName:Sinatra}\"}"));
            Assert.That(testObj.GetMeta<ThingWithLegs>().FirstName, Is.EqualTo("Frank"));
            Assert.That(testObj.GetMeta<ThingWithLegs>().LastName, Is.EqualTo("Sinatra"));
            Assert.That(testObj.GetMeta<ThingWithLegs>().NumberOfLegs, Is.EqualTo(2));

            testObj.SetMeta(new ThingWithArms { FirstName = "Nancy", LastName = "Sinatra", NumberOfArms = 2 });

            Assert.That(testObj.Meta, Is.EqualTo("{ThingWithLegs:\"{NumberOfLegs:2,FirstName:Frank,LastName:Sinatra}\",ThingWithArms:\"{NumberOfArms:2,FirstName:Nancy,LastName:Sinatra}\"}"));
            Assert.That(testObj.GetMeta<ThingWithLegs>().FirstName, Is.EqualTo("Frank"));
            Assert.That(testObj.GetMeta<ThingWithLegs>().LastName, Is.EqualTo("Sinatra"));
            Assert.That(testObj.GetMeta<ThingWithLegs>().NumberOfLegs, Is.EqualTo(2));
            Assert.That(testObj.GetMeta<ThingWithArms>().FirstName, Is.EqualTo("Nancy"));
            Assert.That(testObj.GetMeta<ThingWithArms>().LastName, Is.EqualTo("Sinatra"));
            Assert.That(testObj.GetMeta<ThingWithArms>().NumberOfArms, Is.EqualTo(2));
        }

        [Test]
        public void TestSerializingObjectPersistsMeta()
        {
            var testObj = new TestObjectWithMetaData();
            testObj.SetMeta(new ThingWithLegs { FirstName = "Frank", LastName = "Sinatra", NumberOfLegs = 2 });
            testObj.SetMeta(new ThingWithArms { FirstName = "Nancy", LastName = "Sinatra", NumberOfArms = 2 });

            var testObjSerialized = TypeSerializer.SerializeToString(testObj);
            Assert.That(testObjSerialized, Is.Not.Empty);

            var testObjDeserialized = TypeSerializer.DeserializeFromString<TestObjectWithMetaData>(testObjSerialized);
            Assert.That(testObjDeserialized.GetMeta<ThingWithLegs>().FirstName, Is.EqualTo("Frank"));
            Assert.That(testObjDeserialized.GetMeta<ThingWithLegs>().LastName, Is.EqualTo("Sinatra"));
            Assert.That(testObjDeserialized.GetMeta<ThingWithLegs>().NumberOfLegs, Is.EqualTo(2));
            Assert.That(testObjDeserialized.GetMeta<ThingWithArms>().FirstName, Is.EqualTo("Nancy"));
            Assert.That(testObjDeserialized.GetMeta<ThingWithArms>().LastName, Is.EqualTo("Sinatra"));
            Assert.That(testObjDeserialized.GetMeta<ThingWithArms>().NumberOfArms, Is.EqualTo(2));
        }
    }
}
