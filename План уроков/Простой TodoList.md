В папке backend, создай папку TodoListService. И по аналогии со структурой проекта TemplateService, реализуй проект по заданию:

Простой TodoList на ASP.NET: схема БД и API
Сделаем максимально минималистичный вариант, но с демонстрацией всех трёх типов связей.

Схема базы данных (PostgreSQL)
Таблицы
-- Пользователи (владелец списка задач)
CREATE TABLE users (
    id            SERIAL PRIMARY KEY,
    username      VARCHAR(50) NOT NULL UNIQUE,
    email         VARCHAR(255) NOT NULL UNIQUE
);

-- Профиль пользователя: связь один-к-одному с users
CREATE TABLE user_profiles (
    user_id       INT PRIMARY KEY REFERENCES users(id) ON DELETE CASCADE,
    bio           TEXT,
    timezone      VARCHAR(50) DEFAULT 'UTC'
);

-- Списки задач (например, «Работа», «Личное»)
CREATE TABLE todo_lists (
    id            SERIAL PRIMARY KEY,
    user_id       INT NOT NULL REFERENCES users(id) ON DELETE CASCADE,
    name          VARCHAR(100) NOT NULL,
    created_at    TIMESTAMP NOT NULL DEFAULT NOW()
);

-- Задачи: связь один-ко-многим с todo_lists
CREATE TABLE tasks (
    id            SERIAL PRIMARY KEY,
    list_id       INT NOT NULL REFERENCES todo_lists(id) ON DELETE CASCADE,
    title         VARCHAR(200) NOT NULL,
    description   TEXT,
    is_completed  BOOLEAN NOT NULL DEFAULT FALSE,
    due_date      TIMESTAMP,
    created_at    TIMESTAMP NOT NULL DEFAULT NOW()
);

-- Теги: справочник тегов
CREATE TABLE tags (
    id            SERIAL PRIMARY KEY,
    name          VARCHAR(50) NOT NULL UNIQUE
);

-- Связь многие-ко-многим: задачи и теги
CREATE TABLE task_tags (
    task_id       INT NOT NULL REFERENCES tasks(id) ON DELETE CASCADE,
    tag_id        INT NOT NULL REFERENCES tags(id) ON DELETE CASCADE,
    PRIMARY KEY (task_id, tag_id)
);

Разбор связей
Один-к-одному: users ↔ user_profiles. У каждого пользователя максимум один профиль.
Один-ко-многим: todo_lists ↔ tasks. В одном списке может быть много задач, каждая задача принадлежит ровно одному списку.
Многие-ко-многим: tasks ↔ tags через таблицу task_tags. Одна задача может иметь много тегов, один тег может быть у многих задач.
Необходимые API (минимальный набор)
Будем придерживаться REST-стиля.

Пользователи и профили
GET /api/users/{id} — получить пользователя.
POST /api/users — создать пользователя.
GET /api/users/{id}/profile — получить профиль (один-к-одному).
PUT /api/users/{id}/profile — обновить профиль.
Списки задач
GET /api/users/{userId}/lists — получить списки пользователя.
POST /api/users/{userId}/lists — создать список.
GET /api/lists/{id} — получить список.
PUT /api/lists/{id} — обновить список.
DELETE /api/lists/{id} — удалить список (вместе с задачами из-за ON DELETE CASCADE).
Задачи
GET /api/lists/{listId}/tasks — получить задачи списка.
POST /api/lists/{listId}/tasks — создать задачу.
GET /api/tasks/{id} — получить задачу.
PUT /api/tasks/{id} — обновить задачу (в т.ч. is_completed, due_date).
DELETE /api/tasks/{id} — удалить задачу.
Теги и связь с задачами
GET /api/tags — получить все теги.
POST /api/tags — создать тег (если нужно, иначе можно захардкодить).
POST /api/tasks/{taskId}/tags/{tagId} — добавить тег к задаче.
DELETE /api/tasks/{taskId}/tags/{tagId} — убрать тег из задачи.
GET /api/tasks/{taskId}/tags — получить теги задачи.
Этого достаточно, чтобы показать все типы связей и не усложнять проект.

Как это ложится на C# / ASP.NET (очень кратко)
Для users и user_profiles — отдельные DTO и контроллеры; в EF Core связь один-к-одному делается через навигационное свойство и конфигурацию HasOne/WithOne.
Для todo_lists и tasks — связь один-ко-многим через HasMany/WithOne.
Для tasks и tags — многие-ко-многим в EF Core удобно делать через явную сущность TaskTag либо через Fluent API без неё (зависит от версии EF).
