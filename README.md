# �������� ������� ��� ������.����������

## ��� ������ ��������� ���������� ��������� � � Docker. ��������� ��������������� ��������� �������:

```bash
docker build -t xslt-reformator .
```

```bash
docker run -d -p 8080:8080 --name xslt-container xslt-reformator
```

����� ��������� ������� ���������� ����� �������� �� ������:
http://localhost:8080