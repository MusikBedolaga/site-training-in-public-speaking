using Nest;
using TrainingWebsiteBack.Models;

public class ElasticSearchService
{
    private readonly ElasticClient _client;

    public ElasticSearchService(string elasticUri)
    {
        var settings = new ConnectionSettings(new Uri(elasticUri))
            .DefaultIndex("courses");
        _client = new ElasticClient(settings);
    }

    public async Task IndexCourseAsync(Course course)
    {
        var response = await _client.IndexDocumentAsync(course);
        if (!response.IsValid)
        {
            // Логика обработки ошибки
            Console.WriteLine("Ошибка индексации: " + response.ServerError);
        }
    }
    
    public async Task DeleteCourseFromIndexAsync(int courseId)
    {
        var response = await _client.DeleteAsync<Course>(courseId);
        if (!response.IsValid)
        {
            // Логика обработки ошибки
            Console.WriteLine("Ошибка удаления из индекса: " + response.ServerError);
        }
    }
    
    public async Task<List<Course>> SearchCoursesAsync(string query)
    {
        var response = await _client.SearchAsync<Course>(s => s
            .Query(q => q
                .MultiMatch(m => m
                    .Fields(f => f
                        .Field(c => c.Name)
                        .Field(c => c.Lectures.Select(l => l.Title))
                    )
                    .Query(query)
                )
            )
        );

        return response.Documents.ToList();
    }
}