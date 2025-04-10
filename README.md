
# SCANS.Analyzers

SCANS (Short Cuts Are Never Shorter) is a framework focused on **clean, testable, and logic-separated .NET code**. The **SCANS.Analyzers** project contains **Roslyn analyzers** that enforce key architectural rules and best practices within your .NET codebase, ensuring consistent quality and maintainability, ensuring compliance with the SCARS (Short Cuts Are Rarely Shorter) philosophy.

## Features

- **Enforce architectural principles** using Roslyn-based analyzers.
- **Prevent mocking of unmockable types**.
- **Ensure services follow proper dependency injection**.
- **Enforce rules around method length and logic**.
- **Helps maintain clean, scalable code** by enforcing coding guidelines during the build process.

## Installation

To install and use SCANS.Analyzers in your project, follow the steps below:

### Install via NuGet

You can add the SCANS.Analyzers NuGet package to your project by running the following command:

```bash
dotnet add package SCANS.Analyzers --version x.y.z
```

Replace `x.y.z` with the latest stable version of **SCANS.Analyzers**.

### Install via Visual Studio (Package Manager)

If you are using Visual Studio, you can install the SCANS.Analyzers NuGet package from the **NuGet Package Manager** by searching for `SCANS.Analyzers`.

## Usage

Once the package is installed, the analyzers will automatically be included in your build process. They will run as part of your **compiler warnings** during compilation. You will receive feedback and error messages directly in your build output, indicating any violations of SCANS architectural principles.

### Example Rules

- **Unmockable Types**: If a type is marked as `[Unmockable]`, any attempt to mock it will result in a compile-time error.
- **Glue vs Logic**: Classes with the `[Glue]` attribute must only contain orchestration logic, while classes marked with `[Logic]` must contain pure computation logic with no dependencies.
- **Method Length**: Methods with fewer than 20 bytes of IL code are flagged, as long methods typically indicate that too much logic has been included in a single method.
  
### Customizing Rules

If you want to disable specific rules or change their behavior, you can do so by modifying the configuration in your `.editorconfig` file or through direct project file settings. 

For example, you can disable a rule globally using the following in your `.editorconfig`:

```ini
# Disable Unmockable rule globally
[*.cs]
dotnet_diagnostic.SCANS001.severity = none
```

## Configuration

The analyzers can be configured via the `.editorconfig` file, which allows you to specify custom behavior for each rule.

### Example Configuration:

```ini
# .editorconfig file configuration for SCANS.Analyzers

[*.cs]
# Enable or disable SCANS rules
dotnet_diagnostic.SCANS001.severity = error # Block mocking unmockable types
dotnet_diagnostic.SCANS002.severity = warning # Enforce Glue vs Logic
dotnet_diagnostic.SCANS003.severity = none # Disable method length check
```

## Contributing

Contributions to the SCANS.Analyzers project are welcome! To contribute:

1. Fork the repository.
2. Create a new branch for your feature or bugfix (`git checkout -b feature/your-feature`).
3. Commit your changes (`git commit -am 'Add your feature'`).
4. Push to your forked repository (`git push origin feature/your-feature`).
5. Open a pull request with a description of your changes.

### Testing and Running the Analyzers Locally

To run the analyzers locally or test them in your project, you can use **Roslyn-based test projects** or manually build and test against your codebase. Ensure your project is correctly referencing the `SCANS.Analyzers` NuGet package.

## License

SCANS.Analyzers is licensed under the **MIT License**. See the [LICENSE](LICENSE) file for details.

## Acknowledgments

- This project makes use of **Roslyn Analyzers** to enforce architectural rules in .NET.
- Inspired by the SCANS philosophy, which emphasizes clean code and avoiding shortcuts that lead to technical debt.
