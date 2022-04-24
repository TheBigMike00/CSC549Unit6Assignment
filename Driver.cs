using System;
using System.Collections.Generic;
using System.Linq;

namespace Unit6Assignment
{
    class Driver
    {
        #region Exercise 2.53
        /*
        (list 'a 'b 'c)
            -> (a b c)

        (list (list 'george))
            -> ((george))

        (cdr '((x1 x2) (y1 y2)))
            -> ((y1 y2))

        (cadr '((x1 x2) (y1 y2)))
            -> (y1 y2)

        (pair? (car '(a short list)))
            -> false

        (memq 'red '((red shoes) (blue socks)))
            -> false

        (memq 'red '(red shoes blue socks))
            -> (red shoes blue socks)
        */
        #endregion


        #region Exercise 2.56
        public static bool IsVariable(string e)
        {
            return !Int32.TryParse(e, out int number) && e.Length == 1;
        }

        public static bool IsNum(string e)
        {
            return Int32.TryParse(e, out int number);
        }

        public static bool IsSameVariable(string v1, string v2)
        {
            return v1 == v2;
        }

        public static bool IsSum(string e)
        {
            return e.Contains('+');
        }

        public static string BeforePlus(string e)
        {
            return e.Substring(0, e.IndexOf("+"));
        }

        public static string AfterPlus(string e)
        {
            return e.Substring(e.IndexOf("+") + 1);
        }

        public static string MakeSum(string a1, string a2)
        {
            if (a1 == "0")
                return a2;
            if (a2 == "0")
                return a1;
            if (IsNum(a1) && IsNum(a2))
                return (Convert.ToInt32(a1) + Convert.ToInt32(a2)).ToString();
            return a1 + a2;
        }

        public static bool IsProduct(string e)
        {
            return ((!IsSum(e)) && e.Length == 2);
        }

        public static string Multiplier(string e)
        {
            return e[0].ToString();
        }
        public static string Multiplicand(string e)
        {
            return e[1].ToString();
        }

        public static string MakeProduct(string m1, string m2)
        {
            if (m1 == "0" || m2 == "0")
                return "0";
            if (m1 == "1")
                return m2;
            if (m2 == "1")
                return m1;
            if (IsNum(m1) && IsNum(m2))
                return (Convert.ToInt32(m1) * Convert.ToInt32(m2)).ToString();
            return m1 + m2;
        }

        public static bool IsExponent(string s1)
        {
            return s1.Contains("^");
        }

        public static string Base(string e)
        {
            return e.Substring(0, e.IndexOf("^"));
        }

        public static string Exponent(string e)
        {
            return e.Substring(e.IndexOf("^") + 1);
        }

        public static string MakeExponent(string b, string e)
        {
            if (b == "1")
                return "1";
            if (e == "1")
                return b;
            if (e == "0")
                return "1";
            return b + "^" + e;
        }

        public static string Derivative(string exp, string variable)
        {
            if (IsNum(exp))
                return "0";
            if (IsVariable(exp))
            {
                if (IsSameVariable(exp, variable))
                    return "1";
                return "0";
            }
            if(IsSum(exp))
            {
                return MakeSum(Derivative(BeforePlus(exp), variable), Derivative(AfterPlus(exp), variable));
            }
            if(IsProduct(exp))
            {
                return MakeSum(
                    MakeProduct(Multiplier(exp), Derivative(Multiplicand(exp), variable)),
                    MakeProduct(Derivative(Multiplier(exp), variable), Multiplicand(exp)));
            }
            if(IsExponent(exp))
            {
                return MakeProduct(
                    MakeProduct(
                        Exponent(exp), MakeExponent(Base(exp), MakeSum(Exponent(exp), "-1"))),
                    Derivative(Base(exp), variable));
            }

            return "Error";
        }



        #endregion


        #region Exercise 2.59
        public static List<string> result = new List<string>();

        public static bool IsElementOfSet(string x, List<string> set) //Using recursion like book instead of .Contains
        {
            List<string> setCopy = new List<string>();
            foreach (var element in set)
                setCopy.Add(element);

            if (setCopy.Count == 0)
                return false;
            if (x == setCopy[0])
                return true;
            setCopy.RemoveAt(0);
            return IsElementOfSet(x, setCopy);
        }

        public static List<string> AdjoinSet(string x, List<string> set)
        {
            if (IsElementOfSet(x, set))
                return set;
            set.Add(x);
            return set;
        }

        public static List<string> IntersectionSet(List<string> set1, List<string> set2)
        {
            if (set1.Count == 0 || set2.Count == 0)
                return result;
            if(IsElementOfSet(set1[0], set2) && !result.Contains(set1[0]))
            {
                result = AdjoinSet(set1[0], result);
                set1.RemoveAt(0);
                return IntersectionSet(set1, set2);
            }
            set1.RemoveAt(0);
            return IntersectionSet(set1, set2);
        }

        public static List<string> UnionSet(List<string> set1, List<string> set2)
        {
            if (set1.Count == 0 && set2.Count == 0)
                return result;
            if(set1.Count == 0)
            {
                result = AdjoinSet(set2[0], result);
                set2.RemoveAt(0);
                return UnionSet(set1, set2);
            }
            result = AdjoinSet(set1[0], result);
            set1.RemoveAt(0);
            return UnionSet(set1, set2);
        }

        #endregion



        static void Main(string[] args)
        {
            //Exercise 2.56
            Console.WriteLine(Derivative("x^5", "x") + "\n"); //Result: 5x^4

            //Exercise 2.59
            //Intersection
            Driver.result.Clear();
            var set1 = new List<string> { "1", "2", "5" };
            var set2 = new List<string> { "2", "3", "4" };
            var response = IntersectionSet(set1, set2);
            foreach(var element in response)
                Console.WriteLine(element);

            //Union
            Console.WriteLine("\n\n");
            Driver.result.Clear();
            set1 = new List<string> { "1", "2", "5" };
            set2 = new List<string> { "2", "3", "4" };
            var response2 = UnionSet(set1, set2);
            foreach (var element in response2)
                Console.WriteLine(element);
        }
    }
}
