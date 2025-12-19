# Tutoring Platform

## Огляд проєкту

Даний проєкт присвячений створенню повноцінного CRUD бекенд додатку системи пошуку та вибору персональних репетиторів. Система дозволяє учням знаходити підходящих викладачів за різними критеріями, а репетиторам - презентувати свої послуги та керувати розкладом.

### Члени команди

- Лебедєва Софія ІМ-44 
- Криницький Ярослав ІМ-44

## Технологічний стек

- **Мова програмування**: C# / .NET 10.0
- **ORM**: Entity Framework Core 10.0.0
- **База даних**: PostgreSQL 16.10
- **Фреймворк тестування**: xUnit, Moq, InMemory 10.0.0
- **Валідація**: FluentValidation 12.1.1
- **Маппінг**: AutoMapper 12.0.1
- **Контейнеризація**: Docker & Docker Compose

## Інструкції з налаштування

### Передумови

- [.NET SDK 10.0](https://dotnet.microsoft.com/download/dotnet/10.0)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/)
- [PostgreSQL Client](https://www.postgresql.org/download/)
- [Git](https://git-scm.com/)

### Налаштування проєкту & Запуск

1. **Клонування репозиторію**
   ```bash
   git clone <repository-url>
   cd tutoring-platform
   ```

2. **Створити міграції**
	```bash
	cd src/TutoringPlatform
	dotnet ef migrations add InitialCreate
	```

3. **Встановлення залежностей**
   ```bash
   dotnet restore
   ```

4. **Запуск бази даних PostgreSQL та API через Docker**
   ```bash
   docker-compose up -d
   ```

5. **Застосування міграцій**
   ```bash
   dotnet ef database update
   ```

Додаток буде доступний за адресою: `https://localhost:8080`

### Доступ до документації API
Після запуску додатку документація API буде доступна за адресою:
- **Scalar UI**: `https://localhost:5001/scalar/v1`

## Запуск тестів

### Запустити всі тести
```bash
cd tests/TutoringPlatform.Tests
dotnet test
```

### Запустити тести з детальною інформацією
```bash
dotnet test --verbosity normal
```

### Запустити конкретний тестовий файл
```bash
dotnet test --filter "FullyQualifiedName~TutorRepositoryIntegrationTests"
# Або запуск всіх тестів у конкретному файлі
dotnet test tests/TutoringPlatform.Tests/Repositories/TutorRepositoryIntegrationTests.cs
```

### Запуск тестів з покриттям коду
```bash
dotnet test --collect:"XPlat Code Coverage"
```

### Запуск конкретного тесту
```bash
dotnet test --filter "MethodName=GetAllAsync_ShouldReturnAllTutors"
```

## Огляд структури проєкту

```
tutoring-platform/
│
├── src/TutoringPlatform/          # Основний проєкт додатку
│   ├── Controllers/               # API контролери (MVC endpoints)
│   ├── Models/                    # Моделі даних
│   │   ├── Entities/              # Сутності бази даних
│   │   ├── Configuration/         # Конфігурації Entity Framework
│   │   ├── Enums/                 # Перерахування
│   │   └── TutoringDbContext.cs   # Контекст бази даних
│   ├── Repositories/              # Репозиторії для доступу до даних
│   │   └── Interfaces/            # Інтерфейси репозиторіїв
│   ├── Services/                  # Бізнес-логіка
│   │   ├── DTOs/                  # Data Transfer Objects
│   │   ├── Interfaces/            # Інтерфейси сервісів
│   │   ├── Mapping/               # AutoMapper профілі
│   │   └── Validators/            # FluentValidation валідатори
│   └── Program.cs                 # Точка входу додатку
│
├── tests/TutoringPlatform.Tests/  # Тестовий проєкт
│   ├── Services/                  # Юніт-тести сервісів
│   ├── Repositories/              # Інтеграційні тести репозиторіїв
│   └── Infrastructure/            # Тестова інфраструктура
│
├── scripts/                       # SQL скрипти
│   └── schema.sql                 # Схема бази даних
│
├── docs/                          # Документація
│   ├── schema.md                  # Опис схеми бази даних
│   ├── queries.md                 # Приклади запитів
│   └── diagrams/                  # Діаграми
│
├── docker-compose.yml             # Конфігурація Docker Compose
├── Dockerfile                     # Dockerfile для додатку
└── README.md                      # Цей файл
```

### Ключові компоненти

- **Controllers**: REST API endpoints для взаємодії з клієнтом
- **Services**: Бізнес-логіка та обробка даних, валідація
- **Repositories**: Абстракція доступу до даних через паттерн Repository
- **Models/Entities**: Сутності бази даних
- **DTOs**: Об'єкти передачі даних для API
- **Validators**: Правила валідації для вхідних даних

## Приклади API/використання

### Отримати всіх репетиторів
```http
GET /api/tutor
```

**Відповідь:**
```json
[
  {
    "id": 1,
    "firstName": "Андрей",
    "lastName": "Бармалей",
    "email": "barmaleyika@example.com",
    "phoneNumber": "+380123456789",
    "bio": "Досвідчений викладач математики",
    "hourlyRate": 600.00,
    "rating": 4.8,
    "cityId": 1,
    "cityName": "Київ"
  }
]
```

### Отримати репетитора за ID
```http
GET /api/tutor/1
```

### Отримати репетиторів за містом
```http
GET /api/tutor/city/1
```

### Пагінація репетиторів
```http
GET /api/tutor/paginated?pageNumber=1&pageSize=10&sortBy=lastName&isAscending=true
```

### Створити нового студента
```http
POST /api/student
Content-Type: application/json

{
  "firstName": "Степан",
  "lastName": "Степанович",
  "email": "stepa@example.com",
  "phoneNumber": "+380123456789",
  "cityId": 1
}
```

### Створити бронювання
```http
POST /api/booking
Content-Type: application/json

{
  "tutorId": 1,
  "studentId": 1,
  "scheduleId": 5,
  "bookingDate": "2025-12-20T10:00:00Z",
  "notes": "Підготовка до ЗНО з математики"
}
```

### Отримати всі предмети
```http
GET /api/subject
```

### Додати відгук
```http
POST /api/review
Content-Type: application/json

{
  "tutorId": 1,
  "studentId": 1,
  "rating": 5,
  "comment": "Чудовий викладач! Все зрозуміло пояснює."
}
```

### Отримати розклад репетитора
```http
GET /api/schedule/tutor/1
```

### Отримати доступний розклад
```http
GET /api/schedule/available
```

---