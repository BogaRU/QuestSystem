# QuestSystem

## Описание

QuestSystem — это демонстрационная система управления квестами с использованием ASP.NET Core и PostgreSQL. Проект разработан на основе DDD и Clean Architecture, сделан по ТЗ из файла в корне репозитория.

## Содержание репозитория

- **src/**: Основной код проекта.
- **tests/**: Проекты для тестирования (на базе NUnit).

## Технологии

- .NET 8.0
- ASP.NET Core
- Entity Framework Core
- PostgreSQL
- Docker & Docker Compose
- NUnit для тестирования

## Как запустить проект

### Шаг 1: Клонирование репозитория

Сначала клонируйте репозиторий на свою локальную машину:

```bash
git clone https://github.com/your-username/quest-system.git
cd quest-system
```

### Шаг 2: Настройка переменных окружения

Создайте файл `.env` в корне проекта и добавьте следующие переменные:

```env
POSTGRES_USER=your_pg_user
POSTGRES_PASSWORD=your_pg_password
POSTGRES_DB=questsystem_db
PGADMIN_EMAIL=your_pgadmin_email
PGADMIN_PASSWORD=your_pgadmin_password
```

### Шаг 3: Запуск Docker Compose

Чтобы собрать и запустить все необходимые сервисы (API, база данных и pgAdmin), используйте следующую команду:

```bash
docker-compose up --build
```

Это запустит контейнеры для:

- **PostgreSQL** (порт: 5432)
- **QuestSystem.API** (порт: 5002)
- **pgAdmin** (порт: 8081)

API будет доступно по адресу [http://localhost:5002](http://localhost:5002), а pgAdmin — по [http://localhost:8081](http://localhost:8081). Swagger для удобства [http://localhost:5002/swagger](http://localhost:5002/swagger).

### Шаг 4: Управление pgAdmin

Для управления базой данных через pgAdmin:

- Откройте [http://localhost:8081](http://localhost:8081)
- Введите email: Значение переменной `${PGADMIN_EMAIL}` из файла `.env`
- Введите пароль: Значение переменной `${PGADMIN_PASSWORD}` из файла `.env`
- Подключитесь к базе данных PostgreSQL с помощью следующих данных:
  - Host: `db`
  - Port: `5432`
  - Username: Значение переменной `${POSTGRES_USER}` из файла `.env`
  - Password: Значение переменной `${POSTGRES_PASSWORD}` из файла `.env`

## Запуск тестов

Тесты написаны с использованием NUnit и разделены на два проекта:

- **QuestSystem.Application.Tests**: Тесты уровня приложения.
- **QuestSystem.Domain.Tests**: Тесты для доменных сущностей.

### Запуск тестов через Docker

Чтобы запустить тесты, используйте следующую команду:

```bash
docker-compose up --build tests
```

Эта команда соберет проекты тестов, выполнит их в отдельном контейнере и покажет результаты в терминале. Тесты будут подключаться к базе данных, развернутой в контейнере `db`.


## Структура проекта

- `src/QuestSystem.API`: Веб-API проект.
- `src/QuestSystem.Application`: Логика приложения.
- `src/QuestSystem.Domain`: Доменная логика.
- `src/QuestSystem.Infrastructure`: Работа с внешними сервисами.
- `tests/QuestSystem.Application.Tests`: Тесты для приложения.
- `tests/QuestSystem.Domain.Tests`: Тесты для доменных сущностей.