namespace GalleryMobile.DataPersistence
{
    public static class DataConstants
    {
        private const string DATABASE_NAME = "GalleryApp.db3";

        public const SQLite.SQLiteOpenFlags Flags =
        // open the database in read/write mode
        SQLite.SQLiteOpenFlags.ReadWrite |
        // create the database if it doesn't exist
        SQLite.SQLiteOpenFlags.Create |
        // enable multi-threaded database access
        SQLite.SQLiteOpenFlags.SharedCache;

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
