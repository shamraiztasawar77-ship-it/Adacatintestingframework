namespace Automationassignment_01.Models
{
    public sealed class BookHotelData
    {
        public string TestCaseId { get; init; } = string.Empty;

        public string FirstName { get; init; } = string.Empty;

        public string LastName { get; init; } = string.Empty;

        public string Address { get; init; } = string.Empty;

        public string CreditCardNumber { get; init; } = string.Empty;

        public string CreditCardType { get; init; } = string.Empty;

        public string ExpiryMonth { get; init; } = string.Empty;

        public string ExpiryYear { get; init; } = string.Empty;

        public string Cvv { get; init; } = string.Empty;

        public string ExpectedFirstNameMessage { get; init; } =
            string.Empty;

        public string ExpectedLastNameMessage { get; init; } =
            string.Empty;

        public string ExpectedAddressMessage { get; init; } =
            string.Empty;

        public string ExpectedCreditCardMessage { get; init; } =
            string.Empty;

        public string ExpectedCardTypeMessage { get; init; } =
            string.Empty;

        public string ExpectedExpiryMessage { get; init; } =
            string.Empty;

        public string ExpectedCvvMessage { get; init; } =
            string.Empty;
    }
}