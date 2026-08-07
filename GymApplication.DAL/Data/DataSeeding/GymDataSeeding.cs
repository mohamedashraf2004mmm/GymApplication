using GymApplication.DAL.Data.DbContextss;
using GymApplication.DAL.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace GymApplication.DAL.Data.DataSeeding
{
    public static class GymDataSeeding
    {
        public async static Task SeedAsync(GymDbContext dbContext , string seedFolderPath , CancellationToken ct = default)
        {
            try
            {
                if(!await dbContext.Plans.AnyAsync(ct))
                {
                    var plans = LoadDataFromJsonFile<Plan>(seedFolderPath , "Plans.json");
                    if (plans.Any())
                    {
                        dbContext.Plans.AddRange(plans);
                    }
                }
            }
            catch
            {

            }
        }

        private static List<T>LoadDataFromJsonFile<T>(string folderPath , string fileName)
        {
            var filePath = Path.Combine(folderPath, fileName);
            if (!File.Exists(filePath))
                throw new FileNotFoundException($"Can not seed data from {filePath}");

            var data = File.ReadAllText(filePath);

            var options = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            };

            return JsonSerializer.Deserialize<List<T>>(data, options) ?? [];

        }

    }
}
