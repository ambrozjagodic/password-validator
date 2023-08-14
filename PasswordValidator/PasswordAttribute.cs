using System.ComponentModel.DataAnnotations;

/// <summary>
/// Validates the format and complexity of a password string.
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class PasswordAttribute : ValidationAttribute
{
    private int _minLength = 12;
    private int _maxLength = 64;
    private int _requiredCharacterTypes = 3;

    /// <summary>
    /// Gets or sets the minimum allowed password length. Default is 12.
    /// </summary>
    public int MinLength
    {
        get => _minLength;
        set
        {
            if (value <= 0)
            {
                throw new ArgumentException($"{nameof(MinLength)} must be greater than 0.");
            }
            _minLength = value;
        }
    }

    /// <summary>
    /// Gets or sets the maximum allowed password length. Default is 64.
    /// </summary>
    public int MaxLength
    {
        get => _maxLength;
        set
        {
            if (value < MinLength)
            {
                throw new ArgumentException($"{nameof(MaxLength)} must be greater than or equal to MinLength.");
            }
            _maxLength = value;
        }
    }

    /// <summary>
    /// Gets or sets the number of required distinct character types in the password (uppercase letters, lowercase letters, numbers, special characters). Default is 3.
    /// </summary>
    public int RequiredCharacterTypes
    {
        get => _requiredCharacterTypes;
        set
        {
            if (value < 0 || value > 4)
            {
                throw new ArgumentOutOfRangeException($"{nameof(RequiredCharacterTypes)} must be between 0 and 4.");
            }

            _requiredCharacterTypes = value;
        }
    }

    /// <summary>
    /// Gets or sets whether uppercase letters must be enforced in the password.
    /// </summary>
    public bool EnforceUppercase { get; set; } = false;

    /// <summary>
    /// Gets or sets whether lowercase letters must be enforced in the password.
    /// </summary>
    public bool EnforceLowercase { get; set; } = false;

    /// <summary>
    /// Gets or sets whether numbers must be enforced in the password.
    /// </summary>
    public bool EnforceNumbers { get; set; } = false;

    /// <summary>
    /// Gets or sets whether special characters must be enforced in the password.
    /// </summary>
    public bool EnforceSpecialChars { get; set; } = false;

    /// <summary>
    /// Initializes a new instance of the <see cref="PasswordAttribute"/> class.
    /// </summary>
    public PasswordAttribute()
    {
    }

    /// <inheritdoc />
    public override bool IsValid(object? value)
    {
        if (value == null || value is not string password)
        {
            return false;
        }

        // Check password length
        if (!IsValidLength(password, out var lengthErrorMessage))
        {
            ErrorMessage = lengthErrorMessage;
            return false;
        }

        var overrideRequiredCharTypeValidation = EnforceUppercase || EnforceLowercase || EnforceNumbers || EnforceSpecialChars;
        if (overrideRequiredCharTypeValidation)
        {
            // Check specific character requirements based on properties
            if (!IsValidByEnforceProps(password, out var enforcePropsErrorMessage))
            {
                ErrorMessage = enforcePropsErrorMessage;
                return false;
            }
        }
        else
        {
            // Check required character types if no Enforce properties are set
            if (!IsValidByCharTypes(password, out var charTypesErrorMessage))
            {
                ErrorMessage = charTypesErrorMessage;
                return false;
            }

        }

        return true;
    }



    private bool IsValidLength(string password, out string errorMessage)
    {
        errorMessage = string.Empty;

        if (password.Length < MinLength || password.Length > MaxLength)
        {
            errorMessage = $"Password length must be between {MinLength} and {MaxLength} characters.";
            return false;
        }

        return true;
    }

    private bool IsValidByEnforceProps(string password, out string errorMessage)
    {
        errorMessage = string.Empty;
        if (EnforceUppercase && !password.Any(char.IsUpper))
        {
            errorMessage = "Password must include at least one uppercase letter.";
            return false;
        }

        if (EnforceLowercase && !password.Any(char.IsLower))
        {
            errorMessage = "Password must include at least one lowercase letter.";
            return false;
        }

        if (EnforceNumbers && !password.Any(char.IsDigit))
        {
            errorMessage = "Password must include at least one number.";
            return false;
        }

        if (EnforceSpecialChars && !password.Any(c => char.IsSymbol(c) || char.IsPunctuation(c)))
        {
            errorMessage = "Password must include at least one special character.";
            return false;
        }

        return true;
    }

    private bool IsValidByCharTypes(string password, out string errorMessage)
    {
        errorMessage = string.Empty;

        int actualCharacterTypes = 0;

        if (password.Any(char.IsUpper))
        {
            actualCharacterTypes++;
        }

        if (password.Any(char.IsLower))
        {
            actualCharacterTypes++;
        }

        if (password.Any(char.IsDigit))
        {
            actualCharacterTypes++;
        }

        if (password.Any(c => char.IsSymbol(c) || char.IsPunctuation(c)))
        {
            actualCharacterTypes++;
        }

        if (!EnforceUppercase && !EnforceLowercase && !EnforceNumbers && !EnforceSpecialChars)
        {
            if (actualCharacterTypes < RequiredCharacterTypes)
            {
                errorMessage = $"Password must include at least {RequiredCharacterTypes} of the following character types: uppercase letters, lowercase letters, digits, special symbols.";
                return false;
            }
        }

        return true;
    }
}