namespace PortfolioTracking.Models
{
    public class Cash
    {

        // Attribut pour le niveau de cash
        public decimal CashAmount { get; set; }

        // Constructeur avec le niveau de cash initial
        public Cash(decimal initialAmount)
        {
            CashAmount = initialAmount;
        }

        // Constructeur par défaut
        public Cash()
        {
            CashAmount = 0;           // Valeur par défaut
        }

        // Méthode pour ajouter du cash
        public void Add(decimal amount)
        {
            CashAmount += amount;
        }

        // Méthode pour soustraire du cash
        public void Subtract(decimal amount)
        {
           CashAmount -= amount;
        }

    }
}
