# Запуск лабораторной работы в Visual Studio

Вариант 2 — «Служба доставки». В решении два проекта: библиотека `Lab2.Core` и модульные тесты `Lab2.Tests`. Все исходники совпадают с листингами Приложения А первого исправленного отчёта.

## Подготовка

Нужны Visual Studio с поддержкой .NET 8 и установленный **.NET SDK 8.0**. В установщике Visual Studio выберите рабочую нагрузку «Разработка классических приложений .NET» и компонент SDK .NET 8. Для первого восстановления пакетов NuGet требуется интернет.

1. Полностью распакуйте архив в отдельную папку.
2. Откройте файл `Lab2/Lab2.sln` в Visual Studio.
3. Дождитесь восстановления пакетов. При необходимости нажмите правой кнопкой по решению в обозревателе решений и выберите «Восстановить пакеты NuGet».
4. Выполните «Сборка → Собрать решение».
5. Откройте «Тест → Обозреватель тестов» (Test → Test Explorer).
6. Нажмите «Выполнить все тесты» (Run All).

Ожидаемый результат — **77 успешных тестовых случаев**. Запуск этой лабораторной выполняется через обозреватель тестов: `Lab2.Core` является библиотекой, у неё нет окна приложения или метода `Main` для запуска через F5. Каждый тест создаёт заявку и вызывает `DeliveryCalculator.CalculateCost`.

## Запуск из терминала

Откройте терминал в папке, где лежит `Lab2.sln`, и выполните:

```powershell
dotnet restore Lab2.sln
dotnet test Lab2.sln
```

## Покрытие кода

В терминале PowerShell из этой же папки выполните:

```powershell
.\scripts\Test-WithCoverage.ps1
```

Скрипт восстанавливает пакеты и ReportGenerator, запускает все тесты, проверяет порог 90% для строк и ветвей и создаёт `coveragereport/index.html`. Откройте этот файл в браузере.

Можно выполнить команды вручную:

```powershell
dotnet tool restore
dotnet test Lab2.sln --collect:"XPlat Code Coverage" --settings coverage.runsettings
dotnet tool run reportgenerator "-reports:Lab2.Tests/TestResults/**/coverage.cobertura.xml" "-targetdir:coveragereport" "-reporttypes:Html"
```

Перед повторным ручным запуском удалите только созданную предыдущим запуском папку `Lab2.Tests/TestResults`, чтобы маска выбирала результаты текущего запуска. Скрипт выше автоматически использует отдельную папку для каждого запуска.

Сохранённый контрольный запуск: 77 из 77 успешно, покрытие строк 100% (57/57), ветвей 100% (32/32). Подтверждения находятся в `docs/evidence/compliance-review.trx` и `docs/evidence/coverage-compliance-review.cobertura.xml`.

## Что находится в архиве

- `Lab2.sln` и два проекта со всеми девятью файлами C#.
- `global.json`, манифест ReportGenerator, настройки покрытия и скрипт проверки.
- `docs/test-matrix.md` — 36 сценариев.
- `docs/tdd-history.md` и `docs/evidence/tdd/` — этапы Red, Green, Refactor и журналы запусков.
- `docs/defense.md` — пояснения и ответы для защиты.
- `docs/evidence/` — сохранённые результаты тестов и XML покрытия.

Документация Microsoft: [запуск тестов через Test Explorer](https://learn.microsoft.com/en-us/visualstudio/test/run-unit-tests-with-test-explorer?view=visualstudio).
