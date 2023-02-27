using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace RandomPasswordGenerator
{
    public class Characters
    {
        public Characters() {
            number = new string[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };
            lowerCase = new string[] { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l",
                "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z" };
            upperCase = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L",
                "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
            specialCharacter = new string[] { "~", "!", "@", "#", "$", "%", "^", "&", "*", "(", ")",
                "_", "+", "|", "{", "}", ":", "?", "<", ">", "[", "]", "/", ";", ",", "'", "=" };

            _rnd = new Random();

            storage = new List<string[]>();
            
            
        }
        private string[] number { get; set; }
        private string[] lowerCase { get; set; }
        private string[] upperCase { get; set; }
        private string[] specialCharacter { get; set; }
        private List<string[]> storage { get; set; }
        private Random _rnd { get; set; }

        private ArrayList listNumber = new ArrayList(); //seçili kategorilerin numarasını tutar
        private StringBuilder password=new StringBuilder();

        private int passwordLength;

        private string passwordLengthValidate { get; set; }

        private ConsoleKeyInfo answer { get; set; }


        public void getAnswers() //kullanıcı arayüzü ve cevaplarının toplanması
        {
            Console.WriteLine("******************************************************");
            Console.WriteLine("Welcome to the B E S T P A S S W O R D M A N A G E R !");
            Console.WriteLine("******************************************************");
            Console.WriteLine();
            Console.WriteLine("Do u want to include Numbers?(y/n)");
            answer=Console.ReadKey();
            switch(answer.Key)
            {
                case ConsoleKey.Y:
                    storage.Add(number);
                    break;

                default:
                    break;

            }
            Console.WriteLine();
            Console.WriteLine("OK! How about lowercase characters?(y/n)");
            answer=Console.ReadKey(); ;
            switch (answer.Key)
            {
                case ConsoleKey.Y:
                    storage.Add(lowerCase);
                    break;

                default:
                    break;

            }
            Console.WriteLine();
            Console.WriteLine("Very nice! How about uppercase characters?(y/n)");
            answer=Console.ReadKey();
            switch (answer.Key)
            {
                case ConsoleKey.Y:
                    storage.Add(upperCase);
                    break;

                default:
                    break;

            }
            Console.WriteLine();
            Console.WriteLine("All right! We are almost done. Would you also want to add special characters?");
            answer=Console.ReadKey();
            switch (answer.Key)
            {
                case ConsoleKey.Y:
                    storage.Add(specialCharacter);
                    break;

                default:
                    break;

            }
            Console.WriteLine();
            Console.WriteLine("Great! Lastly. How long do you want to keep your password length?");
            getPasswordLength();
            
        }
        public void getPasswordLength()
        {
            passwordLengthValidate=Console.ReadLine();
            if (int.TryParse(passwordLengthValidate, out passwordLength))
            {

                while (passwordLength<4)
                {

                    Console.WriteLine("Password must be minimum 4 digits 'number'");
                    Console.Write("Try again: ");
                    passwordLengthValidate=Console.ReadLine();
                    if (int.TryParse(passwordLengthValidate, out passwordLength)) ;

                }
                getList();
            }
            else
            {
                Console.WriteLine("----------------------------");
                Console.WriteLine("password length must be number");
                Console.Write("Try Again: ");
                getPasswordLength();

            }
        }

        public void getList()
        {
            if (storage.Count>0)
            {
                for (int i = 0; i<storage.Count; i++)
                {
                    selectedList();
                }
                for (int i = 0; i< (passwordLength-storage.Count); i++)
                {
                    var randomIndex = _rnd.Next(storage.Count);
                    listNumber.Add(randomIndex);
                }
                getPassword();

            }
            else
            {
                Console.WriteLine("All categories cannot be 'no'");
                getPassword();
            }
                

        }
        public void getPassword()
        {
                for (int i = 0; i<listNumber.Count; i++)
                {
                    int temp = (int)listNumber[i];
                    var randomIndex = _rnd.Next(storage[temp].Length);
                    password.Append(Convert.ToString(storage[temp][randomIndex])); 
                }
                Console.WriteLine("--------------------");
                Console.WriteLine(password);
                Console.WriteLine("--------------------");

            

        }
        public int selectedList() //seçili herbir kategoriden en az 1 tane ögenin seçilmesini garantileyen kod bloğu
        {
            var randomIndex=_rnd.Next(storage.Count);
            
            if (listNumber.Contains(randomIndex))
            {
                return selectedList();
            }
            else
            {
                listNumber.Add(randomIndex);
            }
            
             return randomIndex;
        }

    }
}
