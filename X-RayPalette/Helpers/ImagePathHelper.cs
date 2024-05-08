namespace X_RayPalette.Helpers
{
    public class ImagePathHelper
    {
        public static string ImagesFolderPath()
        {
            // Pobierz ścieżkę do bieżącego katalogu aplikacji
            string currentDirectory = Directory.GetCurrentDirectory();
            for (int i = 0; i < 3; i++)
            {
                DirectoryInfo destinationFolder = Directory.GetParent(currentDirectory);
                currentDirectory = destinationFolder.FullName;
            }
            // Dodaj folder "images" do ścieżki
            string imagesFolder = Path.Combine(currentDirectory, "Images");
            return imagesFolder;
        }
    }
}
