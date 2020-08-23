namespace RasmiOnline.Console.PaymentStrategy
{
    using Domain.Enum;

    public static class PaymentFactory
    {
        public static IPaymentStrategy GetInstance(BankNames predict)
        {
            switch (predict)
            {
                case BankNames.ZarinPal:
                    return new ZarinPalStrategy();
                case BankNames.Pay:
                    return new PayStrategy();
                case BankNames.Melli:
                    return new SadadStrategy();
                case BankNames.Pasargad:
                    return new PasargadStrategy();
            }
            return new ZarinPalStrategy();
        }
    }
}