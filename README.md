# CsCheck Extension

The project is an extension of the C# random testing library [CsCheck](https://github.com/AnthonyLloyd/CsCheck?tab=readme-ov-file). It extends it with additional data types for the random-based generation of values.

In addition to the data types provided by CsCheck, the following data types can also be generated:
- Email addresses
- IPv4 addresses
- Host and domain names
## Examples

The following examples show generators for creating IPv4 addresses with customized configuration and with default settings.

### Using default behaviour

Each generator can be used by instantiating it as follows. By default, all reserved address ranges are ignored.

```csharp
var ipGen = new GenIPv4Address();
var ipAddress = ipGen.Single();
Console.WriteLine("Generated ip address is {0}", ipAddress);
```

### Customize generator behaviour

Alternatively an option object can be passed to each generator, which changes the behavior and create alternative results. The generator can now also generate IPv4 addresses from private network ranges.

```csharp
var options = new IPv4GenOptions();
options.IncludePrivateNetworks();

var ipGen = new GenIPv4Address(options);
var ipAddress = ipGen.Single();
Console.WriteLine("Generated ip address is {0}", ipAddress);
```

# Builder

Alternatively, the implemented builder can be used to create and configure a generator to generate less boilerplate code

```csharp
var ipAddress = GenBuilder.IPv4
    .IncludePrivateNetworks()
    .Build()
    .Single()
    .ToString();
```
## Configuration

Every generator accepts a option object that can change the behaviour of generating random values.

### Email

| Option               | Decscription                                                                          |
|----------------------|---------------------------------------------------------------------------------------|
| AllowQuotedLocalPart | Allows to generated email addresses with quoted local part.                           |
| AllowIPv4            | Allows the generation of email addresses that can have an ipv4 address as domain.     |
| PolluteEmails        | Generated email addresses will be changed in a way to make them invalid afterwards.   |

### Domain

| Option        | Decscription                                                                          |
|---------------|---------------------------------------------------------------------------------------|
| DenyHostNames | Generated values have at least two labels.,                                           |

### IPv4

| Option                           | Decscription                                                                      |
|----------------------------------|-----------------------------------------------------------------------------------|
| IncludePrivateNetworks           | Includes private network address blocks                                           |
| IncludeLoopback                  | Includes loopback address blocks                                                  |
| IncludeTestNet                   | Includes Test-Net address blocks provided for documentation purposes              |
| IncludeHostNet                   | Includes host net address block                                                   |
| IncludeReserved                  | Includes reserved address blocks                                                  |
| IncludeSubnet                    | Includes subnet address blocks.                                                   |
| IncludeBroadcast                 | Includes broadcast address blocks                                                 |
| IncludeAnycast                   | Includes anycast address blocks                                                   |
| IncludeBenchmark                 | Includes benchmark address blocks                                                 |
| IncludeAll                       | Includes all reserved address blocks                                              |
| ExcludeAll                       | Excludes all reserved address blocks                                              |
| ExcludeHostAndBroadcastAddresses | Exckudes the host and broadcast address of an address area.                       |
| DoRetries                        | The number of retries if the generated value belongs to an include reserved area. |


## License

[APACHE 2](LICENSE)

