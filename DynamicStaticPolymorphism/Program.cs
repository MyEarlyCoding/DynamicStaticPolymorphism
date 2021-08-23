using System;
using System.Collections.Generic;

/// <summary>
/// Программа реализует систему продуктов 
/// Реализуется всё это добро через базовый класс Product c атрибутами имя* и цена*
/// Также у Product есть демонстративный метод Discount который выдает скидку на определённый процент
/// В случае если нам нужно как-то модернизировать, расширить класс Product (Допустим поменять логику метода Discount) для определённой категории товаров,например: 
/// мы продаём молочный продукты и хотим сделать для них отдельно определённую скидку допустим понижать цену на 100 рубасов.
/// то мы можем модернизировать, расширить по SOLID(2ой принцип) класс Product c помощью так-называемого ПОЛИМОРФИЗМА ЁПТА
/// для этого используется динамический полиморфизм , когда создаётся виртуальный метод , который в будущем для производных классов будем переопределять виртуальный метод
/// статический полиморфизм также помогает расширить базовый класс, но это не является полиморфизмом по своей сути - т.к просто реализуется перезагрузка, замещение/скрытие базого, невиртуального метода.
/// Также отличие статического полиморфизма от динамического, в том что в статическом - на этапе компиляции, определяется какой будет вызываться метод в родительском классе и его производном.
/// В динамическом же полиморфизме непонятно какой будет вызываться метод в родительском классе - переопределённый(override) метод или базовый метод(virtual).Это определяется во время выполнения программы.
/// </summary>
namespace DynamicStaticPolymorphism
{
    //Базовый класс Product
    class Product
    {
        public string Name { get; set; }

        public int Price { get; set; }

        public Product(string name, int price)
        {
            Name = name;
            Price = price;
        }

        public virtual void Discount(int percent)
        {
            Price = Price * (100 - percent) / 100;

            Name += " class Product";
        }

    }
    //1ый тип полиморфизма - динамический , когда Product не понимает еще какой метод он будет вызывать virtual или override
    // Product s = new Milk(); - вызовется переопределённый метод , т.к после приведения произовдного типа Milk к базовому Product ссылка Product s будет ссылаться на метод Milk.Discount().
    // Milk s = new Milk(); - вызовется переопределённый метод
    class Milk : Product
    {
        public Milk(string name, int price): base(name, price)
        {
            this.Name = name;
            this.Price = price;
        }
        public override void Discount(int percent) 
        {
            base.Discount(percent);
            this.Price -= 55;

            Name += " class Milk";
        }
    }
    //2ой тип полиморфизма - статический или еще как можно назвать замещением по урокам ITDVN, но в случае, если у методов совпадают сигнатуры.
    //Но по своей сути это является перегрузкой метода,  и можно вызвать как производный метод так и базовый метод ,если сигнатуры различаются.
    // Product s = new Drinks(); - вызовется базовый метод, т.к после приведения производного класса Household к базовому Product - метод Household.Discount() заместится на Product.Discount() 
    // ,все потому что s будет ссылаться на класс Product, соответственно т.к в Household метод не переопределен, а просто перегружен ,то метод Household.Discount ,просто будет не виден.
    // Drinks s = new Drinks(); - вызовется производный метод Product.Discount().
    // Drinks s = new Product(); - ну тут тебя нахер компилятор пошлёт с такими запросами) 
    class Drinks : Product
    {
        public Drinks(string name, int price) : base(name, price)
        {
            this.Name = name;
            this.Price = price;
        }
        public void Discount(int percent) 
        {
            base.Discount(percent);
            this.Price -= 77;

            Name += " class Drinks";
        }

        public void Discount()
        {
          
            this.Price -= 90;

            Name += " class Household";
        }
    }

    //2ой тип полиморфизма(аналогичный только для удобства исп ключ слово new) - статический или еще как можно назвать замещением по урокам ITDVN, но в случае, если у методов совпадают сигнатуры. 
    //Но по своей сути это является перегрузкой метода,  и можно вызвать как производный метод так и базовый метод ,если сигнатуры различаются.
    //В зависимости от того на какой класс ссылается объект, такой и вызывается метод.
    // Product s = new Household(); - вызовется базовый метод, т.к после приведения производного класса Household к базовому Product - метод Household.Discount() заместится на Product.Discount() 
    // ,все потому что s будет ссылаться на класс Product, соответственно т.к в Household метод не переопределен, а просто перегружен ,то метод Household.Discount ,просто будет не виден.
    // Household s = new Household(); - вызовется производный метод Product.Discount().
    // Household s = new Product(); - ну тут тебя нахер компилятор пошлёт с такими запросами) 
    class Household : Product
    {
        public Household(string name, int price) : base(name, price)
        {
            this.Name = name;
            this.Price = price;
        }

        public new void Discount(int percent)
        {
            base.Discount(percent);
            this.Price -= 100;

            Name += " class Household"; 
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Milk d = new Milk("Milk vologodskoye", 1000);
            Drinks s= new Drinks("Coca-cola", 1000);
            Household p= new Household("Sponges Ultra", 1000);

            d.Discount(10);
            s.Discount(10);
            p.Discount(10);

            Console.WriteLine($"Name {d.Name}, Price: {d.Price}");
            Console.WriteLine($"Name {s.Name}, Price: {s.Price}");
            Console.WriteLine($"Name {p.Name}, Price: {p.Price}");

            /////////////////////////////////////////////////////////////
            d = new Milk("Milk vologodskoye", 1000);
            s = new Drinks("Coca-cola", 1000);
            p = new Household("Sponges Ultra", 1000);

           
            List<Product> list = new List<Product>() { new Product("Tvorog", 1000), d, s, p };

            foreach(var el in list)
            {
                el.Discount(10);
                Console.WriteLine($"Name {el.Name}, Price: {el.Price}");
            }

            
        }
    }
}
