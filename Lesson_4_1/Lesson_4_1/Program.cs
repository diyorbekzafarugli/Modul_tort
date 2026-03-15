namespace Lesson_4_1;

internal class Program
{
    static void Main(string[] args)
    {
        //try
        //{
        //    int age = 19;

        //    if (age < 18)
        //    {
        //        throw new AgeException("yosh 18 dan kichi");
        //    }

        //    Console.WriteLine(age);
        //}
        //catch (AgeException)
        //{
        //    Console.WriteLine("xatolik");
        //}

        //while (!false)
        //{
        //    try
        //    {
        //        Console.Write("Yosh kiriting : ");
        //        int age = int.Parse(Console.ReadLine()!);

        //        int res = 100 / age;

        //        if (age < 18)
        //        {
        //            throw new AgeException("yoshi 18 dan kichik ekan");
        //        }

        //        Console.WriteLine(age);
        //    }
        //    catch (AgeException ex)
        //    {
        //        Console.WriteLine("Bu yerda yoshdagi xatoolik ushlandi" + ex);
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("Bu yerda boshqa xatolik ushlandi" + ex);
        //    }
        //}

        /*
         *  1. Agar `age < 18` bo‘lsa exception chiqaradigan dastur yozing./
            2. `DivideByZeroException` ni ushlab (catch qilib) qayta ishlang./
            3. Sonni parse qilishda (`int.Parse`) yuz beradigan `FormatException` ni ushlang./
            4. Agar `name` bo‘sh bo‘lsa exception chiqaradigan metod yozing./
            5. `finally` blokidan foydalanib `"Operation finished"` degan yozuvni chiqaring./
            6. `InvalidEmailException` nomli custom (maxsus) exception yarating./
            7. Agar email ichida `"@"` belgisi bo‘lmasa custom exception chiqaring./
            8. `IndexOutOfRangeException` ni ushlab qayta ishlaydigan dastur yozing./
            9. Agar student topilmasa 404 qaytaradigan metod yarating.
            10. Agar `ID <= 0` bo‘lsa 400 status code qaytaradigan API endpoint yozing.

         */


        //try
        //{
        //    int a = 34;
        //    int zero = 0;
        //    int res = a / zero;

        //}
        //catch(DivideByZeroException ex)
        //{
        //    Console.WriteLine(ex.Message);
        //}

        //while (true)
        //{
        //    try
        //    {
        //        Console.Write("son kiriting : ");
        //        int a = int.Parse(Console.ReadLine()!);
        //    }
        //    catch (FormatException ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //    }
        //}

        //while (true)
        //{


        //    try
        //    {
        //        Console.Write("Son kiriting : ");
        //        int a = int.Parse(Console.ReadLine()!);
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //    }
        //    finally
        //    {
        //        Console.WriteLine("Finally block ishladi");
        //    }

        //}


        //try
        //{
        //    string email = "examplegmail.com";

        //    if (!email.Contains("@"))
        //    {
        //        throw new InvalidEmailException("Email yo'q ekan. ");
        //    }

        //    Console.WriteLine("@ belgisi bor.");
        //}
        //catch (InvalidEmailException ex)
        //{
        //    Console.WriteLine(ex.Message);
        //}

        //int[] ints = new int[10];
        //Random random = new();
        //for (int i = 0; i < ints.Length; i++)
        //{
        //    ints[i] = random.Next(1, 100);
        //}

        //while (true)
        //{
        //    try
        //    {
        //        Console.WriteLine(ints[random.Next(0, 11)]);
        //    } 
        //    catch(IndexOutOfRangeException ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //    }

        //    Console.ReadKey();
        //}


        /**
         * ### 🔹 O‘rta Daraja

            11. `ProductService` yarating va u `ProductNotFoundException` exceptionini chiqarsin.
            12. Controller ichida custom exceptionni ushlab 404 status code qaytaring.
            13. `BankAccount` klassi yarating. Agar `withdraw > balance` bo‘lsa exception chiqarsin.
            14. Exceptionni 500 status code ga aylantiring.
            15. Qo‘shimcha property (masalan, `ErrorCode`) ga ega bo‘lgan custom exception yarating.
            16. Ichma-ich (nested) metodlar bilan dastur yozing va stack unwinding qanday ishlashini ko‘rsating.
            17. Quyidagicha javob qaytaradigan API endpoint yozing:

            * noto‘g‘ri input bo‘lsa → 400
            * ma’lumot topilmasa → 404
            * noma’lum xatolik bo‘lsa → 500

            18. Agar ro‘yxat (List) bo‘sh bo‘lsa exception chiqaradigan metod yozing.
            19. `UnauthorizedAccessException` uchun custom handling yozing va 401 qaytaring.
            20. `try-catch` dan foydalanib va to‘g‘ri status code lar bilan mini CRUD controller yarating.

         */

        BankAccount account = new();

        account.Balance = 10_000_000m;
        while (true)
        {
            try
            {
                Console.Write("summani kiriting : ");
                var paymant = decimal.Parse(Console.ReadLine()!);

                if (paymant > account.Balance)
                {
                    throw new Exception("Balansingizda mablag' yetarli emas");
                }

                account.Balance -= paymant;

                if (account.Balance == 0)
                {
                    throw new Exception("Balansingizda mablag' yetarli emas");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }



}
public class BankAccount
{
    public decimal Balance { get; set; }
}