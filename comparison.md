# Comparing Things
## Using Bogus
### The Faker
```csharp
Randomizer.Seed = new Random(3897234);
var fruit = new[] { "apple", "banana", "orange", "strawberry", "kiwi" };
var orderIds = 0;
var testOrders = new Faker<Order>()
   .StrictMode(true)
   .RuleFor(o => o.OrderId, f => orderIds++)
   .RuleFor(o => o.Item, f => f.PickRandom(fruit))
   .RuleFor(o => o.Quantity, f => f.Random.Number(1, 10))
   .RuleFor(o => o.LotNumber, f => f.Random.Int(0, 100).OrNull(f, .8f));
var userIds = 0;
var testUsers = new Faker<User>()
   .CustomInstantiator(f => new User(userIds++, f.Random.Replace("###-##-####")))
   .RuleFor(u => u.FirstName, f => f.Name.FirstName())
   .RuleFor(u => u.LastName, f => f.Name.LastName())
   .RuleFor(u => u.Avatar, f => f.Internet.Avatar())
   .RuleFor(u => u.UserName, (f, u) => f.Internet.UserName(u.FirstName, u.LastName))
   .RuleFor(u => u.Email, (f, u) => f.Internet.Email(u.FirstName, u.LastName))
   .RuleFor(u => u.SomethingUnique, f => $"Value {f.UniqueIndex}")
   .RuleFor(u => u.SomeGuid, Guid.NewGuid)
   .RuleFor(u => u.Gender, f => f.PickRandom<Gender>())
   .RuleFor(u => u.CartId, f => Guid.NewGuid())
   .RuleFor(u => u.FullName, (f, u) => u.FirstName + " " + u.LastName)
   .RuleFor(u => u.Orders, f => testOrders.Generate(3))
   .FinishWith((f, u) => $"User Created! Name={u.Id}".PulseToLog(logFile));
```
### Execution
```csharp
TheFaker().Generate(3).PulseToLog(logFile);
```
### Result
```bash
"User Created! Name=0"
"User Created! Name=1"
"User Created! Name=2"
[
    {
        Id: 0,
        FirstName: "Laila",
        LastName: "Luettgen",
        FullName: "Laila Luettgen",
        UserName: "Laila99",
        Email: "Laila52@gmail.com",
        SomethingUnique: "Value 0",
        SomeGuid: 1e1b793b-23c8-40a9-b2fa-4900a830ef1a,
        Avatar: "https://ipfs.io/ipfs/Qmd3W5DuhgHirLHGVixi6V76LhCkZUz6pnFt5AJBiyvHye/avatar/434.jpg",
        CartId: bd01a88e-7424-495f-af5b-c9d6cda34cf1,
        SSN: "485-67-8799",
        Gender: Male,
        Orders: [
            {
                OrderId: 0,
                Item: "banana",
                Quantity: 4,
                LotNumber: null
            },
            {
                OrderId: 1,
                Item: "apple",
                Quantity: 7,
                LotNumber: null
            },
            {
                OrderId: 2,
                Item: "apple",
                Quantity: 9,
                LotNumber: 22
            }
        ]
    },
    {
        Id: 1,
        FirstName: "Heidi",
        LastName: "Franecki",
        FullName: "Heidi Franecki",
        UserName: "Heidi_Franecki88",
        Email: "Heidi.Franecki@hotmail.com",
        SomethingUnique: "Value 4",
        SomeGuid: a5c9dbf3-1f09-4113-90ee-dbc64baf57a2,
        Avatar: "https://ipfs.io/ipfs/Qmd3W5DuhgHirLHGVixi6V76LhCkZUz6pnFt5AJBiyvHye/avatar/277.jpg",
        CartId: e4ad3cfe-4082-4511-9ef3-419d353b2e51,
        SSN: "244-04-1171",
        Gender: Male,
        Orders: [
            {
                OrderId: 3,
                Item: "apple",
                Quantity: 6,
                LotNumber: null
            },
            {
                OrderId: 4,
                Item: "kiwi",
                Quantity: 1,
                LotNumber: null
            },
            {
                OrderId: 5,
                Item: "kiwi",
                Quantity: 8,
                LotNumber: null
            }
        ]
    },
    {
        Id: 2,
        FirstName: "Yoshiko",
        LastName: "Sanford",
        FullName: "Yoshiko Sanford",
        UserName: "Yoshiko.Sanford17",
        Email: "Yoshiko78@hotmail.com",
        SomethingUnique: "Value 8",
        SomeGuid: 8c38690a-5369-4b8c-9f5b-ba4292a4c49b,
        Avatar: "https://ipfs.io/ipfs/Qmd3W5DuhgHirLHGVixi6V76LhCkZUz6pnFt5AJBiyvHye/avatar/434.jpg",
        CartId: cc740c5c-db6c-49e5-b5aa-fe91b3498040,
        SSN: "870-30-8159",
        Gender: Male,
        Orders: [
            {
                OrderId: 6,
                Item: "strawberry",
                Quantity: 1,
                LotNumber: null
            },
            {
                OrderId: 7,
                Item: "banana",
                Quantity: 4,
                LotNumber: null
            },
            {
                OrderId: 8,
                Item: "apple",
                Quantity: 4,
                LotNumber: null
            }
        ]
    }
]
```
## Using Reactor
### The Fuzzr
```csharp
var orderFuzzr =
    from _1 in Configr<Order>.Property(a => a.OrderId, Fuzzr.Counter("order-id"))
    from _2 in Configr<Order>.Property(a => a.Item, Fuze.Fruit)
    from _3 in Configr<Order>.Property(a => a.Quantity, Fuzzr.Int(1, 10))
    from _4 in Configr<Order>.Property(a => a.LotNumber, Fuzzr.Int(0, 99).Nullable(0.8))
    from order in Fuzzr.One<Order>()
    select order;
var ssnFuzzr =
    from a in Fuzzr.Int(100, 999)
    from b in Fuzzr.Int(10, 99)
    from c in Fuzzr.Int(1000, 9999)
    select $"{a}-{b}-{c}";
var userFuzzr =
    from info in Fuze<User>.With(new PersonalInfo())
    from _1 in Configr<User>.Ignore(a => a.Id)
    from _2 in Configr<User>.Ignore(a => a.SSN)
    from _3 in Configr<User>.Property(a => a.Avatar, Fuze.Avatar)
    from _4 in Configr<User>.Property(a => a.SomethingUnique, Fuzzr.String().Unique("something"))
    from _5 in Configr<User>.Property(a => a.Gender, info.IsMale ? Gender.Male : Gender.Female)
    from _6 in Configr<User>.Property(a => a.Orders, orderFuzzr.Many(3))
    from id in Fuzzr.Counter("user-id")
    from ssn in ssnFuzzr
    from user in Fuzzr.One(() => new User(id, ssn)).Apply(a => $"User Created! Id={a.Id}".PulseToLog(logFile))
    select user;
```
### Execution
```csharp
TheFuzzr().Many(3).Generate(8675309).PulseToLog(logFile);
```
### Result
```bash
"User Created! Id=1"
"User Created! Id=2"
"User Created! Id=3"
[
    {
        Id: 1,
        FirstName: "Adrienne",
        LastName: "Pittman",
        FullName: "Adrienne Pittman",
        UserName: "Adrienne_Pittman13",
        Email: "adrienne.pittman@gmx.dev",
        SomethingUnique: "gnyrftrsqh",
        SomeGuid: 3c8079c4-aff3-46e2-8a80-8d16ffaa4e61,
        Avatar: "https://grovotor.com/userimage/8136825/6876c00852d444e0a90ad2beffb66e6b.jpeg?size=256",
        CartId: 0f175ea0-f542-4ab9-a5c8-f14a2982300b,
        SSN: "286-68-6238",
        Gender: Male,
        Orders: [
            {
                OrderId: 1,
                Item: "Surinam Cherry",
                Quantity: 7,
                LotNumber: null
            },
            {
                OrderId: 2,
                Item: "Black Currant",
                Quantity: 8,
                LotNumber: null
            },
            {
                OrderId: 3,
                Item: "Persimmon",
                Quantity: 6,
                LotNumber: null
            }
        ]
    },
    {
        Id: 2,
        FirstName: "Brian",
        LastName: "Zhang",
        FullName: "Brian Zhang",
        UserName: "Brian_Zhang48",
        Email: "brian.zhang@icloud.com",
        SomethingUnique: "bi",
        SomeGuid: 07bfc98d-4e41-485b-afcc-24c2786c2fe7,
        Avatar: "https://grovotor.com/userimage/5706381/7693e4f19fb54e1ab97e278e308fab5b.jpeg?size=256",
        CartId: 6fccdc96-7e56-4a69-a12d-abe309f19490,
        SSN: "298-28-6130",
        Gender: Female,
        Orders: [
            {
                OrderId: 4,
                Item: "Jocote",
                Quantity: 9,
                LotNumber: null
            },
            {
                OrderId: 5,
                Item: "Bearberry",
                Quantity: 9,
                LotNumber: null
            },
            {
                OrderId: 6,
                Item: "Gac Fruit",
                Quantity: 3,
                LotNumber: null
            }
        ]
    },
    {
        Id: 3,
        FirstName: "Isaac",
        LastName: "Rojas",
        FullName: "Isaac Rojas",
        UserName: "Isaac_Rojas15",
        Email: "isaac.rojas@yahoo.io",
        SomethingUnique: "a",
        SomeGuid: 84a755f0-60e2-4619-a80f-3e48091000f7,
        Avatar: "https://grovotor.com/userimage/2759382/142e5aff4e034bcdb022e5fd45169b8b.jpeg?size=256",
        CartId: 8dfc34ed-9c8a-43a3-9d60-07c852f50f15,
        SSN: "447-65-4635",
        Gender: Male,
        Orders: [
            {
                OrderId: 7,
                Item: "Key Lime",
                Quantity: 3,
                LotNumber: null
            },
            {
                OrderId: 8,
                Item: "Goji Berry",
                Quantity: 6,
                LotNumber: null
            },
            {
                OrderId: 9,
                Item: "Chokecherry",
                Quantity: 5,
                LotNumber: null
            }
        ]
    }
]
```
