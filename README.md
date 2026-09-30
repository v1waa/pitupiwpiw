# Лабораторная работа 2 — Служба доставки

Вариант 2: автоматизированное модульное тестирование бизнес-логики и оценка покрытия кода. Исходный код разработан самостоятельно **по TDD**.

**Результат:** 77 тестовых случаев проходят; Line Coverage — **100% (57/57)**; Branch Coverage — **100% (32/32)**. Матрица содержит 36 сценариев. Метрики измерены Coverlet для всей сборки `Lab2.Core`, без исключения строк бизнес-логики.

## Состав работы

- `Lab2.sln` — решение ровно с двумя проектами.
- `Lab2.Core` — Class Library: заявка, тип доставки и калькулятор.
- `Lab2.Tests` — xUnit, FluentAssertions, AAA, `[Fact]`, `[Theory]`, `[InlineData]`.
- [Матрица сценариев](docs/test-matrix.md).
- [Этапы TDD](docs/tdd-history.md) и [результаты запусков](docs/evidence/tdd-runs.json).
- [Основной отчёт Word](docs/Lab2_Delivery_Report.docx) и три дополнительных: [отчёт 2](docs/Lab2_Delivery_Report_2.docx), [отчёт 3](docs/Lab2_Delivery_Report_3.docx), [отчёт 4](docs/Lab2_Delivery_Report_4.docx). В каждом есть все требуемые разделы, листинги и настоящие скриншоты покрытия. Исходники и результаты общие; формулировки анализа различаются. Титульные листы перенесены из предоставленного образца КНИТУ-КАИ, колледжа «КИТ»; ФИО студента и группа оставлены для заполнения.
- [Проверка выполнения методички](docs/requirements-checklist.md) — требования, результаты и ссылки на подтверждения.
- `docs/coverage/final/index.html` — готовый HTML-отчёт ReportGenerator. Скачайте репозиторий и откройте файл локально.
- `docs/coverage/intermediate/index.html` — промежуточный анализ отдельной группы валидации.
- `docs/evidence` — скриншоты, XML покрытия, итоговый TRX и журналы TDD.
- [Ответы для защиты](docs/defense.md).
- [Исходники для Visual Studio одним архивом](deliverables/Lab2_VisualStudio_Source.zip) и [пошаговый запуск](RUN_IN_VISUAL_STUDIO.md).

Первый дополнительный отчёт (`Lab2_Delivery_Report_2.docx`) исправлен отдельно: интервалы перед и после абзацев — 0 пт; названия Таблиц выровнены по ширине с одинарным интервалом; на продолжениях повторены подписи и шапки. Полные листинги всех девяти файлов C# добавлены после заключения в Приложение А с новой страницы.

## Запуск

Установите **.NET SDK 8.0**, скачайте репозиторий и откройте терминал в его корне. В Visual Studio откройте `Lab2.sln` и используйте обозреватель тестов.

```sh
dotnet restore Lab2.sln
dotnet test Lab2.sln
```

## Покрытие и HTML

```sh
dotnet tool restore
dotnet test Lab2.sln --collect:"XPlat Code Coverage" --settings coverage.runsettings
dotnet tool run reportgenerator "-reports:Lab2.Tests/TestResults/**/coverage.cobertura.xml" "-targetdir:coveragereport" "-reporttypes:Html"
```

Откройте `coveragereport/index.html`. Команды соответствуют шагам 4–5 методички; ReportGenerator зафиксирован в локальном манифесте, поэтому глобальная установка не требуется. Автоматическая проверка порога 90% из PowerShell:

```powershell
.\scripts\Test-WithCoverage.ps1
```

## Оба пакета Coverlet

В тестовом проекте предусмотрены `coverlet.collector` и `coverlet.msbuild`, как требует методичка. Они включаются **по отдельности**: по умолчанию collector для `--collect`, альтернативно msbuild через свойство `CoverageTool`. Это учитывает рекомендацию разработчиков Coverlet не подключать обе интеграции одновременно.

```sh
dotnet test Lab2.sln -p:CoverageTool=msbuild -p:CollectCoverage=true -p:CoverletOutputFormat=cobertura -p:Threshold=90 "-p:ThresholdType=line%2cbranch" "-p:Include=[Lab2.Core]*"
```

При переключении режима эта команда восстанавливает соответствующий пакет. Для следующего запуска collector используйте обычный `dotnet test` с восстановлением, без `--no-restore`.

## Правила расчёта

```text
Цена = min(50000,
    (тариф расстояния × коэффициент типа + надбавка за вес)
    × (хрупкий груз ? 1.15 : 1.0)
    + (страховка и стоимость > 0 ? max(100, стоимость × 0.02) : 0))
```

Границы 10, 100 и 1000 км относятся к предыдущему диапазону; аналогично 5, 20 и 100 кг. Коэффициент типа умножает только базовый тариф. Страховка добавляется после надбавки за хрупкость в порядке правил задания. Используется `decimal`, дополнительного округления нет.

Проверяются расстояние `(0; 5000]` и вес `(0; 1000]`; нарушение вызывает `ArgumentException`. Дополнительно отвергаются null-заявка и неопределённый тип enum. Отрицательная объявленная стоимость не вызывает исключение: при ней страховка не начисляется, как следует из условия `declared value > 0`.

## Источники

- Прилагаемая методичка «Лабораторная работа №2. Автоматизированное модульное тестирование», страницы 8–9 и 13–17.
- [Microsoft Learn — покрытие модульных тестов](https://learn.microsoft.com/dotnet/core/testing/unit-testing-code-coverage).
- [Coverlet — способы интеграции](https://github.com/coverlet-coverage/coverlet#installation).
