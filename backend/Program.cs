using Microsoft.EntityFrameworkCore;
using backend.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Настройка базы данных PostgreSQL
builder.Services.AddDbContext<LibraryDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Настройка CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy => policy.WithOrigins("http://localhost:3000")
                        .AllowAnyMethod()
                        .AllowAnyHeader());
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowFrontend");
app.UseAuthorization();
app.MapControllers();

// Инициализация базы данных с тестовыми данными
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<LibraryDbContext>();
    dbContext.Database.EnsureCreated();
    
    // Добавление тестовых данных если таблица пуста
    if (!dbContext.Books.Any())
    {
        dbContext.Books.AddRange(
            new Book { Title = "Война и мир", Author = "Лев Толстой", Year = 1869, Genre = "Роман", IsAvailable = true },
            new Book { Title = "Преступление и наказание", Author = "Федор Достоевский", Year = 1866, Genre = "Роман", IsAvailable = true },
            new Book { Title = "Мастер и Маргарита", Author = "Михаил Булгаков", Year = 1967, Genre = "Фантастика", IsAvailable = false },
            new Book { Title = "1984", Author = "Джордж Оруэлл", Year = 1949, Genre = "Антиутопия", IsAvailable = true },
            new Book { Title = "Гарри Поттер и философский камень", Author = "Дж. К. Роулинг", Year = 1997, Genre = "Фэнтези", IsAvailable = true }
        );
        dbContext.SaveChanges();
    }
}

app.Run();
