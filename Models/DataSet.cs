
using System.Text.Json; // Ajoutez cet import pour JsonSerializer
using System.Globalization;

namespace PortfolioTracking.Services
{
    // Mod�le pour d�s�rialiser la r�ponse JSON
    public class ResponseModel
    {
        public List<string> Data { get; set; }
    }

    public static class DataSet
    {
        // Ajoutez un HttpClient statique pour r�utiliser les connexions
        private static readonly HttpClient httpClient = new HttpClient();

        public static async Task<List<string>> GetNomsFichiersCsvAsync()
        {
            List<string> nomsFichiers = new List<string>();

            // Appeler l'API pour r�cup�rer des donn�es (facultatif)
            try
            {
                var response = await httpClient.GetStringAsync("https://localhost:7069/file_csv.json");
                //Console.WriteLine("Donn�es r�cup�r�es de l'API : " + response);

                // D�s�rialiser la r�ponse JSON
                var data = JsonSerializer.Deserialize<ResponseModel>(response);

                // V�rifiez que les donn�es ne sont pas nulles
                if (data?.Data != null)
                {
                    foreach (var fichier in data.Data)
                    {
                        nomsFichiers.Add(Path.GetFileNameWithoutExtension(fichier)); // Ajoute le nom sans l'extension
                    }
                }
                else
                {
                    Console.WriteLine("Aucun fichier trouv� dans la r�ponse.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur lors de l'appel de l'API : " + ex.Message);
            }

            return nomsFichiers;
        }

        // Cr�ation de la liste de liste [["close price","date"],["2.83","2022-12-22"], ...] quand l'utilisateur selectionne un ticker dans le menu d�roulant
        public static async Task<List<List<object>>> GetCsvContent(string Ticker)
        {
            List<List<object>> nomsFichiers = new List<List<object>>();

            try
            {
                var response = await httpClient.GetStringAsync("https://localhost:7069/Data/" + Ticker + ".csv");

                Console.WriteLine(Ticker);

                //////////////// split le csv en ligne et en colonne (liste de liste)

                // liste de liste, lines[0] = ["open,high,low,close,date,time"]
                string[] lines = response.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

                // liste de liste, stringColumns[0] = "open"
                string[] SelectedColumns = lines[0].Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                // indices des colonnes "close" et "date" du csv
                int IndexClosePrice = Array.IndexOf(SelectedColumns, "close");
                int IndexDate = Array.IndexOf(SelectedColumns, "date");

                // Mise en forme du Csv en liste de liste
                List<List<string>> listOfListsOfCsvContent = new List<List<string>>();
                for (int i = 0; i < lines.Length; i++)
                {
                    // S�parer chaque ligne par des virgules
                    string[] SousListe = lines[i].Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                    listOfListsOfCsvContent.Add(new List<string>(SousListe));
                }

                //////////////// On garde que la colonne "close" et "date" et on convertit la colonne 'close' en float si possible

                // Liste pour stocker les colonnes qu'on souhaite garder
                List<List<object>> selectedElements = new List<List<object>>();

                // Cr�ation de la liste de liste [["close","date"],["2.83","2022-12-22"], ...]
                foreach (var innerList in listOfListsOfCsvContent)
                {
                    // V�rifier que la sous-liste contient au moins 4 �l�ments
                    if (innerList.Count >= 4)
                    {
                        List<object> tempList;

                        // Essayer de convertir le 3e �l�ment en float
                        if (float.TryParse(innerList[IndexClosePrice], NumberStyles.Float, CultureInfo.InvariantCulture, out float convertedValue))
                        {
                            // Cr�er une nouvelle liste pour stocker les �l�ments souhait�s
                            tempList = new List<object>
                            {
                                innerList[IndexDate],  // date
                                convertedValue, // prix close 
                            };

                            selectedElements.Add(tempList);
                        }
                        else
                        {
                            Console.WriteLine($"La cha�ne '{innerList[IndexClosePrice]}' ne peut pas �tre convertie en float.");
                        }
                    }
                    else
                    {
                        // cas qui n'est jamais cens� arriv�
                        Console.WriteLine("La sous-liste n'a pas suffisamment d'�l�ments.");
                    }
                }

                Console.WriteLine(selectedElements);
                Console.WriteLine(selectedElements[0]);
                Console.WriteLine(selectedElements[0][0]);
                Console.WriteLine(selectedElements[0][1]);
                return selectedElements;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur lors de l'appel de l'API : " + ex.Message);
            }

            return nomsFichiers; // retourne liste vide que si le try ne fonctionne pas
        }
    }
}

