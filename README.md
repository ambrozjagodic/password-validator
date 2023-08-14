# PasswordValidator

The **PasswordAttribute** is a .NET validation attribute that allows you to validate the format and complexity of password strings. It provides customizable rules to enforce password requirements such as length, character types, and more.

## Installation

To use the PasswordAttribute in your .NET Core project, you can install it via NuGet Package Manager:

```sh
dotnet add package PasswordValidator
```

## Usage

1. Add the `PasswordAttribute` to your Password property in your desired class:

```csharp
using System.ComponentModel.DataAnnotations;

public class UserModel
{
    public string Name { get; set; }

    [Password]
    public string Password { get; set; }
}
```

2. Apply the model validation in your code, such as during registration or password update:

```csharp
var userModel = new UserModel
{
    Name = "JohnDoe",
    Password = "P@ssw0rd123"
};

var validationContext = new ValidationContext(userModel);
var validationResults = new List<ValidationResult>();

bool isValid = Validator.TryValidateObject(userModel, validationContext, validationResults, validateAllProperties: true);

if (!isValid)
{
    foreach (var validationResult in validationResults)
    {
        Console.WriteLine(validationResult.ErrorMessage);
    }
}
```

> **Note:** The example provided shows a manual validation process. If you're using a framework like ASP.NET Core, model validation is often handled automatically as part of the request processing.

## Customization

The `PasswordAttribute` provides several properties that allow you to customize the password validation rules according to your requirements. Here is an overview of each property, its default value, and an explanation of its purpose:

- **MinLength (int, default: 12)**

Specifies the minimum allowed length for the password. Must be greater than 0.

- **MaxLength (int, default: 64)**

Specifies the maximum allowed length for the password. The maximum length must be greater than or equal to the `MinLength`.

- **RequiredCharacterTypes (int, default: 3)**

Specifies the number of required distinct character types in the password. Character types include:

- Uppercase letters
- Lowercase letters
- Numbers
- Special characters

This property ensures that the password contains a combination of the specified character types. So by default, 3 of the 4 character types must be included in a password.

- **EnforceUppercase (bool, default: false)**

Indicates whether uppercase letters must be enforced in the password. If set to `true`, the password must contain at least one uppercase letter.

- **EnforceLowercase (bool, default: false)**

Indicates whether lowercase letters must be enforced in the password. If set to `true`, the password must contain at least one lowercase letter.

- **EnforceNumbers (bool, default: false)**

Indicates whether numbers must be enforced in the password. If set to `true`, the password must contain at least one number.

- **EnforceSpecialChars (bool, default: false)**

Indicates whether special characters must be enforced in the password. If set to `true`, the password must contain at least one special character (symbol or punctuation).

> **Note:** If any of the above `Enforce` properties are set, the default `RequiredCharacterTypes` validation is bypassed.

### Example Usage with Cusomization

Here's an example of how to use the `PasswordAttribute` with customized validation rules:

```csharp
using System.ComponentModel.DataAnnotations;

public class UserModel
{
    [Password(MinLength = 8, MaxLength = 20, EnforceUppercase = true, EnforceNumbers = true)]
    public string Password { get; set; }
}
```

## Contributions

Contributions are welcome! If you encounter any issues or have suggestions for improvements, please feel free to open an issue or submit a pull request.

## License
This project is licensed under the MIT License - see the [LICENSE](https://github.com/ambrozjagodic/PasswordValidator/blob/dcd2c8bd114309682e792aa9d40f693a133bd2f7/LICENSE) file for details.
