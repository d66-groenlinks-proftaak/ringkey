namespace ringkey.Common.Models
{
    public enum AccountError
    {
        NoError = 0,
        InvalidEmail = 1,
        FirstNameTooShort = 2,
        FirstNameTooLong = 3,
        LastNameTooShort = 4,
        LastNameTooLong = 5,
        PasswordTooShort = 6,
        PasswordTooLong = 7,
        EmailInUse = 8,
        InvalidCaptcha = 9,
        InvalidLogin = 10,
        InvalidPasswordCharacters = 11
    }
}