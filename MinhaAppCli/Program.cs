// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello SonarQube!");
MetodoComErro();
MetodoMau();
Soma(1, 2);
MetodoInseguro("Miguel");

void MetodoInseguro(string input)
{
    var sql = "SELECT * FROM Users WHERE name = '" + input + "'"; // ❌ SQL Injection
}

try
{
    int.Parse("abc");
}
catch (Exception)
{
    // ❌ vazio de propósito
}

int Soma(int a, int b)
{
    return a + b;
}

void MetodoMau()
{
    int numero = 10; // ❌ variável nunca usada

    if (true)
    {
        return;
    }

    Console.WriteLine("Nunca será executado"); // ❌ código morto
}

void MetodoComErro()
{
    int x = 0;
    int y = 10 / x; // ❌ Divisão por zero (BUG)
}
