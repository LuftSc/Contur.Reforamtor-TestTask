# Тестовое задание для Контур.Реформатор

## Для работы программы необходимо запустить её в Docker. Выполните последовательно следующие команды:

```bash
docker build -t xslt-reformator .
```

```bash
docker run -d -p 8080:8080 --name xslt-container xslt-reformator
```

После успешного запуска приложение будет доступно по адресу:
http://localhost:8080