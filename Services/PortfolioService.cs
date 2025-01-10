using PortfolioTracking.Models;

namespace PortfolioTracking.Services
{
    public class PortfolioService
    {
        private Portefeuille portefeuille = new Portefeuille(0);  // Instanciation du portefeuille

        public bool AjouterAssetSiPresent(string ticker, decimal buyPrice, DateTime datePrice, int quantite)
        {
            // Vérifiez si l'actif existe déjà dans le portefeuille
            var existingAsset = portefeuille.GetAssets().FirstOrDefault(a => a.Ticker == ticker);

            if (existingAsset != null)
            {
                // Si l'actif existe, mettez à jour la quantité
                existingAsset.Quantite += quantite;

                // Calcul du nouveau prix moyen d'entrée
                decimal totalCost = (existingAsset.BuyPrice * (existingAsset.Quantite - quantite)) + (buyPrice * quantite);
                existingAsset.BuyPrice = Math.Round(totalCost / existingAsset.Quantite, 2); // Mettre à jour le prix moyen d'entrée à 2 décimales

                // Mettez également à jour la date de l'entrée
                existingAsset.DatePrice = datePrice > existingAsset.DatePrice ? datePrice : existingAsset.DatePrice; // Conserver la date la plus récente

                return true; // Indiquer que la mise à jour a réussi
            }
            else
            {
                // Ajouter un nouvel actif
                var newAsset = new Asset(ticker, buyPrice, datePrice, quantite);
                portefeuille.AjouterAsset(newAsset);
                return true; // Indiquer que l'ajout a réussi
            }
        }

        // Vendre un actif
        public void VendreAsset(Asset asset, int quantite)
        {
            // Vérifiez si l'actif existe déjà dans le portefeuille
            var existingAsset = portefeuille.GetAssets().FirstOrDefault(a => a.Ticker == asset.Ticker);

            if (existingAsset != null)
            {
                if (existingAsset.Quantite >= quantite)
                {
                    // Si l'actif existe, mise à jour la quantité
                    existingAsset.Quantite -= quantite;

                    // Si la quantité atteint zéro, supprime l'actif
                    if (existingAsset.Quantite == 0)
                    {
                        portefeuille.SupprimerAsset(existingAsset.Ticker); // Supprime l'actif si la quantité est zéro
                    }
                }
                else
                {
                    throw new InvalidOperationException("Quantité insuffisante pour la vente.");
                }
            }
            else
            {
                throw new InvalidOperationException("Actif non trouvé dans le portefeuille.");
            }
        }

        // Ajout de cash
        public void AddCash(decimal amount)
        {   
            portefeuille.GetCash().Add(amount);
        }

        // Retire du cash
        public void SubtractCash(decimal amount)
        {
            portefeuille.GetCash().Subtract(amount);
        }

        // Liste des actifs
        public List<Asset> GetAssets()
        {
            return portefeuille.GetAssets();
        }

        // Suppression de l'actif
        public void SupprimerAsset(string ticker)
        {
            try
            {
                portefeuille.SupprimerAsset(ticker); // Appele de la méthode de Portefeuille
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la suppression de l'actif : {ex.Message}");
            }
        }

        // Méthode pour obtenir les noms de fichiers CSV de manière asynchrone
        public async Task<List<string>> GetNomsFichiersCsvAsync()
        {
            return await DataSet.GetNomsFichiersCsvAsync(); 
        }

        // Méthode pour obtenir le contenu de fichiers CSV
        public async Task<List<List<object>>> GetCsvContent(string ticker)
        {
            return await DataSet.GetCsvContent(ticker);
        }

        public override string ToString()
        {
            return portefeuille.ToString(); // Appeler la méthode ToString() de Portefeuille qui renvoie la valeur des actifs non cash
        }

        public decimal ValeurTotale()
        {
            return Math.Round(portefeuille.ValeurTotale(), 2); // Arrondir la valeur totale à 2 décimales
        }

        public decimal CashAmount()
        {
            return portefeuille.CashAmount();
        }
    }
}


