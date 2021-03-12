namespace ringkey.Common.Models
{
    public enum AccountError
    {
        NoError,
        InvalidEmail,
        FirstNameTooShort,
        FirstNameTooLong,
        LastNameTooShort,
        LastNameTooLong,
        PasswordTooShort,
        PasswordTooLong,
        EmailInUse,
        InvalidCaptcha,
        InvalidLogin
    }
}