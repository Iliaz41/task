**Задание 1:**
Определён следующий класс:
```csharp
public class ShopManage
{
	public ShopManage() {}
	public int AddItem() {/*тело метода*/}
	public static void UpdateCounts() {/*тело метода*/}
}
```
Какие варианты обращения к методам этого класса из другого класса выдадут ошибку при компиляции и почему:

_a._  не выдаст ошибку компиляции (будут предупреждения)
```csharp
ShopManage mng = new ShopManage();
int x = mng.AddItem();
mng.UpdateCounts(); // можно, но bad practice
```
_b._ не выдаст ошибку компиляции
```csharp
ShopManage mng = new ShopManage();
int x = mng.AddItem();
ShopManage.UpdateCounts();
```
_c._ выдаст ошибку компиляции
```csharp
int x = ShopManage.AddItem(); //ошибка компиляции, требует контекст выполнения
ShopManage.UpdateCounts();
```
**Задание 2:**
Есть контекст базы данных db, через который происходит обращение к таблице Contracts. Необходимо вывести в консоль поля Name первой 1000 записей таблицы. Выберите наиболее оптимальный вариант и объясните причину выбора:

_Вариант №1_
```csharp
string message = new string();
var names = db.Contracts.ToList().Take(1000).Select(x => x.Name);

foreach (var name in names)
		message += name + “\r\n”;

Console.WriteLine(message);
```
_Вариант №2_
```csharp
string message = new string();
var names = db.Contracts.Take(1000).Select(x => x.Name).ToList();
	
foreach (var name in names)
		message += name + “\r\n”;

Console.WriteLine(message);
```
_Вариант №3_ - более оптимальный вариант, не используется дополнительная переменная, не используем конкатенацию строк, также более оптимальный порядок функций в LINQ to Entity 
```csharp
var names = db.Contracts.Select(x => x.Name).Take(1000).ToList();

foreach (var name in names)
	Console.WriteLine(name + “\r\n”);
```

**Задание 3:**
Напишите запрос который выведет данные об имени, фамилии, городе и штате проживания.
Если адрес для конкретного человека не представлен в таблице адресов, можно вывести null вместо города и штата.
```sql
SELECT p.firstName, p.lastName, a.city, a.state
FROM Person p
LEFT JOIN Address a ON p.personId = a.personId;
```
Используем ```LEFT JOIN``` так как нам необходимо взять всю таблицу person и объединить с таблицей address, чтобы добавить к выводу адреса проживания, если они присутствуют.

**Задание 4:**
```csharp
public List<int> getBalls(List<int> balls, int capacity)
{
    int sum = balls.Sum(); // кол-во всех шариков

    double coefficient = (double)capacity / (double)sum; // расчет пропорций

    List<int> result = balls.Select(item => (int)(item * coefficient)).ToList(); // список шариков в мешке

    int resultSum = result.Sum(); // кол-во шариков в мешке

    int remainder = capacity - resultSum; // заполнен ли мешок полностью
    if (remainder != 0) // если нет 
    {
        List<int> orderedResult = result.OrderByDescending(x => x).ToList(); // сортируем по убыванию приоритета, исходя из пропорции 

        foreach (var item in orderedResult)
        {
            if(remainder > 0)
            {
                result[result.IndexOf(item)]++; // раскидываем остаток в порядке приоритета 
                remainder--;
            }
        }
    }

    return result;
}
```
