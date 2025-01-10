namespace PortfolioTracking.Models
{
    public class Portefeuille
    {
        private List<Asset> assets;  // Liste d'actifs

        private Cash cash;  // Liste d'actifs

        // Constructeur
        public Portefeuille(decimal montantInitial)
        {
            assets = new List<Asset>();         // Initialisation de la liste des actifs
            cash = new Cash(montantInitial);    // Initialisation du cash
        }

        // Ajouter un actif au portefeuille
        public void AjouterAsset(Asset asset)
        {
            // Vérifiez si l'actif existe déjà dans le portefeuille
            var existingAsset = assets.FirstOrDefault(a => a.Ticker == asset.Ticker);

            if (existingAsset != null)
            {
                // Si l'actif existe, mettez à jour la quantité
                existingAsset.Quantite += asset.Quantite;

                // Calcul du nouveau prix moyen d'entrée
                decimal totalCost = (existingAsset.BuyPrice * existingAsset.Quantite) + (asset.BuyPrice * asset.Quantite);
                existingAsset.BuyPrice = Math.Round(totalCost / existingAsset.Quantite, 2); // Mettre à jour le prix moyen d'entrée à 2 décimales
            }
            else
            {
                // Ajouter un nouvel actif
                assets.Add(asset);
            }
        }

        // Méthode pour supprimer un actif
        public void SupprimerAsset(string nom)
        {
            var asset = assets.FirstOrDefault(a => a.Ticker == nom);

            if (asset != null)
            {
                assets.Remove(asset);
                cash.CashAmount += asset.Quantite * asset.BuyPrice;
            }
            else
            {
                throw new InvalidOperationException("Actif non trouvé dans le portefeuille.");
            }
        }

        // Récupérer tous les actifs
        public List<Asset> GetAssets()
        {
            return assets;
        }

        // Récupérer le cash
        public Cash GetCash()
        {
            return cash;
        }

        // Calculer la valeur totale du portefeuille
        public decimal ValeurTotale()
        {
            return Math.Round(assets.Sum(a => a.ValeurTotalActif), 2); // Arrondir à 2 décimales
        }

        // Récupérer le niveau de cash
        public decimal CashAmount()
        {
            return cash.CashAmount;
        }

        public override string ToString()
        {
            decimal valeurTotale = ValeurTotale();

            if (valeurTotale == 0)
            {
                return "La valeur des actifs est de 0 $.";
            }

            return $"La valeur totale des actifs est de : {valeurTotale} $."; // Ajoutez la devise si nécessaire
        }
    }
}
