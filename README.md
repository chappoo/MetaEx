MetaEx
======

MetaEx-tensions.   Provides extensions for providing metadata off arbitrary POCO objects.  Based heavily on the @ServiceStack UserAuth Meta implementation (uses ServiceStack.Text).

###What for?###

I found myself working with an ORM (no names mentioned) and wanting to extend a table type object with various parameters.  Rather than keep writing database migration scripts and resyncing the (unmentioned) model every time we needed a new parameter, I opted to create a string field in the database and manage a `Dictionary<string,string>` approach with serialization, much like the simple implementation in @ServiceStack **UserAuth.Meta**.

Kudos to the guys at @ServiceStack for the core tech.

###Usage###

`IMeta` interface requires the Meta string field on the POCO.

    public class TestObjectWithMetaData : IMeta
    {
        public string Meta { get; set; }
    }

This hooks up a couple of extension methods on the POCO (via `IMeta`)

	public static T GetMeta<T>(this IMeta meta)
	public static void SetMeta<T>(this IMeta meta, T value)

Allowing for use such as

	var testObj = new TestObjectWithMetaData();
	testObj.SetMeta(new ThingWithArms { FirstName = "Nancy", LastName = "Sinatra", NumberOfArms = 2 });

Get the meta object via:

	var thingWithArms = testObj.GetMeta<ThingWithArms>();

You can save multiple meta objects (of different types):

	var testObj = new TestObjectWithMetaData();
	testObj.SetMeta(new ThingWithArms { FirstName = "Nancy", LastName = "Sinatra", NumberOfArms = 2 });
	testObj.SetMeta(new ThingWithLegs { FirstName = "Frank", LastName = "Sinatra", NumberOfLegs = 2 });

and get them:

	var thingWithArms = testObj.GetMeta<ThingWithArms>();
	var thingWithLegs = testObj.GetMeta<ThingWithLegs>();

###Dependencies###

    <packages>
      <package id="NUnit" version="2.6.2" /> (for unit test project)
      <package id="ServiceStack.Text" version="3.9.55" />
    </packages>