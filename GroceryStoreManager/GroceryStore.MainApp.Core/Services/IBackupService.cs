namespace GroceryStore.MainApp.Core.Services;

public interface IBackupService
{
    void BackupDatabase(string backupFilePath);
    void RestoreDatabase(string backupFilePath);
}
