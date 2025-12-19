
# Аналітичні запити

## Запит 1: Щомісячний дохід за предметами

**Бізнес-питання:**  
Який щомісячний розподіл доходу відповідно до предметів навчання?

**SQL-запит:**
```sql
SELECT 
    sub.category AS category,
    TO_CHAR(sch.date, 'YYYY-MM') AS month,
    COUNT(DISTINCT b.booking_id) AS booking_count,
    SUM(ts.hourly_rate) AS total_revenue
FROM "booking" b
INNER JOIN "schedule" sch ON b.schedule_id = sch.schedule_id
INNER JOIN "tutor_subject" ts ON b.tutor_subject_id = ts.tutor_subject_id
INNER JOIN "subject" sub ON ts.subject_id = sub.subject_id
WHERE b.status = 'completed'
GROUP BY sub.category, TO_CHAR(sch.date, 'YYYY-MM')
ORDER BY month DESC, total_revenue DESC;
```

**Пояснення:**
- **JOIN таблиць:** об'єднуємо таблиці `booking`, `schedule`, `tutor_subject` та `subject` для отримання повної інформації про бронювання
- **Групування:** групуємо дані за категорією предмету та місяцем (формат `YYYY-MM`)
- **Обчислення доходу:** підраховуємо кількість бронювань та сумарний дохід (сума погодинних ставок)
- **Фільтрація:** враховуємо лише завершені заняття (`status = 'completed'`)
- **Сортування:** результати відсортовані спочатку за місяцем (найновіші першими), потім за доходом

**Приклад виводу:**
| category | month | booking_count | total_revenue |
|----------|-------|---------------|---------------|
| Точні науки | 2025-12 | 145 | 43,500.00 |
| Іноземні мови | 2025-12 | 89 | 26,700.00 |
| Гуманітарні науки | 2025-12 | 67 | 20,100.00 |
| Точні науки | 2025-11 | 132 | 39,600.00 |
| Іноземні мови | 2025-11 | 78 | 23,400.00 |

---

## Запит 2: Топ репетиторів за доходом та кількістю занять

**Бізнес-питання:**  
Які репетитори були найбільш продуктивними на платформі за останній квартал?

**SQL-запит:**
```sql
SELECT 
    t.tutor_id,
    CONCAT(u.first_name, ' ', u.last_name) AS tutor_name,
    COUNT(DISTINCT b.booking_id) AS completed_bookings,
    SUM(ts.hourly_rate) AS total_earnings,
    COALESCE(AVG(r.rating), 0) AS average_rating,
    COUNT(DISTINCT r.review_id) AS review_count
FROM "tutor" t
INNER JOIN "user" u ON t.tutor_id = u.user_id
INNER JOIN "tutor_subject" ts ON t.tutor_id = ts.tutor_id
INNER JOIN "booking" b ON ts.tutor_subject_id = b.tutor_subject_id
INNER JOIN "schedule" sch ON b.schedule_id = sch.schedule_id
LEFT JOIN "review" r ON b.booking_id = r.booking_id
WHERE b.status = 'completed'
  AND sch.date >= CURRENT_DATE - INTERVAL '3 months'
GROUP BY t.tutor_id, u.first_name, u.last_name
HAVING COUNT(DISTINCT b.booking_id) > 0
ORDER BY total_earnings DESC, completed_bookings DESC
LIMIT 20;
```

**Пояснення:**
- **JOIN таблиць:** з'єднуємо таблиці репетиторів, користувачів, предметів, бронювань, розкладу та відгуків
- **Групування:** дані групуються за репетитором для агрегації статистики
- **Обчислення метрик:** підраховуємо кількість завершених бронювань, загальний заробіток, середній рейтинг та кількість відгуків
- **Фільтрація:** враховуємо лише завершені заняття за останні 3 місяці
- **Сортування:** за загальним доходом, потім за кількістю занять
- **Обмеження:** повертаємо топ-20 репетиторів

**Приклад виводу:**
| tutor_id | tutor_name | completed_bookings | total_earnings | average_rating | review_count |
|----------|------------|-------------------|----------------|----------------|--------------|
| 42 | Іванова Олена | 87 | 26,100.00 | 4.8 | 82 |
| 15 | Петренко Андрій | 75 | 22,500.00 | 4.9 | 70 |
| 33 | Коваленко Марія | 68 | 20,400.00 | 4.7 | 65 |
| 91 | Шевченко Ігор | 62 | 18,600.00 | 4.6 | 58 |

---
