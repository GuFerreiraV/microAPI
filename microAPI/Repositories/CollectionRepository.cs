using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using microAPI.Interfaces;
using microAPI.Models;

namespace microAPI.Repositories
{
    public class CollectionRepository : ICollectionRepository
    {
        private readonly string _filePath;

        public CollectionRepository()
        {
            // Salva na pasta AppData/Local/microAPI (ou ~/.local/share/microAPI no Linux)
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string appFolder = Path.Combine(appDataPath, "microAPI");
            if (!Directory.Exists(appFolder))
            {
                Directory.CreateDirectory(appFolder);
            }

            _filePath = Path.Combine(appFolder, "collections.json");
        }

        public async Task<List<CollectionModel>> LoadCollectionsAsync()
        {
            if (!File.Exists(_filePath))
            {
                return new List<CollectionModel>();
            }

            try
            {
                string json = await File.ReadAllTextAsync(_filePath);
                return JsonSerializer.Deserialize<List<CollectionModel>>(json) ?? new List<CollectionModel>();
            }
            catch
            {
                return new List<CollectionModel>(); 
            }
        }

        public async Task SaveCollectionsAsync(List<CollectionModel> collections)
        {
          try {
             var options = new JsonSerializerOptions {WriteIndented = true};
             string json = JsonSerializer.Serialize(collections, options);
             await File.WriteAllTextAsync(_filePath, json);
          }catch(Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erro ao salvar coleções: {ex.Message}");
            }
        }
    }
}