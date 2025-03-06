namespace GalleryMobile.DataPersistence
{
    public static class DataConstants
    {
        private const string DATABASE_NAME = "GalleryApp.db3";

        public static string DataBasePath
        {
            get
            {
                string path = Path.Combine(FileSystem.AppDataDirectory, DATABASE_NAME);
                return path;
            }
        }
    }
}
