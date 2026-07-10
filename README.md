# ScienceLogic.Em7.Api

[![Codacy Badge](https://app.codacy.com/project/badge/Grade/0d3554bb45f44678a9b108fa0fb8c425)](https://app.codacy.com/gh/panoramicdata/ScienceLogic.Em7.Api/dashboard?utm_source=gh&utm_medium=referral&utm_content=&utm_campaign=Badge_grade)
[![NuGet Version](https://img.shields.io/nuget/v/ScienceLogic.Em7.Api)](https://www.nuget.org/packages/ScienceLogic.Em7.Api)
[![NuGet Downloads](https://img.shields.io/nuget/dt/ScienceLogic.Em7.Api)](https://www.nuget.org/packages/ScienceLogic.Em7.Api)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

## Introduction

ScienceLogic.Em7.Api is a .NET 10 client library for querying the ScienceLogic EM7 API with HTTP Basic authentication.

## Installation

```shell
dotnet add package ScienceLogic.Em7.Api
```

## Usage

```csharp
using ScienceLogic.Em7.Api;

using var client = new Client("em7.example.com", "username", "password");
```

Use `Get`, `GetPage`, and unpaged query objects to retrieve endpoint models using explicit relative API paths.

## Contributing

See [CONTRIBUTING.md](CONTRIBUTING.md).

## License

MIT — see [LICENSE](LICENSE).
