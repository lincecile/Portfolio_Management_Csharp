namespace PortfolioTracking.Models
{
    public class Asset
    {
        public string Ticker { get; set; }  // Ticker de l'actif
        public decimal BuyPrice { get; set; }  // Prix d'achat
        public DateTime DatePrice { get; set; }  // Date du prix
        public int Quantite { get; set; }  // Quantité détenue

        // Valeur totale de l'actif, calculée à la volée
        public decimal ValeurTotalActif => Math.Round(BuyPrice * Quantite, 2);
        public List<float> Historique { get; set; } // Valeur historique

        // Constructeur avec paramètres
        public Asset(string ticker, decimal buyPrice, DateTime datePrice, int quantite = 1, List<float> historique = null)
        {
            Ticker = ticker;
            BuyPrice = Math.Round(buyPrice, 2); // Arrondir le prix d'achat à 2 décimales
            DatePrice = datePrice;
            Quantite = quantite; // Initialiser la quantité
            Historique = historique ?? new List<float>(); // Initialiser l'historique si null
        }

        // Constructeur par défaut
        public Asset()
        {
            Ticker = string.Empty;  // Valeur par défaut
            BuyPrice = 0;           // Valeur par défaut
            DatePrice = DateTime.Now; // Valeur par défaut
            Quantite = 1;           // Valeur par défaut
            Historique = new List<float>();
        }

        // Représentation de l'objet en chaîne de caractères
        public override string ToString()
        {
            return $"{Ticker} - Prix: {Math.Round(BuyPrice, 2)} € - Date: {DatePrice.ToShortDateString()} - Quantité: {Quantite} - Valeur Totale: {ValeurTotalActif} €";
        }
    }
}
