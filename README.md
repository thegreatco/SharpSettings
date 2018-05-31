# SharpSettings
This is the base package for implementations of SharpSettings. This provides the interfaces and base types for storing settings in a database and getting background updates without explicitly updating each time.

| dev | master |
| --- | ------ |
| [![CircleCI](https://circleci.com/gh/thegreatco/SharpSettings/tree/dev.svg?style=svg)](https://circleci.com/gh/thegreatco/SharpSettings/tree/dev) | [![CircleCI](https://circleci.com/gh/thegreatco/SharpSettings/tree/master.svg?style=svg)](https://circleci.com/gh/thegreatco/SharpSettings/tree/master) |

# Usage

WIP

### Logger
To be as flexible as possible and not requiring a particular logging framework, a shim must be implemented that implements the `ISharpSettingsLogger` interface. It follows similar patterns to `Serilog.ILogger` but is easily adapted to `Microsoft.Extensions.Logging` as well.